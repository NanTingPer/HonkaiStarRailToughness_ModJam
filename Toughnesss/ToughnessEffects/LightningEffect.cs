using Microsoft.Xna.Framework.Graphics;
using ModJam.Nets;
using System.IO;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace ModJam.Toughnesss.ToughnessEffects;

/// <summary>
/// 雷
/// </summary>
public class LightningEffect : TEffect
{    
    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var rom = npc.position + RandomVector2(rand, -npc.height, npc.height);
        var dust = Dust.NewDustPerfect(rom, DustID.VioletMoss);
        dust.noGravity = true;
        dust.velocity = Zero;
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
