using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;
using UnityEngine.Events;

public class SubmitScore : MonoBehaviour
{
	public UnityEvent OnSubmit;
	public void TrySubmit()
	{
		string name = GetComponentInChildren<TMP_InputField>().text;
		StartCoroutine(ValidateName(name, (Response response) =>
		{
			if (response.valid)
			{
				transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
				Leaderboard.Instance.SubmitScore(name);
				OnSubmit.Invoke();
			}
			else
			{
				if (response.reason != null)
					transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = response.reason;
				else
					transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Inavlid Name";
			}
		}));
	}

	IEnumerator ValidateName(string name, Action<Response> callback)
	{
		using UnityWebRequest webRequest = UnityWebRequest.Get("https://test.trentshailer.com/valid-name?name=" + name);
		yield return webRequest.SendWebRequest();
		if (webRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(webRequest.error);
		}
		else
		{
			Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
			callback.Invoke(response);
		}
	}
}

struct Response
{
	public bool valid;
#nullable enable
	public string? reason;
}