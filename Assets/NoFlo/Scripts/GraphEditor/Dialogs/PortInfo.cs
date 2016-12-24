using NoFlo_Basic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class PortInfo : MonoBehaviour {

        public bool IsInPort;
        public Text PortName;
        public Button PortNameButton;
        public Text PortType;
        public InputField DefaultValueInput;
        public Button SelectVariable;
        public Button ClearVariable;

        public Port Port;

        DefaultValue dv;

        public void Setup(OutPort Port, NodeInfoDialog NodeInfoDialog) {
            this.Port = Port;
            PortName.text = Port.Name;
            PortType.text = Port.TypesToString();

            PortNameButton.onClick.AddListener(() => NodeInfoDialog.SelectDetailsModeFor(this));

        }

        public void Setup(InPort Port, NodeInfoDialog NodeInfoDialog) {
            this.Port = Port;
            PortName.text = Port.Name;
            PortType.text = Port.TypesToString();

            PortNameButton.onClick.AddListener(() => NodeInfoDialog.SelectDetailsModeFor(this));

            bool contentHasChanged = false;
            DefaultValueInput.gameObject.AddComponent<UIEventRedirector>().Setup(() => {
                if (UpdateDV()) {
                    DefaultValueInput.text = DataTreatment.GetDataEditable(dv.Data);
                }
                contentHasChanged = false;
            }, null);

            DefaultValueInput.onEndEdit.AddListener((s) => {
                if (contentHasChanged) { 
                    //if (s == "") {
                        // TODO Delete default value
                    //    Port.Component.Graph.RemoveDefaultValue(Port);
                    //    Debug.Log("removing dv from port: " + Port.Name);
                    //} else {
                        // TODO Check data types, if string, ok.
                        // If number, try to parse.

                        if (UpdateDV()) {
                            dv.SetData(s);
                            RefreshDefaultValueShown();
                        } else {
                            Port.Component.Graph.AddDefaultValue(s, Port);
                            RefreshDefaultValueShown();
                        }
                    //}
                } else {
                    RefreshDefaultValueShown();
                }
            });

            DefaultValueInput.onValueChanged.AddListener((s) => {
                contentHasChanged = true;
            });

            SelectVariable.onClick.AddListener(() => {
                NodeInfoDialog.SelectVariableModeFor(this);
            });

            ClearVariable.onClick.AddListener(() => {
                Port.Component.Graph.RemoveDefaultValue(Port);
                RefreshDefaultValueShown();
            });

            RefreshDefaultValueShown();

        }

        private bool UpdateDV() {
            return Port.Component.Graph.DefaultValuesByInPort.TryGetValue(Port as InPort, out dv);
        }

        public void RefreshDefaultValueShown() {
            if (UpdateDV())
                DefaultValueInput.text = DataTreatment.GetDataRepresentative(dv.Data);
            else
                DefaultValueInput.text = "";
        }

    }

}