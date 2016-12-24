
namespace NoFlo_Basic {

    public abstract class ExecutionContext {

        public abstract void Setup();
        public abstract void Reset();
        public abstract void QueueTask(Processable processable);

        public ExecutionContext() {
            Setup();
        }

    }

}