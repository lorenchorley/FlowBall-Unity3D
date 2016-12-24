using System;
using System.Collections;
using System.Collections.Generic;

namespace NoFlo_Basic {

    [ComponentName("Delay")]
    [ComponentPackage("events")]
    public class Delay : Component {

        InPort DelayPort;
        InPort InPort;
        OutPort OutPort;

        public override void Process(ExecutionContext context) {
            if (DelayPort.HasData()) {
                string delayString = DelayPort.GetData() as string;

                if (delayString == null) {
                    Output.Done(context);
                    return;
                }
                    
                float delay = float.Parse(delayString);

                // Collect data in local queue
                Queue data = new Queue();
                HashSet<Processable> targets = new HashSet<Processable>();
                while (InPort.HasData()) {
                    data.Enqueue(InPort.GetData());
                }

                Graph.gameObject.AddComponent<DelayedCallback>().SetTimedCallback(() => { // TODO Upon stopping a graph, remove all delayed callback components
                    while (data.Count > 0)
                        Output.Send(OutPort, data.Dequeue(), Graph.CurrentExecutor.GetExecutionContext());
                    Output.Done(context);
                    Graph.CurrentExecutor.ContinueExecution(this);
                }, delay);

            }
            Output.Done(context);
        }

        protected override void SetupInterfaces() {

            DelayPort = Input.AddPort(new InPort() {
                Name = "Delay",
                Description = "The period to wait before sending messages on",
                Types = new string[] { "Number" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                },
                RememberOnlyLatest = true
            });

            InPort = Input.AddPort(new InPort() {
                Name = "In",
                Description = "The data that will be sent on",
                Types = new string[] { "All" },
                ProcessConnection = (OutPort otherPort) => {
                },
                ProcessDisconnection = (OutPort otherPort) => {
                }
            });

            OutPort = Output.AddPort(new OutPort() {
                Name = "Out",
                Description = "The delayed data",
                Types = new string[] { "All" },
                ProcessConnection = (InPort otherPort) => {
                },
                ProcessDisconnection = (InPort otherPort) => {
                },
            });


        }

    }

}