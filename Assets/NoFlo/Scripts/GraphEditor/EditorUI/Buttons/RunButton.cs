using NoFlo_Basic;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class RunButton : MonoBehaviour {

        public Graph Graph;
        public GraphEditor GraphEditor;

        private Text Text;
        private bool initialised = false;

        private void Init() {
            if (initialised)
                return;

            Text = GetComponentInChildren<Text>();
            GetComponent<Button>().onClick.AddListener(() => {
                if (Text.text == "Run") {
                    Graph.DisableDebug();
                    Graph.Run();
                    Text.text = "Stop";
                } else {
                    Graph.ForceStop();
                    Text.text = "Run";
                }

            });

            initialised = true;
        }

        public void SetGraph(Graph Graph) {
            this.Graph = Graph;
            Init();

            if (Graph.IsRunning() && !Graph.InDebugMode()) {
                Text.text = "Stop";
            } else {
                Text.text = "Run";
            }

            Graph.PrimaryExecutor.OnStop.AddListener(() => {
                Text.text = "Run";
            });

        }

    }

}