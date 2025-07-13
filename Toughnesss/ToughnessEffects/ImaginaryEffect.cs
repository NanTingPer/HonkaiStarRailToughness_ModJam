using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;
using ModJam.Nets;

namespace ModJam.Toughnesss.ToughnessEffects;

/// <summary>
/// 虚数
/// </summary>
public class ImaginaryEffect : TEffect
{
    /// <summary>
    /// 减速
    /// </summary>
    protected override void SelfAI(NPC npc)
    {
        npc.velocity *= 0.8f;
        base.SelfAI(npc);
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        for (int i = 0; i < 10; i++) {
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
}
