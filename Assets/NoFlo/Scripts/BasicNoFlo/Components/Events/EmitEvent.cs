using System;

namespace NoFlo_Basic {

    [ComponentName("EmitEvent")]
    [ComponentPackage("events")]
    public class EmitEvent : Component {

        InPort EventNamePort;
        InPort DataPort;

        public override void Process(ExecutionContext context) {
            if (EventNamePort.HasData()) {
                string eventName = EventNamePort.GetData() as string;
                while (DataPort.HasData()) {
                    object data = DataPort.GetData();
                    Graph.AssociatedInterlink.EmitEvent(eventName, data);
                }
            }
            Output.Done(context);
        }

        protected override void SetupInterfaces() {

            EventNamePort = Input.AddPort(new InPort() {
                Name = "EventName",
                Description = "The name of the event",
                Types = new string[] { "Text" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                RememberOnlyLatest = true
            });
            DataPort = Input.AddPort(new InPort() {
                Name = "Data",
                Description = "The data to send with the event",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            
        }

    }

}