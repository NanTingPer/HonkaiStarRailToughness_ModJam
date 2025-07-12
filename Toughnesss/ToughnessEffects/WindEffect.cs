using Terraria;
using Terraria.GameContent.Drawing;

namespace ModJam.Toughnesss.ToughnessEffects;

public class WindEffect : TEffect
{
    /// <summary>
    /// 处于瘫痪时，受到的伤害 += 120%
    /// </summary>
    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 1f + 0.2f;
        base.ModifyHitByItem(npc, player, item, ref modifiers);
    }

    /// <summary>
    /// 处于瘫痪时，受到的伤害 += 120%
    /// </summary>
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 1f + 0.2f;
        base.ModifyHitByProjectile(npc, projectile, ref modifiers);
    }

    protected override void SelfApply(NPC npc, Projectile proj)
    {
    }

    protected override void SelfApply(NPC npc, Item item)
    {
    }

    protected override void EndEffect(NPC npc)
    {
        ParticleOrchestrator.RequestParticleSpawn(false,
            ParticleOrchestraType.Keybrand,
            new ParticleOrchestraSettings()
            {
                PositionInWorld = npc.Center,
            }
        );
        //npc.StrikeNPC();
        npc.life -= damage * 2;
        var rect = new Rectangle((int)npc.position.X, (int)npc.position.Y, 20, 20);
        CombatText.NewText(rect, Green, damage * 2);
        base.EndEffect(npc);
    }
}
