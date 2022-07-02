using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    //Create a singleton for EventGenerator
    public static EventGenerator Instance { get; private set; }

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    public string[] selector = new string[] { "stability", "profit" };
    public int[] multiplier = new int[] { 2, 5, 10 };
    public int[] rounds = new int[] { 1, 2, 5 };
    public string[] blockSelector = new string[] { "", " of the Insurance Type", " of the Low Risk Type", " of the High Risk Type" };
  

    //Moved variables to public static so that GameManager can refer to them for newly added blocks.
    public string selectType;
    public int selectMult;
    public int selectRounds;
    public string selectBlocks;
    public EventType eventRecord;
    public BlockType blockRecord;
    public int blockIndex;
    public int selectIndex;

    //Not sure whether I want to use a dictionary for the event.
    private static readonly Dictionary<EventType, string> eventList = new()
    {
        { EventType.Multiplier, "Multiply {0} by {1} for the next {2} rounds for all blocks{3}" },
        { EventType.Fractional, "Divide {0} by {1} for the next {2} rounds for all blocks{3}" },
        { EventType.BlockRemoval, "Removes a random block from the tower" },
        { EventType.BlockAddition, "Adds a random block to the tower"},
        { EventType.BlockNullification, "all blocks{3} no longer generate profit for a single round."}
    };

    //Where all altering factors should be for ownedblocks.
    public string GenerateEvent(EventType eventType)
    {
        //If there is an ongoing event, do not do anything. This code SHOULD NOT OCCUR, as Generate Event should only be called when eventoccuring is false.
        if (GameManager.Instance.eventOccuring)
        {
            return "";
        }
        //Sets indexes for values GameManager uses
        blockIndex = Random.Range(0, blockSelector.Length);
        selectIndex = Random.Range(0, selector.Length);

        //Selects accordingly.
        selectType = selector[selectIndex];
        selectRounds = rounds[Random.Range(0, rounds.Length)];
        selectBlocks = blockSelector[blockIndex];
        eventRecord = eventType;


        if (eventType == EventType.Fractional)
        {
            selectMult = 1/multiplier[Random.Range(0, multiplier.Length)];
        } else
        {
            selectMult = multiplier[Random.Range(0, multiplier.Length)];
        }

        if (blockIndex == 1)
        {
            blockRecord = BlockType.Insurance;
        }
        if (blockIndex == 2)
        {
            blockRecord = BlockType.LowRiskInvestment;
        }
        if (blockIndex == 3)
        {
            blockRecord = BlockType.HighRiskInvestment;
        }

        //String that is generated. 
        string printList = string.Format(eventList[eventType], selectType, selectMult, selectRounds, selectBlocks).ToUpper();
   
        //Example Generation: Multiply stability by 5 for the next 10 rounds for all blocks of a random type
        //So: GameManager.Instance.ownedblocks contains some of the needed blocks, and newly purchased ones must be altered as well. 
        //Must also revert the stability after duration is over. 

        return printList;
    }
}
