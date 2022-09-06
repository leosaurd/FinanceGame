using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ContactButton : MonoBehaviour
{

	public void OpenURL(string url)
	{
		SessionManager.Instance.Session.ClickedContact = true;
		SessionManager.Instance.SaveSession();
		Application.OpenURL(url);
	}
}
