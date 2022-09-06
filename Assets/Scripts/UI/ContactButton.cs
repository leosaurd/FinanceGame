using UnityEngine;

public class ContactButton : MonoBehaviour
{

	public void OpenURL(string url)
	{
		StartCoroutine(WebRequest.PUT("/api/v1/player/" + SessionManager.Instance.ID + "/clicked_contact",
			"",
			(string response) => { },
			(long status) => { }));
		Application.OpenURL(url);
	}
}
