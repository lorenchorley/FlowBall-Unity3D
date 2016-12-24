using System;
using NoFlo_Basic;
using UnityEngine;

public class ButtonController : UnityGraphObject {

    public Material Hightlighted;
    private MeshRenderer Renderer;
    private Material originalMaterial;

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
        if (collision.gameObject.tag == "Player")
            TriggerEvent("Pressed");
    }

}
