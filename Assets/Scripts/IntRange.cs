using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntRange
{
	public int min;
	public int max;
	public IntRange(int min, int max)
	{
		this.min = min;
		this.max = max;
	}

	public int New()
	{
		return Random.Range(min, max);
	}
}
