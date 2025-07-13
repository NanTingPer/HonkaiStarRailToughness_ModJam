using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HonkaiStarRailToughness.Items;

namespace HonkaiStarRailToughness.Toughnesss;

public class ToughnessPlayer : ModPlayer
{
    public readonly static List<Action<Player>> postUpdateAction = [];

    /// <summary>
    /// 击破倍率
    /// </summary>
    public float killEfficiency = 1f;
    /// <summary>
    /// <see cref="SilverWolf"/>
    /// </summary>
    public bool silverWolf = false;
    public override void PostUpdate()
    {
        foreach (var item1 in postUpdateAction) {
            item1.Invoke(Player);
        }
        postUpdateAction.Clear();
        base.PostUpdate();
    }

    
    public override void ResetInfoAccessories()
    {
        silverWolf = false;
        base.ResetInfoAccessories();
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        var gnpc = target.GetGlobalNPC<ToughnessNPC>();
        var item = Player.HeldItem;
        if (!gnpc.ContainToughness(item, out var type)) {
            gnpc.types.Add(type);
        }

        base.ModifyHitNPC(target, ref modifiers);
    }
}
