using Godot;
using Godot.Collections;
using System;
using System.Linq;



public partial class EnemyController : CharacterBody2D
{
    public const string ATTACK_ANIMATION = "Attack";
    public const string DIE_ANIMATION = "Die";

    [Export] public EnemyResource EnemyResource;

    public float Health;
    public float MaxHealth;

    // UI
    Label _stateNameLabel;
    HealthBar _healthBar;

    // Nodes
	StateMachine _stateMachine;
    AnimationPlayer _animationPlayer;
    AnimationPlayer _effectAnimationPlayer;
    AudioStreamPlayer2D _audioStreamPlayer2D;
    Sprite2D _sprite2D;

    // Internal
    RandomNumberGenerator _rng;
    PackedScene _pickupableItemScene;
    PackedScene _projectileScene;
    Item _goldItem;

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
        _rng.Randomize();

        _stateMachine = GetNode<StateMachine>("StateMachine");
        _stateNameLabel = GetNode<Label>("StateName");
        _healthBar = GetNode<HealthBar>("HealthBar");

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _effectAnimationPlayer = GetNode<AnimationPlayer>("EffectAnimationPlayer");
        _audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        _sprite2D = GetNode<Sprite2D>("Sprite2D");

        _pickupableItemScene = ResourceLoader.Load<PackedScene>("res://Scenes/Interactable/PickupableItem.tscn");
        _projectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Interactable/Projectile.tscn");
        _goldItem = ResourceLoader.Load <Item>("res://Resources/Item/Gold.tres");

        _sprite2D.Texture = EnemyResource.Texture;

        MaxHealth = EnemyResource.MaxHealth;
        Health = MaxHealth;

        _healthBar.MaxHealth = MaxHealth;
        _healthBar.Health = Health;

        if (GameState.DebugEnemies)
        {
            _healthBar.Show();
            _stateNameLabel.Show();
        }
        else
        {
            _healthBar.Hide();
            _stateNameLabel.Hide();
        }
    }

    public override void _Process(double delta)
    {
        _stateNameLabel.Text = _stateMachine.State.Name;
    }

    public void GenerateLoot()
    {
        // Spawn Gold
        int goldToSpawn = ((int)GD.Randi() % (EnemyResource.GoldMax - EnemyResource.GoldMin)) + EnemyResource.GoldMin;
        for (int i = 0; i <= goldToSpawn; i++)
        {
            SpawnLoot(_goldItem);
        }

        // Spawn Items
        foreach (Item loot in EnemyResource.Loot)
        {
            if (_rng.Randf() < loot.SpawnChance)
                SpawnLoot(loot);
        }
    }

    public void SpawnLoot(Item item)
    {
        Pickupable lootItem = (Pickupable)_pickupableItemScene.Instantiate();
        lootItem.Item = item;
        lootItem.GlobalPosition = new Vector2(
            GlobalPosition.X + _rng.RandiRange(-15, 15),
            GlobalPosition.Y + _rng.RandiRange(-15, 15)
        );
        GetParent().AddChild(lootItem);
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        if (Health <= 0)
            return;

        Health -= damage;
        _healthBar.Health = Health;
        _effectAnimationPlayer.Play("Hit");

        // Knockback
        GlobalPosition -= (GlobalPosition.DirectionTo(damageSourcePosition) * 4);

        if (Health <= 0)
        {
            _stateMachine.TransitionTo("Die");
        }

    }

    public void MeleeAttackPlayer()
    {
        Player.player.TakeDamage(EnemyResource.AttackDamage, GlobalPosition);
    }

    public void RangedAttackPlayer()
    {
        Vector2 directionToPlayer = GlobalPosition.DirectionTo(Player.player.GlobalPosition);

        Projectile projectile = (Projectile)_projectileScene.Instantiate();
        projectile.Target = Player.player.GlobalPosition;
        projectile.Origin = GlobalPosition;
        projectile.ProjectileResource = EnemyResource.ProjectileResource;
        GetParent().AddChild(projectile);
    }

    public float GetAnimationLength(string animationName)
    {
        float animationPlayerLength = 0f;

        if (_animationPlayer.HasAnimation(animationName))
        {
            animationPlayerLength = _animationPlayer.GetAnimation(animationName).Length;
        }


        return animationPlayerLength;
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
    }

    public void PlaySound(AudioStream audioStream)
    {
        if (!_audioStreamPlayer2D.Playing)
        {
            _audioStreamPlayer2D.Stream = audioStream;
            _audioStreamPlayer2D.Play();
        }
    }

    public void _on_state_machine_transitioned(string stateName)
    {
        Logger.Log("Enemy Controller Detected State Change - " + stateName);
    }
}
