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

	public StaticBlockStats(int cost, int profits, float stability)
	{
		this.cost = cost;
		this.profit = profits;
		this.stability = stability;
	}

	//  Turns a StaticBlockStats with their values into a StaticBlockStats with game usable values, and adds some randomness
	public StaticBlockStats GenerateStats()
	{
		float roundScaling = (GameManager.Instance.ownedBlocks.Count + 1) * 0.5f;

		// Scale values to usable level
		int finalCost = Mathf.RoundToInt(cost * roundScaling);
		int finalProfits = Mathf.RoundToInt(profit * profitsMultiplier);
		float finalStability = stability * stabilityMultiplier;

		// Add randomness to final values (between -10% and + 10%)
		float costJitter = Random.Range(-0.1f, 0.1f);
		float profitsJitter = Random.Range(-0.1f, 0.1f);
		float stabilityJitter = Random.Range(-0.1f, 0.1f);

		finalCost += Mathf.RoundToInt(finalCost * costJitter);
		finalProfits += Mathf.RoundToInt(finalProfits * profitsJitter);
		finalStability += finalStability * stabilityJitter;

		return new StaticBlockStats(finalCost, finalProfits, finalStability);
	}
}
