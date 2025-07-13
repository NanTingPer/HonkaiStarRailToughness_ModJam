using System.IO;
using static Terraria.ID.NetmodeID;
using Terraria.ModLoader;
using HonkaiStarRailToughness.Toughnesss;
using Terraria;
using HonkaiStarRailToughness.Toughnesss.ToughnessEffects;
using System;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.ID;

namespace HonkaiStarRailToughness;

/// <summary>
/// 1. 为敌人添加韧性条，击破后获得效果
/// </summary>
public class HonkaiStarRailToughness : Mod
{
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        if(netMode == Server) {
            ToughnessTypes type = (ToughnessTypes)reader.ReadInt32();
            int npcIndex = reader.ReadInt32();
            Item item = player[whoAmI].HeldItem;
            try {
                NPC nPC = npc[npcIndex];
                switch (type) {
                    case ToughnessTypes.冰:
                    case ToughnessTypes.火:
                    case ToughnessTypes.物理:
                    case ToughnessTypes.虚数:
                    case ToughnessTypes.量子:
                    case ToughnessTypes.风:
                    case ToughnessTypes.雷:
                        TEffect.Applys[type](nPC).Apply(nPC, item);
                        Logging.PublicLogger.Info("Server-SendApplyEffect: " + type.ToString());
                        break;
                    case ToughnessTypes.未设置: //sub lenght
                        Player ply = player[whoAmI];
                        var l = new NPC.HitModifiers();
                        nPC.GetGlobalNPC<ToughnessOnHit>().SubToughnessLenght(nPC, ply, item);
                        Logging.PublicLogger.Info("Server-SendSubLenght: " + type.ToString());
                        break;
                    default:
                        break;
                }
            } catch(Exception e) {
                Logging.PublicLogger.Error(e.Message + e.StackTrace);
            }
        }
        base.HandlePacket(reader, whoAmI);
    }
}

public class HonkaiStarRailToughnessSystem : ModSystem
{
    public static RecipeGroup Fairy { get; private set; }
    public override void AddRecipeGroups()
    {
        //RecipeGroup.
        List<int> ids =[
            ItemID.FairyCritterPink,
            ItemID.FairyCritterGreen,
            ItemID.FairyCritterBlue
        ];
        Fairy = new RecipeGroup(() => Language.GetTextValue("Mods.HonkaiStarRailToughness.Fairy"), [.. ids]);
        RecipeGroup.RegisterGroup("HonkaiStarRailToughness:Fairy", Fairy);
        base.AddRecipeGroups();
    }
}