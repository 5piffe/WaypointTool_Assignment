using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>();
	[System.NonSerialized] public GameObject targetWaypoint;
    [System.NonSerialized] public int targetWaypointIndex = 0;
	[System.NonSerialized] public Vector3 spawnPos;
	private int spawnOffset = 1;

	private void Start()
	{
		targetWaypoint = waypoints[targetWaypointIndex];
	}

	public void SetNextTarget()
	{
		if (targetWaypointIndex < waypoints.Count)
		{
			targetWaypoint = waypoints[targetWaypointIndex];
		}
		else if (waypoints.Count > 0)
		{
			targetWaypointIndex = targetWaypointIndex - waypoints.Count;
			targetWaypoint = waypoints[targetWaypointIndex];
		}
	}

	public void AddNewWaypoint()
	{
		GameObject waypoint = new GameObject($"wp: {waypoints.Count + 1}");
		waypoints.Add(waypoint);
		waypoint.tag = "Waypoint";

		if (waypoints.Count < 2)
		{
			waypoint.transform.position = transform.position;
		}
		else
		{
			waypoint.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z + spawnOffset);
			
		}

		waypoint.transform.parent = transform;
	}

	public void RemoveLastWaypoint()
	{
		if (waypoints.Count > 0)
		{
			DestroyImmediate(waypoints.Last<GameObject>());
			waypoints.RemoveAt(waypoints.Count - 1);

			spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - spawnOffset);

			if (targetWaypoint == null)
			{
				SetNextTarget();
			}
		}
		else
		{
			Debug.Log("There's no waypoint to remove from list");
		}
	}
}