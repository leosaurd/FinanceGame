using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRange
{
    public float min;
    public float max;
    public FloatRange(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public float Generate()
	{
		return Random.Range(min, max);
	}
}
