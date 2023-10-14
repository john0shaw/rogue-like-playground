using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class GameState
{
    public static bool DialogOpen = false;
    public static int Level = 1;

    public static float GetLevelEnemyMultiplier()
    {
        return 0.75f + ((float)Level * 0.25f);
    }
}
