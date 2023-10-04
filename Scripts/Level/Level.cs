using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
    private RandomDungeonGenerator _randomDungeonGenerator;
    private RandomNumberGenerator _rng;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _randomDungeonGenerator = GetNode<RandomDungeonGenerator>("RandomDungeonGenerator");

        _rng = new RandomNumberGenerator();
        _rng.Randomize();

        _randomDungeonGenerator.SetRng(_rng);
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
