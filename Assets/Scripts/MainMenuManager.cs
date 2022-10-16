using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	public void StartGame() {
		SceneManager.LoadScene("Game");
		SessionManager.Instance.StartSession();
	}

}
