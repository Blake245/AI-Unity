using Unity.VisualScripting;
using UnityEngine;

public class StateAgent : AIAgent
{
    [SerializeField] Perception perception;

    public StateMachine stateMachine = new StateMachine();

    public ValueRef<float> timer = new ValueRef<float>();
    public ValueRef<float> health = new ValueRef<float>();
    public ValueRef<float> destinationDistance = new ValueRef<float>();
    
    public ValueRef<bool> enemySeen = new ValueRef<bool>();
    public ValueRef<float> enemyDistance = new ValueRef<float>();

    public AIAgent enemy;

    private void Start()
    {
        stateMachine.AddState(nameof(AIIdleState), new AIIdleState(this));
        stateMachine.AddState(nameof(AIPatrolState), new AIPatrolState(this));

        stateMachine.SetState(nameof(AIIdleState));
    }

    private void Update()
    {
        // update parameter
        timer.value -= Time.deltaTime;

        if (perception != null)
        {
            var gameObjects = perception.GetGameObjects();
            enemySeen.value = gameObjects.Length > 0;
            if (gameObjects.Length > 0)
            {
                gameObjects[0].TryGetComponent<AIAgent>(out enemy);
                enemyDistance.value = transform.position.DistanceXZ(gameObjects[0].transform.position);
                //movement.Destination = gameObjects[0].transform.position;
            }

        }

        destinationDistance.value = transform.position.DistanceXZ(movement.Destination);

        stateMachine.CurrentState?.CheckTransitions();
        stateMachine.Update();
    }

    private void OnGUI()
    {
        // draw label of current state above agent
        GUI.backgroundColor = Color.black;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        Rect rect = new Rect(0, 0, 100, 20);
        // get point above agent
        Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
        rect.x = point.x - (rect.width / 2);
        rect.y = Screen.height - point.y - rect.height - 20;
        // draw label with current state name
        GUI.Label(rect, stateMachine.CurrentState.Name);
    }
}    
