using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
	public static SettingsManager Instance {
		get; private set;
	}

	public bool music = true;
	public bool sfx = true;

	public bool seenTutorial = false;


	public Sprite[] soundSprites;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			GameObject c = EventSystem.current.currentSelectedGameObject;
			if (c == null) {
				return;
			}

			Selectable s = c.GetComponent<Selectable>();
			if (s == null) {
				return;
			}

			Selectable jump = Input.GetKey(KeyCode.LeftShift)
				? s.FindSelectableOnUp() : s.FindSelectableOnDown();
			if (jump != null) {
				jump.Select();
			}
		}
	}
}
