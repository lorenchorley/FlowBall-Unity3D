using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class DebugMessageVisualisation : Visualisation {

        public Text Text;
        public Text Quantity;

        public void SetMessage(string message) {
            Text.text = message;
        }

        public void SetQuantity(int n) {
            Quantity.text = n.ToString();
        }

        public void Setup(Transform target) {
            transform.SetParent(target);
            transform.localPosition = Vector3.zero;
        }

        public override void Select() {
        }

        public override void Deselect() {
        }

    }

}