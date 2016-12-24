using NoFlo_Basic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoFloEditor {

    public class UIEventRedirector : MonoBehaviour, ISelectHandler, IDeselectHandler {

        public Action OnSelectHandler;
        public Action OnDeselectHandler;

        public void Setup(Action OnSelectHandler, Action OnDeselectHandler) {
            this.OnSelectHandler = OnSelectHandler;
            this.OnDeselectHandler = OnDeselectHandler;
        }

        public void OnSelect(BaseEventData eventData) {
            if (OnSelectHandler != null)
                OnSelectHandler.Invoke();
        }

        public void OnDeselect(BaseEventData eventData) {
            if (OnDeselectHandler != null)
                OnDeselectHandler.Invoke();
        }
    }

}