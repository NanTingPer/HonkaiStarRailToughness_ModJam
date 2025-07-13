using ModJam.Nets;
using ModJam.Toughnesss.ToughnessEffects;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ModJam.Toughnesss;

/// <summary>
/// OnHit只进行削韧与应用瘫痪效果
/// <para>OnHit only toughens</para>
/// </summary>
public class ToughnessOnHit : GlobalNPC
{
    public readonly static Dictionary<ToughnessTypes, float> toughnessCoefficient = new()
    {
        { ToughnessTypes.物理, 2f },
        { ToughnessTypes.火, 2f },
        { ToughnessTypes.风, 1.5f },
        { ToughnessTypes.雷, 1f },
        { ToughnessTypes.冰, 1f },
        { ToughnessTypes.量子, 0.5f },
        { ToughnessTypes.虚数, 0.5f }
    };

    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        //在这里进行削韧
        if (tougNpc.ContainToughness(item, out var type) && tougNpc.currentLenght > 0) {
            if (netMode != SinglePlayer && netMode != Server) {
                Mod.GetPacket().SendSubLength(npc);
                npc.netUpdate = true;
            }

            if (netMode == SinglePlayer) {
                SubToughnessLenght(npc, player, item);
            }
        }
        base.OnHitByItem(npc, player, item, hit, damageDone);
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        //在这里进行削韧
        if (tougNpc.ContainToughness(projectile, out var type) && tougNpc.currentLenght > 0) {
            if(netMode != SinglePlayer && netMode != Server) {
                Mod.GetPacket().SendSubLength(npc);
                npc.netUpdate = true;
            }

            if (netMode == SinglePlayer) {
                if(projectile.owner != 255)
                    SubToughnessLenght(npc, player[projectile.owner], projectile);
            }

        }
        base.OnHitByProjectile(npc, projectile, hit, damageDone);
    }

    public void SubToughnessLenght(NPC npc, Player player, Item item)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        var type = item.GetGlobalItem<ToughnessItem>().type;
        tougNpc.currentLenght -= (1 * player.GetModPlayer<ToughnessPlayer>().killEfficiency) * toughnessCoefficient[type];
        if (tougNpc.currentLenght <= 0) {
            ApplyEffect(npc, type, item);
        }
    }

    public void SubToughnessLenght(NPC npc, Player player, Projectile proj)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        var type = proj.GetGlobalProjectile<ToughnessProjectile>().type;
        tougNpc.currentLenght -= (1 * player.GetModPlayer<ToughnessPlayer>().killEfficiency) * toughnessCoefficient[type];
        if(tougNpc.currentLenght <= 0) {
            ApplyEffect(npc, type, null, proj);
        }
    }

    public void ApplyEffect(NPC npc, ToughnessTypes type, Item item = null, Projectile proj = null)
    {
        if(item != null) {
            TEffect.Applys[type](npc).Apply(npc, item);
        } else if(proj != null) {
            TEffect.Applys[type](npc).Apply(npc, proj);
        } else {
            var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
            tougNpc.currentLenght = tougNpc.lengthMax;
        }
    }
}