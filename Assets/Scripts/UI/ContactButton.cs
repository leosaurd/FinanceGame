using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ContactButton : MonoBehaviour
{

	public void OpenURL(string url)
	{
		Application.OpenURL(url);
	}
}
