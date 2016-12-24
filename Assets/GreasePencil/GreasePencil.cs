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

## History

2015/09/07 - v1 - Basic functionality

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_ASSERTIONS
using UnityEngine.Assertions;
#endif

public class GreasePencil : MonoBehaviour
{
	[System.Serializable]
	public class GreasePencilStroke
	{
		public GreasePencilStroke ()
		{
			points = new List<Vector3> ();
		}
		
		public GreasePencilStroke (Vector3 initialPosition)
		{
			points = new List<Vector3> ();
			
			points.Add (initialPosition);
			points.Add (initialPosition);
		}
		
		public List<Vector3> points;
	
		public void Add (Vector3 position)
		{
			if (points == null)
			{
				points = new List<Vector3> ();
			}
			
			points.Add (position);
			lastPoint = position;
		}
		
		public Vector3 lastPoint
		{
			get
			{
				return points[points.Count-1];
			}
		
			set
			{
				if (points.Count > 0)
				{
					points[points.Count-1] = value;
				}
			}
		}
		
		public bool IsEmpty
		{
			get { return points == null || points.Count == 0; }
		}
		
		public float LastDistance ()
		{
			if (points.Count > 0)
			{
				return (points[points.Count-2] - lastPoint).magnitude;
			}
			
			return 0f;
		}

		public void Clear ()
		{
			points.Clear ();
		}
	}

	[System.Serializable]
	public class GreasePencilLayer
	{
		public GreasePencilLayer ()
		{
			strokes = new List<GreasePencilStroke> ();
			enabled = true;
			width = 4f;
		}
		
		public bool enabled;
		
		[HideInInspector] public List<GreasePencilStroke> strokes;
		
		public Color strokeColor = Color.black;
		public Color fillColor = Color.clear;
		
		[Range (1f, 5f)] public float width = 1f;
		
		[HideInInspector] public GameObject gameObject;
		
		GreasePencilStroke currentStroke;
		
		float _worldSize = 1f;
		
		public void StartStroke (Vector3 position, float worldSize)
		{
			currentStroke = new GreasePencilStroke (position);
			strokes.Add (currentStroke);
			_worldSize = worldSize;
		}
		
		public void UpdateStroke (Vector3 position)
		{
			currentStroke.lastPoint = position;
			
			if (currentStroke.LastDistance () > (0.1f * _worldSize))
			{
				currentStroke.Add (position);
			}
		}
		
		public void EndStroke (Vector3 position)
		{
			currentStroke.lastPoint = position;
		}
		
		public void Erase (int stroke, List<int> points)
		{
#if UNITY_ASSERTIONS
			Assert.IsTrue (stroke >= 0 && stroke < strokes.Count);
#endif
		
			/*// erasing from the ends, just remove points
			if ((point == 0) || point == strokes[stroke].points.Count-1)
			{
				strokes[stroke].points.RemoveAt (point);
				return;
			}
			
			// erasing from the middle, split the stroke
			GreasePencilStroke newStroke = new GreasePencilStroke ();
			
			strokes[stroke].points.RemoveAt (point);
			
			for (int i=point; i<strokes[stroke].points.Count; ++i)
			{
				newStroke.Add (strokes[stroke].points[i]);
			}
			
			for (int i=point; i<strokes[stroke].points.Count; ++i)
			{
				strokes[stroke].points.RemoveAt (point);
			}
			
			strokes.Add (newStroke);*/
			
			GreasePencilStroke oldStroke = strokes[stroke];
			GreasePencilStroke newStroke = null;
			List<GreasePencilStroke> newStrokes = new List<GreasePencilStroke> ();
			
			for (int i=0; i<oldStroke.points.Count; ++i)
			{
				if (!points.Contains (i))
				{
					if (newStroke == null)
					{
						newStroke = new GreasePencilStroke ();
						newStrokes.Add (newStroke);
					}
					
					newStroke.Add (oldStroke.points[i]);
				}
				else if (newStroke != null)
				{
					newStroke = null;
				}
			}
			
			if (newStrokes.Count > 0)
			{
				strokes[stroke] = newStrokes[0];
				
				for (int i=1; i<newStrokes.Count; ++i)
				{
					strokes.Add (newStrokes[i]);
				}
			}
			else
			{
				strokes[stroke].Clear ();
			}
			
			newStrokes.RemoveAll (s => s.IsEmpty);
		}
	}
	
	public List<GreasePencilLayer> layers;
	
	public int activeLayer;
	
	public GreasePencilLayer ActiveLayer
	{
		get
		{
			if ((layers == null) || (layers.Count == 0))
			{
				AddLayer ();
			}
		
			if ((activeLayer >= 0) && (activeLayer < layers.Count))
			{
				return layers[activeLayer];
			}
			
			return null;
		}
	}
	
	public void StartStroke (Vector3 position, float worldSize)
	{
		if (ActiveLayer != null)
		{
			ActiveLayer.StartStroke (position, worldSize);
		}
	}
	
	public void UpdateStroke (Vector3 position)
	{
		if (ActiveLayer != null)
		{
			ActiveLayer.UpdateStroke (position);
		}
	}
	
	public void EndStroke (Vector3 position)
	{
		if (ActiveLayer != null)
		{
			ActiveLayer.EndStroke (position);
		}
	}
	
	public void AddLayer ()
	{
		if (layers == null)
		{
			layers = new List<GreasePencilLayer> ();
		}
		
		activeLayer = layers.Count;
		layers.Add (new GreasePencilLayer ());
	}
	
	void OnValidate ()
	{
		activeLayer = Mathf.Clamp (activeLayer, 0, layers.Count-1);
	}
}
