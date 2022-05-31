using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstance
{
    //Values to be configured upon block creation
    public float stability;
    public int cost;
    public int profit;
    public BlockType blockType;
    public string name;

    public BlockInstance(BlockType blockType, float stability, int cost, int profit)
	{
        this.blockType = blockType;
        this.stability = stability;
        this.profit = profit;
        this.cost = cost;

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
