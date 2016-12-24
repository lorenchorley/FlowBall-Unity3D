using geniikw;
using NoFlo_Basic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NoFloEditor { 

    public class GraphEditor : MonoBehaviour {

        [NonSerialized]
        public Graph CurrentGraph;

        public bool StartClosed = true;
        public bool isOpen { get; private set; }
        public PlayerController PlayerController;

        public RectTransform GraphRenderingPanel;

        [Serializable]
        public class TemplateContainer {
            public GameObject NodeTemplate;
            public GameObject PortTemplate;
            public GameObject EdgeTemplate;
            public GameObject DebugMessageTemplate;
            public GameObject DefaultValueTemplate;
        }
        public TemplateContainer Templates;

        [Serializable]
        public class ButtonsContainer {
            public AddNodeButton AddNode;
            public RemoveButton Remove;
            public AutoLayoutButton AutoLayout;
            public RunButton Run;
            public DebugButton Debug;
        }
        public ButtonsContainer Buttons;

        [Serializable]
        public class LayoutOptions {
            public float HorizontalSeparation = 250;
            public float VerticalSeparation = 120;
        }
        public LayoutOptions Layout;

        public NodeInfoDialog NodeInfoDialog;

        public float RemoveEdgeDraggingDistance = 20;

        public UnityEvent StartedDragging;
        public UnityEvent EndedDragging;

        [Serializable]
        public class UnityEventNode : UnityEvent<NoFlo_Basic.Component> { }
        [Serializable]
        public class UnityEventDefaultValue : UnityEvent<DefaultValue> { }
        [Serializable]
        public class UnityEventEdge : UnityEvent<Edge> { }
        [Serializable]
        public class UnityEventDataAdded : UnityEvent<InPort, object> { }
        [Serializable]
        public class UnityEventDataRemove : UnityEvent<InPort> { }

        [Serializable]
        public class UIEvents {
            public UnityEventNode NodeAdded;
            public UnityEventNode NodeRemoved;
            public UnityEventDefaultValue DefaultValueAdded;
            public UnityEventDefaultValue DefaultValueRemoved;
            public UnityEventEdge EdgeAdded;
            public UnityEventEdge EdgeRemoved;
            public UnityEventDataAdded DataAdded;
            public UnityEventDataRemove DataRemoved;
            public UnityEvent EditorOpened;
            public UnityEvent EditorClosed;
        }
        public UIEvents EventOptions;

        public void Open(Graph Graph) {
            Init();

            if (isOpen)
                throw new Exception("TODO");

            StartClosed = false;

            CurrentGraph = Graph;
            Graph.GraphEditor = this;
            Graph.Init();

            Buttons.AddNode.SetGraph(Graph);
            Buttons.Remove.SetGraph(Graph);
            Buttons.AutoLayout.SetGraph(Graph);
            Buttons.Run.SetGraph(Graph);
            Buttons.Debug.SetGraph(Graph);

            Graph.PrimaryExecutor.OnStart.AddListener(HandleStartExecution);
            Graph.PrimaryExecutor.OnStop.AddListener(HandleEndExecution);
            Graph.DebugExecutor.OnStart.AddListener(HandleStartExecution);
            Graph.DebugExecutor.OnStop.AddListener(HandleEndExecution);

            RenderGraph(Graph, GraphRenderingPanel);

            GraphRenderingPanel.anchoredPosition = Vector2.zero;

            isOpen = true;
            gameObject.SetActive(true);

            if (EventOptions.EditorOpened != null)
                EventOptions.EditorOpened.Invoke();
        }

        public void Close() {
            if (!isOpen)
                throw new Exception("TODO");

            Deselect();
            ResetEditorPanel();
            isOpen = false;
            gameObject.SetActive(false);

            if (EventOptions.EditorClosed != null)
                EventOptions.EditorClosed.Invoke();
        }

        public void ResetEditorPanel() {
            foreach (Transform t in GraphRenderingPanel) {
                Destroy(t.gameObject);
            }
        }

        public void Toggle() {
            gameObject.SetActive(!GraphRenderingPanel.gameObject.activeSelf);
        }

        public void SetupEventHandlers() {

            if (EventOptions.NodeAdded == null)
                EventOptions.NodeAdded = new UnityEventNode();

            if (EventOptions.NodeRemoved == null)
                EventOptions.NodeRemoved = new UnityEventNode();

            if (EventOptions.DefaultValueAdded == null)
                EventOptions.DefaultValueAdded = new UnityEventDefaultValue();

            if (EventOptions.DefaultValueRemoved == null)
                EventOptions.DefaultValueRemoved = new UnityEventDefaultValue();

            if (EventOptions.EdgeAdded == null)
                EventOptions.EdgeAdded = new UnityEventEdge();

            if (EventOptions.EdgeRemoved == null)
                EventOptions.EdgeRemoved = new UnityEventEdge();

            if (EventOptions.DataAdded == null)
                EventOptions.DataAdded = new UnityEventDataAdded();

            if (EventOptions.DataRemoved == null)
                EventOptions.DataRemoved = new UnityEventDataRemove();

            if (EventOptions.EditorOpened == null)
                EventOptions.EditorOpened = new UnityEvent();

            if (EventOptions.EditorClosed == null)
                EventOptions.EditorClosed = new UnityEvent();

            EventOptions.NodeAdded.AddListener(HandleAddNode);
            EventOptions.NodeRemoved.AddListener(HandleRemoveNode);
            EventOptions.DefaultValueRemoved.AddListener(HandleRemoveDefaultValue);
            EventOptions.DefaultValueAdded.AddListener(HandleAddDefaultValue);
            EventOptions.EdgeRemoved.AddListener(HandleRemoveEdge);
            EventOptions.EdgeAdded.AddListener(HandleAddEdge);
            EventOptions.DataRemoved.AddListener(HandleRemoveData);
            EventOptions.DataAdded.AddListener(HandleAddData);

        }

        public void HandleStartExecution() {
            foreach (DefaultValue dv in CurrentGraph.DefaultValuesByInPort.Values) {
                if (dv.Visualisation != null)
                    dv.Visualisation.gameObject.SetActive(false);
            }
        }

        public void HandleEndExecution() {
            foreach (DefaultValue dv in CurrentGraph.DefaultValuesByInPort.Values) {
                if (dv.Visualisation != null)
                    dv.Visualisation.gameObject.SetActive(true);
            }
            foreach (NoFlo_Basic.Component c in CurrentGraph.NodesByName.Values) {
                foreach (InPort p in c.Input.GetPorts()) {
                    if (p.currentMessage != null)
                        p.currentMessage.gameObject.SetActive(false);
                }
            }
        }

        public void HandleAddNode(NoFlo_Basic.Component component) {
            GameObject newNode = Instantiate<GameObject>(Templates.NodeTemplate);
            newNode.transform.SetParent(GraphRenderingPanel);

            NodeVisualisation v = newNode.GetComponent<NodeVisualisation>();
            v.NodeName = component.ComponentName;
            v.Graph = CurrentGraph;
            v.gameObject.GetComponentInChildren<ComponentNameVisualisation>().Component = component;
            component.Visualisation = v;
            v.SetupWithComponent(component);

            v.transform.localPosition = v.Component.MetadataPosition;
        }

        public void HandleRemoveNode(NoFlo_Basic.Component component) {
            if (component.Visualisation != null)
                Destroy(component.Visualisation.gameObject);
        }

        [NonSerialized]
        public LinePointManager Manager;

        public void UpdateEdges() {
            if (Manager != null)
                Manager.UpdateLPs();
        }

        public void HandleAddDefaultValue(DefaultValue DefaultValue) {
            DefaultValueVisualisation v = GameObject.Instantiate<GameObject>(Templates.DefaultValueTemplate).GetComponent<DefaultValueVisualisation>();
            v.Graph = CurrentGraph;
            v.SetupWithDefaultValue(DefaultValue);
            DefaultValue.Visualisation = v;

            if (!CurrentGraph.CurrentExecutor.IsStopped()) {
                v.gameObject.SetActive(false);
            }

        }

        public void HandleRemoveDefaultValue(DefaultValue DefaultValue) {
            if (DefaultValue.Visualisation != null)
                Destroy(DefaultValue.Visualisation.gameObject);
        }

        public void HandleAddEdge(Edge edge) {
            GameObject newEdge = Instantiate<GameObject>(Templates.EdgeTemplate);
            newEdge.transform.SetParent(GraphRenderingPanel);

            EdgeVisualisation v = newEdge.GetComponent<EdgeVisualisation>();
            edge.Visualisation = v;

            v.SetupWithEdge(edge, Manager);
            v.Graph = CurrentGraph;
            v.transform.SetAsFirstSibling();

        }

        public void HandleRemoveEdge(Edge edge) {
            if (edge.Visualisation != null)
                Destroy(edge.Visualisation.gameObject);
        }

        public void HandleAddData(InPort port, object Data) {
            if (port.Hidden)
                return;

            if (port.currentMessage == null) {
                port.currentMessage = GameObject.Instantiate<GameObject>(Templates.DebugMessageTemplate).GetComponent<DebugMessageVisualisation>();
                if (port.Visualisation != null)
                    port.currentMessage.Setup(port.Visualisation.transform); // TODO fix InvalidOperationException
                else
                    port.currentMessage.gameObject.SetActive(false);
            }
            if (!port.currentMessage.gameObject.activeSelf)
                port.currentMessage.gameObject.SetActive(true);

            port.currentMessage.SetMessage(Data.ToString());
            port.currentMessage.SetQuantity(port.GetDataCount());

        }

        public void HandleRemoveData(InPort port) {
            if (port.Hidden)
                return;

            if (port.currentMessage != null)
                port.currentMessage.gameObject.SetActive(false);
        }

        bool isDragging;
        Vector3 StartingDragOffset;
        Vector3 StartingPosition;
        Action draggingAction;
        Action endDraggingAction;
        Visualisation v;

        EventSystem EventSystem;
        List<RaycastResult> results;
        PointerEventData data;

        [NonSerialized]
        public Visualisation selected;

        public void RenderGraph(Graph graph, Transform obj) {

            if (graph.GraphFile == null)
                throw new Exception("TODO");
        
            foreach (NoFlo_Basic.Component n in graph.NodesByName.Values) {
                HandleAddNode(n);
            }

            foreach (Edge e in graph.Edges) {
                HandleAddEdge(e);
            }

            foreach (DefaultValue dv in graph.DefaultValuesByInPort.Values) {
                HandleAddDefaultValue(dv);
            }

            //gameObject.AddComponent<NextFrameCallback>().SetCallback(() => {
            //    UpdateEdges();
            //});

        }

        Transform tmpMousePosition;
        UIMeshLine tmpLine;
        private void PortStartDraggingAction() {

            Deselect();

            tmpLine = GameObject.Instantiate<GameObject>(Templates.EdgeTemplate).GetComponent<UIMeshLine>();
            tmpLine.transform.SetParent(GraphRenderingPanel);

            (v as PortVisualisation).gameObject.AddComponent<LinePointUpdater>().Setup(tmpLine, 0, Vector3.zero, tmpLine.transform, Manager);

            tmpMousePosition = new GameObject("Mouse Position").transform;
            tmpMousePosition.gameObject.AddComponent<LinePointUpdater>().Setup(tmpLine, 1, Vector3.zero, tmpLine.transform, Manager);
            tmpMousePosition.SetParent(GraphRenderingPanel);
            tmpMousePosition.gameObject.AddComponent<MouseTracker>();


        }

        private void PortDraggingAction() {

        }

        private bool PortTypesAreCompatible(string[] startPortTypes, string[] endPortTypes) {
            string endType;
            for (int i = 0; i < endPortTypes.Length; i++) {
                endType = endPortTypes[i];
                if (endType == "All")
                    return true;

                for (int j = 0; j < startPortTypes.Length; j++) {
                    if (startPortTypes[i] == endType)
                        return true;
                }
            }
            return false;
        }

        private void PortEndDraggingAction() {
            Destroy(tmpMousePosition.gameObject);
            Destroy(tmpLine.gameObject);

            Raycast();
            for (int i = 0; i < results.Count; i++) {
                PortVisualisation EndPort = results[i].gameObject.GetComponent<PortVisualisation>();
                if (EndPort != null) {
                    PortVisualisation StartPort = v as PortVisualisation;

                    if (StartPort.Port.GetType() == EndPort.Port.GetType()) {
                        Debug.Log("Ports are not compatible (Either both in ports or both out ports)");
                        return;
                    }

                    if (!PortTypesAreCompatible(StartPort.Port.Types, EndPort.Port.Types)) {
                        Debug.Log("Port data types are incompatible");
                        return;
                    }

                    if (StartPort.Port is OutPort)
                        v.Graph.AddEdge(StartPort.Port.Component, StartPort.Port.Name, EndPort.Port.Component, EndPort.Port.Name);
                    else
                        v.Graph.AddEdge(EndPort.Port.Component, EndPort.Port.Name, StartPort.Port.Component, StartPort.Port.Name);
                }
            }
        }



        private void EdgeStartDraggingAction() {
            StartingDragOffset = UnityEngine.Input.mousePosition; // Just the position
        
            Deselect();

        }

        private void EdgeDraggingAction() {
            float mag = (StartingDragOffset - UnityEngine.Input.mousePosition).magnitude;
            if (mag > RemoveEdgeDraggingDistance) {
                v.gameObject.SetActive(false);
            } else {
                v.gameObject.SetActive(true);
            }
        }

        private void EdgeEndDraggingAction() {
            if (!v.gameObject.activeSelf)
                v.Graph.RemoveEdge((v as EdgeVisualisation).Edge);
        }



        private void NodeStartDraggingAction() {
            if (v != selected)
                Deselect();

            if (v is ComponentNameVisualisation) {
                StartingDragOffset = (v as ComponentNameVisualisation).Component.Visualisation.transform.position - UnityEngine.Input.mousePosition;
            } else {
                StartingDragOffset = v.transform.position - UnityEngine.Input.mousePosition;
                StartingPosition = UnityEngine.Input.mousePosition;
            }
        }

        private void NodeDraggingAction() {
            Vector3 potentialNewPosition = UnityEngine.Input.mousePosition + StartingDragOffset;
            if (v is ComponentNameVisualisation) {
                if (((v as ComponentNameVisualisation).Component.Visualisation.transform.position - potentialNewPosition).magnitude > 0.5f) {
                    v = (v as ComponentNameVisualisation).Component.Visualisation;
                }
            }

            if (v is NodeVisualisation) { 
                v.transform.position = potentialNewPosition;
                UpdateEdges();
            }
        }

        private void NodeEndDraggingAction() {
            if (v is ComponentNameVisualisation) {
                (v as ComponentNameVisualisation).StartEdit();
            } else if (v is NodeVisualisation) {
                if ((StartingPosition - UnityEngine.Input.mousePosition).magnitude < 0.5f) {
                    Select(v);
                }
                (v as NodeVisualisation).Component.MetadataPosition = v.transform.localPosition;
            }
        }

        void Awake() {
            Init();
        }

        private bool initialised = false;
        public void Init() {
            if (initialised)
                return;

            initialised = true;

            isOpen = !StartClosed;
            gameObject.SetActive(isOpen);

            SetupEventHandlers();

            Buttons.AddNode.GraphEditor = this;
            Buttons.Remove.GraphEditor = this;
            Buttons.AutoLayout.GraphEditor = this;
            Buttons.Run.GraphEditor = this;
            Buttons.Debug.GraphEditor = this;

            if (StartedDragging == null)
                StartedDragging = new UnityEvent();

            if (EndedDragging == null)
                EndedDragging = new UnityEvent();

            results = new List<RaycastResult>();
            EventSystem = GameObject.FindObjectOfType<EventSystem>();
            isDragging = false;

            if (Manager == null)
                Manager = gameObject.AddComponent<LinePointManager>();

        }

        void Update() {

            if (isDragging) {

                draggingAction.Invoke();

                if (UnityEngine.Input.GetMouseButtonUp(0)) {

                    EndDragging();
                }

            } else {

                if (UnityEngine.Input.GetMouseButtonDown(0)) {
                    bool foundViewport = false;
                    Raycast();

                    // Initial pass to check for forbidden areas
                    for (int i = 0; i < results.Count; i++) {
                        if (results[i].gameObject.name == "NodeInfoDialog") {
                            return;
                        }
                    }

                    for (int i = 0; i < results.Count; i++) {
                        v = results[i].gameObject.GetComponent<Visualisation>();
                        if (v != null) {

                            if (v is PortVisualisation) {

                                PortStartDraggingAction();
                                draggingAction = PortDraggingAction;
                                endDraggingAction = PortEndDraggingAction;

                                StartDragging();
                                break;
                            } else if (v is EdgeVisualisation) {

                                EdgeStartDraggingAction();
                                draggingAction = EdgeDraggingAction;
                                endDraggingAction = EdgeEndDraggingAction;

                                StartDragging();
                                break;
                            } else if (v is ComponentNameVisualisation) {

                                NodeStartDraggingAction();
                                draggingAction = NodeDraggingAction;
                                endDraggingAction = NodeEndDraggingAction;

                                StartDragging();
                                break;
                            } else if (v is NodeVisualisation) {

                                NodeStartDraggingAction();
                                draggingAction = NodeDraggingAction;
                                endDraggingAction = NodeEndDraggingAction;
                            
                                StartDragging();
                                break;
                            }

                        } else if (results[i].gameObject.name == "Viewport") {
                            foundViewport = true;
                        }

                    }

                    if (foundViewport)
                        Deselect();

                }

            }
        }

        private void Raycast() {
            data = new PointerEventData(null);
            data.position = UnityEngine.Input.mousePosition;
            results.Clear();
            EventSystem.RaycastAll(data, results);
        }

        private void StartDragging() {
            isDragging = true;
            StartedDragging.Invoke();
        }

        private void EndDragging() {
            endDraggingAction.Invoke();
            isDragging = false;
            EndedDragging.Invoke();
        }

        public void Select(Visualisation obj) {
            obj.Select();
            selected = obj;

            if (obj is NodeVisualisation) {
                NodeInfoDialog.GraphEditor = this;
                NodeInfoDialog.gameObject.SetActive(true);
                NodeInfoDialog.SetNode((obj as NodeVisualisation).Component);
            }

        }

        public void Deselect() {
            if (selected != null) {
                selected.Deselect();
                selected = null;
            }

            NodeInfoDialog.gameObject.SetActive(false);

        }

        public void CenterGraph() {
            if (CurrentGraph == null)
                return;

            Vector3 average = Vector3.zero;
            foreach (NoFlo_Basic.Component c in CurrentGraph.NodesByName.Values) {
                average += c.Visualisation.Component.MetadataPosition;
            }
            average = average * (1f / CurrentGraph.NodesByName.Values.Count);
            foreach (NoFlo_Basic.Component c in CurrentGraph.NodesByName.Values) {
                c.MetadataPosition -= average;
                c.Visualisation.transform.localPosition = c.MetadataPosition;
            }
        }

    }

}
