using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	public static Player player;

	[Export] public const float Speed = 100.0f;
	[Export] public Weapon StartingWeapon;

	[Export] public int MaxHealth;
    [Export] public int Strength;
	[Export] public int Defence;
	[Export] public int Magic;
	[Export] public int Luck;

    public int Health;
	public int Keys;
	public int Gold;

	private List<Item> _inventory = new List<Item>();

	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite2D;

	private Weapon _equippedWeapon;
	private WeaponNode _weaponNode;

    public override void _Ready()
    {
        base._Ready();
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_weaponNode = GetNode<WeaponNode>("Weapon");
		player = this;

		_weaponNode.SetWeapon(StartingWeapon);
        Health = MaxHealth;
    }

	public int GetItemCountByID(int ID)
	{
		for (int i = 0; i < _inventory.Count; i++)
		{
			if (_inventory[i].ID == ID)
				return _inventory[i].Count;
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
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].ID == item.ID)
                {
                    _inventory[i].Count += item.Count;
                    return;
                }
            }

            _inventory.Add(item);
        }
	}

	public void TakeDamage(int damage)
	{
		Logger.Log("Took " + damage + " damage");
		Health -= damage;
	}

	public void Attack()
	{
		_weaponNode.Attack();
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

		if (Input.IsActionJustPressed("Attack"))
		{
			Attack();
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
