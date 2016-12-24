using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoFlo_Basic {

    public class DataTreatment {

        public static string GetDataType(object Data) {
            if (Data is string) {
                return "Text";
            } else if (Data is float || Data is double || Data is Int64) {
                return "Number";
            } else if (Data is UnityGraphObject) {
                return (Data as UnityGraphObject).GetObjectType();
            } else
                throw new Exception("TODO");
        }

        public static string GetDataRepresentative(object Data) {
            string toString = Data.ToString();
            if (Data is string) {
                return "\"" + toString + "\"";
            } else if (Data is float || Data is double || Data is Int64) {
                return toString;
            } else {
                return "<" + toString + ">";
            }
        }

        public static string GetDataEditable(object Data) {
            string toString = Data.ToString();
            if (Data is string || Data is float || Data is double || Data is Int64) {
                return toString;
            } else {
                return "";
            }
        }

        public static object TreatData(object Data, Graph Graph) {
            if (Data is IDictionary) {
                Dictionary<string, object> d = Data as Dictionary<string, object>;
                if (d.ContainsKey("type") && d.ContainsKey("id")) {
                    string id = d["id"] as string;
                    IGraphObject variable = Graph.AssociatedInterlink.GetLinkedVariableByID(id);

                    if (d["type"] as string != variable.GetObjectType())
                        throw new Exception("Types for variable " + id + " do not match: " + (d["type"] as string) + " != " + variable.GetObjectType());

                    return variable;
                } else {
                    throw new Exception("TODO");
                }
            } else {
                return Data;
            }
        }

    }

}