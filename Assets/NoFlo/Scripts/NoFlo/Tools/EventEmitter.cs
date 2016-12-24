using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace NoFlo { 

    public class EventEmitter {

        private class EventEmitterEvent : UnityEvent<object[]> { }

        private readonly object[] defaultArguments;
        private bool isEmitting;
        private bool isInitialised;

        private Dictionary<string, EventEmitterEvent> Events;
        private Dictionary<string, EventEmitterEvent> OnceEvents; // Handlers that are called a single time and then removed

        private Queue<Action> QueuedActionsOnEmitter;

        public EventEmitter() {
            Events = new Dictionary<string, EventEmitterEvent>();
            OnceEvents = new Dictionary<string, EventEmitterEvent>();
            QueuedActionsOnEmitter = new Queue<Action>();
            isEmitting = false;
            isInitialised = false;
        }

        public void InitialiseEvent(string eventName) {
            if (isInitialised)
                throw new Exception("Cannot add more events once initialised");

            Events.Add(eventName, new EventEmitterEvent());
            OnceEvents.Add(eventName, new EventEmitterEvent());
        }
        
        public void FinishInitialisation() {
            isInitialised = true;
        }

        public void AddListener(string eventName, UnityAction<object[]> listener) {
            on(eventName, listener);
        }

        public void Emit(string eventName, object[] args = null) {
            if (!Events.ContainsKey(eventName))
                throw new Exception("No event with name: " + eventName);

            if (args == null)
                args = defaultArguments;

            isEmitting = true;

            UnityEvent<object[]> permanentEvent = Events[eventName];
            UnityEvent<object[]> oneTimeEvent = OnceEvents[eventName];

            permanentEvent.Invoke(args);
            oneTimeEvent.Invoke(args);

            oneTimeEvent.RemoveAllListeners();

            isEmitting = false;

            while (QueuedActionsOnEmitter.Count > 0) {
                QueuedActionsOnEmitter.Dequeue().Invoke();
            }
        }

        public IEnumerable<string> EventNames() {
            List<string> names = new List<string>();
            names.AddRange(Events.Keys);
            names.AddRange(OnceEvents.Keys);
            return names;
        }

        public void on(string eventName, UnityAction<object[]> listener) {
            if (!Events.ContainsKey(eventName))
                throw new Exception("No event with name: " + eventName);

            if (isEmitting)
                QueuedActionsOnEmitter.Enqueue(() => on(eventName, listener));

            Events[eventName].AddListener(listener);
        }

        public void once(string eventName, UnityAction<object[]> listener) {
            if (!Events.ContainsKey(eventName))
                throw new Exception("No event with name: " + eventName);

            if (isEmitting)
                QueuedActionsOnEmitter.Enqueue(() => once(eventName, listener));

            OnceEvents[eventName].AddListener(listener);
        }

        public void removeAllListeners() {
            if (isEmitting)
                QueuedActionsOnEmitter.Enqueue(() => removeAllListeners());

            foreach (string eventName in Events.Keys) {
                Events[eventName].RemoveAllListeners();
                OnceEvents[eventName].RemoveAllListeners();
            }
        }

        public void removeAllListeners(string eventName) {
            if (!Events.ContainsKey(eventName))
                throw new Exception("No event with name: " + eventName);

            if (isEmitting)
                QueuedActionsOnEmitter.Enqueue(() => removeAllListeners(eventName));

            Events[eventName].RemoveAllListeners();
        }

        public void removeListener(string eventName, UnityAction<object[]> listener) {
            if (!Events.ContainsKey(eventName))
                throw new Exception("No event with name: " + eventName);

            if (isEmitting)
                QueuedActionsOnEmitter.Enqueue(() => removeListener(eventName, listener));

            Events[eventName].RemoveListener(listener);
        }

        //public void prependListener(string eventName, UnityAction<object[]> listener) {
        //}
        //public void prependOnceListener(string eventName, UnityAction<object[]> listener) {
        //}
        //public void getMaxListeners() {
        //}
        //public void listenerCount(string eventName) {
        //}
        //public void listeners(string eventName) {
        //}
        //public void setMaxListeners(int n) {
        //}


    }

}