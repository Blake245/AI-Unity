using System;

/// <summary>
/// A condition class that compares values of any type implementing IComparable<T>.
/// This allows for comparison of numbers, strings, dates, and any other comparable types.
/// </summary>
/// <typeparam name="T">The type of value to compare. Must implement IComparable<T>.</typeparam>
public class ValueCondition<T> : Condition where T : IComparable<T>
{
	// The reference value to compare against the condition
	private ValueRef<T> parameter;

	// The value to compare the parameter against
	private T condition;

	// The type of comparison to perform
	private Predicate predicate;

	/// <summary>
	/// Creates a new value condition.
	/// </summary>
	/// <param name="parameter">The reference value to check</param>
	/// <param name="predicate">The type of comparison to perform</param>
	/// <param name="condition">The value to compare against</param>
	public ValueCondition(ValueRef<T> parameter, Predicate predicate, T condition)
	{
		this.parameter = parameter;
		this.predicate = predicate;
		this.condition = condition;
	}

	/// <summary>
	/// Checks if the condition is true based on the current parameter value
	/// and the specified predicate comparison against the condition value.
	/// </summary>
	/// <returns>True if the comparison evaluates to true, false otherwise</returns>
	public override bool IsTrue()
	{
		// CompareTo returns:
		// -1 if parameter is less than condition
		//  0 if parameter equals condition
		//  1 if parameter is greater than condition
		int comparison = parameter.value.CompareTo(condition);

		bool result = false;
		switch (predicate)
		{
			case Predicate.Greater:
				result = comparison > 0;
				break;
			case Predicate.GreaterOrEqual:
				result = comparison >= 0;
				break;
			case Predicate.Less:
				result = comparison < 0;
				break;
			case Predicate.LessOrEqual:
				result = comparison <= 0;
				break;
			case Predicate.Equal:
				result = comparison == 0;
				break;
			case Predicate.NotEqual:
				result = comparison != 0;
				break;
			default:
				break;
		}

		return result;
	}
}