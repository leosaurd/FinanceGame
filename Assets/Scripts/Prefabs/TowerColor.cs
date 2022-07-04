using System.Collections.Generic;
using UnityEngine;

public enum TowerColor
{
	Green,
	Red,
	Purple,
	Yellow
}

public class TowerColorUtils
{

	public static Dictionary<TowerColor, Sprite> GetCubeSprite = new() {
			{TowerColor.Green, Resources.Load<Sprite>("CubeGreen") },
			{TowerColor.Red, Resources.Load<Sprite>("CubeRed") },
			{TowerColor.Purple, Resources.Load<Sprite>("CubePurple")},
			{TowerColor.Yellow, Resources.Load<Sprite>("CubeYellow") },
		};

	public static Sprite GetBlockSprite(TowerColor color, int height) => color switch
	{
		TowerColor.Green => Resources.Load<Sprite>("BlockGreen" + height),
		TowerColor.Red => Resources.Load<Sprite>("BlockRed" + height),
		TowerColor.Purple => Resources.Load<Sprite>("BlockPurple" + height),
		TowerColor.Yellow => Resources.Load<Sprite>("BlockYellow" + height),
		_ => Resources.Load<Sprite>("BlockGreen" + height),
	};
	public static Sprite GetGlowSprite(int height) => Resources.Load<Sprite>("BlockGlow" + height);

	public static Dictionary<TowerColor, Color> GetTextColor = new() {
			{TowerColor.Green, new Color(66, 188, 160) },
			{TowerColor.Red, new Color(239, 81, 60) },
			{TowerColor.Purple, new Color(187, 103, 201) },
			{TowerColor.Yellow, new Color(255, 202, 5) },
		};
}


