
using NoFloEditor;

namespace NoFlo_Basic {

    public class DefaultValue {

        public object Data;
        public Component Component;
        public InPort Port;
        public bool Persistent; // TODO

        public DefaultValueVisualisation Visualisation;

        public void SetData(object Data) {
            this.Data = Data;
            //if (Visualisation != null)
            //    Visualisation.UpdateData();
        }

    }

}