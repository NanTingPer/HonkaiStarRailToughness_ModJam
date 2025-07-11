using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ModJam;

public static class ToughnessTextures
{
    public static Dictionary<string, Asset<Texture2D>> Textures = [];
    static ToughnessTextures()
    {
        Textures["冰"] = 冰  ;
        Textures["火"] = 火  ;
        Textures["雷"] = 雷  ;
        Textures["量子"] = 量子;
        Textures["风"] = 风  ;
        Textures["物理"] = 物理;
        Textures["虚数"] = 虚数;
        WhiteTexture = new Texture2D(instance.GraphicsDevice, 1, 1);
        WhiteTexture.SetData([White]);
    }

    private const string BasePath = "ModJam/Textures/";
    public static readonly Texture2D WhiteTexture;
    private static readonly Asset<Texture2D> 冰  = ModContent.Request<Texture2D>(BasePath + nameof(冰), AssetRequestMode.ImmediateLoad);
    private static readonly Asset<Texture2D> 火  = ModContent.Request<Texture2D>(BasePath + nameof(火), AssetRequestMode.ImmediateLoad);
    private static readonly Asset<Texture2D> 雷  = ModContent.Request<Texture2D>(BasePath + nameof(雷), AssetRequestMode.ImmediateLoad);
    private static readonly Asset<Texture2D> 量子 = ModContent.Request<Texture2D>(BasePath + nameof(量子), AssetRequestMode.ImmediateLoad);
    private static readonly Asset<Texture2D> 风  = ModContent.Request<Texture2D>(BasePath + nameof(风), AssetRequestMode.ImmediateLoad);
    private static readonly Asset<Texture2D> 物理 = ModContent.Request<Texture2D>(BasePath + nameof(物理), AssetRequestMode.ImmediateLoad);
    private static readonly Asset<Texture2D> 虚数 = ModContent.Request<Texture2D>(BasePath + nameof(虚数), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> BreakTheBar = ModContent.Request<Texture2D>(BasePath + "击破条", AssetRequestMode.ImmediateLoad);
}
