using NoFloEditor;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public CameraSettings CameraSettings;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float zoom = 1.0f;

    public PlayerController player;
    public GraphEditor GraphEditor;

    private Vector3 offset;

	void Start () {
		// Create an offset by subtracting the Camera's position from the player's position
		offset = transform.position - player.transform.position;
        Cursor.lockState = CursorLockMode.Confined;
	}

	void LateUpdate () {

        if (GraphEditor != null && GraphEditor.isOpen)
            return;

        if (Input.GetKey(KeyCode.Space)) {

            if (!Cursor.visible) { 
                Cursor.visible = true;
            }

        } else {

            if (Cursor.visible && !player.LosersDialog.gameObject.activeSelf && !player.WinnersDialog.gameObject.activeSelf) { 
                Cursor.visible = false;
            }

            // Adjust current pitch, yaw and zoom values according to mouse input
            yaw += CameraSettings.speedH * Input.GetAxis("Mouse X");
            pitch -= CameraSettings.speedV * Input.GetAxis("Mouse Y");
            zoom -= CameraSettings.zoomSpeed * Input.mouseScrollDelta.y;

            // Calculate a yaw rotation to be applied later
            Quaternion yawRotation = Quaternion.AngleAxis(yaw, Vector3.up);

            // Clamp the pitch and zoom values to their min and max extents
            pitch = Mathf.Clamp(pitch, CameraSettings.LowerPitchExtent, CameraSettings.UpperPitchExtent);
            zoom = Mathf.Clamp(zoom, CameraSettings.LowerZoomExtent, CameraSettings.UpperZoomExtent);

            // Set the position of the camera according to the players positions, and the rotated camera offset
            transform.position = player.transform.position + yawRotation * Quaternion.AngleAxis(pitch, Vector3.right) * (zoom * offset);
            
            transform.LookAt(player.transform);

            player.rotation = yawRotation;

        }

    }

}