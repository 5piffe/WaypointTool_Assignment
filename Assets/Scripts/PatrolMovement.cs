using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 20f;
	[SerializeField] private Color targetLinecolor = Color.green;
	public bool continousLoop = true;

    private WaypointManager wpManager;
	private float arrivedDistance = 0.1f;
	private bool backwards = false;

	private void Start()
	{
		wpManager = FindObjectOfType<WaypointManager>();

		if (wpManager.waypoints.Count == 0)
		{ wpManager.AddNewWaypoint(); }
	}

	private void Update() => Move();

	private void Move()
	{
		if (wpManager.targetWaypoint != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, wpManager.targetWaypoint.transform.position, moveSpeed * Time.deltaTime);
			float distanceToTarget = Vector3.Distance(transform.position, wpManager.targetWaypoint.transform.position);
		
			if (distanceToTarget <= arrivedDistance)
			{
				if (wpManager.targetWaypointIndex == wpManager.waypoints.Count - 1)
				{ backwards = true; }
				else if (wpManager.targetWaypointIndex == 0)
				{ backwards = false; }


				if (!continousLoop && wpManager.waypoints.Count > 1)
				{ wpManager.targetWaypointIndex += backwards ? -1 : 1; }
				else
				{ wpManager.targetWaypointIndex++; }

				wpManager.SetNextTarget();
			}

			transform.LookAt(wpManager.targetWaypoint.transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying && wpManager.targetWaypoint != null)
		{
			Gizmos.color = targetLinecolor;
			Gizmos.DrawLine(transform.position, wpManager.targetWaypoint.transform.position);
		}
	}
}