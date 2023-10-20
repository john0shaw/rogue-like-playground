using System;
using Godot;
static class GameState
{
    const string SAVE_LOCATION = "res://save.tres";

    const float SPAWN_MULTIPLIER = 0.25f;
    const float HEALTH_MULTIPLIER = 0.1f;
    const float  DAMAGE_MULTIPLIER = 0.25f;
    const float PLAYER_DAMAGE_MULTIPLIER = 0.1f;
    const float PLAYER_DEFENCE_MULTIPLIER = 0.1f;

    public static bool DebugEnemies = false;
    public static bool DialogOpen = false;
    public static int Level = 1;

    public static float EnemySpawnMultiplier() => GetMultiplier(SPAWN_MULTIPLIER);
    public static float EnemyHealthMultiplier() => GetMultiplier(HEALTH_MULTIPLIER);
    public static float EnemyDamageMultiplier() => GetMultiplier(DAMAGE_MULTIPLIER);
    public static float PlayerStrengthMultiplier() => GetMultiplier(PLAYER_DAMAGE_MULTIPLIER);
    public static float PlayerDefenceDamageReduction()
    {
        return Player.player.Defence * PLAYER_DEFENCE_MULTIPLIER;
    }

    public static void Save(GameStateResource save = null)
    {
        if (save == null)
        {
            save = new GameStateResource
            {
                Level = Level,
                Gold = Player.player.Gold,
                Inventory = Player.player.Inventory.Duplicate(),
                MaxHealth = Player.player.MaxHealth,
                Health = Player.player.Health,
                Strength = Player.player.Strength,
                Defence = Player.player.Defence
            };
        }
        
        ResourceSaver.Save(save, SAVE_LOCATION);
    }

    public static void Load()
    {
        GameStateResource save = ResourceLoader.Load<GameStateResource>(SAVE_LOCATION, "", ResourceLoader.CacheMode.Replace);

        Player.player.Inventory = save.Inventory.Duplicate();
        Player.player.Gold = save.Gold;
        Player.player.MaxHealth = save.MaxHealth;
        Player.player.Health = save.Health;
        Player.player.Strength = save.Strength;
        Player.player.Defence = save.Defence;

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
