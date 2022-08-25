using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBlockStats
{
	private readonly float stabilityMultiplier = 0.05f;
	private readonly float profitsMultiplier = 5f;

	public int cost;
	public int profit;
	public float stability;
	public int height;
	public static float roundScaling;

	public StaticBlockStats(int cost, int profits, float stability, int height)
	{
		this.cost = cost;
		this.profit = profits;
		this.stability = stability;
		this.height = height;
	}

	//  Turns a StaticBlockStats with their values into a StaticBlockStats with game usable values, and adds some randomness
	public StaticBlockStats GenerateStats()
	{
		roundScaling = (0.45f * GameManager.Instance.ownedBlocks.Count) + 1;
		if (GameManager.Instance.ownedBlocks.Count >= 20)
		{
			roundScaling = Mathf.Pow(1.055f, GameManager.Instance.ownedBlocks.Count) / 1.3f + 7.8f;
		}


		// Scale values to usable level
		int finalCost = Mathf.RoundToInt(cost * roundScaling);
		int finalProfits = Mathf.RoundToInt(profit * profitsMultiplier);
		float finalStability = stability * stabilityMultiplier;



		// Add randomness to final values (between -10% and + 10%)
		float costJitter = Random.Range(-0.1f, 0.1f);
		float profitsJitter = Random.Range(-0.1f, 0.1f);
		float stabilityJitter = Random.Range(-0.1f, 0.1f);

		finalCost = GameManager.Instance.RoundDownTwoSF(Mathf.RoundToInt(finalCost + finalCost * costJitter));
		finalProfits = GameManager.Instance.RoundDownTwoSF(Mathf.RoundToInt(finalProfits + finalProfits * profitsJitter));
		finalStability += finalStability * stabilityJitter;


		return new StaticBlockStats(finalCost, finalProfits, finalStability, height);
	}
}
