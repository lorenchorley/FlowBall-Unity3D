using System;

namespace NoFlo_Basic {

    [ComponentName("DoorControls")]
    [ComponentPackage("systems")]
    public class DoorControls : Component {

        InPort DoorPort;
        InPort TogglePort;
        InPort DoorEventsPort;
        OutPort StatePort;
        DoorController Door;

        public override void Process(ExecutionContext context) {

            // Update the door controller
            if (DoorPort.HasData()) {
                if (Door != null)
                    Door.UnsubscribeFromEvents(DoorEventsPort);

                object data = DoorPort.GetData();
                if (data is DoorController)
                    Door = (DoorController) data;
                else
                    throw new Exception("TODO"); // TODO Send exception to graph for display within graph editor

                Door.SubscribeToEvents(DoorEventsPort);
            }

            // Toggle the doors state once if there are any instructions to do so
            if (TogglePort.HasData()) {
                TogglePort.ClearData();
                if (Door != null)
                    Door.Toggle();
            }

            // Send on any state change notifications
            while (DoorEventsPort.HasData()) {
                Output.Send(StatePort, DoorEventsPort.GetData(), context);
            }

            Output.Done(context);

        }

        protected override void SetupInterfaces() {

            DoorPort = Input.AddPort(new InPort() {
                Name = "Door",
                Description = "The controller of the door to be used",
                Types = new string[] { "Door Controller" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            TogglePort = Input.AddPort(new InPort() {
                Name = "Toggle",
                Description = "On receiving any input to this port, the door's state will be toggled",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            DoorEventsPort = Input.AddPort(new InPort() {
                Name = "DoorEventsPort",
                Description = "On receiving any input to this port, the door's state will be toggled",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                Hidden = true,
            });

            StatePort = Output.AddPort(new OutPort() {
                Name = "State",
                Description = "The state of the door is sent on when changed as Opened or Closed",
                Types = new string[] { "Text" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });

        }

    }

}