using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public Transform player;
    private Transform ghost;
    private float pause_count_down;
    private Vector3 start;
    private Vector3 end;

    int m_CurrentWaypointIndex;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        ghost = GetComponent<Transform>();
        pause_count_down = 0.0f;
    }

    void Update()
    {
        pause_count_down = pause_count_down - Time.deltaTime;
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        
        Vector3 difference = ghost.position - player.position;
        
        if (Vector3.Dot(difference, difference) < 15 && pause_count_down < -5.0f)
        {
            pause_count_down = 5.0f;
            navMeshAgent.isStopped = true;
            start = ghost.forward;
            end = -1.0f * difference;
        }
        
        if (pause_count_down <= 0)
        {
            navMeshAgent.isStopped = false;
        }

        if (pause_count_down > 0)
        {
            Vector3 lookdir = Vector3.Slerp(start, end, 0.30f * ( 5.0f - pause_count_down));
            ghost.rotation = Quaternion.LookRotation(lookdir);
        }

    }
}

