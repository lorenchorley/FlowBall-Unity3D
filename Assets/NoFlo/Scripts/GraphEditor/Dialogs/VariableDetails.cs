using UnityEngine;
using UnityEngine.UI;
using NoFlo_Basic;

namespace NoFloEditor {

    public class VariableDetails : MonoBehaviour {

        public Text Name;
        public Text Types;
        public Text Description;
        public Text DefaultValue;
        public Text DefaultValueType;

        Port Port;

        public void SetPort(Port Port) {
            this.Port = Port;

            if (Port is InPort) {
                DefaultValue.transform.parent.gameObject.SetActive(true);
                DefaultValueType.transform.parent.gameObject.SetActive(true);

                Name.text = Port.Name;
                Types.text = Port.TypesToString();
                Description.text = Port.Description;

                DefaultValue dv;
                if (Port.Component.Graph.DefaultValuesByInPort.TryGetValue(Port as InPort, out dv)) {
                    DefaultValue.text = DataTreatment.GetDataRepresentative(dv.Data);
                    DefaultValueType.text = DataTreatment.GetDataType(dv.Data);
                } else {
                    DefaultValue.text = "";
                    DefaultValueType.text = "";
                }

            } else {
                DefaultValue.transform.parent.gameObject.SetActive(false);
                DefaultValueType.transform.parent.gameObject.SetActive(false);

                Name.text = Port.Name;
                Types.text = Port.TypesToString();
                Description.text = Port.Description;

            }
        }

    }

}