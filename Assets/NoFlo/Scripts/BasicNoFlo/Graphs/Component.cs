using NoFloEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoFlo_Basic {

    public class ComponentNameAttribute : Attribute {

        public string name;

        public ComponentNameAttribute(string name) {
            this.name = name;
        }

    }

    public class ComponentPackageAttribute : Attribute {

        public string name;

        public ComponentPackageAttribute(string name) {
            this.name = name;
        }

    }

    public abstract class Component : Processable {

        public string ComponentName;

        public Graph Graph;

        public ComponentInput Input;
        public ComponentOutput Output;

        public NodeVisualisation Visualisation;

        public Vector3 MetadataPosition;

        protected abstract void SetupInterfaces();
        public abstract void Process(ExecutionContext context);

        public Component() {
            Input = new ComponentInput();
            Output = new ComponentOutput();

            Input.Component = this;
            Output.Component = this;

            SetupInterfaces();

        }

        public void SetName(string Name) {
            ComponentName = Name;
            if (Visualisation != null)
                Visualisation.SetName(Name);
        }

        public void SetGraph(Graph Graph) {
            this.Graph = Graph;
        }

        public void AcceptDataOnPort(string portName, object data, HashSet<Component> Targets) {
            InPort port = Input.GetPort(portName);
            port.Accept(data);
        }

        public Action ConnectToInPort(string portName, Edge edge) {
            InPort port = Input.GetPort(portName);
            port.AddConnection(edge);
            edge.Target = port;
            return () => {
                if (port.ProcessConnection != null)
                    port.ProcessConnection.Invoke(edge.Source);
            };
        }

        public Action ConnectToOutPort(string portName, Edge edge) {
            OutPort port = Output.GetPort(portName);
            port.AddConnection(edge);
            edge.Source = port;
            return () => {
                if (port.ProcessConnection != null)
                    port.ProcessConnection.Invoke(edge.Target);
            };
        }

        public Action DisconnectInPort(string portName, Edge edge) {
            InPort port = Input.GetPort(portName);
            port.RemoveConnection(edge);
            return () => {
                if (port.ProcessDisconnection != null)
                    port.ProcessDisconnection.Invoke(edge.Source);
            };
        }

        public Action DisconnectOutPort(string portName, Edge edge) {
            OutPort port = Output.GetPort(portName);
            port.RemoveConnection(edge);
            return () => {
                if (port.ProcessDisconnection != null)
                    port.ProcessDisconnection.Invoke(edge.Target);
            };
        }

        public void DebugHighlight() {
            if (Visualisation != null)
                Visualisation.DebugHighlight(true);
        }

        public void DebugUnhighlight() {
            if (Visualisation != null)
                Visualisation.DebugHighlight(false);
        }

        public void StartProcessing() {
            Input.StartProcessing();
        }

        public bool HasMoreToProcess() {
            return Input.StateHasChanged();
        }

        public override string ToString() {
            return ComponentName;
        }

    }

}