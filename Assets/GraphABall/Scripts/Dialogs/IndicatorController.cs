using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour {

    public Camera Camera;

    public List<Indicator> Overlays;

    private EventSystem EventSystem;
    private Indicator Overlay;

    void Start() {
        EventSystem = GameObject.FindObjectOfType<EventSystem>();

        for (int i = 0; i < Overlays.Count; i++) {
            Overlay = Overlays[i];

            if (Overlay is EditorIndicatorOverlay) {
                EditorIndicatorOverlay EIO = Overlay as EditorIndicatorOverlay;

            } else if (Overlay is IndicatorOverlay) {
                IndicatorOverlay IO = Overlay as IndicatorOverlay;

                IO.InitialDistance = (Camera.transform.position - IO.Target.position).magnitude;
                IO.InitialSize = IO.RectTransform.rect.size;
            }
            
        }

    }

    void LateUpdate() {

        bool show = false;
        Transform Target = null;
        bool screenSpace = false;

        for (int i = 0; i < Overlays.Count; i++) {
            Overlay = Overlays[i];

            if (Overlay is EditorIndicatorOverlay) {
                EditorIndicatorOverlay EIO = Overlay as EditorIndicatorOverlay;

                show = EIO.Activated;
                Target = EIO.Target;
                screenSpace = true;

            } else if (Overlay is IndicatorOverlay) {
                IndicatorOverlay IO = Overlay as IndicatorOverlay;

                float distanceToCamera = (Camera.transform.position - IO.Target.position).magnitude;
                show = IO.Activated && distanceToCamera <= IO.MaxDistance;

                // Adjust the overlays size depending on how far away the target object is
                IO.RectTransform.sizeDelta = IO.InitialSize * IO.ScaleModifier * IO.InitialDistance / distanceToCamera;

                screenSpace = IO.InScreenSpace;
                Target = IO.Target;

            }

            // Enable or disable the overlay depending on the distance to the target object
            if (show) {
                if (!Overlay.gameObject.activeSelf)
                    Overlay.gameObject.SetActive(true);
            } else {
                if (Overlay.gameObject.activeSelf)
                    Overlay.gameObject.SetActive(false);
            }

            if (Overlay.gameObject.activeSelf) {
                // Adjust the position of the overlay

                Vector3 screenPoint;
                if (screenSpace) {
                    screenPoint = Target.position - Vector3.forward;
                    Overlay.transform.SetParent(Target);
                } else {
                    screenPoint = Camera.WorldToScreenPoint(Target.position);
                }

                Overlay.transform.position = screenPoint;
            }

            if (!Cursor.visible)
                continue;

            // If the mouse is over the overlay, disable the overlay
            PointerEventData d = new PointerEventData(EventSystem);
            d.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.RaycastAll(d, results);

            for (int j = results.Count - 1; j >= 0; j--) {
                if (results[j].gameObject == Overlay.gameObject) {
                    Overlays.RemoveAt(i);
                    Destroy(Overlay.gameObject);
                    if (Overlay.OnRemove != null)
                        Overlay.OnRemove.Invoke();
                }
            }

        }

    }

}
 