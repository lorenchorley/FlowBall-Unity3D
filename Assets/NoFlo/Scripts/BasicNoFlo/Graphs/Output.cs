using System;
using System.Collections.Generic;

namespace NoFlo_Basic {

    public class ComponentOutput {

        public Component Component;

        private Dictionary<string, OutPort> Ports;
        //private HashSet<Component> Targets;

        public ComponentOutput() {
            Ports = new Dictionary<string, OutPort>();
            //Targets = new HashSet<Component>();
        }

        public OutPort AddPort(OutPort port) {
            port.Component = Component;
            Ports.Add(port.Name, port);
            return port;
        }

        public OutPort GetPort(string name) {
            OutPort port;

            if (!Ports.TryGetValue(name, out port))
                throw new Exception("Unknown out port name \"" + name + "\" in node \"" + Component.ComponentName + "\"");

            return port;
        }

        public IEnumerable<OutPort> GetPorts() {
            return Ports.Values;
        }

        public void SendDone(string portName, object data, ExecutionContext context) {
            GetPort(portName).Send(data, context);
            Done(context);
        }

        public void Send(OutPort port, object data, ExecutionContext context) {
            port.Send(data, context);
        }

        public void Send(string portName, object data, ExecutionContext context) {
            GetPort(portName).Send(data, context);
        }

        public void Done(ExecutionContext context) {

            // Process targets TODO Move this to manager to do a breadth first search instead of the depth first here
            //foreach (Component target in Targets) {
            //    target.Process();
            //}
            //Targets.Clear();

        }

    }

}