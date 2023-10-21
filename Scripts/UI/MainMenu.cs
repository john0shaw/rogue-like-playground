using Godot;
using Godot.Collections;
using System;

public partial class MainMenu : Node2D
{
	[Export] public PackedScene DungeonScene;
	[Export] public DialogResource StartingDialog;
	[Export] public DialogResource SaveWarningDialog;
	[Export] public PackedScene DialogScene;

	[Export] public float CameraMoveSpeed = 200f;

	bool _selectionMade = false;
	Camera2D _camera;
	AnimationPlayer _animationPlayer;
	ParallaxBackground _parallaxBackground;
	VBoxContainer _buttonsContainer;
	MainMenuUI _uiLayer;
	AudioStreamPlayer _audioStreamPlayer;
	Button _continueButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_uiLayer = GetNode<MainMenuUI>("UI");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_parallaxBackground = GetNode<ParallaxBackground>("UI/ParallaxBackground");
		_buttonsContainer = GetNode<VBoxContainer>("UI/Buttons");
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("UI/AudioStreamPlayer");
		_continueButton = GetNode<Button>("UI/Buttons/Continue");

		_continueButton.Disabled = !GameState.HasSave();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_selectionMade)
			_camera.MoveLocalX((float)delta * CameraMoveSpeed);
	}

	public void NewGame()
	{
		GameStateResource newSave = new GameStateResource
		{
			Level = 1,
			Gold = 0,
			Inventory = new Array<Item>(),
			MaxHealth = 10,
			Health = 10,
			Strength = 1,
			Defence = 1
		};
		Weapon startingWeapon = ResourceLoader.Load<Weapon>(Player.StartingWeaponResource, cacheMode:ResourceLoader.CacheMode.Replace);
		newSave.Inventory.Add(startingWeapon);
		newSave.EquippedWeapon = startingWeapon;

		GameState.Save(newSave);

        Dialog dialog = (Dialog)DialogScene.Instantiate();
        dialog.UIAudioPlayer = _audioStreamPlayer;
        dialog.DialogResource = StartingDialog;
        dialog.DialogFinished += _on_start_dialog_finished;
        _uiLayer.AddChild(dialog);
    }

	public async void _on_new_game_pressed()
	{
		_uiLayer.PlayButtonMouseClickEffect();
		_animationPlayer.Play("FadeOut");
		await ToSignal(_animationPlayer, "animation_finished");
		_parallaxBackground.Hide();
		_buttonsContainer.Hide();
		_animationPlayer.PlayBackwards("FadeOut");
		await ToSignal(_animationPlayer, "animation_finished");

		if (GameState.HasSave())
		{
			Dialog saveWarningDialog = (Dialog)DialogScene.Instantiate();
			saveWarningDialog.UIAudioPlayer = _audioStreamPlayer;
			saveWarningDialog.DialogResource = SaveWarningDialog;
			saveWarningDialog.ConfirmClicked += _on_save_warning_confirm;
			saveWarningDialog.CancelClicked += _on_save_warning_cancel;
			_uiLayer.AddChild(saveWarningDialog);
		}
		else
		{
			NewGame();
		}
	}

	public async void _on_continue_pressed()
	{
        _uiLayer.PlayButtonMouseClickEffect();
        _animationPlayer.Play("FadeOut");
        await ToSignal(_animationPlayer, "animation_finished");

        GetTree().ChangeSceneToPacked(DungeonScene);
    }

	public void _on_exit_pressed()
	{
        _uiLayer.PlayButtonMouseClickEffect();
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
		GetTree().Quit();
	}

	public async void _on_start_dialog_finished()
	{
		_animationPlayer.Play("FadeOut");
		await ToSignal(_animationPlayer, "animation_finished");
		GetTree().ChangeSceneToPacked(DungeonScene);
	}

	public void _on_save_warning_confirm()
	{
		NewGame();
	}

	public async void _on_save_warning_cancel()
	{
		_animationPlayer.Play("FadeOut");
		await ToSignal(_animationPlayer, "animation_finished");
		GetTree().ReloadCurrentScene();
	}
}
