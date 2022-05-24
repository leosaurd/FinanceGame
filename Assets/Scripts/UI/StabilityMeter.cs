using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityMeter : MonoBehaviour
{
    private const int maxY = 225;

    // This value should be pulled from game manager, only here for testing purposes
    // should be between -1 and 1?
    public float stability = 0;

    public float displayValue = 0;


    // Update is called once per frame
    void Update()
    {
        // Animate the motion of the slider
        float diff = stability - displayValue;

		displayValue += diff / 50;

        if (Mathf.Abs(diff) < 0.005) displayValue = stability;

		transform.localPosition = new Vector2(transform.localPosition.x, maxY * displayValue);
    }
}
