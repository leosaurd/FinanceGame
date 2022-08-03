using System.Collections;
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
		EventType eventType = (EventType)Random.Range(0, 5);

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
			Title = "Block Removal Event";
			IsBeneficial = false;

			int index = Random.Range(0, GameManager.Instance.ownedBlocks.Count);
			Block = GameManager.Instance.ownedBlocks[index];

			Description = "Removes a random block from the tower.";
		}
		else if (eventType == EventType.BlockAddition)
		{
			Title = "Block Addition Event";
			IsBeneficial = true;

			BlockType blockType = (BlockType)Random.Range(0, 3);
			string name = NameGenerator.GenerateName(blockType);
			Block = new(name, blockType, StaticObjectManager.BlockStats[name]);

			Description = "Adds a random block from the tower.";
		}
	}
}

public class LastingEvent : GameEvent
{
	public int Duration { get; set; }

	public float Multipler { get; private set; }
	public float VisualMultipler { get; private set; }

	public BlockType AffectedGroup { get; private set; }
	public EventField AffectedField { get; private set; }


	public LastingEvent(EventType eventType)
	{
		Type = eventType;

		AffectedGroup = (BlockType)Random.Range(0, 3);
		AffectedField = (EventField)Random.Range(0, 3);

		if (Type == EventType.BlockNullification)
		{
			Duration = Random.Range(1, 4);
			Multipler = 0;
		}
		else
		{
			Duration = Random.Range(2, 7);

			int isLarge = Random.Range(0, 100);


			float min = 0f;
			float max = 0f;

			if (Type == EventType.Multiplier)
			{
				if (isLarge <= 10)
				{
					min = 1.75f;
					max = 3f;
				}
				else
				{
					min = 1f;
					max = 2f;
				}
			}
			else if (Type == EventType.Fractional)
			{
				if (isLarge <= 10)
				{
					min = 2.5f;
					max = 5f;
				}
				else
				{
					min = 1.5f;
					max = 3f;
				}
			}



			Multipler = Mathf.Round(Random.Range(min, max) * 4) / 4f;
			VisualMultipler = Multipler;

			if (Type == EventType.Fractional)
			{
				Multipler = 1f / Multipler;
			}

		}


		IsBeneficial = true;

		if (
			Type == EventType.BlockNullification ||
			(AffectedField == EventField.stability && Type == EventType.Multiplier) ||
			(AffectedField == EventField.cost && Type == EventType.Multiplier) ||
			(AffectedField == EventField.profit && Type == EventType.Fractional)
			)
		{
			IsBeneficial = false;
		}

		GenerateDescription();
		GenerateTitle();
	}

	public void GenerateDescription()
	{
		switch (Type)
		{
			case EventType.BlockNullification:
				Description = string.Format("<color=Green>{0}</color> blocks no longer generate profit for <color=Green>{1}</color> round{2}.", EventGenerator.BlockTypeToString[AffectedGroup], Duration.ToString(), Duration > 1 ? "s" : "");
				break;
			case EventType.Multiplier:
				Description = string.Format("Multiply <color=Green>{0}</color> by <color=Green>{1}</color> for the next <color=Green>{2}</color> round{3} for <color=Green>{4}</color> blocks in the marketplace.", AffectedField.ToString(), VisualMultipler, Duration, Duration > 1 ? "s" : "", EventGenerator.BlockTypeToString[AffectedGroup]);
				break;
			case EventType.Fractional:
				Description = string.Format("Divide <color=Green>{0}</color> by <color=Green>{1}</color> for the next <color=Green>{2}</color> round{3} for <color=Green>{4}</color> blocks in the marketplace.", AffectedField.ToString(), VisualMultipler, Duration, Duration > 1 ? "s" : "", EventGenerator.BlockTypeToString[AffectedGroup]);
				break;
		}
	}

	public void GenerateTitle()
	{
		switch (Type)
		{
			case EventType.BlockNullification:
				Title = "Profit Nullification Event";
				break;
			case EventType.Multiplier:
				Title = string.Format("{0} Multiplier Event", AffectedField.ToString());
				break;
			case EventType.Fractional:
				Title = string.Format("{0} Divider Event", AffectedField.ToString());
				break;
		}
	}
}