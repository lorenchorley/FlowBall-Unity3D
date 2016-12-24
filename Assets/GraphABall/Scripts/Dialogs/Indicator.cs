using UnityEngine;
using UnityEngine.Events;

public abstract class Indicator : MonoBehaviour {

    public UnityEvent OnRemove;

    public abstract void ActivateIndicator();
    public abstract void DeactivateIndicator();

}
