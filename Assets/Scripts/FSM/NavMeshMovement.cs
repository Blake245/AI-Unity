using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : Movement
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] 

    public override Vector3 Velocity 
    { 
        get => navMeshAgent.velocity; 
        set => navMeshAgent.velocity = value; 
    }

    public override Vector3 Destination 
    { 
        get => navMeshAgent.destination; 
        set => navMeshAgent.destination = value; 
    }

    private void Update()
    {
        navMeshAgent.speed = data.maxSpeed;
        navMeshAgent.acceleration = data.maxForce;
        navMeshAgent.angularSpeed = data.turnRate;
    }

    public override void ApplyForce(Vector3 force)
    {
     //   
    }

    public override void MoveTowards(Vector3 position)
    {
        navMeshAgent.destination = position;
        
    }

    public override void Resume()
    {
        navMeshAgent.isStopped = false;
    }

    public override void Stop()
    {
        navMeshAgent.isStopped = true;
    }
}
