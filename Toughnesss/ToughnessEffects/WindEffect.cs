using Terraria;
using Terraria.GameContent.Drawing;

namespace ModJam.Toughnesss.ToughnessEffects;

/// <summary>
/// 风
/// </summary>
public class WindEffect : TEffect
{
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
        npc.SubNPCLife(damage * 2, color: Green);
        npc.SubNPCLife(damage * 2, color: Green);
        base.EndEffect(npc);
    }
}
