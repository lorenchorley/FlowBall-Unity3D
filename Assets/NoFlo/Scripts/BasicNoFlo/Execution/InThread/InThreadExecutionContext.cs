using System;
using System.Collections.Generic;

namespace NoFlo_Basic {

    public class InThreadExecutionContext : ExecutionContext {

        bool isStopped;
        HashSet<Processable> tasks;
        HashSet<Processable> nextTasks;
        HashSet<Processable> tmp;
        public IEnumerator<Processable> tasksEnumerator { get; private set; }

        public override void Setup() {
            tasks = new HashSet<Processable>();
            nextTasks = new HashSet<Processable>();
            isStopped = false;
        }

        public override void Reset() {
            tasks.Clear();
            nextTasks.Clear();
            isStopped = false;
        }

        public override void QueueTask(Processable processable) {
            if (isStopped)
                throw new Exception("Cannot call QueueTask when stopped");

            nextTasks.Add(processable);
        }

        public void Continue() {
            isStopped = false;
        }

        public void Stop() {
            isStopped = true;
        }

        public void ResetEnumerator() {
            if (isStopped)
                throw new Exception("Cannot call ResetEnumerator when stopped");

            tasksEnumerator = tasks.GetEnumerator();
        }

        public void SwitchToNewlyEnqueuedTasks() {
            if (isStopped)
                throw new Exception("Cannot call SwitchToNewlyEnqueuedTasks when stopped");

            tmp = tasks;
            tasks = nextTasks;
            nextTasks = tmp;
            nextTasks.Clear();

            ResetEnumerator();
        }

        public bool HasTasks() {
            if (isStopped)
                throw new Exception("Cannot call HasTasks when stopped");

            return tasks.Count > 0;
        }

        public bool HasNewlyEnqueuedTasks() {
            if (isStopped)
                throw new Exception("Cannot call HasNewlyEnqueuedTasks when stopped");

            return nextTasks.Count > 0;
        }

    }

}