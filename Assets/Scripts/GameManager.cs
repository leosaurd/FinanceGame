using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    public float stability = 0;
    public float portfolioValue = 0;
    public int profits = 0;
    public int towerHeight = 0;

    //Chance for event to occur
    public float eventChance = 0f;

    //How long the event will be for.
    public int eventDuration = 0;

    //Whether there is an event ongoing
    public bool eventOccuring = false;

    //Whether the stability has already been multiplied 
    public bool stabMultiplied = false;

    public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

    public void AddBlock(BlockInstance block)
    {
        ownedBlocks.Add(block);

        //Only if there is no event happening.
        if (!eventOccuring)
        {
            //Any time a block is added, the chance for an event increases.
            eventChance += 5;
        }
        //If the generated number is less than or equal to event chance, then generate an event.
        if (Random.Range(0, 100) <= eventChance)
        {
            //Randomly picks from one of the 5 events available in EventType.
            EventGenerator.GenerateEvent((EventType)Random.Range(0, 5));
            //Sets event occuring to true.
            eventOccuring = true;
            //Resets event chance.
            eventChance = 0f;
            //sets the duration of the event.
            eventDuration = EventGenerator.selectRounds;
        }
        TowerAnimator.Instance.AddBlockToTower(block);

        for (int i = 0; i < ownedBlocks.Count; i++)
        {
            //Change profit by multiplier from Generated Event.
            float multiplier = 1f;

            //If event is occuring
            if (eventOccuring)
            {
                //If the event is to add a block.
                if (EventGenerator.eventRecord == EventType.BlockAddition)
                {
                    //Add a random block here.
                    eventOccuring = false;
                    eventDuration = 0;
                }
                //If the event is to remove a block.
                else if (EventGenerator.eventRecord == EventType.BlockRemoval)
                {
                    //remove a random block here.
                    eventOccuring = false;
                    eventDuration = 0;
                }
                //If the event is to nullify profit.
                else if (EventGenerator.eventRecord == EventType.BlockNullification)
                {
                    //If the block matches the record, select all is set, or the name matches the group.
                    if (EventGenerator.blockRecord == ownedBlocks[i].blockType || EventGenerator.blockIndex == 0 || EventGenerator.selectGroup.Equals(ownedBlocks[i].name))
                        multiplier = 0;
                }
                //If event is Profit manipulation
                else if (EventGenerator.selectIndex == 1)
                {
                    //If the event is a multiplicative/divisive event, set the multiplier accordingly.
                    if (EventGenerator.eventRecord == EventType.Fractional || EventGenerator.eventRecord == EventType.Multiplier)
                    {
                        if (EventGenerator.blockRecord == ownedBlocks[i].blockType || EventGenerator.blockIndex == 0 || EventGenerator.selectGroup.Equals(ownedBlocks[i].name))
                            multiplier = EventGenerator.selectMult;
                    }
                }
                //If event is Stability manipulation
                else if (EventGenerator.selectIndex == 0)
                {
                    //If the event is a multiplicative/divisive event, set the multiplier accordingly.
                    if (!stabMultiplied)
                    {
                        if (EventGenerator.eventRecord == EventType.Fractional || EventGenerator.eventRecord == EventType.Multiplier)
                        {
                            if (EventGenerator.blockRecord == ownedBlocks[i].blockType || EventGenerator.blockIndex == 0 || EventGenerator.selectGroup.Equals(ownedBlocks[i].name))
                                ownedBlocks[i].stability = ownedBlocks[i].stability * EventGenerator.selectMult;
                        }
                    }
                }
            }

            portfolioValue += (ownedBlocks[i].profit) * multiplier;
        }
        //If the event was a stability event, set multiplication to true.
        if (EventGenerator.selectIndex == 0)
        {
            stabMultiplied = true;
        }

        if (stability < -1)
        {
            stability = -1;
            EndGame(GameOverReason.Stability);
        }
        else if (stability > 1)
        {
            stability = 1;
        }

        //If there is an ongoing event
        if (eventOccuring)
        {
            //If the event is 0, set the event occuring to false.
            if (eventDuration == 0)
            {
                //If the event was a stability event, reset the values.
                if (stabMultiplied)
                {
                    for (int i = 0; i < ownedBlocks.Count; i++)
                    {
                        if (EventGenerator.eventRecord == EventType.Fractional || EventGenerator.eventRecord == EventType.Multiplier)
                        {
                            if (EventGenerator.blockRecord == ownedBlocks[i].blockType || EventGenerator.blockIndex == 0 || EventGenerator.selectGroup.Equals(ownedBlocks[i].name))
                                ownedBlocks[i].stability = ownedBlocks[i].stability / EventGenerator.selectMult;
                        }
                    }
                }
                stabMultiplied = false;
            }

            eventOccuring = false;
        }
        else
        {
            //If not, reduce duration by 1, since we added a block.
            eventDuration--;
        }

    }

    public int GetScore()
    {
        return Mathf.RoundToInt(towerHeight * profits);
    }

    public void EndGame(GameOverReason reason)
    {
        GameOver.Instance.ShowGameover(reason);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}


public enum GameOverReason
{
    Stability,
    Poor,
}
