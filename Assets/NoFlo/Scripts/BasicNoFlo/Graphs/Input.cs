using System;
using System.Collections.Generic;

namespace NoFlo_Basic {

    public class ComponentInput {

        public Component Component;

        protected Dictionary<string, InPort> Ports;

        public ComponentInput() {
            Ports = new Dictionary<string, InPort>();
        }

        public InPort AddPort(InPort port) {
            port.Component = Component;
            Ports.Add(port.Name, port);
            return port;
        }

        public InPort GetPort(string name) {
            InPort port;

            if (!Ports.TryGetValue(name, out port))
                throw new Exception("Could not find port: " + name);

            return port;
        }

        public IEnumerable<InPort> GetPorts() {
            return Ports.Values;
        }

        public bool Has(string name) {
            return GetPort(name).HasData();
        }

        public object GetData(string name) {
            return GetPort(name).GetData();
        }

        int dataCount;
        public void StartProcessing() {
            UpdateDataCount();
        }

        public bool StateHasChanged() {
            return UpdateDataCount();
        }

        private bool UpdateDataCount() { // Returns true if changed
            int newDataCount = 0;
            foreach (InPort p in Ports.Values) {
                newDataCount += p.GetDataCount();
            }
            bool changed = (dataCount != newDataCount);
            dataCount = newDataCount;
            return changed;
        }

    }

}