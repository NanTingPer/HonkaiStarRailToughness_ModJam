using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModJam.Toughnesss;

public class ToughnessPlayer : ModPlayer
{
    public readonly static List<Action<Player>> postUpdateAction = [];

    public float killEfficiency = 1f;
    public override void PostUpdate()
    {
        foreach (var item1 in postUpdateAction) {
            item1.Invoke(Player);
        }
        postUpdateAction.Clear();
        base.PostUpdate();
    }
}
