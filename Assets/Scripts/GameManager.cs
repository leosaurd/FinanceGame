using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get; private set;
	}
	public Transform Vignette;


	void Awake() {
		if (!Instance)
			Instance = this;
	}

	private float stability = 0;
	public float Stability {
		get {
			return stability;
		}
		set {
			if (value != 1) {
				if (value - stability < 0) {
					SFXManager.Instance.PlaySFX(SFX.increaseRisk);
				}
				else {
					SFXManager.Instance.PlaySFX(SFX.decreaseRisk);
				}
			}

			stability = value;
			if (value < -1) {
				stability = -1;
				EndGame(GameOverReason.Stability);
			}
		}
	}
	public float portfolioValue = 0;
	public int profits = 0;
	public int towerHeight = 0;

	public int totalEarnings = 0;
	//Chance for event to occur
	public float eventChance = 0f;

#nullable enable
	public LastingEvent? lastingEvent = null;


	public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

	public bool IsGameOver {
		get; private set;
	}

	public void CalculateProfit() {
		int profit = 0;


		foreach (BlockInstance block in ownedBlocks) {
			if (lastingEvent == null || lastingEvent.Type != EventType.BlockNullification || lastingEvent.AffectedGroup != block.blockType) {
				profit += block.profit;
			}
		}


		profits = profit;
	}

	public void AddBlock(BlockInstance block) {
		//If there is an ongoing event
		if (lastingEvent != null) {
			lastingEvent.Duration--;
			if (lastingEvent.Duration == 0) {
				lastingEvent = null;
			}
		}

		bool refresh = true;

		ownedBlocks.Add(block);

		//Only if there is no event happening AND at least 5 blocks/rounds have occurred.
		if (lastingEvent == null && ownedBlocks.Count >= 5) {
			//Any time a block is added, the chance for an event increases.
			eventChance += 5;
		}
		//If the generated number is less than or equal to event chance AND there is no event happening, then generate an event
		if (Random.Range(0, 100) <= eventChance && lastingEvent == null && ownedBlocks.Count >= 5) {
			eventChance = 0f;

			GameEvent gameEvent = EventGenerator.GenerateEvent();

			if (gameEvent.IsBeneficial) {
				SessionManager.Instance.Session.positive_event_count += 1;
			}
			else {
				SessionManager.Instance.Session.negative_event_count += 1;
			}

			SessionManager.Instance.SaveSession();

			if (gameEvent is InstantEvent instantEvent) {
				refresh = false;
				BlockInstance displayBlock = instantEvent.Block;
				if (EventUIManager.Instance.blockObject != null) {
					EventUIManager.Instance.blockObject.GetComponent<EventBlockScript>().block = displayBlock;
					EventUIManager.Instance.blockObject.GetComponent<EventBlockScript>().updateGraphics();
				}

				EventUIManager.Instance.ShowEvent(gameEvent, () => {

					if (gameEvent.Type == EventType.BlockAddition) {
						CalculateProfit();
						Stability += instantEvent.Block.stability;
						ownedBlocks.Add(instantEvent.Block);
						TowerAnimator.Instance.AddBlockToTower(instantEvent.Block);
					}
					else if (gameEvent.Type == EventType.BlockRemoval) {
						TowerAnimator.Instance.RemoveBlockFromTower(instantEvent.Block);
						ownedBlocks.Remove(instantEvent.Block);
						CalculateProfit();
						Stability -= instantEvent.Block.stability;
					}
					//If an event happens that puts stability down, it'll be an instant game-over, but this SHOULD fix the thing.
					if (Stability < -1) {
						Stability = -1;
						EndGame(GameOverReason.Stability);
						return;
					}
					else if (Stability > 1) {
						Stability = 1;
					}

					SessionManager.Instance.Session.portfolio_value = totalEarnings;
					SessionManager.Instance.Session.insurance_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.Insurance).Count;
					SessionManager.Instance.Session.low_risk_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.LowRiskInvestment).Count;
					SessionManager.Instance.Session.high_risk_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.HighRiskInvestment).Count;
					SessionManager.Instance.Session.turns = ownedBlocks.Count;
					SessionManager.Instance.SaveSession();

					if (EventUIManager.Instance.blockObject != null)
						EventUIManager.Instance.blockObject.gameObject.SetActive(false);
					MarketplaceUI.Instance.RefreshShop();

				});
			}
			else if (gameEvent is LastingEvent) {
				lastingEvent = (LastingEvent)gameEvent;
				EventUIManager.Instance.ShowEvent(gameEvent, () => {
				});
			}
		}
		TowerAnimator.Instance.AddBlockToTower(block);

		for (int i = 0; i < ownedBlocks.Count; i++) {
			float multiplier = 1;

			//If event is occuring
			if (lastingEvent != null && lastingEvent.Type == EventType.BlockNullification) {

				if (lastingEvent.AffectedGroup == ownedBlocks[i].blockType) {
					multiplier = 0;
				}
			}
			portfolioValue += (ownedBlocks[i].profit) * multiplier;
			totalEarnings += Mathf.RoundToInt((ownedBlocks[i].profit) * multiplier);

		}




		CalculateProfit();

		if (Stability < -1) {
			Stability = -1;
			EndGame(GameOverReason.Stability);
			return;
		}
		else if (Stability > 1) {
			Stability = 1;
		}

		if (refresh) {
			MarketplaceUI.Instance.RefreshShop();
		}


		SessionManager.Instance.Session.portfolio_value = totalEarnings;
		SessionManager.Instance.Session.insurance_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.Insurance).Count;
		SessionManager.Instance.Session.low_risk_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.LowRiskInvestment).Count;
		SessionManager.Instance.Session.high_risk_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.HighRiskInvestment).Count;
		SessionManager.Instance.Session.turns = ownedBlocks.Count;
		SessionManager.Instance.SaveSession();

		if (stability < -0.7 || portfolioValue - (250 * StaticBlockStats.roundScaling) < (250 * StaticBlockStats.roundScaling)) {
			Vignette.gameObject.SetActive(true);
		}
		else {
			Vignette.gameObject.SetActive(false);
		}
	}


	public void EndGame(GameOverReason reason) {
		SessionManager.Instance.Session.portfolio_value = totalEarnings;
		SessionManager.Instance.Session.insurance_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.Insurance).Count;
		SessionManager.Instance.Session.low_risk_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.LowRiskInvestment).Count;
		SessionManager.Instance.Session.high_risk_count = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.HighRiskInvestment).Count;
		SessionManager.Instance.Session.turns = ownedBlocks.Count;
		SessionManager.Instance.SaveSession();

		IsGameOver = true;
		if (reason == GameOverReason.Stability) {
			SessionManager.Instance.EndSession(SessionEndReason.GameOverStability);
		}
		else {
			SessionManager.Instance.EndSession(SessionEndReason.GameOverPoor);
		}

		SFXManager.Instance.PlaySFX(SFX.gameOver, 0.5f);

		GameOver.Instance.ShowGameover(reason);
	}

	public void RetunToMainMenu() {
		if (SessionManager.Instance.Session.game_end_reason == SessionEndReason.none && SessionManager.Instance.Session.turns > 0)
			SessionManager.Instance.EndSession(SessionEndReason.MainMenu);
		SceneManager.LoadScene("MainMenu");

	}

	public void Retry() {
		SceneManager.LoadScene("Game");
		SessionManager.Instance.StartSession();
	}


	//Rounding function to round values down for easier reading.
	public int RoundDownTwoSF(float d) {
		if (d == 0)
			return 0;
		float scale = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(Mathf.Abs(d))) + 1);
		return Mathf.RoundToInt(scale * (float)System.Math.Round(d / scale, 2));
	}
}


public enum GameOverReason {
	Stability,
	Poor,
}
