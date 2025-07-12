using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace ModJam.Toughnesss.ToughnessEffects;

/// <summary>
/// 量子
/// </summary>
public class QuantumEffect : TEffect
{    
    /// <summary>
    /// 处于瘫痪时，额外受伤
    /// </summary>
    protected override void SelfModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        npc.SubNPCLife(item.damage * 0.4f, color: Blue);
    }

    /// <summary>
    /// 处于瘫痪时，额外受伤
    /// </summary>
    protected override void SelfModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        npc.SubNPCLife(projectile.damage * 0.4f, color: Blue);
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
        var dust = Dust.NewDustPerfect(rom, DustID.BubbleBurst_Blue);
        dust.noGravity = true;
        dust.velocity = Zero;
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
    }

    protected override void SelfApply(NPC npc, Projectile proj)
    {
        time = 60;
    }

    protected override void SelfApply(NPC npc, Item item)
    {
        time = 60;
    }

    protected override void EndEffect(NPC npc)
    {
        ParticleOrchestrator.RequestParticleSpawn(false,
            ParticleOrchestraType.StellarTune,
            new ParticleOrchestraSettings()
            {
                PositionInWorld = npc.Center,
            }
        );
        //npc.StrikeNPC();
        npc.SubNPCLife(damage * 0.6f, color: Blue);
        base.EndEffect(npc);
    }

}
