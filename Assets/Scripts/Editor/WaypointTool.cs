using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class WaypointTool : EditorWindow // är ett window
{
	private WaypointManager wpManager;
	private PatrolMovement traverseObject;
	private const string UNDO_STR_MOVEWAYPOINT = "move waypoint";
	private float waypointCircleRadius = 0.3f;
	private float minCircleRadius = 0.1f;
	private float maxCircleRadius = 5f;
	private bool hideWaypoints = false;
	private Color wpColor = Color.cyan;
	private Color pathColor = Color.yellow;


    [MenuItem("Tools/Waypoint Tool")]
    public static void OpenTool()
	{
		GetWindow<WaypointTool>("Waypoint tool"); // opens a windows if there's none, focuses on window if you opened it already
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
		GUILayout.Label("[Add or remove waypoints]");
		wpManager = FindObjectOfType<WaypointManager>();
		traverseObject = FindObjectOfType<PatrolMovement>();
		
		if (GUILayout.Button("Add +"))
		{
			wpManager.AddNewWaypoint();
		}

		if (GUILayout.Button("Remove -"))
		{
			wpManager.RemoveLastWaypoint();
		}
		
		GUILayout.Label("\n[Other settings]");
		wpColor = EditorGUILayout.ColorField("Circle color", wpColor);
		waypointCircleRadius = EditorGUILayout.Slider("Circle radius", waypointCircleRadius, minCircleRadius, maxCircleRadius);
		pathColor = EditorGUILayout.ColorField("Path color", pathColor);
		hideWaypoints = EditorGUILayout.Toggle("Hide waypoints", hideWaypoints);

	}


	private void DuringSceneGUI(SceneView sceneView) // updates
	{
		if (!hideWaypoints)
		{
			DrawWaypoints();
		}
	}

	private void DrawWaypoints()
	{
		if (wpManager != null)
		{
			if (wpManager.waypoints.Count > 0)
			{
				foreach (GameObject waypoint in wpManager.waypoints)
				{
					if (waypoint == null)
					{
						wpManager.RemoveLastWaypoint();
						Debug.Log("Could not draw handle, missing Waypoint object in scene - removing from list");
					}
					else
					{
						Undo.RecordObject(waypoint.transform, UNDO_STR_MOVEWAYPOINT);
						waypoint.transform.position = Handles.PositionHandle(waypoint.transform.position, Quaternion.identity);
						DrawWPSpheres(waypoint.transform, waypointCircleRadius);
						//DrawWPPath(waypoint.transform.position, ); // Draw from start to previous waypoint
						wpManager.spawnPos = waypoint.transform.position;
					}
				}
			}

			for (int i = 0; i < wpManager.waypoints.Count; i++)
			{
				if (i < wpManager.waypoints.Count - 1)
				{
					DrawWPPath(wpManager.waypoints[i].transform.position, wpManager.waypoints[i + 1].transform.position);
				}
				else if (i == wpManager.waypoints.Count -1 && traverseObject.continousLoop)
				{
					DrawWPPath(wpManager.waypoints[i].transform.position, wpManager.waypoints[0].transform.position);
				}
			}
		}
	}

	private void DrawWPSpheres(Transform center, float radius)
	{
		Handles.color = wpColor;
		Handles.DrawWireDisc(center.position, Camera.current.transform.position, radius);
	}

	private void DrawWPPath(Vector3 start, Vector3 end)
	{
		Handles.color = pathColor;
		Handles.DrawDottedLine(start, end, 6f);
	}
}