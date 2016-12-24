using UnityEngine;
using System;

public class NextFrameCallback : MonoBehaviour {
        
	private Action callback;

	public void SetCallback(Action callback) {
        this.callback = callback;
	}

    void Update() {
        if (callback != null) { 
            callback.Invoke();
        }
        Destroy(this);
    }

}

