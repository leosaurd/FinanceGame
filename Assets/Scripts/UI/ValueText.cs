using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    private GameManager gameManager;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameManager = GameManager.GetInstance();
    }

    void Update()
    {
        string text = "";
        if (gameManager.portfolioValue < 0)
        {
            text += "-";
        }
        text += "$" + Mathf.Abs(gameManager.portfolioValue).ToString("N0");
        textComponent.text = text;
    }
}
