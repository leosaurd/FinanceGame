using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	public static GameManager GetInstance(){
		return instance;
	}

    void Awake(){
		if(!instance){
			instance = this;
		}
		else{
			Destroy(gameObject);
		}
	}

	public float stability = 0;
	public int portfolioValue = 0;
	public int profits = 0;

	public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

	public void AddBlock(BlockInstance block)
	{
		ownedBlocks.Add(block);
	}
}
