using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace HonkaiStarRailToughness.Toughnesss;

public static class ToughnessTextures
{
    public readonly static Dictionary<ToughnessTypes, Asset<Texture2D>> Textures = [];
    static ToughnessTextures()
    {
        Textures[ToughnessTypes.冰] = 冰  ;
        Textures[ToughnessTypes.火] = 火  ;
        Textures[ToughnessTypes.雷] = 雷  ;
        Textures[ToughnessTypes.量子] = 量子;
        Textures[ToughnessTypes.风] = 风  ;
        Textures[ToughnessTypes.物理] = 物理;
        Textures[ToughnessTypes.虚数] = 虚数;
        WhiteTexture = new Texture2D(instance.GraphicsDevice, 1, 1);
        WhiteTexture.SetData([White]);
    }

    private const string BasePath = "HonkaiStarRailToughness/Textures/";
    public static readonly Texture2D WhiteTexture;
    public static readonly Asset<Texture2D> 冰  = ModContent.Request<Texture2D>(BasePath + nameof(冰), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> 火  = ModContent.Request<Texture2D>(BasePath + nameof(火), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> 雷  = ModContent.Request<Texture2D>(BasePath + nameof(雷), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> 量子 = ModContent.Request<Texture2D>(BasePath + nameof(量子), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> 风  = ModContent.Request<Texture2D>(BasePath + nameof(风), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> 物理 = ModContent.Request<Texture2D>(BasePath + nameof(物理), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> 虚数 = ModContent.Request<Texture2D>(BasePath + nameof(虚数), AssetRequestMode.ImmediateLoad);
    public static readonly Asset<Texture2D> BreakTheBar = ModContent.Request<Texture2D>(BasePath + "击破条", AssetRequestMode.ImmediateLoad);
}
