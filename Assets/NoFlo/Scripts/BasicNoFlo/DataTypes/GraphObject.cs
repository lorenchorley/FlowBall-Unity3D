//using System;
//using System.Collections.Generic;

//namespace NoFlo_Basic {

//    public abstract class GraphObject : IGraphObject {

//        public string ID;

//        private List<InPort> Subscribers;
//        private Dictionary<Graph, List<Processable>> SubscribedComponentsByGraph;

//        public abstract string GetObjectType();

//        public GraphObject() {
//            Subscribers = new List<InPort>();
//            SubscribedComponentsByGraph = new Dictionary<Graph, List<Processable>>();
//        }

//        public string GetObjectID() {
//            return ID;
//        }

//        public void TriggerEvent(object Data) {
//            for (int i = 0; i < Subscribers.Count; i++) {
//                Subscribers[i].Accept(Data);
//            }
//            foreach (Graph Graph in SubscribedComponentsByGraph.Keys) {
//                Graph.CurrentExecutor.ContinueExecution(SubscribedComponentsByGraph[Graph]);
//            }
//        }

//        public void SubscribeToEvents(InPort InPort) {
//            if (Subscribers.Contains(InPort))
//                throw new Exception("TODO");

//            Subscribers.Add(InPort);

//            List<Processable> SubscriptionsByGraph;
//            if (!SubscribedComponentsByGraph.TryGetValue(InPort.Component.Graph, out SubscriptionsByGraph)) {
//                SubscriptionsByGraph = new List<Processable>();
//                SubscribedComponentsByGraph.Add(InPort.Component.Graph, SubscriptionsByGraph);
//            }

//            SubscriptionsByGraph.Add(InPort);
//        }

//        public void UnsubscribeFromEvents(InPort InPort) {
//            if (Subscribers.Contains(InPort)) {
//                Subscribers.Remove(InPort);

//                List<Processable> SubscriptionsByGraph = SubscribedComponentsByGraph[InPort.Component.Graph];
//                SubscriptionsByGraph.Remove(InPort);

//                if (SubscriptionsByGraph.Count == 0)
//                    SubscribedComponentsByGraph.Remove(InPort.Component.Graph);
//            }
//        }

//        public bool IsSubscribedToGraph(Graph Graph) {
//            return SubscribedComponentsByGraph.ContainsKey(Graph);
//        }

//        public void ForcablyUnsubscribeFromGraph(Graph Graph) {

//            if (SubscribedComponentsByGraph.ContainsKey(Graph)) {
//                List<Processable> SubscriptionsByGraph = SubscribedComponentsByGraph[Graph];
//                SubscribedComponentsByGraph.Remove(Graph);

//                for (int i = 0; i < SubscriptionsByGraph.Count; i++)
//                    Subscribers.Remove(SubscriptionsByGraph[i] as InPort);

//            }
//        }

//    }

//}