﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace HonkaiStarRailToughness.Toughnesss.ToughnessEffects;

/// <summary>
/// 火
/// <para>受到2次武器伤害2倍的伤害</para>
/// </summary>
public class FirEffect : TEffect
{
    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
        var dust = Dust.NewDustPerfect(rom, DustID.Firefly);
        dust.noGravity = true;
        dust.velocity = Zero;
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
        npc.SubNPCLife(damage * 2, color: DarkRed);
        npc.SubNPCLife(damage * 2, color: DarkRed);
        base.EndEffect(npc);
    }
}
