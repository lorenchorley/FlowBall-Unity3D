using NoFlo_Basic;
using UnityEngine;

namespace NoFloEditor {

    public abstract class Visualisation : MonoBehaviour {

        public Graph Graph;

        public abstract void Select();
        public abstract void Deselect();

    }

}