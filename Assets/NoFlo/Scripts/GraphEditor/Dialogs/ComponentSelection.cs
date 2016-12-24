using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoFloEditor {

    public class ComponentSelection : MonoBehaviour {

        public Text TitleText;
        public Button Button;
        public Type ComponentType;

        public void Setup(string Title, Type ComponentType, UnityAction action) {
            TitleText.text = Title;
            this.ComponentType = ComponentType;
            Button.onClick.AddListener(action);
        }

    }

}