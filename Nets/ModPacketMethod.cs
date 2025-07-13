using HonkaiStarRailToughness.Toughnesss;
using Terraria;
using Terraria.ModLoader;

namespace HonkaiStarRailToughness.Nets;

public static class ModPacketMethod
{
    public static void SendApplyEffect(this ModPacket mp, ToughnessTypes type, NPC npc)
    {
        mp.Write((int)type);
        mp.Write(npc.whoAmI);
        mp.Send();
    }

    public static void SendSubLength(this ModPacket mp, NPC npc)
    {
        mp.Write((int)ToughnessTypes.未设置);
        mp.Write(npc.whoAmI);
        mp.Send();
    }
}
