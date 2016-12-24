using NoFlo_Basic;
using geniikw;
using UnityEngine;

namespace NoFloEditor {

    public class EdgeVisualisation : Visualisation {

        public Edge Edge;
        public UIMeshLine Line;

        public PortVisualisation InPort;
        public PortVisualisation OutPort;

        public void Highlight(bool enable) {
            Line.m_width = enable ? 10 : 7;
        }

        public void SetupWithEdge(Edge Edge, LinePointManager manager) {
            this.Edge = Edge;

            OutPort = Edge.Source.Visualisation;
            InPort = Edge.Target.Visualisation;

            LinePoint startPoint = new LinePoint();
            LinePoint endPoint = new LinePoint();

            Line.points.Clear();
            Line.points.Add(startPoint);
            Line.points.Add(endPoint);

            OutPort.gameObject.AddComponent<LinePointUpdater>().Setup(Line, 0, Vector3.zero, Line.transform, manager);
            InPort.gameObject.AddComponent<LinePointUpdater>().Setup(Line, 1, Vector3.zero, Line.transform, manager);

            RectTransform rect = transform as RectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;

        }

        private void SetLinePointToGlobalPosition(LinePoint lp, Vector3 pos) {
            lp.point = pos - transform.position;
        }

        public override void Select() {
        }

        public override void Deselect() {
        }

    }
}