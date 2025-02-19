using UnityEngine;

public class AIPatrolState : AIState
{
    public AIPatrolState(StateAgent agent) : base(agent)
    {
        CreateTransition(nameof(AIIdleState)).AddCondition(agent.destinationDistance, Condition.Predicate.Less, 0.5f);
    }

    public override void OnEnter()
    {
        agent.movement.Destination = NavNode.GetRandomNavNode().transform.position;
        agent.movement.Resume();
    }
    public override void OnUpdate()
    {
       
        if (agent.destinationDistance <= 1)
        {
            agent.stateMachine.SetState(nameof(AIIdleState));
        }
    }

    public override void OnExit()
    {
      //  
    }

}
