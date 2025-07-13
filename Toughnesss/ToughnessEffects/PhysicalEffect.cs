using Microsoft.Xna.Framework.Graphics;
using HonkaiStarRailToughness.Nets;
using System.IO;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace HonkaiStarRailToughness.Toughnesss.ToughnessEffects;

/// <summary>
/// 物理
/// <para>造成生命百分比伤害</para>
/// </summary>
public class PhysicalEffect : TEffect
{
    protected override void SelfApply(NPC npc, Projectile proj)
    {
        time = 240;
    }

    protected override void SelfApply(NPC npc, Item item)
    {
        time = 240;
    }

    /// <summary>
    /// 造成16%生命上限的伤害
    /// </summary>
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
        var subLife = npc.lifeMax * 0.16f;
        npc.SubNPCLife((int)subLife, color: WhiteSmoke);
        base.EndEffect(npc);
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        for (int i = 0; i < 2; i++) {
            var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
            var dust = Dust.NewDustPerfect(rom, DustID.PlanteraBulb);
            dust.noGravity = true;
            dust.velocity = Zero;
        }
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
    }
}
