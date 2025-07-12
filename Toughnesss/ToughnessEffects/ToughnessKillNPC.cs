using Terraria;

namespace ModJam.Toughnesss.ToughnessEffects;

public static class ToughnessKillNPC
{
    public static void SubNPCLife(this NPC npc, int life)
    {
        if(npc.life - life <= 0) {
            npc.NPCLoot();
            npc.active = false;
        } else {
            npc.life -= life;
        }

    }
}
