using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class CloseButton : MonoBehaviour {

        public GraphEditor GraphEditor;

        void Start() {
            GetComponent<Button>().onClick.AddListener(() => {
                GraphEditor.Close();
            });
        }

    }

}