using System;
using Godot;
static class GameState
{
    const string SAVE_LOCATION = "res://save.tres";

    public static bool DebugEnemies = false;
    public static bool DialogOpen = false;
    public static int Level = 1;

    public static float GetLevelEnemyMultiplier()
    {
        return 0.75f + ((float)Level * 0.25f);
    }

    public static void Save()
    {
        GameStateResource save = new GameStateResource();
        save.Level = 5;
        ResourceSaver.Save(save, SAVE_LOCATION);
    }

    public static void Load()
    {
        GameStateResource save = ResourceLoader.Load<GameStateResource>(SAVE_LOCATION);
        Level = save.Level;
    }

    public static bool HasSave()
    {
        return FileAccess.FileExists(SAVE_LOCATION);
    }
}
