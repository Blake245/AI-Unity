using Unity.VisualScripting;
using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] AutonomousAgentData data;

    [Header("Perception")]
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;
    public Perception obstaclePerception;

    float angle;

    private void Update()
    {
        //movement.ApplyForce(Vector3.forward * 10);
        transform.position = Utilities.Wrap(transform.position, new Vector3(-15, -5, -5), new Vector3(14, 14, 14));

        //Debug.DrawRay(transform.position, transform.forward, Color.green);

        // SEEK
        if (seekPerception != null)
        {
            var gameObjects = seekPerception.GetGameObjects();
            Debug.Log($"Seeking: { gameObjects.Length}");
            if (gameObjects.Length > 0)
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
            
        }

        // Flee
        if (fleePerception != null)
        {
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        // Flock
        if (flockPerception != null)
        {
            var gameObjects = flockPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Debug.Log($"Flock: { gameObjects.Length}");
                movement.ApplyForce(Cohesion(gameObjects) * data.cohesionWeight);
                movement.ApplyForce(Seperation(gameObjects, data.seperationRadius) * data.seperationWeight);
                movement.ApplyForce(Alignment(gameObjects) * data.alignmentWeight);
            }
        }

        // Wander
        if (movement.Acceleration.sqrMagnitude == 0)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }

        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);
        }

        //Obstacle Detection
        if (obstaclePerception != null)
        {
            Vector3 direction = Vector3.zero;
            if (obstaclePerception.GetOpenDirection(ref direction))
            {
                Debug.DrawRay(transform.position, direction * 5, Color.red, 0.2f);
                movement.ApplyForce(GetSteeringForce(direction) * data.obstacleWeight);
            }
        }
    }

    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }
    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = transform.position - go.transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }
    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }

        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;

        Vector3 force = GetSteeringForce(direction);

        return force;
    }
    private Vector3 Seperation(GameObject[] neighbors, float radius)
    {
        Vector3 seperation = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            Vector3 direction = transform.position - neighbor.transform.position;
            float distance = direction.magnitude;
            if (distance < radius)
            {
                seperation += direction / (distance * distance);
            }
        }
        
        Vector3 force = GetSteeringForce(seperation);

        return force;
    }
    private Vector3 Alignment(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;

        foreach (var neighbor in neighbors)
        {
            // neighbor.velocity don't exist
        }    

        Vector3 averageVelocity = velocities / neighbors.Length;

        Vector3 force = GetSteeringForce(averageVelocity);

        return force;
    }
    private Vector3 Wander()
    {
        //randomly adjust angle +/- displacement
        angle += Random.Range(-data.displacement, data.displacement);
        //create rotation quaternion around y axis (up)
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 point = rotation * (Vector3.forward * data.radius);
        Vector3 forward = movement.Direction * data.distance;
        Vector3 force = GetSteeringForce(forward + point);

        return force;
    }

    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxForce);

        return force;
    }
}
