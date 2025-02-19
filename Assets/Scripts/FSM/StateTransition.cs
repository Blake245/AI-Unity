using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a transition between states with conditional logic.
/// The transition will occur when all conditions are met.
/// </summary>
public class StateTransition
{
	/// <summary>
	/// The state to transition to when conditions are met.
	/// Made readonly to prevent changes after construction.
	/// </summary>
	readonly string nextState;

	/// <summary>
	/// List of conditions that must all be true for transition to occur.
	/// Made readonly to prevent reassignment of the list itself.
	/// </summary>
	readonly List<Condition> conditions = new List<Condition>();

	/// <summary>
	/// Gets the name of the state this transition leads to.
	/// </summary>
	public string NextState => nextState;

	/// <summary>
	/// Creates a new state transition with a set of initial conditions.
	/// </summary>
	/// <param name="nextState">The state to transition to</param>
	/// <param name="conditions">Optional collection of conditions that must be met</param>
	public StateTransition(string nextState, IEnumerable<Condition> conditions)
	{
		this.nextState = nextState;
		if (conditions != null)
		{
			foreach (var condition in conditions)
			{
				AddCondition(condition);
			}
		}
	}

	/// <summary>
	/// Creates a new state transition with no initial conditions.
	/// </summary>
	/// <param name="nextState">The state to transition to</param>
	public StateTransition(string nextState)
	{
		this.nextState = nextState;
	}

	/// <summary>
	/// Adds a condition that must be met for this transition to occur.
	/// </summary>
	/// <param name="condition">The condition to add</param>
	public void AddCondition(Condition condition)
	{
		Debug.Assert(condition != null, "Condition is null.");
		conditions.Add(condition);
	}

	/// <summary>
	/// Helper method to add a value-based condition to this transition.
	/// Creates and adds a ValueCondition using the specified comparison.
	/// </summary>
	/// <typeparam name="T">Type of value to compare (must be comparable)</typeparam>
	/// <param name="value">The value reference to check in the condition</param>
	/// <param name="predicate">How to compare the value (Greater, Less, Equal, etc)</param>
	/// <param name="condition">The value to compare against</param>
	public void AddCondition<T>(ValueRef<T> value, Condition.Predicate predicate, T condition) where T : IComparable<T>
	{
		AddCondition(new ValueCondition<T>(value, predicate, condition));
	}

	/// <summary>
	/// Removes a condition from this transition.
	/// </summary>
	/// <param name="condition">The condition to remove</param>
	public void RemoveCondition(Condition condition)  // Note: Typo in method name "Remve"
	{
		Debug.Assert(condition != null, "Condition is null.");
		conditions.Remove(condition);
	}

	/// <summary>
	/// Checks if all conditions are met for this transition.
	/// </summary>
	/// <returns>
	/// True if there are no conditions or all conditions are met.
	/// False if any condition is not met.
	/// </returns>
	public bool ShouldTransition()
	{
		// If no conditions, automatically transition
		if (conditions.Count == 0)
			return true;

		// Check all conditions (AND logic)
		return conditions.TrueForAll(condition => condition.IsTrue());
	}
}