using System;
using System.Collections;
using UnityEngine.Networking;

public static class WebRequest {
	public const string SECRET = "b7c97be8-cf53-4500-9c27-7d262fd07510";
	public const string URL = "http://localhost:8080";

	public static IEnumerator GET(string path, Action<string> success, Action<string> fail) {
		using UnityWebRequest webRequest = UnityWebRequest.Get(URL + path);
		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success) {
			string response = webRequest.downloadHandler.text;
			success(response);
		}
		else {
			fail(webRequest.responseCode.ToString());
		}
	}

	public static IEnumerator POST(string path, string body, Action<string> success, Action<long> fail) {
		using UnityWebRequest webRequest = UnityWebRequest.Post(URL + path, body);
		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success) {
			string response = webRequest.downloadHandler.text;
			success(response);
		}
		else {
			fail(webRequest.responseCode);
		}
	}

	public static IEnumerator PUT(string path, string body, Action<string> success, Action<long> fail) {
		using UnityWebRequest webRequest = UnityWebRequest.Put(URL + path, body);
		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success) {
			string response = webRequest.downloadHandler.text;
			success(response);
		}
		else {
			fail(webRequest.responseCode);
		}
	}
}