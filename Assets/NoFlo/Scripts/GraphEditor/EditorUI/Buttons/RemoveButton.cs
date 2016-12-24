using NoFlo_Basic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class RemoveButton : MonoBehaviour {

        public Graph Graph;
        public GraphEditor GraphEditor;
        public GraphEditor MouseManager;

        void Start() {

            if (MouseManager == null) {
                MouseManager = GameObject.FindObjectOfType<GraphEditor>();
                if (MouseManager == null)
                    throw new Exception("");
            }

            GetComponent<Button>().onClick.AddListener(() => {
                if (MouseManager.selected != null && MouseManager.selected is NodeVisualisation)
                    Graph.RemoveNode((MouseManager.selected as NodeVisualisation).Component);
            });

        }

        public void SetGraph(Graph Graph) {
            this.Graph = Graph;
        }


    }

}