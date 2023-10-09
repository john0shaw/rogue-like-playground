using Godot;
using System;
using System.ComponentModel;

public partial class Pickupable : Node2D
{
    [Export] public Item Item;

    Sprite2D _sprite2D;
    AnimationPlayer _animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite2D = GetNode<Sprite2D>("Item/Sprite2D");
        _sprite2D.Texture = Item.Texture;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Play("Spawn");
    }

    public void _on_item_body_entered(Node2D node)
    {
        if (node is Player)
        {
            Item clonedItem = Item.Duplicate() as Item;
            Player.player.AddItem(clonedItem);
            QueueFree();
        }
    }
}
