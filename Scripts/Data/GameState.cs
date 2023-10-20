using System;
using Godot;
static class GameState
{
    const string SAVE_LOCATION = "res://save.tres";

    const float SPAWN_MULTIPLIER = 0.25f;
    const float HEALTH_MULTIPLIER = 0.1f;
    const float  DAMAGE_MULTIPLIER = 0.25f;

    public static bool DebugEnemies = false;
    public static bool DialogOpen = false;
    public static int Level = 1;

    public static float EnemySpawnMultiplier() => GetMultiplier(SPAWN_MULTIPLIER);
    public static float EnemyHealthMultiplier() => GetMultiplier(HEALTH_MULTIPLIER);
    public static float EnemyDamageMultiplier() => GetMultiplier(DAMAGE_MULTIPLIER);

    public static void Save()
    {
        GameStateResource save = new GameStateResource();

        save.Level = Level;
        save.Gold = Player.player.Gold;
        save.Inventory = Player.player.Inventory.Duplicate();

        ResourceSaver.Save(save, SAVE_LOCATION);
    }

    public static void Load()
    {
        GameStateResource save = ResourceLoader.Load<GameStateResource>(SAVE_LOCATION, "", ResourceLoader.CacheMode.Replace);
        Player.player.Inventory = save.Inventory.Duplicate();
        Player.player.Gold = save.Gold;
        Player.player.EmitSignal("InventoryUpdated");
        Level = save.Level;
    }

    public static bool HasSave()
    {
        return FileAccess.FileExists(SAVE_LOCATION);
    }

    private static float GetMultiplier(float multiplier)
    {
        return (1 - multiplier) + ((float)Level * multiplier);
    }
}
