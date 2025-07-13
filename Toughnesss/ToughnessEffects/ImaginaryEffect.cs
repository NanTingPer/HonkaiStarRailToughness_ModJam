using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;
using HonkaiStarRailToughness.Nets;

namespace HonkaiStarRailToughness.Toughnesss.ToughnessEffects;

/// <summary>
/// 虚数
/// <para>敌人速度 减半</para>
/// <para>受到的伤害 +40%</para>
/// </summary>
public class ImaginaryEffect : TEffect
{
    /// <summary>
    /// 减速
    /// </summary>
    protected override void SelfAI(NPC npc)
    {
        npc.velocity *= 0.5f;
        base.SelfAI(npc);
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        for (int i = 0; i < 2; i++) {
            var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
            var dust = Dust.NewDustPerfect(rom, DustID.YellowStarDust);
            dust.noGravity = true;
            dust.velocity = Zero;
        }
        npc.rotation = 90f;
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
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
        npc.rotation = 0f;
        base.EndEffect(npc);
    }
    protected override void SelfApply(NPC npc, Projectile proj)
    {
        time = 120;
    }

    protected override void SelfApply(NPC npc, Item item)
    {
        time = 120;
    }

    protected override void SelfModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 1.4f;
        base.SelfModifyHitByItem(npc, player, item, ref modifiers);
    }

    protected override void SelfModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 1.4f;
        base.SelfModifyHitByProjectile(npc, projectile, ref modifiers);
    }
}
