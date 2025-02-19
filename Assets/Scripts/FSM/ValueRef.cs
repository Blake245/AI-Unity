/// <summary>
/// A serializable wrapper that makes values shareable across Unity components.
/// Enables multiple components to reference and modify the same value.
/// </summary>
/// <typeparam name="T">The type of value to share</typeparam>
[System.Serializable]
public class ValueRef<T>
{
	/// <summary>
	/// The value being shared. Serialized to be visible in Unity's Inspector.
	/// </summary>
	public T value;

	/// <summary>
	/// Creates a new wrapper with default(T) as the initial value.
	/// </summary>
	public ValueRef() { }

	/// <summary>
	/// Creates a new wrapper with the specified initial value.
	/// </summary>
	/// <param name="value">Initial value to store</param>
	public ValueRef(T value) { this.value = value; }

	/// <summary>
	/// Allows using this wrapper directly as its contained type.
	/// For example: if(healthRef) instead of if(healthRef.value)
	/// </summary>
	/// <param name="reference">The wrapper to convert from</param>
	public static implicit operator T(ValueRef<T> reference) => reference.value;
}