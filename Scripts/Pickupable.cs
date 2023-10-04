using Godot;
using System;
using System.ComponentModel;

public partial class Pickupable : Area2D
{
    [Export] public Item Item;

    Sprite2D _sprite2D;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _sprite2D.Texture = Item.Texture;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    public void _on_body_entered(Node2D node)
    {
        if (node is Player)
        {
            Item clonedItem = Item.Duplicate() as Item;
            Player.player.AddItem(clonedItem);
            QueueFree();
        }
    }
}
