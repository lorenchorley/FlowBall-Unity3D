using NoFlo_Basic;
using NoFloEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorIndicatorOverlay : Indicator {

    public bool Activated = false;
    public float ScaleModifier = 1;

    [Space]

    public GraphEditor GraphEditor;
    public string ComponentName;
    public string PortName;
    public bool IsInPort;
    [NonSerialized]
    public RectTransform Target;

    [NonSerialized]
    public float InitialDistance;
    [NonSerialized]
    public Vector2 InitialSize;

    private RectTransform _RectTransform;
    public RectTransform RectTransform {
        get {
            if (_RectTransform == null)
                _RectTransform = GetComponent<RectTransform>();

            return _RectTransform;
        }
    }

    private bool isRegistered = false;
    private void RegisterSelf() {
        if (isRegistered)
            return;
        isRegistered = true;

        IndicatorController IndicatorController = GameObject.FindObjectOfType<IndicatorController>();
        IndicatorController.RegisterOverlay(this);
    }

    void Start() {
        RegisterSelf();
    }

    public override void ActivateIndicator() {
        if (!GraphEditor.isOpen) {
            GraphEditor.EventOptions.EditorOpened.AddListener(DeferredActivation);
            return;
        }

        RegisterSelf();
        Activated = true;

        if (ComponentName == null || ComponentName == "")
            throw new Exception("TODO");

        NoFlo_Basic.Component node;
        if (!GraphEditor.CurrentGraph.NodesByName.TryGetValue(ComponentName, out node))
            throw new Exception("TODO");

        if (PortName == null || PortName == "") {

            if (node.Visualisation == null)
                throw new Exception("TODO");

            Target = node.Visualisation.GetComponent<RectTransform>();

        } else {

            Port port;
            if (IsInPort) {
                port = node.Input.GetPort(PortName);
            } else {
                port = node.Output.GetPort(PortName);
            }

            if (port.Visualisation == null)
                throw new Exception("TODO");

            Target = port.Visualisation.GetComponent<RectTransform>();

        }

        // Adjust the overlays size depending on how far away the target object is
        RectTransform.sizeDelta = InitialSize * ScaleModifier;

    }

    private void DeferredActivation() {
        GraphEditor.EventOptions.EditorOpened.RemoveListener(DeferredActivation);
        ActivateIndicator();

        Assert.IsTrue(GraphEditor.isOpen);
    }

    public override void DeactivateIndicator() {
        Activated = false;
    }

}
