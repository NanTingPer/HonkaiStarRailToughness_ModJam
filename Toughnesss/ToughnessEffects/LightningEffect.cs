using Microsoft.Xna.Framework.Graphics;
using HonkaiStarRailToughness.Nets;
using System.IO;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace HonkaiStarRailToughness.Toughnesss.ToughnessEffects;

/// <summary>
/// 雷
/// <para>受到2次武器伤害2倍的伤害</para>
/// </summary>
public class LightningEffect : TEffect
{    
    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        for (int i = 0; i < 2; i++) {
            var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
            var dust = Dust.NewDustPerfect(rom, DustID.VioletMoss);
            dust.noGravity = true;
            dust.velocity = Zero;
        }
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
    }

    protected override void SelfApply(NPC npc, Projectile proj)
    {
        time = 10;
    }

    protected override void SelfApply(NPC npc, Item item)
    {
        time = 10;
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
        npc.SubNPCLife(damage * 2, color: Violet);
        npc.SubNPCLife(damage * 2, color: Violet);
        base.EndEffect(npc);
    }
}
