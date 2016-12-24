using NoFloEditor;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace NoFlo_Basic {

    public class Graph : MonoBehaviour {

        private static char[] characters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public bool RunOnStart = true;
        public TextAsset GraphFile;
        public GraphExecutor PrimaryExecutor;
        public GraphExecutor DebugExecutor;
        //public List<UnityGraphObject> ConnectedUnityObjects;
        //public List<GraphObject> ConnectedExternalObjects;
        public GraphInterlink AssociatedInterlink;

        public UnityEvent OnChangeExecutor;

        [NonSerialized]
        public GraphEditor GraphEditor;

        public Dictionary<string, Component> NodesByName;
        public Dictionary<InPort, DefaultValue> DefaultValuesByInPort;
        public List<Edge> Edges;

        public GraphExecutor CurrentExecutor { get; private set; }

        protected bool isInitialised = false;

        void Awake() {
            Init();
        }

        void Start() {
            if (PrimaryExecutor == null)
                throw new Exception("TODO");
            if (DebugExecutor == null)
                throw new Exception("TODO");

            if (AssociatedInterlink == null)
                AssociatedInterlink = GetComponent<GraphInterlink>();

            if (AssociatedInterlink != null)
                AssociatedInterlink.ValidateVariableConnections();

            CurrentExecutor = PrimaryExecutor;
            OnChangeExecutor.Invoke();

            if (RunOnStart)
                Run();

        }

        public void Init() {
            if (isInitialised)
                return;

            if (OnChangeExecutor == null)
                OnChangeExecutor = new UnityEvent();

            NodesByName = new Dictionary<string, Component>();
            DefaultValuesByInPort = new Dictionary<InPort, DefaultValue>();
            Edges = new List<Edge>();
            //ConnectedExternalObjects = new List<GraphObject>();
            //if (ConnectedUnityObjects == null)
            //    ConnectedUnityObjects = new List<UnityGraphObject>();
            LoadGraphFile();

            isInitialised = true;
        }

        public void EnableDebug() {
            if (CurrentExecutor.IsRunningGraph(this))
                throw new Exception("TODO");

            CurrentExecutor = DebugExecutor;
            OnChangeExecutor.Invoke();
        }

        public void DisableDebug() {
            if (CurrentExecutor.IsRunningGraph(this))
                throw new Exception("TODO");

            CurrentExecutor = PrimaryExecutor;
            OnChangeExecutor.Invoke();
        }

        public void LoadGraphFile() {

            // Populate graph with nodes, edges and default vaules
            JSONGraphFileReader reader = new JSONGraphFileReader(GraphFile.text);

            JSONGraphFileReader.RawNode rawNodeData;
            while (reader.NextNode(out rawNodeData)) {
                Type componentType;

                if (!ComponentCatalog.RequestComponentsByQualifiedName().TryGetValue(rawNodeData.QualifiedComponentName, out componentType))
                    throw new Exception("TODO");

                Component c = AddNode(rawNodeData.Name, componentType);
                c.MetadataPosition = rawNodeData.metadataPosition;

            }

            JSONGraphFileReader.RawEdge rawEdgeData;
            while (reader.NextEdge(out rawEdgeData)) {
                AddEdge(rawEdgeData.srcProcess, rawEdgeData.srcPort, rawEdgeData.tgtProcess, rawEdgeData.tgtPort);
            }

            JSONGraphFileReader.RawDefaultValue rawDefaultValueData;
            while (reader.NextDefaultValue(out rawDefaultValueData)) {
                object treatedData = DataTreatment.TreatData(rawDefaultValueData.Data, this);
                if (treatedData == null)
                    continue;

                AddDefaultValue(treatedData, rawDefaultValueData.tgtProcess, rawDefaultValueData.tgtPort);
            }

        }

        public void Run() {
            CurrentExecutor.StartExecution();
            CurrentExecutor.ExecuteGraph(this);
        }

        public void TryToStop() {

            if (HasSubscriptions())
                return;

            _Stop();
        }

        public void ForceStop() {

            // Forcably unsubscribe all external components
            ForcablyUnsubscribeAll();

            _Stop();
        }

        private void _Stop() {
            CurrentExecutor.StopExecution();
        }

        public void ForcablyUnsubscribeAll() {
            AssociatedInterlink.ForcablyUnsubscribeAll(this);
        }

        public void Reset() {
            NodesByName.Clear();
            DefaultValuesByInPort.Clear();
            Edges.Clear();
        }

        //public bool ForcablyUnsubscribeAll() {
        //    //for (int i = 0; i < ConnectedExternalObjects.Count; i++) {
        //    //    ConnectedExternalObjects[i].ForcablyUnsubscribeFromGraph(this);
        //    //}
        //    for (int i = 0; i < ConnectedUnityObjects.Count; i++) {
        //        ConnectedUnityObjects[i].ForcablyUnsubscribeFromGraph(this);
        //    }
        //    return false;
        //}

        public bool HasSubscriptions() {
            return AssociatedInterlink.HasSubscriptions(this);
        }

        public bool HasDelayedExecution() {
            return gameObject.GetComponents<DelayedCallback>().Length > 0; // TODO find more efficient way to do this
        }

        //public bool HasSubscriptions() {
        //    //for (int i = 0; i < ConnectedExternalObjects.Count; i++) {
        //    //    if (ConnectedExternalObjects[i].IsSubscribedToGraph(this))
        //    //        return true;
        //    //}
        //    for (int i = 0; i < ConnectedUnityObjects.Count; i++) {
        //        if (ConnectedUnityObjects[i].IsSubscribedToGraph(this))
        //            return true;
        //    }
        //    return false;
        //}

        //public void RegisterExternalObject(GraphObject Object) {
        //    if (ConnectedExternalObjects.Contains(Object))
        //        throw new Exception("TODO");

        //    ConnectedExternalObjects.Add(Object);
        //}

        //public void UnregisterExternalObject(GraphObject Object) {
        //    if (!ConnectedExternalObjects.Contains(Object))
        //        throw new Exception("TODO");

        //    ConnectedExternalObjects.Remove(Object);
        //}

        public C AddNode<C>(string Name) where C : Component {
            C component = (C) Activator.CreateInstance(typeof(C));
            NodesByName.Add(Name, component);
            component.SetName(Name);
            component.SetGraph(this);

            if (GraphEditor != null)
                GraphEditor.EventOptions.NodeAdded.Invoke(component);

            return component;
        }

        public Component AddNode(string Name, Type ComponentType) {
            Component component = (Component) Activator.CreateInstance(ComponentType);
            NodesByName.Add(Name, component);
            component.SetName(Name);
            component.SetGraph(this);

            if (GraphEditor != null)
                GraphEditor.EventOptions.NodeAdded.Invoke(component);

            return component;
        }

        public Component AddNode(Type ComponentType) {
            string name = ComponentType.Name + "_" + GenerateRandomString(5);

            while (NodesByName.ContainsKey(name)) {
                name = ComponentType.Name + "_" + GenerateRandomString(5);
            }

            return AddNode(name, ComponentType);
        }

        private void CheckDeletedNodesPortsForDefaultValues(Component node) {

            foreach (InPort p in node.Input.GetPorts())
                if (DefaultValuesByInPort.ContainsKey(p))
                    DefaultValuesByInPort.Remove(p);

        }

        private string GenerateRandomString(int len) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++) {
                sb.Append(characters[UnityEngine.Random.Range(0, characters.Length)]);
            }
            return sb.ToString();
        }

        public void RemoveNode(Component Node) {
            NodesByName.Remove(Node.ComponentName);

            if (GraphEditor != null)
                GraphEditor.EventOptions.NodeRemoved.Invoke(Node);

            RemoveEdgesByNode(Node);

            CheckDeletedNodesPortsForDefaultValues(Node);

        }

        public void RemoveNode(string name) {
            Component Node;

            if (!NodesByName.TryGetValue(name, out Node))
                throw new Exception("");

            NodesByName.Remove(name);

            if (GraphEditor != null)
                GraphEditor.EventOptions.NodeRemoved.Invoke(Node);

            RemoveEdgesByNode(Node);

            CheckDeletedNodesPortsForDefaultValues(Node);

        }

        private void RemoveEdgesByNode(Component Node) {
            foreach (Port p in Node.Input.GetPorts()) {
                p.RemoveAllConnections();
            }
            foreach (Port p in Node.Output.GetPorts()) {
                p.RemoveAllConnections();
            }
        }

        public void AddDefaultValue(object Data, string NodeName, string PortName) {
            Component Component;
            if (!NodesByName.TryGetValue(NodeName, out Component))
                throw new Exception("Could not find node: " + NodeName);

            InPort Port = Component.Input.GetPort(PortName);

            AddDefaultValue(Data, Port);
        }

        public void AddDefaultValue(object Data, InPort Port) {

            DefaultValue defaultValue = new DefaultValue() {
                Data = Data,
                Component = Port.Component,
                Port = Port
            };

            if (DefaultValuesByInPort.ContainsKey(Port))
                throw new Exception("");

            DefaultValuesByInPort.Add(Port, defaultValue);

            if (GraphEditor != null)
                GraphEditor.EventOptions.DefaultValueAdded.Invoke(defaultValue);

        }

        public void RemoveDefaultValue(string NodeName, string PortName) {
            Component Component;
            if (!NodesByName.TryGetValue(NodeName, out Component))
                throw new Exception("");

            InPort Port = Component.Input.GetPort(PortName);

            RemoveDefaultValue(Port);
        }

        public void RemoveDefaultValue(InPort Port) {

            DefaultValue defaultValue;
            if (!DefaultValuesByInPort.TryGetValue(Port, out defaultValue))
                throw new Exception("TODO");

            DefaultValuesByInPort.Remove(Port);

            if (GraphEditor != null)
                GraphEditor.EventOptions.DefaultValueRemoved.Invoke(defaultValue);
        }

        public void SetDefaultValue(object Data, InPort Port) {
            DefaultValue defaultValue;
            if (DefaultValuesByInPort.TryGetValue(Port, out defaultValue)) {
                DefaultValuesByInPort.Remove(Port);

                if (GraphEditor != null)
                    GraphEditor.EventOptions.DefaultValueRemoved.Invoke(defaultValue);
            }

            AddDefaultValue(Data, Port);
        }

        public void AddEdge(string sendingNodeName, string sendingPortName, string receivingNodeName, string receivingPortName) {
            Component SendingNode, ReceivingNode;

            if (!NodesByName.TryGetValue(sendingNodeName, out SendingNode))
                throw new Exception("Could not find sending node for edge: " + sendingNodeName);

            if (!NodesByName.TryGetValue(receivingNodeName, out ReceivingNode))
                throw new Exception("TODO");

            AddEdge(SendingNode, sendingPortName, ReceivingNode, receivingPortName);
        }

        public void AddEdge(Component SendingNode, string sendingPortName, Component ReceivingNode, string receivingPortName) {
            Edge edge = new Edge();

            Action sendingAction = SendingNode.ConnectToOutPort(sendingPortName, edge);
            Action receivingAction = ReceivingNode.ConnectToInPort(receivingPortName, edge);

            if (Edges.Contains(edge))
                throw new Exception("");

            Edges.Add(edge);

            sendingAction.Invoke();
            receivingAction.Invoke();

            if (GraphEditor != null)
                GraphEditor.EventOptions.EdgeAdded.Invoke(edge);

        }

        public void RemoveEdge(string sourceComponentName, string sourcePortName, string targetComponentName, string targetPortName) {
            Component source, target;

            if (!NodesByName.TryGetValue(sourceComponentName, out source))
                throw new Exception("TODO");

            if (!NodesByName.TryGetValue(targetComponentName, out target))
                throw new Exception("TODO");

            RemoveEdge(new Edge() {
                Source = source.Output.GetPort(sourcePortName),
                Target = target.Input.GetPort(targetPortName)
            });
        }

        public void RemoveEdge(Component source, string sourcePortName, Component target, string targetPortName) {
            RemoveEdge(new Edge() {
                Source = source.Output.GetPort(sourcePortName),
                Target = target.Input.GetPort(targetPortName)
            });
        }

        public void RemoveEdge(Edge edge) {
            if (!Edges.Contains(edge))
                throw new Exception("");

            edge.Source.RemoveConnection(edge);
            edge.Target.RemoveConnection(edge);

            Edges.Remove(edge);

            if (GraphEditor != null)
                GraphEditor.EventOptions.EdgeRemoved.Invoke(edge);

        }

        public void AddInitial(string nodeName, string portName, object data, HashSet<Component> Targets) {
            Component Node;

            if (!NodesByName.TryGetValue(nodeName, out Node))
                throw new Exception("TODO");

            Node.AcceptDataOnPort(portName, data, Targets);
        }

        public void AddData(InPort port, object data) {
            port.Accept(data);
        }

        public bool IsRunning() {
            return CurrentExecutor.IsRunningGraph(this);
        }

        public bool InDebugMode() {
            return CurrentExecutor == DebugExecutor;
        }

    }

}