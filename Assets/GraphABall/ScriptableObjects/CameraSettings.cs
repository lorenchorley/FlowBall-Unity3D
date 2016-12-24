using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Crappy/CameraSettings")]
public class CameraSettings : ScriptableObject {

    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float zoomSpeed = 0.1f;

    public float LowerPitchExtent = -30f;
    public float UpperPitchExtent = 35f;

    public float LowerZoomExtent = 0.3f;
    public float UpperZoomExtent = 1.7f;

}
