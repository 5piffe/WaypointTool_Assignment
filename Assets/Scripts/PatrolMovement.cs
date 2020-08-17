using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
	public float moveSpeed = 20f;
    private WaypointManager wpManager;
	private float arrivedDistance = 0.1f;

	private void Start()
	{
		wpManager = FindObjectOfType<WaypointManager>();
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
				wpManager.targetWaypointIndex++;
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