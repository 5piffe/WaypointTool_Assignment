using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

/* 
 * When mouseclick/hover on a handle, figure out how to select that gameobject so you can move.
 * then you can hide them in hierarchy so you can't fuck up by deleting them from there
 * 
 * UNDO REDO still left to do
 */


public class WaypointTool : EditorWindow // är ett window
{
	private WaypointManager wpManager;
	private List<GameObject> addedWaypoints = new List<GameObject>();

    [MenuItem("Tools/Waypoint Tool")]
    public static void OpenTool()
	{
		GetWindow<WaypointTool>("WaypointYoda"); // opens a windows if there's none, focuses on window if you opened it already
	}

	private void OnEnable()
	{
		SceneView.duringSceneGui += DuringSceneGUI;
	}

	private void OnDisable()
	{
		SceneView.duringSceneGui -= DuringSceneGUI;
	}

	private void OnGUI() // GUI stuff för window
	{
		GUILayout.Label("Waypoint stuff");
		wpManager = FindObjectOfType<WaypointManager>();
		
		if (GUILayout.Button("Add +"))
		{
			GameObject waypoint = new GameObject($"wp: {wpManager.waypoints.Count + 1}");
			wpManager.AddNewWaypoint(waypoint);

			addedWaypoints.Add(waypoint);
		}

		if (GUILayout.Button("Remove -"))
		{
			wpManager.RemoveLastWaypoint();

			if (addedWaypoints.Count > 0)
			{
				DestroyImmediate(addedWaypoints.Last<GameObject>());
				addedWaypoints.RemoveAt(addedWaypoints.Count -1);

			}
		}
	}


	private void DuringSceneGUI(SceneView sceneView)
	{
		DrawHandles();
	}

	private void DrawHandles()
	{

		

		if (wpManager.waypoints.Count > 0)
		{
			foreach (GameObject waypoint in wpManager.waypoints)
			{
				if (waypoint == null)
				{
					wpManager.RemoveLastWaypoint();
					Debug.Log("Could not draw handle, missing Waypoint object in scene - removing from list");
					//wpManager.waypoints.RemoveAt(wpManager.waypoints.Count -1);
				}
				else
				{
					Handles.DoPositionHandle(waypoint.transform.position, Quaternion.identity); // Gets error if there's an item in the list but you've deleted it from the hierarchy
				}
			}
		}
	}
}
