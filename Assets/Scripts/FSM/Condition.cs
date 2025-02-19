using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition
{
	public enum Predicate
	{
		Greater,
		GreaterOrEqual,
		Less,
		LessOrEqual,
		Equal,
		NotEqual
	}

	public abstract bool IsTrue();
}
