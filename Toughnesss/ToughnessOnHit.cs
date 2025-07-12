using ModJam.Toughnesss.ToughnessEffects;
using Terraria;
using Terraria.ModLoader;

namespace ModJam.Toughnesss;

/// <summary>
/// OnHit只进行削韧与应用瘫痪效果
/// <para>OnHit only toughens</para>
/// </summary>
public class ToughnessOnHit : GlobalNPC
{
    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        //在这里进行削韧
        if (tougNpc.ContainToughness(item, out var type) && 
            tougNpc.currentLenght > 0) {
            tougNpc.currentLenght -= 1 * player.GetModPlayer<ToughnessPlayer>().killEfficiency;
            if(tougNpc.currentLenght <= 0) {
                //apply
                if(TEffect.Applys.TryGetValue(type, out var value)){
                    value.Invoke(npc).Apply(npc, item);
                } else {
                    tougNpc.currentLenght = tougNpc.lengthMax;
                }
            }
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
            }
            else {
                try {
                    var pla = player[projectile.owner];
                    tougNpc.currentLenght -= 1 * pla.GetModPlayer<ToughnessPlayer>().killEfficiency;
                } catch {
                    tougNpc.currentLenght -= 1;
                }
            }
            if(tougNpc.currentLenght <= 0) {
                //apply
                if (TEffect.Applys.TryGetValue(type, out var value)) {
                    value.Invoke(npc).Apply(npc, projectile);
                } else { //防止没有找到对于的元素而永远无法被削韧
                    tougNpc.currentLenght = tougNpc.lengthMax;
                }
            }
        }

        base.OnHitByProjectile(npc, projectile, hit, damageDone);
    }
}