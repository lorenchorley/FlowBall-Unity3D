using System;

namespace NoFlo_Basic {

    [ComponentName("SplitString")]
    [ComponentPackage("core")]
    public class SplitString : Component {

        public override void Process(ExecutionContext context) {
            InPort firstPort = Input.GetPort("Delimiter");
            InPort secondPort = Input.GetPort("String");
            if (firstPort.HasData() && secondPort.HasData()) {
                string delimiter = firstPort.GetData().ToString();
                string str = secondPort.GetData().ToString();

                string[] strs = str.Split(new string[] { delimiter }, new StringSplitOptions());

                for (int i = 0; i < strs.Length; i++) {
                    Output.SendDone("Out", strs[i], context);
                }
            }
        }

        protected override void SetupInterfaces() {

            Input.AddPort(new InPort() {
                Name = "Delimiter",
                Description = "The delimiter to use to separate the string",
                Types = new string[] { "Text" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
            });
            Input.AddPort(new InPort() {
                Name = "String",
                Description = "The string to split",
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