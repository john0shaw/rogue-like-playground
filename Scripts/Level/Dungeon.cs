using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Dungeon : Node2D
{
    [Export] public DialogResource DeathDialog;

    InGameUI _ui;
    RandomDungeonGenerator _randomDungeonGenerator;
    AnimationPlayer _animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GameState.Load();
        _ui = GetNode<InGameUI>("GameplayUI");
        _randomDungeonGenerator = GetNode<RandomDungeonGenerator>("RandomDungeonGenerator");
        if (_randomDungeonGenerator != null)
            _randomDungeonGenerator.Generate();

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Play("FadeIn");
	}

    public async void _on_player_died()
    {
        _animationPlayer.PlayBackwards("FadeIn");
        await ToSignal(_animationPlayer, "animation_finished");

        Dialog dialog = _ui.SayDialog(DeathDialog);
        await ToSignal(dialog, "DialogFinished");
        
        GameState.ClearSave();
        GetTree().ChangeSceneToFile("res://Scenes/Levels/MainMenu.tscn");
    }
}
