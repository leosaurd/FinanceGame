using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    public static int[] rounds = new int[] { 1, 2, 5 };
    public static int[] multiplier = new int[] { 2, 5, 10 };

    //Not sure whether I want to use a dictionary for the event.
    private static readonly Dictionary<EventType, string> eventList = new()
    {
        { EventType.Multiplier, "Multiply stability by {0} for the next {1} rounds" },
    };

    //Where all altering factors should be.
    public static string GenerateEvent()
    {
        int roundIndex = Random.Range(0, rounds.Length);
        int multIndex = Random.Range(0, multiplier.Length);

        return "x";
    }
}
