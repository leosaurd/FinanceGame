using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBlockStats
{
	private readonly float stabilityMultiplier = 0.05f;
	private readonly float profitsMultiplier = 10.5f;

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
		// Scale values to usable level
		int finalCost = cost;
		int finalProfits = Mathf.RoundToInt(profit * profitsMultiplier);
		float finalStability = stability * stabilityMultiplier;

		// Add randomness to final values (between -10% and + 10%)
		float costJitter = Random.Range(-0.1f, 0.1f);
		float profitsJitter = Random.Range(-0.1f, 0.1f);
		float stabilityJitter = Random.Range(-0.1f, 0.1f);
		
		finalCost += Mathf.RoundToInt(finalCost * costJitter);
		finalProfits += Mathf.RoundToInt(finalProfits * profitsJitter);
		finalStability += Mathf.RoundToInt(finalStability * stabilityJitter);

		return new StaticBlockStats(finalCost, finalProfits, finalStability);
	}
}
