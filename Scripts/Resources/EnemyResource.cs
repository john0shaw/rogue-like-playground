using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] public string Name;

    [ExportGroup("Stats")]
    [Export] public float MaxHealth;
    [Export] public float MoveSpeed;
    [Export] public int MinSpawnLevel;

    [ExportGroup("Media")]
    [Export] public Texture2D Texture;
    [Export] public AudioStream DieEffect;

    [ExportGroup("Combat")]
    [Export] public float DetectionDistance;
    [Export] public float AttackDamage;
    [Export] public float AttackRange;
    [Export] public float AttackSpeed;
    [Export] public bool IsRanged;
    [Export] public ProjectileResource ProjectileResource;

    [ExportGroup("Loot")]
    [Export] public int GoldMin;
    [Export] public int GoldMax;
    [Export] public Array<Item> Loot;
}
