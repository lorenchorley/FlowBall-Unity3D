using geniikw;
using UnityEngine;

namespace NoFloEditor {

    public class LinePointUpdater : MonoBehaviour {

        private UIMeshLine Line;
        private int Index;
        private Vector3 StaticOffset;
        private Transform DynamicOffset;

        private LinePoint lp;

        void Update() {
            UpdateLP(); // TODO optimise
        }

        public void UpdateLP() {
            try {
                lp = Line.points[Index];
            } catch (MissingReferenceException) {
                Destroy(this);
                return;
            }
            lp.point = transform.position + StaticOffset - DynamicOffset.position;
            Line.points[Index] = lp;
        }

        public void Setup(UIMeshLine Line, int index, Vector3 StaticOffset, Transform DynamicOffset, LinePointManager manager) {
            this.Line = Line;
            this.Index = index;
            this.StaticOffset = StaticOffset;
            this.DynamicOffset = DynamicOffset;

            manager.RegisterUpdater(this);

        }

    }

}