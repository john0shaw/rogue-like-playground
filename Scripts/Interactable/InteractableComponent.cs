using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
[Tool]
public partial class InteractableComponent : Node2D
{
    [Signal] public delegate void interactedEventHandler();
    [Signal] public delegate void entered_rangeEventHandler();
    [Signal] public delegate void exited_rangeEventHandler();

    AnimatedSprite2D _animatedSprite2D;
    
    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite2D.Play("default");
        _animatedSprite2D.Hide();
    }

    public void EnteredRange()
    {
        _animatedSprite2D.Show();
    }

    public void ExitedRange()
    {
        _animatedSprite2D.Hide();
    }

	public void Interact()
    {
        EmitSignal("interacted");
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();
        if (GetNode<AnimatedSprite2D>("AnimatedSprite2D") is not AnimatedSprite2D)
            warnings.Add("Must have 'AnimatedSprite2D' as indicator");

        return warnings.ToArray();
    }

    public void _on_child_entered_tree(Node node)
    {
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
        }
    }

    public void _on_child_exiting_tree(Node node)
    {
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
        }
    }
}
