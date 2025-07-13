using Microsoft.Xna.Framework.Graphics;
using ModJam.Nets;
using System.IO;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace ModJam.Toughnesss.ToughnessEffects;

/// <summary>
/// 风
/// </summary>
public class WindEffect : TEffect
{
    protected override void SelfApply(NPC npc, Projectile proj)
    {
        time = 1;
    }

    protected override void SelfApply(NPC npc, Item item)
    {
        time = 1;
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
        npc.SubNPCLife(damage * 2, color: Green);
        npc.SubNPCLife(damage * 2, color: Green);
        base.EndEffect(npc);
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        for (int i = 0; i < 10; i++) {
            var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
            var dust = Dust.NewDustPerfect(rom, DustID.GreenFairy);
            dust.noGravity = true;
            dust.velocity = Zero;
        }
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
    }
}
