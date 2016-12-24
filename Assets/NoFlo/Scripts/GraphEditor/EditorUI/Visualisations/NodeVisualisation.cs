using System;
using System.Collections.Generic;
using NoFlo_Basic;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class NodeVisualisation : Visualisation {

        public string NodeName;

        public Text ComponentName;
        public Text ComponentQualifiedName;
        public InputField InputField;
        public RectTransform InPortContainer;
        public RectTransform OutPortContainer;

        public List<PortVisualisation> InPortVisualisations;
        public List<PortVisualisation> OutPortVisualisations;

        public NoFlo_Basic.Component Component;

        [Serializable]
        public class SelectionOptions {
            public Image Image;
            public Color Selected;

            [NonSerialized]
            public Color Unhighlighted;

        }
        public SelectionOptions Selection;

        [Serializable]
        public class DebugOptions {
            public Color Highlighted;

        }
        public DebugOptions Debug;

        void Start() {
            //RectTransform rect = GetComponent<RectTransform>();
            //rect.sizeDelta = new Vector2(100,50);
            Selection.Unhighlighted = Selection.Image.color;
        }

        public override void Select() {
            Selection.Image.color = Selection.Selected;
        }

        public override void Deselect() {
            Selection.Image.color = Selection.Unhighlighted;
        }

        public void DebugHighlight(bool enable) {
            Selection.Image.color = enable ? Debug.Highlighted : Selection.Unhighlighted;
        }

        public void SetName(string name) {
            ComponentName.text = name;
        }

        public void SetupWithComponent(NoFlo_Basic.Component component) {
            this.Component = component;

            SetName(component.ComponentName);
            ComponentQualifiedName.text = ComponentCatalog.RequestQualifedNameByComponentType()[component.GetType()];

            InPortVisualisations = new List<PortVisualisation>();
            foreach (NoFlo_Basic.InPort p in component.Input.GetPorts()) {
                if (p.Hidden)
                    continue;

                GameObject newPort = Instantiate<GameObject>(Graph.GraphEditor.Templates.PortTemplate);
                PortVisualisation v = newPort.GetComponent<PortVisualisation>();
                InPortVisualisations.Add(v);
                newPort.transform.SetParent(InPortContainer);
                p.Visualisation = v;
                v.Port = p;
                v.Graph = Component.Graph;
            }

            OutPortVisualisations = new List<PortVisualisation>();
            foreach (NoFlo_Basic.OutPort p in component.Output.GetPorts()) {
                if (p.Hidden)
                    continue;

                GameObject newPort = Instantiate<GameObject>(Graph.GraphEditor.Templates.PortTemplate);
                PortVisualisation v = newPort.GetComponent<PortVisualisation>();
                OutPortVisualisations.Add(v);
                newPort.transform.SetParent(OutPortContainer);
                p.Visualisation = v;
                v.Port = p;
                v.Graph = Component.Graph;
            }

        }

    }

}