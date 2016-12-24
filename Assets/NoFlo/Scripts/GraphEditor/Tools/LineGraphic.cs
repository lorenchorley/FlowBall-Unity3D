using System;
using UnityEngine;

namespace NoFloEditor {

    // Assume that the graphic attached to this componet is normalised
    // That is that when position == Vector3.zero, the graphic will start at (0,0,0) and end at (0,0,1)
    public class LineGraphic : MonoBehaviour {

        public Transform src;
        public Transform tgt;
        public float width = 1;

        Transform container;
        Vector3? initialWidth = null;

        Vector3 previousSrcPosition;
        Vector3 previousTgtPosition;
        
        public void Setup(Transform src, Transform tgt, float? width = null, Transform container = null) {
            this.src = src;
            this.tgt = tgt;
            
            if (container != null) {
                this.container = container;
            }

            if (!initialWidth.HasValue) {
                initialWidth = new Vector3(transform.localScale.x, transform.localScale.y, 0);
            }

            if (width.HasValue) {
                this.width = width.Value;
            }

            gameObject.SetActive(true);

            UpdateLine();
            previousSrcPosition = src.position;
            previousTgtPosition = tgt.position;

        }

        public void Enable() {
            if (!initialWidth.HasValue)
                throw new Exception("LineGraphic has not be set up");

            gameObject.SetActive(true);
        }

        public void Disable() {
            gameObject.SetActive(false);
        }

        public void UpdateWidth(float width) {
            this.width = width;
            if (initialWidth.HasValue)
                transform.localScale = new Vector3(0, 0, transform.localScale.z) + width * initialWidth.Value;
        }

        void Update() {
            if (src.position != previousSrcPosition || tgt.position != previousTgtPosition) {
                UpdateLine();
                previousSrcPosition = src.position;
                previousTgtPosition = tgt.position;
            }
        }

        public void UpdateLine() {
            Vector3 localSrc;

            if (container == null) {
                localSrc = src.position;
            } else {
                localSrc = container.InverseTransformPoint(src.position);
            }

            float distance = (src.position - tgt.position).magnitude;

            transform.localScale = new Vector3(0, 0, distance) + width * initialWidth.Value;
            transform.localPosition = localSrc;
            transform.LookAt(tgt);

        }

    }

}