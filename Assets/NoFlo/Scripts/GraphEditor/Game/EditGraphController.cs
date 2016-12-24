using NoFlo_Basic;
using UnityEngine;

namespace NoFloEditor {

    [RequireComponent(typeof(GraphInterlink))]
    public class EditGraphController : MonoBehaviour {

        public Graph Graph;
        public GraphEditor GraphEditor;
        public GraphInterlink AssociatedInterlink;

        public Animator Animator;

        public Renderer Renderer;
        public Material NormalExecution;
        public Material DebugExecution;

        void Awake() {
            if (AssociatedInterlink == null)
                AssociatedInterlink = GetComponent<GraphInterlink>();

            UpdateListeners();
            UpdateAppearence();
        }

        void OnMouseOver() {
            if (Input.GetMouseButtonUp(0) && !GraphEditor.isOpen) {
                GraphEditor.Open(Graph);
                AssociatedInterlink.HideInterconnections();
                AssociatedInterlink.HideRange();
            }
        }

        public void UpdateListeners() {
            if (Graph != null) {
                Graph.OnChangeExecutor.AddListener(OnChangeExecutor);
                Graph.DebugExecutor.OnStart.AddListener(OnGraphStart);
                Graph.DebugExecutor.OnStop.AddListener(OnGraphStop);
                Graph.DebugExecutor.OnIdle.AddListener(OnGraphIdle);
                Graph.DebugExecutor.OnResume.AddListener(OnGraphStart);
                Graph.PrimaryExecutor.OnStart.AddListener(OnGraphStart);
                Graph.PrimaryExecutor.OnStop.AddListener(OnGraphStop);
                Graph.PrimaryExecutor.OnIdle.AddListener(OnGraphIdle);
                Graph.PrimaryExecutor.OnResume.AddListener(OnGraphStart);
            }
        }

        public void UpdateAppearence() {
            if (Graph != null && Graph.CurrentExecutor != null) {
                
                if (Graph.CurrentExecutor.IsStopped()) {
                    OnGraphStop();
                } else if (Graph.CurrentExecutor.IsIdle()) {
                    OnGraphIdle();
                } else {
                    OnGraphStart();
                }

            }
        }

        private void OnGraphStart() {
            Animator.SetTrigger("Start");
        }

        private void OnGraphStop() {
            Animator.SetTrigger("Stop");
        }

        private void OnGraphIdle() {
            Animator.SetTrigger("Idle");
        }

        private void OnChangeExecutor() {
            if (Graph.InDebugMode()) {
                SetExecutionModeDebug();
            } else {
                SetExecutionModeNormal();
            }
        }

        private void SetExecutionModeNormal() {
            Renderer.material = NormalExecution;
        }

        private void SetExecutionModeDebug() {
            Renderer.material = DebugExecution;
        }

    }

}