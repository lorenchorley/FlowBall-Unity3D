/*
The MIT License (MIT)

Copyright (c) 2015 Keith Boshoff

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof (GreasePencil))]
public class GreasePencilEditor : Editor
{
	GreasePencil instance;
	bool drawing;
	bool erasing;
	
	public static float eraserSize
	{
		get
		{
			return EditorPrefs.GetFloat ("GP_eraserSize", 50f);
		}
		
		set
		{
			EditorPrefs.SetFloat ("GP_eraserSize", value);
		}
	}
	
	void OnEnable ()
	{
		instance = target as GreasePencil;
	}
	
	public override void OnInspectorGUI ()
	{
		drawing = GUILayout.Toggle (drawing, "Draw", "Button");
		
		eraserSize = EditorGUILayout.Slider ("Eraser Size", eraserSize, 10f, 100f);
		GUILayout.Label ("Shift+LMB-Drag to Erase", EditorStyles.miniBoldLabel);
		
		serializedObject.Update ();
		
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("layers"), true);
		
		if (GUILayout.Button ("Add Layer"))
		{
			serializedObject.ApplyModifiedProperties ();
			instance.AddLayer ();
			serializedObject.Update ();
			drawing = true;
		}
		
		EditorGUILayout.IntSlider (serializedObject.FindProperty ("activeLayer"), 0, serializedObject.FindProperty ("layers").arraySize-1);
		serializedObject.ApplyModifiedProperties ();
	}
	
	void OnSceneGUI ()
	{
		Event evt = Event.current;
		
		if ((evt.type == EventType.KeyUp) && (evt.keyCode == KeyCode.D))
		{
			evt.Use ();
			drawing = !drawing;
		}
		
		if (drawing)
		{
			Handles.BeginGUI ();
			GUILayout.BeginArea (new Rect(Screen.width - 150, Screen.height - 65, 140,20), "Grease Pencil Drawing", "Box");
			GUILayout.EndArea();
			Handles.EndGUI ();
		
			if (evt.alt)
			{
				// don't hijack camera controls
				return;
			}
			
			float distance = 0f;
			
			Vector3 screenPosition = evt.mousePosition;
			screenPosition.y = Screen.height - (screenPosition.y + 38f);
			
			Camera cam = SceneView.currentDrawingSceneView.camera;
			
			Ray ray = cam.ScreenPointToRay (screenPosition);
			Plane plane = new Plane (cam.transform.forward, instance.transform.position);
			
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			
			if (plane.Raycast (ray, out distance))
			{
				Vector3 worldPosition = ray.GetPoint (distance);
				
				if ((evt.button == 0) && (!evt.shift))
				{
					if (evt.type == EventType.MouseDown)
					{
						instance.StartStroke (worldPosition, HandleUtility.GetHandleSize (instance.transform.position));
						evt.Use ();
						EditorUtility.SetDirty (instance);
					}
					else if (evt.type == EventType.MouseDrag)
					{
						instance.UpdateStroke (worldPosition);
						evt.Use ();
						EditorUtility.SetDirty (instance);
					}
					else if (evt.type == EventType.MouseUp)
					{
						instance.EndStroke (worldPosition);
						evt.Use ();
						EditorUtility.SetDirty (instance);
					}
				}
				else if ((evt.button == 0) && (evt.shift))
				{
					if ((evt.type == EventType.MouseDrag) || (evt.type == EventType.MouseDown))
					{
						erasing = true;
						EditorGUIUtility.AddCursorRect (new Rect (0f, 0f, Screen.width, Screen.height), MouseCursor.Arrow);
						EditorGUIUtility.SetWantsMouseJumping (0);
						
						// erase
						List<int> points = new List<int> (100);
						GreasePencil.GreasePencilStroke stroke;
						
						for (int s=0; s<instance.ActiveLayer.strokes.Count; ++s)
						{
							stroke = instance.ActiveLayer.strokes[s];
							points.Clear ();
							
							for (int i=0; i<stroke.points.Count; ++i)
							{
								if ((cam.WorldToScreenPoint (stroke.points[i]) - screenPosition).magnitude < eraserSize)
								{
									points.Add (i);
								}
							}
							
							instance.ActiveLayer.Erase (s, points);
						}
						
						instance.UpdateStroke (worldPosition);
						evt.Use ();
						EditorUtility.SetDirty (instance);
					}
					else if ((evt.type == EventType.MouseUp) || (evt.type == EventType.MouseMove)
						|| ((evt.type == EventType.KeyUp) && ((evt.keyCode == KeyCode.LeftShift) || (evt.keyCode == KeyCode.RightShift))))
					{
						erasing = false;
						SceneView.RepaintAll ();
					}
					else if (evt.type == EventType.Repaint)
					{
						Handles.BeginGUI ();
						Handles.matrix = Matrix4x4.identity;
						Handles.color = erasing?Color.red:Color.black;
						Handles.DrawWireDisc (evt.mousePosition, Vector3.forward, eraserSize);
						Handles.color = new Color (erasing?1f:0f, 0f, 0f, 0.125f);
						Handles.DrawSolidDisc (evt.mousePosition, Vector3.forward, eraserSize);
						Handles.EndGUI ();
					}
				}
			}
		}
	}
	
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	[DrawGizmo (GizmoType.Active | GizmoType.NotSelected | GizmoType.Selected)]
#else
	[DrawGizmo (GizmoType.Active | GizmoType.NonSelected)]
#endif
	static void DrawGizmoForMyScript (GreasePencil instance, GizmoType gizmoType)
	{
		if (instance.layers != null)
		{
			for (int i=0; i<instance.layers.Count; ++i)
			{
				if (instance.layers[i].enabled && (instance.layers[i].strokes != null))
				{
					Handles.color = instance.layers[i].strokeColor;
					
					if (Handles.color.a > 0f)
					{
						for (int s=0; s<instance.layers[i].strokes.Count; ++s)
						{
							Handles.DrawAAPolyLine (instance.layers[i].width, instance.layers[i].strokes[s].points.ToArray ());
						}
					}
					
#if UNITY_5
					Handles.color = instance.layers[i].fillColor;
					
					if (Handles.color.a > 0f)
					{
						for (int s=0; s<instance.layers[i].strokes.Count; ++s)
						{
							Handles.DrawAAConvexPolygon (instance.layers[i].strokes[s].points.ToArray ());
						}
					}
#endif
				}
			}
		}
	}
}
