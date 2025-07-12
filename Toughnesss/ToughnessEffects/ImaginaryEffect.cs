using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria;

namespace ModJam.Toughnesss.ToughnessEffects;

public class ImaginaryEffect : TEffect
{
    /// <summary>
    /// 处于瘫痪时，受到的伤害 += 120%
    /// </summary>
    protected override void SelfModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 1f + 0.2f;
    }

    /// <summary>
    /// 处于瘫痪时，受到的伤害 += 120%
    /// </summary>
    protected override void SelfModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 1f + 0.2f;
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
        var dust = Dust.NewDustPerfect(rom, DustID.YellowStarDust);
        dust.noGravity = true;
        dust.velocity = Zero;
        npc.rotation = 90f;
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
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
            ParticleOrchestraType.StellarTune,
            new ParticleOrchestraSettings()
            {
                PositionInWorld = npc.Center,
            }
        );
        //npc.StrikeNPC();
        npc.life -= damage * 2;
        var rect = new Rectangle((int)npc.position.X, (int)npc.position.Y, 20, 20);
        CombatText.NewText(rect, YellowGreen, damage * 2);
        base.EndEffect(npc);
    }

}
