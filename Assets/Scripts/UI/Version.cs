using TMPro;
using UnityEngine;

public class Version : MonoBehaviour
{
	void Awake()
	{
		GetComponent<TextMeshProUGUI>().text = Application.version;

	}

}
