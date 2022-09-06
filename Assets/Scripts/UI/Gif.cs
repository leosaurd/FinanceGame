using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gif : MonoBehaviour
{
	public Sprite[] frames;
	public int frameRate;

	private Image image;

	private int frame = 0;

	void Awake()
	{
		image = GetComponent<Image>();
		//StartLoop();
	}

	public void StartLoop()
	{
		StartCoroutine(Animate());
	}

	IEnumerator Animate()
	{
		image.sprite = frames[frame];
		frame++;
		if (frame == frames.Length) frame = 0;
		yield return new WaitForSeconds(1f / frameRate);

		StartCoroutine(Animate());
	}
}
