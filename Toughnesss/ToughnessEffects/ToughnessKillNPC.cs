using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace HonkaiStarRailToughness.Toughnesss.ToughnessEffects;

public static class ToughnessKillNPC
{
    public static void SubNPCLife(this NPC npc, int life, bool upFont = true, Color color = default)
    {
        if(npc.life - life <= 0) {
            npc.NPCLoot();
            npc.active = false;
        } else {
            npc.life -= life;
        }

        NetMessage.SendStrikeNPC(npc, new NPC.HitInfo() { Damage = life});

        if (upFont) {
            var rect = new Rectangle((int)npc.position.X, (int)npc.position.Y, 20, 20);
            CombatText.NewText(rect, color, life);
        }
    }

    public static void SubNPCLife(this NPC npc, float life, bool upFont = true, Color color = default) => 
        npc.SubNPCLife((int)life, upFont, color);

}
