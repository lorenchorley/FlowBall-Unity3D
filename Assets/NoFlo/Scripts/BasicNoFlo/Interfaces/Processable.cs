
namespace NoFlo_Basic {

    public interface Processable {

        void Process(ExecutionContext context);
        void DebugHighlight();
        void DebugUnhighlight();
        void StartProcessing();
        bool HasMoreToProcess();

    }

}