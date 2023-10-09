using Godot;
using Godot.Collections;
using System;
using System.Linq;



public partial class EnemyController : CharacterBody2D
{
    public const string ATTACK_ANIMATION = "Attack";
    public const string DIE_ANIMATION = "Die";

    [Export] public EnemyResource EnemyResource;
    [Export] public PackedScene PickupableItemScene;

    public int Health;
    public int MaxHealth;

    // UI
    Label _stateNameLabel;
    HealthBar _healthBar;

    // Nodes
	StateMachine _stateMachine;
    AnimationPlayer _animationPlayer;
    AnimatedSprite2D _animatedSprite2D;

    // Internal
    RandomNumberGenerator _rng;

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
        _rng.Randomize();

        _stateMachine = GetNode<StateMachine>("StateMachine");
        _stateNameLabel = GetNode<Label>("StateName");
        _healthBar = GetNode<HealthBar>("HealthBar");

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        MaxHealth = EnemyResource.MaxHealth;
        Health = MaxHealth;

        _healthBar.MaxHealth = MaxHealth;
        _healthBar.Health = Health;
    }

    public override void _Process(double delta)
    {
        _stateNameLabel.Text = _stateMachine.State.Name;
    }

    public void GenerateLoot()
    {
        foreach(Loot loot in EnemyResource.Loot)
        {
            if (loot.MaxSpawn == 1)
            {
                // Use SpawnChance
                if (_rng.Randf() < loot.SpawnChance)
                    SpawnLoot(loot.Item);
            }
            else
            {
                // Use Min to Max
                for (int i = 0; i < _rng.RandiRange(loot.MinSpawn, loot.MaxSpawn); i++)
                    SpawnLoot(loot.Item);
            }
        }
    }

    public void SpawnLoot(Item item)
    {
        Pickupable lootItem = (Pickupable)PickupableItemScene.Instantiate();
        lootItem.Item = item;
        lootItem.GlobalPosition = new Vector2(
            GlobalPosition.X + _rng.RandiRange(-15, 15),
            GlobalPosition.Y + _rng.RandiRange(-15, 15)
        );
        Owner.AddChild(lootItem);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.Health = Health;

        if (Health <= 0)
        {
            _stateMachine.TransitionTo("Die");
        }
    }

    public float GetAnimationLength(string animationName)
    {
        float animationPlayerLength = 0f;
        float animatedSpriteLength = 0f;

        if (_animationPlayer.HasAnimation(animationName))
        {
            animationPlayerLength = _animationPlayer.GetAnimation(animationName).Length;
        }

        if (_animatedSprite2D.SpriteFrames.HasAnimation(animationName))
        {
            animatedSpriteLength = (float)_animatedSprite2D.SpriteFrames.GetFrameCount(animationName) / (float)_animatedSprite2D.SpriteFrames.GetAnimationSpeed(animationName);
        }

        return Mathf.Max(animationPlayerLength, animatedSpriteLength);
    }

    public void PlayAnimation(string animationName, bool forwards = true)
    {
        if (_animationPlayer.HasAnimation(animationName))
        {
            if (forwards)
                _animationPlayer.Play(animationName);
            else
                _animationPlayer.PlayBackwards(animationName);
        }
 
        if (_animatedSprite2D.SpriteFrames.HasAnimation(animationName))
        {
            if (_animatedSprite2D.Animation != animationName)
                _animatedSprite2D.Animation = animationName;

            if (forwards)
                _animatedSprite2D.Play(animationName);
            else
                _animatedSprite2D.PlayBackwards(animationName);
        }  
    }

    public void _on_state_machine_transitioned(string stateName)
    {
        Logger.Log("Enemy Controller Detected State Change - " + stateName);
    }
}
