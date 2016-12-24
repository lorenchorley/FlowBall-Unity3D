using NoFlo_Basic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class AddNodeButton : MonoBehaviour {

        public Graph Graph;
        public GraphEditor GraphEditor;
        public GameObject SideMenu;
        public RectTransform SideMenuContents;

        [Space]

        public GameObject ComponentSelectionTemplate;

        Button Button;
        Text Text;

        public void SetGraph(Graph Graph) {
            this.Graph = Graph;
        }

        void Start() {
            Text = GetComponentInChildren<Text>();
            Button = GetComponent<Button>();
            Button.onClick.AddListener(Action);

            Dictionary<string, Type> ComponentsByQualifiedName = ComponentCatalog.RequestComponentsByQualifiedName();

            foreach (string QualifiedName in ComponentsByQualifiedName.Keys) {
                ComponentSelection cs = GameObject.Instantiate<GameObject>(ComponentSelectionTemplate).GetComponent<ComponentSelection>();
                cs.transform.SetParent(SideMenuContents);
                cs.Setup(QualifiedName, ComponentsByQualifiedName[QualifiedName], () => {
                    SideMenu.SetActive(false);
                    NoFlo_Basic.Component c = Graph.AddNode(cs.ComponentType);
                    c.SetName(cs.ComponentType.Name);
                    c.Visualisation.transform.position = Camera.main.ViewportToScreenPoint(0.5f * Vector2.one);
                    Text.text = "Add";
                });
            }

            SideMenu.SetActive(false);
        }

        public void Action() {
            SideMenu.SetActive(!SideMenu.activeSelf);

            if (Text.text == "Add") {
                Text.text = "Close";
            } else {
                Text.text = "Add";
            }
        }

    }

}