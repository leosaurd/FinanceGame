using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
	[Range(0, 10)]
	public float radius;
	[Range(0, 0.5f)]
	public float step;
	[Range(-1, 1)]
	public float startOffset;

	public int direction = 1;

	public bool overrideRandom = false;

	private float startY;
	private float maxY;
	private float minY;
	private float target;


	private void Awake()
	{
		if (!overrideRandom)
		{
			startOffset = Random.Range(-1f, 1f);
			step = Random.Range(0.05f, 0.15f);
			radius = Random.Range(3, 10);
		}

		startY = transform.localPosition.y + radius * startOffset;

		maxY = startY + radius;
		minY = startY - radius;

		if (direction == -1)
		{
			target = minY;
		}
		else if (direction == 1)
		{
			target = minY;
		}

	}


	private void FixedUpdate()
	{
		if (transform.localPosition.y >= maxY && direction == 1)
		{
			direction = -1;
			target = minY;
		}
		else if (transform.localPosition.y <= minY && direction == -1)
		{
			direction = 1;
			target = maxY;
		}

		float distance = Mathf.Abs(target - transform.localPosition.y);
		// between 0 and 1
		float scaledDistance = distance / (radius * 2);

		// between -Pi and Pi
		float functionDistance = scaledDistance * 2 * Mathf.PI - Mathf.PI;

		float speed = Mathf.Cos(functionDistance) / 4 + 0.75f;

		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + step * direction * speed, transform.localPosition.z);
	}
}
