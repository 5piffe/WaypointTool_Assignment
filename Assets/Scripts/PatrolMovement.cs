using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 20f;
	public bool continousLoop = true;

    private WaypointManager wpManager;
	private float arrivedDistance = 0.1f;
	private bool backwards = false;

	private void Start()
	{
		wpManager = FindObjectOfType<WaypointManager>();

		if (wpManager.waypoints.Count == 0)
		{
			wpManager.AddNewWaypoint();
		}
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		if (wpManager.targetWaypoint != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, wpManager.targetWaypoint.transform.position, moveSpeed * Time.deltaTime);
			float distanceToTarget = Vector3.Distance(transform.position, wpManager.targetWaypoint.transform.position);
		
			if (distanceToTarget <= arrivedDistance)
			{
				if (wpManager.targetWaypointIndex == wpManager.waypoints.Count -1)
				{
					backwards = true;
				}
				else if (wpManager.targetWaypointIndex == 0)
				{
					backwards = false;
				}

				if (!continousLoop)
				{
					if (backwards)
					{
						wpManager.targetWaypointIndex--;
					}
					else
					{
						wpManager.targetWaypointIndex++;
					}
				}
				else
				{
					wpManager.targetWaypointIndex++;
				}

				wpManager.SetNextTarget();
			}

			transform.LookAt(wpManager.targetWaypoint.transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying && wpManager.targetWaypoint != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, wpManager.targetWaypoint.transform.position);
		}
	}
}