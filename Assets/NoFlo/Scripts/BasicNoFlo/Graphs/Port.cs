using NoFloEditor;
using System;
using System.Collections.Generic;

namespace NoFlo_Basic {

    public abstract class Port : Processable {

        public string Name;
        public string Description;
        public string[] Types;
        public bool Hidden;

        public Component Component;
        public PortVisualisation Visualisation;

        protected List<Edge> Connections;

        public abstract bool IsInput();

        public abstract void Process(ExecutionContext context);
        public abstract void DebugHighlight();
        public abstract void DebugUnhighlight();

        public IEnumerable<Edge> Edges() {
            return Connections;
        }

        public string TypesToString() {
            string types = "";
            for (int i = 0; i < Types.Length; i++) {
                if (i != 0)
                    types += ", ";
                if (Types[i] == "All") {
                    return "All";
                } else {
                    types += Types[i];
                }
            }
            return types;
        }

        public Port() {
            Connections = new List<Edge>();
            Hidden = false;
        }

        public void AddConnection(Edge edge) {
            if (Connections.Contains(edge))
                throw new Exception("");

            Connections.Add(edge);
        }

        public void RemoveConnection(Edge edge) {
            if (!Connections.Contains(edge))
                throw new Exception("");

            Connections.Remove(edge);
        }

        public void RemoveAllConnections() {
            for (int i = 0; i < Connections.Count; i++) {
                Component.Graph.RemoveEdge(Connections[i]);
            }
        }

        public void StartProcessing() {
        }

        public bool HasMoreToProcess() {
            return false;
        }

    }

}