using System;

namespace NoFlo_Basic {

    [ComponentName("CombineStrings")]
    [ComponentPackage("core")]
    public class CombineStrings : Component {

        public override void Process(ExecutionContext context) {
            InPort firstPort = Input.GetPort("First");
            InPort secondPort = Input.GetPort("Second");
            if (firstPort.HasData() && secondPort.HasData()) {
                object first = firstPort.GetData();
                object second = secondPort.GetData();
                Output.SendDone("Out", first.ToString() + second.ToString(), context);
            }
        }

        protected override void SetupInterfaces() {

            Input.AddPort(new InPort() {
                Name = "First",
                Description = "The string to output to the console",
                Types = new string[] { "Text" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            Input.AddPort(new InPort() {
                Name = "Second",
                Description = "The string to output to the console",
                Types = new string[] { "Text" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });

            Output.AddPort(new OutPort() {
                Name = "Out",
                Description = "The combination of the two inputed strings",
                Types = new string[] { "Text" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });

        }

    }

}