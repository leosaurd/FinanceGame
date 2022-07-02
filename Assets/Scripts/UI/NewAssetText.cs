using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewAssetText : MonoBehaviour
{
	float opacity = 0;
	float yPos = 0;
	Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
		StartCoroutine(ForceDeath());
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		while (opacity < 1)
		{
			yPos += 1.5f;
			opacity += 0.04f;

			transform.localPosition = new Vector3(0, yPos, 0);
			image.color = new Color(1, 1, 1, opacity);

			yield return new WaitForFixedUpdate();
		}
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut()
	{
		while (opacity > 0)
		{
			yPos += 1.5f;
			opacity -= 0.02f;

			transform.localPosition = new Vector3(0, yPos, 0);
			image.color = new Color(1, 1, 1, opacity);

			yield return new WaitForFixedUpdate();
		}

		Destroy(gameObject);
	}


	IEnumerator ForceDeath()
	{
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}
}
