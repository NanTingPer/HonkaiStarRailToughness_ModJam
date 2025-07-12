using ModJam.Toughnesss.ToughnessEffects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
        if (tougNpc.ContainToughness(item, out var type) && 
            tougNpc.currentLenght > 0) {
            SubToughnessLenght(npc, player, type);
            if (tougNpc.currentLenght <= 0) {
                //apply
                if (TEffect.Applys.TryGetValue(type, out var value)){
                    value.Invoke(npc).Apply(npc, item);
                } else {
                    tougNpc.currentLenght = tougNpc.lengthMax;
                }
            }
            npc.netUpdate = true;
        }
        base.OnHitByItem(npc, player, item, hit, damageDone);
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
            var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        //在这里进行削韧
        if (tougNpc.ContainToughness(projectile, out var type) &&
            tougNpc.currentLenght > 0) {
            if (projectile.owner > player.Length) {
                tougNpc.currentLenght -= 1;
            } else {
                try {
                    var pla = player[projectile.owner];
                    SubToughnessLenght(npc, pla, type);
                } catch {
                    tougNpc.currentLenght -= 0.5f;
                }
            }
            if (tougNpc.currentLenght <= 0) {
                //apply
                if (TEffect.Applys.TryGetValue(type, out var value)) {
                    value.Invoke(npc).Apply(npc, projectile);
                } else { //防止没有找到对于的元素而永远无法被削韧
                    tougNpc.currentLenght = tougNpc.lengthMax;
                }
            }
            npc.netUpdate = true;
            Logging.PublicLogger.Info(string.Join(',', tougNpc.types.Select(f => f.ToString())));

        }
        base.OnHitByProjectile(npc, projectile, hit, damageDone);
    }

    public void SubToughnessLenght(NPC npc, Player player, ToughnessTypes type)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        tougNpc.currentLenght -= (1 * player.GetModPlayer<ToughnessPlayer>().killEfficiency) * toughnessCoefficient[type];
    }
}