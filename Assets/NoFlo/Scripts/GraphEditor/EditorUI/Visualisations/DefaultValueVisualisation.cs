using NoFlo_Basic;
using System;
using UnityEngine.UI;

namespace NoFloEditor {

    public class DefaultValueVisualisation : Visualisation {

        public DefaultValue DefaultValue;
        public Text Text;

        void Start() {
            if (Text == null)
                Text = GetComponentInChildren<Text>();
            UpdateData();
        }

        public void SetupWithDefaultValue(DefaultValue DefaultValue) {
            this.DefaultValue = DefaultValue;

            if (DefaultValue.Port.Visualisation == null)
                throw new Exception("TODO");

            transform.position = DefaultValue.Port.Visualisation.transform.position;
            transform.SetParent(DefaultValue.Port.Visualisation.transform);

        }

        public void UpdateData() {
            Text.text = DataTreatment.GetDataRepresentative(DefaultValue.Data);
        }

        public override void Select() {
        }

        public override void Deselect() {
        }

    }

}