using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	public const int INVENTORY_SIZE = 25;

    [Signal] public delegate void InventoryUpdatedEventHandler();
	[Signal] public delegate void ChangedWeaponEventHandler();

    public static Player player;

	[Export] public const float Speed = 100.0f;
	[Export] public Weapon StartingWeapon;

	[Export] public float MaxHealth;
	[Export] public float MaxMana;
    [Export] public int Strength;
	[Export] public int Defence;
	[Export] public int Magic;
	[Export] public int Luck;

    public float Health;
	public float Mana;

	public int Keys;
	public int Gold;

	public bool TrackMouseEvents = true;
	public List<Item> Inventory = new List<Item>();

	private AnimationPlayer _animationPlayer;
	private AnimationPlayer _effectAnimationPlayer;
	private Sprite2D _sprite2D;

	private Timer _speedPotionTimer;
	private Timer _defensePotionTimer;

	private float _tempSpeed;
	private float _tempDefense;

	public Weapon EquipedWeapon;
	private WeaponNode _weaponNode;

    public override void _Ready()
    {
        player = this;

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_effectAnimationPlayer = GetNode<AnimationPlayer>("EffectAnimationPlayer");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_weaponNode = GetNode<WeaponNode>("Weapon");

		_speedPotionTimer = GetNode<Timer>("Timers/SpeedPotion");
		_defensePotionTimer = GetNode<Timer>("Timers/DefensePotion");

		SetWeapon(StartingWeapon);
		AddItem(StartingWeapon);

        Health = MaxHealth;
		Mana = MaxMana;
    }

	public int GetItemCountByID(int ID)
	{
		for (int i = 0; i < Inventory.Count; i++)
		{
			if (Inventory[i].ID == ID)
				return Inventory[i].Count;
		}

		return 0;
	}

	public bool AddItem(Item item)
	{
		if (item.ID == 0)
		{
			Gold += item.Count;
			return true;
		}
        else if(Inventory.Count < INVENTORY_SIZE)
        {
            Inventory.Add(item);
            EmitSignal("InventoryUpdated");
			return true;
        }

		return false;
	}

	public void TakeDamage(float damage)
	{
		Logger.Log("Took " + damage + " damage");
		_effectAnimationPlayer.Play("Hit");
		Health -= damage;
	}

	public void Attack()
	{
		_weaponNode.Attack();
	}

	public void SetWeapon(Weapon weapon)
	{
		_weaponNode.SetWeapon(weapon);
		EquipedWeapon = weapon;
		EmitSignal("ChangedWeapon");
	}

	public void DrinkPotion(Potion potion)
	{
        
        switch (potion.Effect)
		{
			case Potion.PotionEffect.Health:
				Health = Mathf.Min((Health + potion.Strength), MaxHealth);
				break;
			case Potion.PotionEffect.Mana:
				Mana = Mathf.Min((Mana + potion.Strength), MaxMana);
				break;
			case Potion.PotionEffect.Defense:
				_tempDefense = potion.Strength;
				_defensePotionTimer.Start(potion.Duration);
				break;
			case Potion.PotionEffect.Speed:
				_tempSpeed = potion.Strength * Speed;
				_speedPotionTimer.Start(potion.Duration);
				break;
		}

		Logger.Log("Drank a " + potion.Effect.ToString() + " potion");
	}

	public void RemoveItemAt(int index)
	{
		Inventory.RemoveAt(index);
		EmitSignal("InventoryUpdated");
	}

	private void RotateWeapon()
	{
        Vector2 mousePosition = GetGlobalMousePosition();
		_weaponNode.RotateWeapon(Mathf.Atan2(mousePosition.Y - Position.Y, mousePosition.X - Position.X) + 1.571f);
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		RotateWeapon();

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * (Speed + _tempSpeed);
			velocity.Y = direction.Y * (Speed + _tempSpeed);

			if (direction.X > 0)
				_animationPlayer.Play("Walk");
			else
				_animationPlayer.PlayBackwards("Walk");
		}
		else
		{
			velocity = Vector2.Zero;
			_animationPlayer.Play("Idle");
		}

		if (TrackMouseEvents)
		{
            if (Input.IsActionJustPressed("Attack"))
            {
                Attack();
            }
        }

		Velocity = velocity;
		MoveAndSlide();
	}

	public void _on_speed_potion_timeout()
	{
		Logger.Log("Speed Potion Ended");
		_tempSpeed = 0f;
	}

	public void _on_defense_potion_timeout()
	{
		Logger.Log("Defense Potion Ended");
		_tempDefense = 0;
	}
}
