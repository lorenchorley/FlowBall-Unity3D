using UnityEngine;

namespace NoFloEditor {

    public class MouseTracker : MonoBehaviour {

        void Update() {
            transform.position = UnityEngine.Input.mousePosition;
        }

    }

}