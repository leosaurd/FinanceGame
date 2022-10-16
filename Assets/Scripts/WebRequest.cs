using System;
using System.Collections;
using UnityEngine.Networking;

public static class WebRequest {
	public const string SECRET = "fmzAs9noj1eiNpNUuUl0ujrwOWN9DEjtLd1RHRtjyKoHb59paDWhzW3sklkBXsFY";
#if UNITY_EDITOR
	//public const string URL = "https://investible.trentshailer.com";
	public const string URL = "http://localhost:4008";
#else
	public const string URL = "https://api_investible.ippfa.com";
#endif

	public static IEnumerator GET(string uri, Action<string> success, Action<string> fail) {
		using UnityWebRequest webRequest = UnityWebRequest.Get(URL + uri);
		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success) {
			string response = webRequest.downloadHandler.text;
			success(response);
		}
		else {
			fail(webRequest.responseCode.ToString());
		}
	}

	public static IEnumerator POST(string uri, string json, Action<string> success, Action<long> fail) {
		var webRequest = new UnityWebRequest(URL + uri, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		webRequest.SetRequestHeader("Content-Type", "application/json");

		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success) {
			string response = webRequest.downloadHandler.text;
			success(response);
		}
		else {
			fail(webRequest.responseCode);
		}
		webRequest.Dispose();
	}

	public static IEnumerator PUT(string uri, string json, Action<string> success, Action<long> fail) {
		var webRequest = new UnityWebRequest(URL + uri, "PUT");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		webRequest.SetRequestHeader("Content-Type", "application/json");

		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success) {
			string response = webRequest.downloadHandler.text;
			success(response);
		}
		else {
			fail(webRequest.responseCode);
		}
		webRequest.Dispose();
	}
}