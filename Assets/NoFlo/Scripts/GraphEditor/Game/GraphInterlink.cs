using NoFlo_Basic;
using NoFloEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoFloEditor {

    public class GraphInterlink : MonoBehaviour {

        public float ConnectionRange = 1;

        public LayerMask Mask;

        public GraphEditor GraphEditor;
        public List<UnityGraphObject> ConnectedUnityObjects;
        public List<GraphInterlink> AssociatedLinks;

        public LineGraphic LineGraphicTemplate;
        private List<LineGraphic> Lines;

        public GameObject RangeDoodadTemplate;
        private GameObject RangeDoodad;

        private HashSet<GraphInterlink> interlinks;
        private Dictionary<string, UnityGraphObject> VariablesByID;

        void Awake() {

            // Check for null entries
            for (int i = AssociatedLinks.Count - 1; i >= 0; i--) {
                if (AssociatedLinks[i] == null)
                    AssociatedLinks.RemoveAt(i);
            }

            if (ConnectedUnityObjects == null)
                ConnectedUnityObjects = new List<UnityGraphObject>();

            Lines = new List<LineGraphic>();
            SubscribersByEventName = new Dictionary<string, List<InPort>>();
            SubscribedComponents = new List<Processable>();

            RangeDoodad = GameObject.Instantiate<GameObject>(RangeDoodadTemplate);
            RangeDoodad.gameObject.SetActive(false);

            if (!Mathf.IsPowerOfTwo(Mask.value))
                throw new Exception("Select only one layer");

            gameObject.layer = (int) Mathf.Log((float) Mask.value, 2f);

        }

        private void VerifyReferences() {
            if (interlinks == null)
                CompileAndShareReferences(this, new HashSet<GraphInterlink>(), new Dictionary<string, UnityGraphObject>());
        }

        private void RescanReferences() {
            CompileAndShareReferences(this, new HashSet<GraphInterlink>(), new Dictionary<string, UnityGraphObject>());
        }

        public HashSet<GraphInterlink> AllLinks() {
            VerifyReferences();
            return interlinks;
        }

        protected void CompileAndShareReferences(GraphInterlink requester, HashSet<GraphInterlink> commonInterlinks, Dictionary<string, UnityGraphObject> commonVariablesByID) {
            if (commonInterlinks.Contains(this))
                return;

            if (!AssociatedLinks.Contains(requester))
                AssociatedLinks.Add(requester);

            commonInterlinks.Add(this);
            interlinks = commonInterlinks;

            for (int i = 0; i < ConnectedUnityObjects.Count; i++) {
                UnityGraphObject obj = ConnectedUnityObjects[i];

                if (commonVariablesByID.ContainsKey(obj.ID))
                    Debug.LogWarning("More than one object with ID " + obj.ID + " detected. Only one will be kept.");
                else
                    commonVariablesByID.Add(obj.ID, obj);
            }
            VariablesByID = commonVariablesByID;

            for (int i = 0; i < AssociatedLinks.Count; i++) {
                AssociatedLinks[i].CompileAndShareReferences(this, commonInterlinks, commonVariablesByID);
            }
        }

        public IEnumerable<UnityGraphObject> GetLinkedVariables() {
            VerifyReferences();

            return VariablesByID.Values;
        }

        public UnityGraphObject GetLinkedVariableByID(string ID) {
            UnityGraphObject variable;
            VerifyReferences();

            if (!VariablesByID.TryGetValue(ID, out variable)) {
                Debug.LogError("Variable not present or in range: " + ID);
                return null;
            }

            return variable;
        }

        public void ForcablyUnsubscribeAll(Graph Graph) {
            //for (int i = 0; i < ConnectedExternalObjects.Count; i++) {
            //    ConnectedExternalObjects[i].ForcablyUnsubscribeFromGraph(this);
            //}
            for (int i = 0; i < ConnectedUnityObjects.Count; i++) {
                ConnectedUnityObjects[i].ForcablyUnsubscribeFromGraph(Graph);
            }
        }

        public bool HasSubscriptions(Graph Graph) {
            //for (int i = 0; i < ConnectedExternalObjects.Count; i++) {
            //    if (ConnectedExternalObjects[i].IsSubscribedToGraph(this))
            //        return true;
            //}
            for (int i = 0; i < ConnectedUnityObjects.Count; i++) {
                if (ConnectedUnityObjects[i].IsSubscribedToGraph(Graph))
                    return true;
            }
            return false;
        }

        void OnMouseEnter() {
            if ((GraphEditor == null || !GraphEditor.isOpen) && Cursor.visible) { 
                ShowInterconnections();
                ShowRange();
            }
        }

        void OnMouseExit() {
            if (GraphEditor == null || !GraphEditor.isOpen) {
                HideInterconnections();
                HideRange();
            }
        }

        public void ShowInterconnections() {
            for (int i = 0; i < Mathf.Max(ConnectedUnityObjects.Count + AssociatedLinks.Count, Lines.Count); i++) {
                if (i >= ConnectedUnityObjects.Count) {

                    SetNewLineToObject(AssociatedLinks[i - ConnectedUnityObjects.Count].transform, i);

                    if (i >= ConnectedUnityObjects.Count + AssociatedLinks.Count) {
                        Lines[i].gameObject.SetActive(false);
                    }
                    continue;
                }

                SetNewLineToObject(ConnectedUnityObjects[i].transform, i);

            }
        }

        private void SetNewLineToObject(Transform obj, int index) {
            LineGraphic Line;

            if (index >= Lines.Count) {
                Line = GameObject.Instantiate<GameObject>(LineGraphicTemplate.gameObject).GetComponent<LineGraphic>();
                Line.UpdateWidth(0.1f);
                Lines.Add(Line);
            } else {
                Line = Lines[index];
            }

            Line.Setup(transform, obj);

        }

        public void HideInterconnections() {
            for (int i = 0; i < Lines.Count; i++) {
                Lines[i].gameObject.SetActive(false);
            }
        }

        public void ShowRange() {
            RangeDoodad.gameObject.SetActive(true);
            RangeDoodad.transform.position = transform.position;
            RangeDoodad.transform.localScale = Vector3.one * ConnectionRange;
        }

        public void HideRange() {
            RangeDoodad.SetActive(false);
        }

        public void ValidateVariableConnections() {
            VerifyReferences();

            foreach (GraphInterlink interlink in interlinks) {
                ValidateVariableConnections(interlink);
            }
        }

        protected void ValidateVariableConnections(GraphInterlink interlink) {
            for (int i = interlink.ConnectedUnityObjects.Count - 1; i >= 0; i--) {
                UnityGraphObject obj = interlink.ConnectedUnityObjects[i];
                if (!interlink.IsInRange(obj.gameObject)) {
                    Debug.LogWarning("Object not in range of graph " + name + ": " + obj.ID);
                    interlink.ConnectedUnityObjects.RemoveAt(i);
                    if (VariablesByID != null)
                        VariablesByID.Remove(obj.ID);
                }
            }
        }

        public bool IsInRange(Collider c) {
            Collider[] hits = Physics.OverlapSphere(transform.position, ConnectionRange);

            for (int i = 0; i < hits.Length; i++) {
                if (hits[i] == c)
                    return true;
            }

            return false;
        }

        public bool IsInRange(GameObject g) {
            Collider[] hits = Physics.OverlapSphere(transform.position, ConnectionRange / 2);

            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].gameObject == g)
                    return true;
            }

            return false;
        }

        private Dictionary<string, List<InPort>> SubscribersByEventName;
        private List<Processable> SubscribedComponents;

        public void EmitEvent(string eventName, object data) {
            VerifyReferences();

            foreach (GraphInterlink interlink in interlinks) {
                interlink.ReceiveEvent(eventName, data);
            }
        }

        protected void ReceiveEvent(string eventName, object data) {
            if (SubscribersByEventName.Count == 0)
                return;

            List<InPort> list;
            if (SubscribersByEventName.TryGetValue(eventName, out list)) {
                HashSet<Graph> GraphsToContinue = new HashSet<Graph>();
                InPort p;
                for (int i = 0; i < list.Count; i++) {
                    p = list[i];
                    p.Accept(data);
                    GraphsToContinue.Add(p.Component.Graph);
                }
                foreach (Graph Graph in GraphsToContinue) {
                    Graph.CurrentExecutor.ContinueExecution(SubscribedComponents);
                }
            }
        }

        public void SubscribeToEvent(string eventName, InPort InPort) {
            List<InPort> list;

            if (!SubscribersByEventName.TryGetValue(eventName, out list)) {
                list = new List<InPort>();
                SubscribersByEventName.Add(eventName, list);
            }

            if (list.Contains(InPort))
                throw new Exception("Port is already registered for event: " + eventName);

            list.Add(InPort);

            if (!SubscribedComponents.Contains(InPort.Component)) // TODO take advantage of list indexing. Add the corresponding component at the same index as the port in it's own list
                SubscribedComponents.Add(InPort.Component);
        }

        public void UnsubscribeFromEvent(string eventName, InPort InPort) {
            List<InPort> list;

            if (!SubscribersByEventName.TryGetValue(eventName, out list)) {
                list = new List<InPort>();
                SubscribersByEventName.Add(eventName, list);
            }

            if (!list.Contains(InPort))
                throw new Exception("Port is not registered for event: " + eventName);

            list.Remove(InPort);

            if (SubscribedComponents.Contains(InPort.Component))
                SubscribedComponents.Remove(InPort.Component);
        }

    }

}