using UnityEngine;

public class AIIdleState : AIState
{
    float timer;
    ValueCondition<float> timerCheck;
    public AIIdleState(StateAgent agent) : base(agent)
    {
        CreateTransition(nameof(AIPatrolState)).AddCondition(agent.timer, Condition.Predicate.LessOrEqual, 0);
    }

    public override void OnEnter()
    {
        agent.timer.value = Random.Range(1, 3);
        agent.movement.Stop();
    }


    public override void OnUpdate()
    {
        
    }
    public override void OnExit()
    {
        //
    }
}
