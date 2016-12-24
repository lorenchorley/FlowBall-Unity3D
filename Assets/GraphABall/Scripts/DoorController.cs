using NoFlo_Basic;
using UnityEngine;

public class DoorController : UnityGraphObject {
    
    public Animator Animator;
    private bool isOpen;

    public Material Hightlighted;
    private MeshRenderer Renderer;
    private Material originalMaterial;

    public override string GetObjectType() {
        return "Door Controller";
    }

    public override void Setup() {
        isOpen = false;
        Renderer = GetComponent<MeshRenderer>();
        originalMaterial = Renderer.material;
    }

    public void Toggle() {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open() {
        if (!isOpen) { 
            isOpen = true;
            Animator.SetTrigger("Toggle");
            TriggerEvent("open");
        }
    }

    public void Close() {
        if (isOpen) {
            isOpen = false;
            Animator.SetTrigger("Toggle");
            TriggerEvent("closed");
        }
    }

    public override void SetHighlighted(bool enable) {
        if (enable && Cursor.visible) {
            Renderer.material = Hightlighted;
        } else {
            Renderer.material = originalMaterial;
        }
    }

}
