using UnityEngine;

public class StaticBlockStats {
	private readonly float stabilityMultiplier = 0.05f;
	private readonly float profitsMultiplier = 5f;

	public int cost;
	public int profit;
	public float stability;
	public int height;
	public static float roundScaling;
	public static int baseCostScaling;

	public StaticBlockStats(int cost, int profits, float stability, int height) {
		this.cost = cost;
		this.profit = profits;
		this.stability = stability;
		this.height = height;
	}

	//  Turns a StaticBlockStats with their values into a StaticBlockStats with game usable values, and adds some randomness
	public StaticBlockStats GenerateStats() {
		roundScaling = (0.63f * GameManager.Instance.ownedBlocks.Count) + 1;
		if (GameManager.Instance.ownedBlocks.Count >= 80) {
			roundScaling = Mathf.Pow(GameManager.Instance.ownedBlocks.Count, 2) / 150 - 1;
		}
		else if (GameManager.Instance.ownedBlocks.Count >= 40) {
			roundScaling = (0.66f * GameManager.Instance.ownedBlocks.Count);
		}


		// Scale values to usable level
		int finalCost = Mathf.RoundToInt(cost * roundScaling) + baseCostScaling;
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
