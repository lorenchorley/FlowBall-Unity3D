using UnityEngine;
using UnityEngine.UI;
using NoFlo_Basic;

namespace NoFloEditor {

    public class NodeInfoDialog : MonoBehaviour {

        public GraphEditor GraphEditor;

        public Text ComponentName;
        public Text InPortsTitle;
        public RectTransform InPortsContainer;
        public Text OutPortsTitle;
        public RectTransform OutPortsContainer;

        public GameObject InPortInfoTemplate;
        public GameObject OutPortInfoTemplate;
        public GameObject VariableTemplate;

        public Text VariableSelectionTitle;
        public GameObject VariableSelection;
        public Button VariableSelectionBackButton;

        public Text VariableDetailsTitle;
        public VariableDetails VariableDetails;
        public Button VariableDetailsBackButton;

        void Start() {
            gameObject.SetActive(false);

            VariableSelectionBackButton.onClick.AddListener(NormalMode);
            VariableDetailsBackButton.onClick.AddListener(NormalMode);
        }

        public void SetNode(NoFlo_Basic.Component Component) {
            NormalMode();
            ComponentName.text = Component.ComponentName;

            foreach (Transform t in InPortsContainer)
                Destroy(t.gameObject);
            foreach (Transform t in OutPortsContainer)
                Destroy(t.gameObject);

            foreach (InPort p in Component.Input.GetPorts()) {
                if (p.Hidden)
                    continue;

                PortInfo info = GameObject.Instantiate<GameObject>(InPortInfoTemplate).GetComponent<PortInfo>();
                info.transform.SetParent(InPortsContainer);
                info.Setup(p, this);
            }

            foreach (OutPort p in Component.Output.GetPorts()) {
                if (p.Hidden)
                    continue;

                PortInfo info = GameObject.Instantiate<GameObject>(OutPortInfoTemplate).GetComponent<PortInfo>();
                info.transform.SetParent(OutPortsContainer);
                info.Setup(p, this);
            }

            NormalMode();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);

        }

        public void SelectDetailsModeFor(PortInfo portInfo) {
            InPortsTitle.gameObject.SetActive(false);
            InPortsContainer.gameObject.SetActive(false);
            OutPortsTitle.gameObject.SetActive(false);
            OutPortsContainer.gameObject.SetActive(false);
            VariableDetailsTitle.gameObject.SetActive(true);
            VariableDetails.gameObject.SetActive(true);
            
            VariableDetails.SetPort(portInfo.Port);

            LayoutRebuilder.ForceRebuildLayoutImmediate(VariableDetails.transform as RectTransform);

        }

        public void SelectVariableModeFor(PortInfo portInfo) {
            InPortsTitle.gameObject.SetActive(false);
            InPortsContainer.gameObject.SetActive(false);
            OutPortsTitle.gameObject.SetActive(false);
            OutPortsContainer.gameObject.SetActive(false);
            VariableSelectionTitle.gameObject.SetActive(true);
            VariableSelection.SetActive(true);

            foreach (Transform t in VariableSelection.transform) {
                Destroy(t.gameObject);
            }

            InPort Port = portInfo.Port as InPort;
            foreach (UnityGraphObject v in Port.Component.Graph.AssociatedInterlink.GetLinkedVariables()) {

                // Type check
                bool found = false;
                for (int i = 0; i < Port.Types.Length; i++) {
                    if (Port.Types[i] == v.GetObjectType()) {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    continue;

                GameObject variableSelector = GameObject.Instantiate<GameObject>(VariableTemplate);
                variableSelector.transform.SetParent(VariableSelection.transform);
                variableSelector.GetComponentInChildren<Text>().text = v.GetObjectID() + " (" + v.GetObjectType() + ")";

                UnityGraphObject obj = v;
                variableSelector.GetComponent<Button>().onClick.AddListener(() => {
                    NormalMode();
                    Port.Component.Graph.SetDefaultValue(obj, Port);
                    portInfo.RefreshDefaultValueShown();
                });
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(VariableSelection.transform as RectTransform);

        }

        public void NormalMode() {
            InPortsTitle.gameObject.SetActive(true);
            InPortsContainer.gameObject.SetActive(true);
            OutPortsTitle.gameObject.SetActive(true);
            OutPortsContainer.gameObject.SetActive(true);
            VariableSelectionTitle.gameObject.SetActive(false);
            VariableSelection.SetActive(false);
            VariableDetailsTitle.gameObject.SetActive(false);
            VariableDetails.gameObject.SetActive(false);
        }

    }

}