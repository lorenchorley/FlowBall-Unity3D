
using NoFloEditor;

namespace NoFlo_Basic {

    public class Edge {

        public OutPort Source;
        public InPort Target;

        public EdgeVisualisation Visualisation;

        public override int GetHashCode() {
            return Source.GetHashCode() * Target.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (!(obj is Edge))
                return false;

            Edge e = obj as Edge;

            return e.Source == Source && e.Target == Target;
        }

    }

}