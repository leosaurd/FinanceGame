using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    // Should be fetched from game manager
    public int value = 0;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string text = "";
        if (value < 0)
        {
            text += "-";
        }
        text += "$" + Mathf.Abs(value).ToString("N0");
        textComponent.text = text;
    }
}
