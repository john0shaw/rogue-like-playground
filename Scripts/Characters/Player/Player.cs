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
	[Export] public int MaxMana;
    [Export] public int Strength;
	[Export] public int Defence;
	[Export] public int Magic;
	[Export] public int Luck;

    public float Health;
	public int Mana;
	public int Keys;
	public int Gold;

	public bool TrackMouseEvents = true;
	public List<Item> Inventory = new List<Item>();

	private AnimationPlayer _animationPlayer;
	private AnimationPlayer _effectAnimationPlayer;
	private Sprite2D _sprite2D;

	public Weapon EquipedWeapon;
	private WeaponNode _weaponNode;

    public override void _Ready()
    {
        base._Ready();
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_effectAnimationPlayer = GetNode<AnimationPlayer>("EffectAnimationPlayer");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_weaponNode = GetNode<WeaponNode>("Weapon");
		player = this;

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

	public void AddItem(Item item)
	{
		if (item.ID == 0)
		{
			Gold += item.Count;
		}
		else
		{
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].ID == item.ID)
                {
                    Inventory[i].Count += item.Count;
                    return;
                }
            }

            Inventory.Add(item);
			EmitSignal("InventoryUpdated");
        }
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
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;

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
}
