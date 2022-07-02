using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
	private void Awake()
	{
		StartCoroutine(Kill());
	}

	IEnumerator Kill()
	{
		yield return new WaitForSeconds(3);
		Destroy(gameObject);
	}
}
