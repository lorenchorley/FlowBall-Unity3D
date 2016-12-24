using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NoFlo_Basic {

    public abstract class UnityGraphObject : MonoBehaviour, IGraphObject, IBeginDragHandler, IDragHandler, IEndDragHandler {

        public string ID;

        public LayerMask Mask;

        private List<InPort> Subscribers;
        private List<Processable> SubscribedComponents;

        public abstract string GetObjectType();
        public abstract void SetHighlighted(bool enable);
        public abstract void Setup();

        void Awake() {
            Subscribers = new List<InPort>();
            SubscribedComponents = new List<Processable>();

            if (!Mathf.IsPowerOfTwo(Mask.value))
                throw new Exception("Select only one layer");

            gameObject.layer = (int) Mathf.Log((float) Mask.value, 2f);

            Setup();
        }

        public string GetObjectID() {
            return ID;
        }

        public void TriggerEvent(object Data) {
            HashSet<Graph> GraphsToContinue = new HashSet<Graph>();
            for (int i = 0; i < Subscribers.Count; i++) {
                Subscribers[i].Accept(Data);
                GraphsToContinue.Add(Subscribers[i].Component.Graph);
            }
            foreach (Graph Graph in GraphsToContinue) {
                Graph.CurrentExecutor.ContinueExecution(SubscribedComponents);
            }
        }

        public void SubscribeToEvents(InPort InPort) {
            if (Subscribers.Contains(InPort))
                throw new Exception("TODO");

            Subscribers.Add(InPort);

            if (!SubscribedComponents.Contains(InPort.Component)) // TODO take advantage of list indexing. Add the corresponding component at the same index as the port in it's own list
                SubscribedComponents.Add(InPort.Component);
        }

        public void UnsubscribeFromEvents(InPort InPort) {
            if (Subscribers.Contains(InPort))
                Subscribers.Remove(InPort);
            if (SubscribedComponents.Contains(InPort.Component))
                SubscribedComponents.Remove(InPort.Component);
        }

        public bool IsSubscribedToGraph(Graph Graph) {
            for (int i = 0; i < Subscribers.Count; i++) {
                if (Subscribers[i].Component.Graph == Graph)
                    return true;
            }
            return false;
        }

        public void ForcablyUnsubscribeFromGraph(Graph Graph) {
            for (int i = Subscribers.Count - 1; i >= 0; i--) {
                if (Subscribers[i].Component.Graph == Graph)
                    Subscribers.RemoveAt(i);
            }
        }

        void OnMouseEnter() {
            SetHighlighted(true);
        }

        void OnMouseExit() {
            SetHighlighted(false);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            Debug.Log("OnBeginDrag");
        }

        public void OnDrag(PointerEventData eventData) {
            Debug.Log("OnDrag");

        }

        public void OnEndDrag(PointerEventData eventData) {
            Debug.Log("OnEndDrag");

        }

        public override string ToString() {
            return ID;
        }

    }

}