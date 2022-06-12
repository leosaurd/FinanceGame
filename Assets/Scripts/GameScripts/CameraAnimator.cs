using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : MonoBehaviour
{
    public static CameraAnimator Instance { get; private set; }

	private Vector3 defaultPosition = new Vector3(0, 0, -10);

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void ScreenShake(float duration, float intensity)
	{
		StartCoroutine(Shake(duration, intensity));
	}

	IEnumerator Shake(float duration, float intensity)
	{
		for (int i = 0; i < duration; i++)
		{
			transform.position = defaultPosition + Random.insideUnitSphere * intensity;
			yield return new WaitForFixedUpdate();
		}
		transform.position = defaultPosition;
	}
}
