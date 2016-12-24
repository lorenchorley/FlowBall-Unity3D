using System;

namespace NoFlo_Basic {

    [ComponentName("ButtonPress")]
    [ComponentPackage("systems")]
    public class ButtonPressEvent : Component {

        InPort ButtonPort;
        InPort ButtonEventsPort;
        OutPort OnPressPort;

        ButtonController button;

        public override void Process(ExecutionContext context) {

            // Deal with new button controllers
            if (ButtonPort.HasData()) {

                // Unsubscribe from the previous button if necessary
                if (button != null)
                    button.UnsubscribeFromEvents(ButtonEventsPort);

                // Get the button
                button = (ButtonController) ButtonPort.GetData();

                // Subscribe to the new button
                button.SubscribeToEvents(ButtonEventsPort);

            }

            // Send a kick for each button event received
            for (int i = 0; i < ButtonEventsPort.GetDataCount(); i++)
                Output.Send(OnPressPort, Kick.Instance, context);
            ButtonEventsPort.ClearData();

        }

        protected override void SetupInterfaces() {

            ButtonPort = Input.AddPort(new InPort() {
                Name = "Button",
                Description = "The button to be monitored",
                Types = new string[] { "Button Controller" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            ButtonEventsPort = Input.AddPort(new InPort() {
                Name = "ButtonEvents",
                Description = "This port receives events from the subscribed button",
                Types = new string[] { "Kick" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                Hidden = true,
            });

            OnPressPort = Output.AddPort(new OutPort() {
                Name = "OnPress",
                Description = "Sends a kick when the button is pressed",
                Types = new string[] { "Kick" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });

        }

    }

}