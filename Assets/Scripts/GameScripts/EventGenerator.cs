using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    public static int[] rounds = new int[] { 1, 2, 5 };
    public static int[] multiplier = new int[] { 2, 5, 10 };
    public static string[] selector = new string[] { "stability", "profit" };
    public static string[] blockSelector = new string[] { "all blocks", "all blocks of a random type", "all blocks in a random group" };


    //Moved variables to public static so that GameManager can refer to them for newly added blocks.
    public static string selectType;
    public static int selectMult;
    public static int selectRounds;
    public static string selectBlocks;
    public static EventType eventRecord;

    //Not sure whether I want to use a dictionary for the event.
    private static readonly Dictionary<EventType, string> eventList = new()
    {
        { EventType.Multiplier, "Multiply {0} by {1} for the next {2} rounds for {3}" },
        { EventType.Fractional, "Divide {0} by {1} for the next {2} rounds for {3}" },
        { EventType.BlockRemoval, "Removes a random block from the tower" },
        { EventType.BlockAddition, "Adds a random block to the tower"},
        { EventType.BlockNullification, "{3} no longer generate profit for a single round."}
    };

    //Where all altering factors should be for ownedblocks.
    public static string GenerateEvent(EventType eventType)
    {
        //If there is an ongoing event, do not do anything. This depends on whether they want stacking events or not - If they do, this code is not needed.
        if (GameManager.Instance.eventduration > 0)
        {
            return "";
        }

        //For altering parameters.
        selectType = selector[Random.Range(0, selector.Length)];
        selectMult = multiplier[Random.Range(0, multiplier.Length)];
        selectRounds = rounds[Random.Range(0, rounds.Length)];
        selectBlocks = blockSelector[Random.Range(0, blockSelector.Length)];
        eventRecord = eventType;

        //String that is generated. 
        string printList = string.Format(eventList[eventType], selectType, selectMult, selectRounds, selectBlocks).ToUpper();
   
        //Example Generation: Multiply stability by 5 for the next 10 rounds for all blocks of a random type
        //So: GameManager.Instance.ownedblocks contains some of the needed blocks, and newly purchased ones must be altered as well. 
        //Must also revert the stability after duration is over. 

        return printList;
    }
}
