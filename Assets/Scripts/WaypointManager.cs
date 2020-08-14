using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>(); // Ändrade här till GameObject från transform (och alla references)
    [System.NonSerialized] public /*Transform*/GameObject targetWaypoint; // Samma här ändrade från transform till GameObject osv..
    [System.NonSerialized] public int targetWaypointIndex = 0;
	private int spawnPointMove;

	private void Start()
	{
		targetWaypoint = waypoints[targetWaypointIndex];
	}

	public void SetNextTarget()
	{
		// Traverse through the list, loop back
		if (targetWaypointIndex < waypoints.Count)
		{
			targetWaypoint = waypoints[targetWaypointIndex];
		}
		else
		{
			targetWaypointIndex = targetWaypointIndex - waypoints.Count;
			targetWaypoint = waypoints[targetWaypointIndex];
		}
	}

	public void AddNewWaypoint(GameObject newWP)
	{
		waypoints.Add(newWP);
		newWP.tag = "Waypoint";
		newWP.transform.parent = transform;
		newWP.transform.localPosition = new Vector3(transform.position.x, transform.position.y, spawnPointMove);
		spawnPointMove = 0 + waypoints.Count;
	}

	public void RemoveLastWaypoint()
	{
		if (waypoints.Count > 0)
		{
			waypoints.RemoveAt(waypoints.Count - 1);
			spawnPointMove--;
		}
		else
		{
			Debug.Log("There's no waypoint to remove from list");
		}
	}
}