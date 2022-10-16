using UnityEngine;

public class ContactButton : MonoBehaviour {

	public void OpenURL(string url) {
		ClickedPlayerData CPD = new ClickedPlayerData() {
			device_id = SessionManager.Instance.ID,
			clicked_contact = true,
		};

		string json = JsonUtility.ToJson(CPD);

		StartCoroutine(WebRequest.POST("/api/v1/player",
			json,
			(string response) => { },
			(long status) => { }));
		Application.OpenURL(url);
	}
}

public class ClickedPlayerData {
	public bool clicked_contact;
	public string device_id;
}