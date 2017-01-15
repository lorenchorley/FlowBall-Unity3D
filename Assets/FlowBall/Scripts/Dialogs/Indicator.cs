using UnityEngine;
using UnityEngine.Events;

public abstract class Indicator : MonoBehaviour {

    public UnityEvent OnRemove;
    public Indicator NextIndicator;

    public abstract void ActivateIndicator();
    public abstract void DeactivateIndicator();

    public Indicator() {
        if (OnRemove == null)
            OnRemove = new UnityEvent();

        OnRemove.AddListener(() => {
            if (NextIndicator != null)
                NextIndicator.ActivateIndicator();
        });
    }

}
