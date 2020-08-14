using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolmovementFirstTry : MonoBehaviour
{
    public Transform[] wayPoints;
	public bool randomSelection = false;
    public float moveSpeed = 20f;

	[Tooltip("How close to destination before travelling towards the next waypoint")]
	[Range(0.1f, 2f)]
    public float arrivedDistance = 0.2f;

    private int targetPoint;

	private void Start()
	{
		targetPoint = randomSelection ? Random.Range(0, wayPoints.Length) : 0;
	}

	private void Update()
    {
		Move();
		NextPoint();
    }

    private void Move()
	{
		transform.LookAt(wayPoints[targetPoint].position);
		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}

	private void NextPoint()
	{
		if (Vector3.Distance(transform.position, wayPoints[targetPoint].position) < arrivedDistance)
		{
			if (targetPoint < wayPoints.Length -1)
			{
				if (randomSelection)
				{
					targetPoint = Random.Range(0, wayPoints.Length);
				}
				else
				{
					targetPoint ++;
				}
			}
			else
			{
				targetPoint = 0;
			}

			Debug.Log($"Aiming for point: {targetPoint}");
		}
	}


	private void OnDrawGizmos()
	{
		if (wayPoints.Length > 0)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, wayPoints[targetPoint].position);


			Gizmos.color = Color.grey;
			foreach (Transform point in wayPoints)
			{
				Gizmos.DrawWireSphere(point.position, arrivedDistance);				
			}
		}
	}
}