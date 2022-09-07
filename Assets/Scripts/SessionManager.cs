﻿
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class SessionManager : MonoBehaviour {
	#region extern functions
	[DllImport("__Internal")]
	private static extern void SetID(string id);

	[DllImport("__Internal")]
	private static extern string GetID();

	[DllImport("__Internal")]
	private static extern bool IsMobile();
	#endregion

	public static SessionManager Instance {
		get; private set;
	}

	public string previous_game_id;

	public GameSession Session = new();
	public string ID = null;


	public void Awake() {
		if (Instance != null) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		Instance = this;

		FetchPlayerID();

		StartSession();
	}

	private void FetchPlayerID() {
		if (ID == null || ID == "") {
#if UNITY_WEBGL && !UNITY_EDITOR
			string storedID = GetID();
#else
			string storedID = null;
#endif
			if (storedID != null && storedID != "") {
				ID = storedID;
			}
			else {
				ID = Guid.NewGuid().ToString();

#if UNITY_WEBGL && !UNITY_EDITOR
				SetID(ID);
#endif
			}
#if UNITY_WEBGL && !UNITY_EDITOR
			bool mobile = IsMobile();
#else
			bool mobile = false;
#endif
			NewPlayer player = new() {
				mobile = mobile
			};
			string jsonString = JsonUtility.ToJson(player);

			StartCoroutine(WebRequest.POST("/api/v1/player/" + ID, jsonString, (string response) => { }, (long status) => { Debug.Log("Failed to send player: " + status); }));

		}
	}


	public void StartSession() {
		previous_game_id = Session.id;
		Session = new() {
			player_id = ID,
			game_version = Application.version,
			id = Guid.NewGuid().ToString(),
			game_end_reason = SessionEndReason.none,
		};
	}

	public void EndSession(SessionEndReason endReason) {
		Session.game_end_reason = endReason;
		Session.game_time = Time.time;
		string jsonString = JsonUtility.ToJson(Session);
		StartCoroutine(WebRequest.POST("/api/v1/game/" + Session.id, jsonString,
			(string response) => {
				StartSession();
			},
			(long status) => {
				Debug.Log("Failed to send game data: " + status);
				StartSession();
			}));
	}

	public void SaveSession() {
		Session.game_time = Time.time;
		string jsonString = JsonUtility.ToJson(Session);
		StartCoroutine(WebRequest.POST("/api/v1/game/" + Session.id, jsonString,
			(string response) => { },
			(long status) => { Debug.Log("Failed to send game data: " + status); }));
	}
}

public class NewPlayer {
	public bool mobile;
}

[System.Serializable]
public class GameSession {
	public string id;
	public string game_secret = WebRequest.SECRET;
	public string player_id;
	public string game_version;
	public SessionEndReason game_end_reason;
	public float game_time = 0;
	public int positive_event_count = 0;
	public int negative_event_count = 0;
	public int portfolio_value = 0;
	public int insurance_count = 0;
	public int low_risk_count = 0;
	public int high_risk_count = 0;
	public int turns = 0;
}
[System.Serializable]
public enum SessionEndReason {
	GameOverStability,
	GameOverPoor,
	MainMenu,
	none
}