using NoFloEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoFlo_Basic {

    public class InPort : Port {

        public bool RememberOnlyLatest;

        public Action<OutPort> ProcessConnection;
        public Action<OutPort> ProcessDisconnection;

        private LinkedList<object> receivedQueue;
        private object onlyData;

        public DebugMessageVisualisation currentMessage;

        public InPort() {
            receivedQueue = new LinkedList<object>();
            RememberOnlyLatest = false;
        }

        public override bool IsInput() {
            return true;
        }

        public void Accept(object data) {
            if (RememberOnlyLatest)
                onlyData = data;
            else
                receivedQueue.AddLast(data);
        }

        public bool HasData() {
            if (RememberOnlyLatest)
                return onlyData != null;
            else
                return receivedQueue.Count > 0;
        }

        public int GetDataCount() {
            if (RememberOnlyLatest)
                return (onlyData == null) ? 0 : 1;
            else
                return receivedQueue.Count;
        }

        public void ClearData() {
            if (RememberOnlyLatest)
                return;

            receivedQueue.Clear();

            if (Component.Graph.InDebugMode()) {
                Component.Graph.GraphEditor.EventOptions.DataRemoved.Invoke(this);
            }
        }

        public object GetData() {
            if (RememberOnlyLatest) {
                return onlyData;
            }

            object data = receivedQueue.First.Value;
            receivedQueue.RemoveFirst();

            if (Component.Graph.InDebugMode()) {
                if (RememberOnlyLatest) {
                    if (onlyData == null)
                        Component.Graph.GraphEditor.EventOptions.DataRemoved.Invoke(this);
                    else
                        Component.Graph.GraphEditor.EventOptions.DataAdded.Invoke(this, onlyData);
                } else {
                    if (receivedQueue.Count == 0) {
                        Component.Graph.GraphEditor.EventOptions.DataRemoved.Invoke(this);
                    } else {
                        Component.Graph.GraphEditor.EventOptions.DataAdded.Invoke(this, receivedQueue.First.Value);
                    }
                }
            }

            return data;
        }

        public override void Process(ExecutionContext context) {
            context.QueueTask(Component);
        }

        public override void DebugHighlight() {
            if (Component.Graph.InDebugMode()) {
                if (RememberOnlyLatest) {
                    if (onlyData == null)
                        Component.Graph.GraphEditor.EventOptions.DataRemoved.Invoke(this);
                    else
                        Component.Graph.GraphEditor.EventOptions.DataAdded.Invoke(this, onlyData);
                } else {
                    if (receivedQueue.Count == 0) {
                        Component.Graph.GraphEditor.EventOptions.DataRemoved.Invoke(this);
                    } else {
                        Component.Graph.GraphEditor.EventOptions.DataAdded.Invoke(this, receivedQueue.First.Value);
                    }
                }
            }
        }

        public override void DebugUnhighlight() {
            
        }

        public override string ToString() {
            return Component.ComponentName + "." + Name;
        }

    }

}