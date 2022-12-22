using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class PlayerInfoHelper
{
#region extern functions
	[DllImport("__Internal")]
	private static extern void SetItem(string key, string value);

	[DllImport("__Internal")]
	private static extern string GetItem(string key);
	#endregion

	public static bool IsRegistered() {
#if UNITY_EDITOR
		return false;
#else
		string result = GetItem("player_info");
		return result != null;
#endif
	}

	public static void SavePlayerInfo(PlayerInfo playerInfo) {
#if !UNITY_EDITOR
		string json = JsonUtility.ToJson(playerInfo);
		SetItem("player_info", json);
#endif
	}

	public static PlayerInfo GetPlayerInfo() {
#if UNITY_EDITOR
		return null;
#else
		string result = GetItem("player_info");
		if (result != null) {
			PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(result);
			return playerInfo;
		}
		else return null;
#endif
	}
}


public class PlayerInfo {
	public string playerID;
	public string deviceID;
	public string displayName;
	public string firstName;
	public string lastName;
	public string email;
	public string mobile;
	public bool agreeTerms;
}