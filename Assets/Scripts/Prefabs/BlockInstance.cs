using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockInstance
{
	public float stability;
	public int cost;
	public int profit;
	public BlockType blockType;
	public string name;
	public int height;
	public TowerColor towerColor;
	public string id;
	public bool affectedByEvent;
	public bool beneficialEvent;
	public EventField affectedField;

	public BlockInstance(string name, BlockType blockType, StaticBlockStats defaultStats)
	{
		StaticBlockStats stats = defaultStats.GenerateStats();
		this.blockType = blockType;
		this.name = name;

		stability = stats.stability;
		profit = stats.profit;
		cost = stats.cost;
		height = stats.height;

		id = Guid.NewGuid().ToString();
#nullable enable
		LastingEvent? lastingEvent = GameManager.Instance.lastingEvent;
		if (lastingEvent != null && lastingEvent.AffectedGroup == blockType && lastingEvent.Type != EventType.BlockNullification)
		{

			affectedByEvent = true;
			affectedField = lastingEvent.AffectedField;

			if (lastingEvent.AffectedField == EventField.profit)
			{
				profit = GameManager.Instance.RoundDownTwoSF(profit * lastingEvent.Multipler);
			}
			else if (lastingEvent.AffectedField == EventField.cost)
			{
				cost = GameManager.Instance.RoundDownTwoSF(cost * lastingEvent.Multipler);
			}
			else if (lastingEvent.AffectedField == EventField.stability)
			{
				stability *= lastingEvent.Multipler;
			}

			beneficialEvent = lastingEvent.IsBeneficial;
		}
	}
}
