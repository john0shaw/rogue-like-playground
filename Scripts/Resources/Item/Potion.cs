using Godot;
using System;

public partial class Potion : Consumable
{
	public enum PotionEffect
	{
		Health,
		Mana,
		Defense,
		Speed
	};

	[Export] public float Duration;
	[Export] public float Strength;
	[Export] public PotionEffect Effect;
}
