
namespace NoFlo_Basic {

    public interface IGraphObject {

        string GetObjectID();
        string GetObjectType();
        void TriggerEvent(object Data);
        void SubscribeToEvents(InPort InPort);
        void UnsubscribeFromEvents(InPort InPort);
        bool IsSubscribedToGraph(Graph Graph);
        void ForcablyUnsubscribeFromGraph(Graph Graph);

    }

}