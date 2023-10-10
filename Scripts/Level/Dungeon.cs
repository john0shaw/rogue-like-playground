using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Dungeon : Node2D
{
    private RandomDungeonGenerator _randomDungeonGenerator;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _randomDungeonGenerator = GetNode<RandomDungeonGenerator>("RandomDungeonGenerator");
        if (_randomDungeonGenerator != null)
            _randomDungeonGenerator.Generate();
	}

	/// <summary>
    /// 
    /// </summary>
    /// <param name="delta"></param>
	public override void _Process(double delta)
	{

	}
}