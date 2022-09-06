using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
	public static Dictionary<BlockType, string> BlockTypeToString = new()
	{
		{BlockType.Insurance, "Insurance" },
		{BlockType.HighRiskInvestment, "High Risk Investment" },
		{BlockType.LowRiskInvestment, "Low Risk Investment" }
	};

	public static GameEvent GenerateEvent()
	{
		EventType eventType = (EventType)Random.Range(0, 6);

		if (eventType == EventType.BlockRemoval || eventType == EventType.BlockAddition)
		{
			return new InstantEvent(eventType);
		}
		else
		{
			return new LastingEvent(eventType);
		}
	}
}


public abstract class GameEvent
{
	public string Title { get; protected set; }
	public string Description { get; protected set; }
	public EventType Type { get; protected set; }
	public bool IsBeneficial { get; protected set; }
}

public class InstantEvent : GameEvent
{
	public BlockInstance Block { get; private set; }

	public InstantEvent(EventType eventType)
	{
		Type = eventType;

		if (eventType == EventType.BlockRemoval)
		{
			Title = "Rug Pull!";
			IsBeneficial = false;

			Block = GameManager.Instance.ownedBlocks[GameManager.Instance.ownedBlocks.Count - 1];
			Description = "The asset you bought turned out to be a scam! It has been removed from your portfolio.";
		}
		else if (eventType == EventType.BlockAddition)
		{
			Title = "Inheritance";
			IsBeneficial = true;

			BlockType blockType = (BlockType)Random.Range(0, 3);
			string name = NameGenerator.GenerateName(blockType);
			Block = new(name, blockType, StaticObjectManager.BlockStats[name]);

			Description = "You inherited an asset from a distant relative.";
		}
	}
}

public class LastingEvent : GameEvent
{
	public int Duration { get; set; }

	public float Multipler { get; private set; }

	public BlockType AffectedGroup { get; private set; }


	public LastingEvent(EventType eventType)
	{
		Type = eventType;

		AffectedGroup = (BlockType)Random.Range(0, 3);

		if (Type == EventType.BlockNullification)
		{
			Duration = 1;
			Multipler = 0;
		}
		else
		{
			Duration = Random.Range(1, 4);

			if (Type == EventType.CostIncrease)
			{
				Multipler = 2f;
			}
			else if (Type == EventType.CostDecrease)
			{
				Multipler = 0.5f;
			}
			else if (Type == EventType.ProfitIncrease)
			{
				Multipler = 2f;
			}

		}


		IsBeneficial = GenerateBeneficial();

		GenerateDescription();
		GenerateTitle();
	}

	private bool GenerateBeneficial()
	{
		if (Type == EventType.BlockNullification)
			return false;
		if (Type == EventType.BlockRemoval)
			return false;
		if (Type == EventType.CostIncrease)
		{
			return false;
		}
		return true;
	}

	public void GenerateDescription()
	{
		switch (Type)
		{
			case EventType.BlockNullification:
				Description = string.Format("<color=__>{0}</color> assets will not generate profits for the next turn.", EventGenerator.BlockTypeToString[AffectedGroup]);
				break;
			case EventType.CostIncrease:
				Description = string.Format("<color=__>{0}</color> assets are trending and will cost double for <color=__>{1}</color> turn{2}.", EventGenerator.BlockTypeToString[AffectedGroup], Duration, Duration != 1 ? "s" : "");
				break;
			case EventType.ProfitIncrease:
				Description = string.Format("Reliable sources say <color=__>{0}</color> assets purchased in the next <color=__>{1}</color> turn{2} will generate double profits.", EventGenerator.BlockTypeToString[AffectedGroup], Duration, Duration != 1 ? "s" : "");
				break;
			case EventType.CostDecrease:
				Description = string.Format("An oversupply of <color=__>{0}</color> assets in the market has caused the price to drop by half for <color=__>{1}</color> turn{2}.", EventGenerator.BlockTypeToString[AffectedGroup], Duration, Duration != 1 ? "s" : "");
				break;
		}
	}

	public void GenerateTitle()
	{
		switch (Type)
		{
			case EventType.BlockNullification:
				Title = "Black Swan Event";
				break;
			case EventType.CostIncrease:
				Title = string.Format("{0} Hype", EventGenerator.BlockTypeToString[AffectedGroup]);
				break;
			case EventType.CostDecrease:
				Title = string.Format("Supply Surplus");
				break;
			case EventType.ProfitIncrease:
				Title = string.Format("Insider Tip");
				break;
		}
	}
}