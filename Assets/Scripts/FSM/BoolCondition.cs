/// <summary>
/// A condition class that evaluates a boolean reference against an expected value.
/// Inherits from the base Condition class.
/// </summary>
public class BoolCondition : Condition
{
	/// <summary>
	/// The boolean reference value to check.
	/// This allows multiple classes to share and monitor the same boolean value.
	/// ValueRef<bool> has an implicit conversion to bool, so it can be used directly in comparisons.
	/// </summary>
	private ValueRef<bool> parameter;

	/// <summary>
	/// The expected boolean value to compare against.
	/// </summary>
	private bool condition;

	/// <summary>
	/// Creates a new boolean condition.
	/// </summary>
	/// <param name="parameter">The boolean reference to monitor</param>
	/// <param name="condition">The expected value to compare against (defaults to true)</param>
	public BoolCondition(ValueRef<bool> parameter, bool condition = true)
	{
		this.parameter = parameter;
		this.condition = condition;
	}

	/// <summary>
	/// Checks if the referenced boolean value matches the expected condition.
	/// Uses ValueRef's implicit conversion to bool for direct comparison.
	/// </summary>
	/// <returns>True if the parameter equals the condition value, false otherwise</returns>
	public override bool IsTrue()
	{
		return (parameter == condition);
	}
}