﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class SessionManager : MonoBehaviour
{
	#region extern functions
	[DllImport("__Internal")]
	private static extern void SetID(string id);

	[DllImport("__Internal")]
	private static extern string GetID();
	#endregion

	public static SessionManager Instance { get; private set; }

	public GameSession Session = new();
	public string ID = null;


	public void Awake()
	{
		if (Instance != null) { Destroy(gameObject); return; }

		DontDestroyOnLoad(gameObject);
		Instance = this;

		if (ID == null || ID == "")
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			string storedID = GetID();
			Debug.Log(storedID);
#else
			string storedID = null;
#endif
			if (storedID != null && storedID != "")
			{
				ID = storedID;
			}
			else
			{
				ID = Guid.NewGuid().ToString();
#if UNITY_WEBGL && !UNITY_EDITOR
				SetID(ID);
#endif
			}
		}
		StartSession();
	}

	public void StartSession()
	{
		Session = new()
		{
			StartTime = Time.time,
			SessionID = Guid.NewGuid().ToString()
		};
	}

	public void EndSession(SessionEndReason endReason)
	{
		Session.SessionEndReason = endReason;
		StartCoroutine(SendSession(
			() =>
			{
				StartSession();
			}));
	}

	public void SaveSession()
	{
		StartCoroutine(SendSession());
	}

	IEnumerator SendSession()
	{
		Session.CurrentTime = Time.time;

		string jsonString = JsonUtility.ToJson(Session);

		using UnityWebRequest webRequest = UnityWebRequest.Put("https://test.trentshailer.com/analytics/" + ID, jsonString);
		webRequest.SetRequestHeader("Content-Type", "application/json");

		yield return webRequest.SendWebRequest();

		if (webRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(webRequest.error);
		}
	}

	IEnumerator SendSession(Action callback)
	{
		Session.CurrentTime = Time.time;

		string jsonString = JsonUtility.ToJson(Session);

		using UnityWebRequest webRequest = UnityWebRequest.Put("https://test.trentshailer.com/analytics/" + ID, jsonString);
		webRequest.SetRequestHeader("Content-Type", "application/json");

		yield return webRequest.SendWebRequest();

		if (webRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(webRequest.error);
		}
		else
			callback.Invoke();
	}

}

[System.Serializable]
public class GameSession
{
	public string SessionID;


	public BlockInstance[] Tower;

	public int InsuranceCount = 0;
	public int LowRiskCount = 0;
	public int HighRiskCount = 0;


	public int TotalEarnings = 0;

	public List<float> Stability = new();

	public float StartTime;
	public float CurrentTime;

	public SessionEndReason SessionEndReason;
}
[System.Serializable]
public enum SessionEndReason
{
	GameOverStability,
	GameOverPoor,
	MainMenu,
	ClosedGame
}