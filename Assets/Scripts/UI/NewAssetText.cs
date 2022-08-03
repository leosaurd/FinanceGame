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
			opacity += 0.04f;

			if (opacity > 1f)
			{
				yPos += 1.5f;
			}

			transform.localPosition = new Vector3(0, yPos, 0);
			image.color = new Color(1, 1, 1, opacity);

			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut()
	{
		while (opacity > 0)
		{
			yPos += 1.5f;
			opacity -= 0.015f;

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
