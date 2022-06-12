using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockAnimator : MonoBehaviour
{
    public float targetPosition;
	public BlockInstance block;
	public float speed = 0;

	private ParticleSystem ps;

	public void Start()
	{
		ps = GetComponentInChildren<ParticleSystem>();
		transform.Find("Name").GetComponent<TextMeshPro>().text = block.name;
	}

	private void FixedUpdate()
	{
		if (transform.localPosition.y > targetPosition)
		{
			speed += 0.025f;

			if (transform.localPosition.y - speed < targetPosition)
			{
				transform.localPosition = new Vector2(transform.localPosition.x, targetPosition);
				ps.Play();
				float stabilityModifier = 1 + block.stability * -1;

				CameraAnimator.Instance.ScreenShake(10 * stabilityModifier / 2, stabilityModifier / 5);
			}
			else
				transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - speed);
		}
		
	}
}
