using System;
using System.Collections.Generic;

namespace NoFlo_Basic {

    public class InThreadDebugGraphExecutor : GraphExecutor {

        private enum ExecutorState {
            Stopped,
            Idle,
            ProcessingTasks,
            QueuingTasks,
        }

        public float DebugDelay = 1;

        ExecutorState state;
        Queue<InPort> DefaultPortQueue;
        Processable p;
        InThreadExecutionContext context;

        public override ExecutionContext GetExecutionContext() {
            return context;
        }

        protected override void _Setup() {
            context = new InThreadExecutionContext();
        }

        protected override void _Stop() {
            state = ExecutorState.Stopped;
            context.Stop();
        }

        protected override void _ExecuteGraph(Graph Graph) {
            context.Reset();

            SetupInitialisingTasks();
            if (!HasInitialisingTasks())
                return;

            EnqueueNextInitialisingTask();

            state = ExecutorState.ProcessingTasks;
            Loop();
        }

        protected override void _ContinueExecutionOn(Processable task) {
            if (state == ExecutorState.Idle) {
                state = ExecutorState.ProcessingTasks;
                context.Continue();

                context.QueueTask(task);

                Loop();
            } else {

                // We're already in the loop, just queue up the tasks
                context.QueueTask(task);

            }
        }

        protected override void _ContinueExecutionOn(List<Processable> tasks) {
            if (state == ExecutorState.Idle) {
                state = ExecutorState.ProcessingTasks;
                context.Continue();

                for (int i = 0; i < tasks.Count; i++) {
                    context.QueueTask(tasks[i]);
                }

                Loop();
            } else {

                // We're already in the loop, just queue up the tasks
                for (int i = 0; i < tasks.Count; i++) {
                    context.QueueTask(tasks[i]);
                }

            }
        }

        public override bool IsRunningGraph(Graph Graph) {
            return this.Graph == Graph && state != ExecutorState.Stopped;
        }

        public void SetupInitialisingTasks() {
            DefaultPortQueue = new Queue<InPort>();
            foreach (InPort port in Graph.DefaultValuesByInPort.Keys) {
                DefaultPortQueue.Enqueue(port);
            }
        }

        public bool HasInitialisingTasks() {
            return DefaultPortQueue.Count > 0; // TODO include continued execution data
        }

        public void EnqueueNextInitialisingTask() {
            InPort port = DefaultPortQueue.Dequeue();

            if (Graph.InDebugMode())
                port.DebugHighlight();

            Graph.AddData(port, Graph.DefaultValuesByInPort[port].Data);

            if (Graph.InDebugMode())
                port.DebugUnhighlight();

            context.QueueTask(port.Component);
            context.SwitchToNewlyEnqueuedTasks();

        }

        public void Loop() {
            switch (state) {
            case ExecutorState.Stopped:
                return;
            case ExecutorState.Idle:
                throw new Exception("Executor attempted to run loop while in idle state");
            case ExecutorState.ProcessingTasks:
                if (p != null) {

                    // Process next and unhighlight
                    p.Process(context);

                    if (!p.HasMoreToProcess()) {
                        p.DebugUnhighlight();
                        p = null;
                    }

                    // Repeat loop after delay
                    Invoke("Loop", DebugDelay);

                } else if (context.tasksEnumerator.MoveNext()) {

                    // Get next node to process
                    p = context.tasksEnumerator.Current;

                    // Highlight the node
                    p.DebugHighlight();

                    // Repeat loop after delay
                    Invoke("Loop", DebugDelay);

                } else { // Finished with the current tasks

                    // Go back to queuing tasks
                    state = ExecutorState.QueuingTasks;

                    // Repeat loop, no delay
                    Loop();

                }
                break;
            case ExecutorState.QueuingTasks:
                if (context.HasNewlyEnqueuedTasks()) { // If there is something to process gathered from the previous processes, do them

                    // Swap processing lists and clear the next processing list
                    context.SwitchToNewlyEnqueuedTasks();

                    // Start processing again
                    state = ExecutorState.ProcessingTasks;

                    // Repeat loop, no delay
                    Loop();

                } else if (HasInitialisingTasks()) { // Otherwise if there is data to process gathered from elsewhere, do that

                    EnqueueNextInitialisingTask();

                    // Start processing again
                    state = ExecutorState.ProcessingTasks;

                    // Repeat loop, no delay
                    Loop();

                } else { // If there is no data, stop/idle the execution

                    if (Graph.HasSubscriptions() || Graph.HasDelayedExecution()) {

                        // Idle if the graph is still has active external connections
                        state = ExecutorState.Idle;
                        if (OnIdle != null)
                            OnIdle.Invoke();

                    } else {

                        // Stop if there are no component externally connected
                        StopExecution();

                    }

                }
                break;
            }


        }

        public override bool IsIdle() {
            return state == ExecutorState.Idle;
        }

        public override bool IsExecuting() {
            return state != ExecutorState.Stopped && state != ExecutorState.Idle;
        }

        public override bool IsStopped() {
            return state == ExecutorState.Stopped;
        }

    }

}