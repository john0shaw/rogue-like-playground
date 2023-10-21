using Godot;
using Godot.Collections;
using System;

public partial class Merchant : NPCNode
{
    CanvasLayer _shopLayer;
    InventoryGrid _inventoryGrid;
    Node _availableWeapons;
    ItemByLevel _weaponForSale;

    Sprite2D _sellItemSprite;
    Label _sellItemName;
    Label _sellItemCost;
    Label _sellItemGoldLabel;

    Button _buyItemButton;

    bool _shopOpen = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

        _shopLayer = GetNode<CanvasLayer>("Shop");
        _inventoryGrid = GetNode<InventoryGrid>("Shop/TileMap/InventoryGrid");

        _sellItemSprite = GetNode<Sprite2D>("Shop/TileMap/SellItem/Sprite2D");
        _sellItemName = GetNode<Label>("Shop/TileMap/SellItem/Name");
        _sellItemCost = GetNode<Label>("Shop/TileMap/SellItem/Cost");
        _sellItemGoldLabel = GetNode<Label>("Shop/TileMap/SellItem/GoldLabel");

        _buyItemButton = GetNode<Button>("Shop/TileMap/Buttons/Buy");

        _availableWeapons = GetNode<Node>("Shop/AvailableWeapons");
        _weaponForSale = GetRandomWeapon();

        _shopLayer.Hide();

        SetupDialog();
        SetupSellItem();
	}

    public override void Interact()
    {
		Dialog dialog = SayCurrentDialog();
        NPCDialogState state = (NPCDialogState)_dialogStateMachine.State;

        if (GameState.Level > 1)
            dialog.DialogFinished += OpenShop;

		_dialogStateMachine.TransitionTo("RandomChatter");
    }

    void SetupSellItem()
    {
        _sellItemSprite.Texture = _weaponForSale.Weapon.Texture;
        _sellItemName.Text = _weaponForSale.Weapon.Name;
        _sellItemCost.Text = _weaponForSale.Cost.ToString();
        _sellItemGoldLabel.Text = "g";

        SetBuyButtonState();
    }

    void ClearSellItem()
    {
        _sellItemSprite.Texture = null;
        _sellItemName.Text = "Come Back Later!";
        _sellItemCost.Text = "";
        _sellItemGoldLabel.Text = "";

        SetBuyButtonState();
    }

    void SetBuyButtonState()
    {
        _buyItemButton.Disabled = _weaponForSale == null || Player.player.Gold < _weaponForSale.Cost;
    }

	void SetupDialog()
	{
        if (GameState.Level == 1)
        {
            _dialogStateMachine.TransitionTo("Rescue");
        }
        else if (GameState.Level == 2)
        {
            _dialogStateMachine.TransitionTo("BackAtBar");
        }
        else
        {
            _dialogStateMachine.TransitionTo("RandomChatter");
        }
    }

    private ItemByLevel GetRandomWeapon()
    {
        Array<ItemByLevel> availableWeapons = new();
        foreach (Node child in _availableWeapons.GetChildren())
        {
            ItemByLevel weapon = (ItemByLevel)child;
            if (weapon.MinLevel <= GameState.Level)
            {
                availableWeapons.Add(weapon);
            }    
        }

        return availableWeapons.PickRandom();
    }

    public void OpenShop()
    {
        _shopOpen = true;
        _inventoryGrid.Update();
        _shopLayer.Show();
        GetTree().Paused = true;
    }

    public void CloseShop()
    {
        _shopOpen = false;
        _shopLayer.Hide();
        GetTree().Paused = false;
    }

    public void _on_close_button_pressed()
    {
        CloseShop();
    }

    public void _on_buy_pressed()
    {
        if (Player.player.BuyItem(_weaponForSale))
        {
            ClearSellItem();
            _inventoryGrid.Update();
        }
    }

    public void _on_inventory_grid_button_pressed()
    {
        SetBuyButtonState();
    }
}
