using NoFlo_Basic;
using UnityEngine;

public class LiftController : UnityGraphObject {
    
    public Animator Animator;
    private bool isRaised;

    public Material Hightlighted;
    private MeshRenderer Renderer;
    private Material originalMaterial;

    public override string GetObjectType() {
        return "Lift Controller";
    }

    public override void Setup() {
        isRaised = false;
        Renderer = GetComponent<MeshRenderer>();
        originalMaterial = Renderer.material;
    }

    public void Toggle() {
        if (isRaised)
            Lower();
        else
            Raise();
    }

    public void Raise() {
        if (!isRaised) { 
            isRaised = true;
            Animator.SetTrigger("Toggle");
            TriggerEvent("raised");
        }
    }

    public void Lower() {
        if (isRaised) {
            isRaised = false;
            Animator.SetTrigger("Toggle");
            TriggerEvent("lowered");
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
