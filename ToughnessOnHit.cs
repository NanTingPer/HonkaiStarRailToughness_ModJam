using Terraria;
using Terraria.ModLoader;

namespace ModJam;

public class ToughnessOnHit : GlobalNPC
{
    public override void AI(NPC npc)
    {
        base.AI(npc);
    }

    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        //在这里进行削韧
        if (tougNpc.ContainToughness(item) && tougNpc.CurrentLenght > 0) {
            tougNpc.CurrentLenght -= 1;
        }

        base.OnHitByItem(npc, player, item, hit, damageDone);
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        var tougNpc = npc.GetGlobalNPC<ToughnessNPC>();
        //在这里进行削韧
        if (tougNpc.ContainToughness(projectile) && tougNpc.CurrentLenght > 0) {
            tougNpc.CurrentLenght -= 1;
        }

        base.OnHitByProjectile(npc, projectile, hit, damageDone);
    }
}