using NoFlo_Basic;
using UnityEngine.UI;

namespace NoFloEditor {

    public class ComponentNameVisualisation : Visualisation {

        public Component Component;
        public InputField InputField;

        void Start() {
            InputField.onEndEdit.AddListener((s) => OnEndEdit(s));
        }

        void OnEndEdit(string s) {
            if (s != "") {
                Component.SetName(s);
            }
            InputField.gameObject.SetActive(false);
        }

        public void StartEdit() {
            InputField.gameObject.SetActive(true);
            InputField.Select();
        }

        public override void Select() {
        }

        public override void Deselect() {
        }

    }

}