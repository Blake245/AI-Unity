using System;
using System.Collections.Generic;

/// <summary>
/// Base class for AI states that defines behavior and transitions.
/// Each concrete state should inherit from this class.
/// </summary>
public abstract class AIState
{
    /// <summary>
    /// Reference to the agent/entity that owns this state.
    /// Protected to allow access from derived states.
    /// </summary>
    protected StateAgent agent;

    /// <summary>
    /// Constructor that initializes the state with its owning agent.
    /// </summary>
    /// <param name="agent">The agent that will use this state</param>
    public AIState(StateAgent agent)
    {
        this.agent = agent;
    }

    /// <summary>
    /// Gets the name of this state, derived from the class name.
    /// </summary>
    public string Name => GetType().Name;

    /// <summary>
    /// List of possible transitions from this state to other states.
    /// Made readonly to prevent the list from being replaced.
    /// </summary>
    private readonly List<StateTransition> transitions = new List<StateTransition>();

    /// <summary>
    /// Public read-only access to state transitions.
    /// </summary>
    public IReadOnlyList<StateTransition> Transitions => transitions;

    /// <summary>
    /// Creates a new transition, adds it to this state, and returns it for further configuration.
    /// </summary>
    /// <param name="nextState">The state to transition to</param>
    /// <returns>The created transition for adding conditions</returns>
    public StateTransition CreateTransition(string nextState)
    {
        var transition = new StateTransition(nextState);
        AddTransition(transition);

        return transition;
    }

    /// <summary>
    /// Adds a new possible transition from this state.
    /// </summary>
    /// <param name="transition">The transition to add</param>
    public void AddTransition(StateTransition transition)
    {
        transitions.Add(transition);
    }

    /// <summary>
    /// Helper method to create and add a transition with a single condition.
    /// Simplifies the common case of transitioning based on a value comparison.
    /// </summary>
    /// <typeparam name="T">Type of value to compare (must be comparable)</typeparam>
    /// <param name="nextState">The state to transition to</param>
    /// <param name="value">The value to check in the condition</param>
    /// <param name="predicate">How to compare the value (Greater, Less, Equal, etc)</param>
    /// <param name="condition">The value to compare against</param>
    public void AddTransition<T>(string nextState, ValueRef<T> value, Condition.Predicate predicate, T condition) where T : IComparable<T>
    {
        var transition = new StateTransition(nextState);
        transition.AddCondition(new ValueCondition<T>(value, predicate, condition));
        AddTransition(transition);
    }

    /// <summary>
    /// Helper method to create and add a transition with a single boolean condition.
    /// </summary>
    /// <param name="nextState">The state to transition to</param>
    /// <param name="value">The boolean value to check</param>
    /// <param name="expectedValue">The value to compare against (defaults to true)</param>
    public void AddTransition(string nextState, ValueRef<bool> value, bool condition = true)
    {
        var transition = new StateTransition(nextState);
        transition.AddCondition(new BoolCondition(value, condition));
        AddTransition(transition);
    }

    /// <summary>
    /// Removes a transition from this state.
    /// </summary>
    /// <param name="transition">The transition to remove</param>
    public void RemoveTransition(StateTransition transition)
    {
        transitions.Remove(transition);
    }

    /// <summary>
    /// Evaluates all transitions from the current state in order.
    /// If any transition's conditions are met, changes to that state immediately.
    /// </summary>
    /// <returns>True if a transition occurred, false if no valid transitions found</returns>
    public bool CheckTransitions()
    {
        // Check each transition in order of priority
        foreach (var transition in transitions)
        {
            // Transition if all conditions are met
            if (transition.ShouldTransition())
            {
                // Change state and stop checking remaining transitions
                agent.stateMachine.SetState(transition.NextState);
                return true;
            }
        }

        // No valid transitions found
        return false;
    }

    /// <summary>
    /// Called when entering this state.
    /// Use to setup initial state behavior.
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// Called every frame while this state is active.
    /// Use for continuous state behavior.
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// Called when exiting this state.
    /// Use for cleanup and transitioning out of the state.
    /// </summary>
    public abstract void OnExit();
}