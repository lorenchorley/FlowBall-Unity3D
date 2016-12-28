using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IndicatorOverlay : Indicator {

    public bool Activated = false;
    public float ScaleModifier = 1;
    public float MaxDistance = 5;
    public bool InScreenSpace = false;

    [Space]

    public Transform Target;

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
        Activated = true;
        RegisterSelf();
    }

    public override void DeactivateIndicator() {
        Activated = false;
    }

}
