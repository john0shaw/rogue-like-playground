using Godot;
using System;

public partial class PauseScreen : CanvasLayer
{

	bool _paused = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Pause"))
		{
			if (_paused)
			{
				GetTree().Paused = false;
				_paused = false;
				Visible = false;
			}
			else
			{
				GetTree().Paused = true;
				_paused = true;
				Visible = true;
			}
		}
	}

	public void _on_quit_to_menu_pressed()
	{
		GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/Levels/MainMenu.tscn");
    }

	public void _on_quit_to_desktop_pressed()
	{
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit();
    }
}
