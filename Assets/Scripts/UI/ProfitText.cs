using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfitText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    // Should be fetched from game manager
    public int profit = 0;

    void Awake()
	{
        textComponent = GetComponent<TextMeshProUGUI>();
	}

    void Update()
    {
        string text = "";
        if(profit < 0)
		{
            text += "-";
		}
        text += "$" + Mathf.Abs(profit).ToString("N0");
        textComponent.text = text;
    }
}
