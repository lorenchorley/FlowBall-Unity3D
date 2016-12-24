using System;

namespace NoFlo_Basic {

    [ComponentName("LiftControls")]
    [ComponentPackage("systems")]
    public class LiftControls : Component {

        InPort LiftPort;
        InPort TogglePort;
        InPort LiftEventsPort;
        OutPort StatePort;
        LiftController Lift;

        public override void Process(ExecutionContext context) {

            // Update the lift controller
            if (LiftPort.HasData()) {
                if (Lift != null)
                    Lift.UnsubscribeFromEvents(LiftEventsPort);

                object data = LiftPort.GetData();
                if (data is LiftController)
                    Lift = (LiftController) data;
                else
                    throw new Exception("TODO"); // TODO Send exception to graph for display within graph editor

                Lift.SubscribeToEvents(LiftEventsPort);
            }

            // Toggle the lifts state once if there are any instructions to do so
            if (TogglePort.HasData()) {
                TogglePort.ClearData();
                if (Lift != null)
                    Lift.Toggle();
            }

            // Send on any state change notifications
            while (LiftEventsPort.HasData()) {
                Output.Send(StatePort, LiftEventsPort.GetData(), context);
            }

            Output.Done(context);

        }

        protected override void SetupInterfaces() {

            LiftPort = Input.AddPort(new InPort() {
                Name = "Lift",
                Description = "The controller of the lift to be used",
                Types = new string[] { "Lift Controller" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            TogglePort = Input.AddPort(new InPort() {
                Name = "Toggle",
                Description = "On receiving any input to this port, the lift's state will be toggled",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            LiftEventsPort = Input.AddPort(new InPort() {
                Name = "LiftEventsPort",
                Description = "On receiving any input to this port, the lift's state will be toggled",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                Hidden = true,
            });

            StatePort = Output.AddPort(new OutPort() {
                Name = "State",
                Description = "The state of the lift is sent on when changed as Raised or Lowered",
                Types = new string[] { "Text" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });

        }

    }

}