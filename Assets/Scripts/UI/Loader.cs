using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
	public float speed = 5;

	private bool _isActive = true;
	public bool isActive
	{
		get { return _isActive; }
		set
		{
			_isActive = value;
			GetComponent<Image>().enabled = value;
		}
	}



	void FixedUpdate()
	{
		if (isActive)
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - new Vector3(0, 0, speed));
	}
}
