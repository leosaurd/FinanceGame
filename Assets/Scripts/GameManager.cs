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
                    BlockType blockType = (BlockType)Random.Range(0, 3);
                    string name = NameGenerator.GenerateName(blockType);
                    ownedBlocks.Add(new BlockInstance(name, blockType, StaticObjectManager.BlockStats[name]));
                    TowerAnimator.Instance.AddBlockToTower(block);

                    eventOccuring = false;
                    eventDuration = 0;
                }
                //If the event is to remove a block.
                else if (EventGenerator.eventRecord == EventType.BlockRemoval)
                {
                    //Remove a random block
                    ownedBlocks.RemoveAt(Random.Range(0, ownedBlocks.Count));
                    eventOccuring = false;
                    eventDuration = 0;
                }
                //If the event is to nullify profit.
                else if (EventGenerator.eventRecord == EventType.BlockNullification)
                {
                    //If the block matches the record, select all is set, or the name matches the group.
                    if (SelectBlock(i))
                        multiplier = 0;
                }
                //If event is a multiplier event
                else if (isMultiplier())
                {
                    //If event is a profit-altering event
                    if (EventGenerator.selectIndex == 1)
                    {
                        if (SelectBlock(i))
                            multiplier = EventGenerator.selectMult;
                    }
                    //If event is a stability-altering event
                    else if (EventGenerator.selectIndex == 0)
                    {
                        //If stability has been not been multiplied
                        if (!stabMultiplied)
                        {
                            if (SelectBlock(i))
                                ownedBlocks[i].stability *= EventGenerator.selectMult;
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
                        if (isMultiplier())
                        {
                            if (SelectBlock(i))
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

    //If event matches BlockType, or Name matches
    public bool SelectBlock(int i)
    {
        return ((EventGenerator.blockRecord == ownedBlocks[i].blockType) || EventGenerator.blockRecord == BlockType.All || EventGenerator.selectGroup.Equals(ownedBlocks[i].name));
    }

    //If the event is a Multiplier event
    public bool isMultiplier()
    {
        return EventGenerator.eventRecord == EventType.Fractional || EventGenerator.eventRecord == EventType.Multiplier;
    }
}


public enum GameOverReason
{
    Stability,
    Poor,
}
