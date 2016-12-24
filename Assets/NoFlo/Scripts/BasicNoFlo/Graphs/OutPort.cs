using NoFloEditor;
using System;

namespace NoFlo_Basic {

    public class OutPort : Port {

        public Action<InPort> ProcessConnection;
        public Action<InPort> ProcessDisconnection;

        DebugMessageVisualisation currentMessage;

        public override bool IsInput() {
            return false;
        }

        //public void Send(object data) {
        //    for (int i = 0; i < Connections.Count; i++) {
        //        Connections[i].Target.Accept(data);
        //    }
        //}

        public void Send(object data, ExecutionContext context) {
            InPort inPort;
            for (int i = 0; i < Connections.Count; i++) {
                inPort = Connections[i].Target;
                inPort.Accept(data);
                context.QueueTask(this);
                context.QueueTask(inPort);
            }
        }

        public override void Process(ExecutionContext context) {
            //for (int i = 0; i < Connections.Count; i++) {
            //    targets.Add(Connections[i].Target);
            //}
        }

        public override void DebugHighlight() {
            for (int i = 0; i < Connections.Count; i++) {
                if (Connections[i].Visualisation != null)
                    Connections[i].Visualisation.Highlight(true);
            }
        }

        public override void DebugUnhighlight() {
            for (int i = 0; i < Connections.Count; i++) {
                if (Connections[i].Visualisation != null)
                    Connections[i].Visualisation.Highlight(false);
            }
        }

    }

}