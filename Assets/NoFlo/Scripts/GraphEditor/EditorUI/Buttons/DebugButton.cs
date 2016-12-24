using NoFlo_Basic;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class DebugButton : MonoBehaviour {

        public Graph Graph;
        public GraphEditor GraphEditor;

        private Text Text;
        private bool initialised = false;

        private void Init() {
            if (initialised)
                return;

            Text = GetComponentInChildren<Text>();
            GetComponent<Button>().onClick.AddListener(() => {
                if (Text.text == "Debug") {
                    Text.text = "Stop";

                    Graph.EnableDebug();
                    Graph.Run();

                } else {
                    Graph.ForceStop();
                    //Text.text = "Debug";
                }

            });

            initialised = true;
        }

        public void SetGraph(Graph Graph) {
            this.Graph = Graph;
            Init();

            if (Graph.IsRunning() && Graph.InDebugMode()) {
                Text.text = "Stop";
            } else {
                Text.text = "Debug";
            }

            Graph.DebugExecutor.OnStop.AddListener(() => {
                Text.text = "Debug";
            });

        }

    }

}