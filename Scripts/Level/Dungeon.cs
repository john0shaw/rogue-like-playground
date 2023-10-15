using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Dungeon : Node2D
{
    RandomDungeonGenerator _randomDungeonGenerator;
    AnimationPlayer _animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _randomDungeonGenerator = GetNode<RandomDungeonGenerator>("RandomDungeonGenerator");
        if (_randomDungeonGenerator != null)
            _randomDungeonGenerator.Generate();

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Play("FadeIn");

        GameState.Save();

	}
}
