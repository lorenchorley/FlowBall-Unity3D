using UnityEngine;
using System;

public class DelayedCallback : MonoBehaviour {
        
	public Action callback;

	public void SetTimedCallback(Action callback, float delayInSeconds) {
        this.callback = callback;
        Invoke("callbackFunction", delayInSeconds);
	}

    private void callbackFunction() {
        if (callback != null)
            callback.Invoke();
        Destroy(this);
    }
        
}

