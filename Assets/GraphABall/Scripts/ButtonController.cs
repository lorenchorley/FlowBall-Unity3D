using System;
using NoFlo_Basic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : UnityGraphObject {

    public Material Hightlighted;
    private MeshRenderer Renderer;
    private Material originalMaterial;

    public UnityEvent ButtonPress;

    public override void Setup() {
        Renderer = GetComponent<MeshRenderer>();
        originalMaterial = Renderer.material;
    }

    public override string GetObjectType() {
        return "Button Controller";
    }

    public override void SetHighlighted(bool enable) {
        if (enable && Cursor.visible) {
            Renderer.material = Hightlighted;
        } else {
            Renderer.material = originalMaterial;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            ButtonPress.Invoke();
            TriggerEvent("Pressed");
        }
    }

}
