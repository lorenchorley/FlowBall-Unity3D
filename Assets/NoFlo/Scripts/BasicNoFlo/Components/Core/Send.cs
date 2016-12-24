using System;

namespace NoFlo_Basic {

    [ComponentName("Send")]
    [ComponentPackage("core")]
    public class Send : Component {

        InPort ObjectPort;
        InPort SendPort;
        OutPort OutPort;

        public override void Process(ExecutionContext context) {
            if (ObjectPort.HasData()) {
                object data = ObjectPort.GetData();
                for (int i = 0; i < SendPort.GetDataCount(); i++) {
                    OutPort.Send(data, context);
                }
                SendPort.ClearData();
            }
            Output.Done(context);
        }

        protected override void SetupInterfaces() {

            ObjectPort = Input.AddPort(new InPort() {
                Name = "Object",
                Description = "The object to send",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                RememberOnlyLatest = true,
            });
            SendPort = Input.AddPort(new InPort() {
                Name = "Send",
                Description = "Upon receiving anything on this port, the object is sent on",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });

            OutPort = Output.AddPort(new OutPort() {
                Name = "Out",
                Description = "The sent object",
                Types = new string[] { "All" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });

        }

    }

}