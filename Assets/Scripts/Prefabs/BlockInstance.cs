using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstance
{
    //Values to be configured upon block creation
    public float stability;
    public int profit;
    public int cost;
    public BlockType blockType;
    public string name;

    public BlockInstance(BlockType blockType, float stability, int profit, int cost)
	{
        this.blockType = blockType;
        this.stability = stability;
        this.cost = cost;
        this.profit = profit;

        name = NameGenerator.GenerateName(blockType);
	}

    // This will create the visual block we see on screen
    // that block will have a seperate script for animating
    // etc
    public GameObject CreateBlock()
	{
        return new GameObject(name);
	}
}
