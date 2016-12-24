using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoFlo_Basic {

    public class JSONGraphFileReader {

        public class RawNode {
            public string Name;
            public string QualifiedComponentName;
            public Vector3 metadataPosition;
        }

        public class RawEdge {
            public string srcProcess;
            public string srcPort;
            public string tgtProcess;
            public string tgtPort;
        }

        public class RawDefaultValue {
            public object Data;
            public string tgtProcess;
            public string tgtPort;
        }

        Dictionary<string, object> ParsedJSON;
        Dictionary<string, object> Nodes;
        List<object> Edges;
        Dictionary<string, object> Details;
        Dictionary<string, object> metadata;
        Dictionary<string, object> tgt;
        Dictionary<string, object> src;

        Queue<RawNode> RawNodes;
        Queue<RawEdge> RawEdges;
        Queue<RawDefaultValue> RawDefaultValues;

        public JSONGraphFileReader(string json) {

            RawNodes = new Queue<RawNode>();
            RawEdges = new Queue<RawEdge>();
            RawDefaultValues = new Queue<RawDefaultValue>();

            // Load graph
            ParsedJSON = (Dictionary<string, object>) fastJSON.JSON.Parse(json);
            Nodes = (Dictionary<string, object>) ParsedJSON["processes"];
            Edges = (List<object>) ParsedJSON["connections"];

            foreach (string componentName in Nodes.Keys) {
                Details = (Dictionary<string, object>) Nodes[componentName];

                RawNode node = new RawNode() {
                    Name = componentName,
                    QualifiedComponentName = Details["component"] as string,
                };
                RawNodes.Enqueue(node);

                if (Details.ContainsKey("metadata")) {
                    metadata = (Dictionary<string, object>) Details["metadata"];

                    if (metadata.ContainsKey("x") && metadata.ContainsKey("y")) {
                        float x = 0, y = 0;

                        object X = metadata["x"];
                        switch (X.GetType().Name) {
                        case "Double":
                            x = (float) (double) X;
                            break;
                        case "Int64":
                            x = (float) (Int64) X;
                            break;
                        default:
                            throw new Exception("Uknown number type found: " + X.GetType().Name);
                        }

                        object Y = metadata["y"];
                        switch (Y.GetType().Name) {
                        case "Double":
                            y = (float) (double) Y;
                            break;
                        case "Int64":
                            y = (float) (Int64) Y;
                            break;
                        default:
                            throw new Exception("Uknown number type found: " + Y.GetType().Name);
                        }

                        node.metadataPosition = new Vector3(x, y);

                    }
                }
            }

            for (int i = 0; i < Edges.Count; i++) {
                Details = (Dictionary<string, object>) Edges[i];

                tgt = (Dictionary<string, object>) Details["tgt"];
                if (Details.ContainsKey("src")) {
                    Dictionary<string, object> src = (Dictionary<string, object>) Details["src"];

                    RawEdge edge = new RawEdge() {
                        srcProcess = src["process"] as string,
                        srcPort = src["port"] as string,
                        tgtProcess = tgt["process"] as string,
                        tgtPort = tgt["port"] as string,
                    };

                    RawEdges.Enqueue(edge);
                } else if (Details.ContainsKey("data")) {
                    RawDefaultValue defaultValue = new RawDefaultValue() {
                        Data = Details["data"],
                        tgtProcess = tgt["process"] as string,
                        tgtPort = tgt["port"] as string,
                    };

                    RawDefaultValues.Enqueue(defaultValue);
                }
            }

        }

        public bool NextNode(out RawNode node) {
            if (RawNodes.Count == 0) {
                node = null;
                return false;
            }

            node = RawNodes.Dequeue();
            return true;
        }

        public bool NextEdge(out RawEdge edge) {
            if (RawEdges.Count == 0) {
                edge = null;
                return false;
            }

            edge = RawEdges.Dequeue();
            return true;
        }

        public bool NextDefaultValue(out RawDefaultValue defaultValue) {
            if (RawDefaultValues.Count == 0) {
                defaultValue = null;
                return false;
            }

            defaultValue = RawDefaultValues.Dequeue();
            return true;
        }

    }

}