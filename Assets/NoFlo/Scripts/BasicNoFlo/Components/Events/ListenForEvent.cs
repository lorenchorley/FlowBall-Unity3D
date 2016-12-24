using System;

namespace NoFlo_Basic {

    [ComponentName("ListenForEvent")]
    [ComponentPackage("events")]
    public class ListenForEvent : Component {

        InPort EventNamePort;
        InPort EventPort;
        OutPort OutPort;

        string currentEventName = null;

        public override void Process(ExecutionContext context) {
            if (EventNamePort.HasData()) {
                string eventName = EventNamePort.GetData() as string;

                if (eventName != currentEventName) {
                    if (currentEventName != null)
                        Graph.AssociatedInterlink.UnsubscribeFromEvent(currentEventName, EventPort);

                    currentEventName = eventName;

                    Graph.AssociatedInterlink.SubscribeToEvent(currentEventName, EventPort);
                }

                while (EventPort.HasData()) {
                    object data = EventPort.GetData();
                    OutPort.Send(data, context);
                }
            }
            Output.Done(context);
        }

        protected override void SetupInterfaces() {

            EventNamePort = Input.AddPort(new InPort() {
                Name = "EventName",
                Description = "The name of the event to listen for",
                Types = new string[] { "Text" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                RememberOnlyLatest = true
            });
            EventPort = Input.AddPort(new InPort() {
                Name = "Event",
                Description = "The data that was sent with the event",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                Hidden = true
            });

            OutPort = Output.AddPort(new OutPort() {
                Name = "Out",
                Description = "Sends event data when an event of the given name is received",
                Types = new string[] { "Text" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });


        }

    }

}