//using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;
using ModJam.Toughnesss;
using ReLogic.Content;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Linq;
namespace ModJam.Items;

public class SilverWolf : ModItem
{
    //https://patchwiki.biligame.com/images/sr/3/3f/oy6o9fm6o0rrcwzs9gvgc4rfhvejcts.png
    public static Asset<Texture2D> silverWolfBack;
    public static Texture2D blackTexture;

    public override void Load()
    {
        var filePath = typeof(SilverWolf).FullName.Split(".")[..^1];
        var rootPath = string.Join("/", filePath);
        silverWolfBack = ModContent.Request<Texture2D>(rootPath + "/SilverWolfBack");

        QueueMainThreadAction(() => {
            blackTexture = new Texture2D(instance.GraphicsDevice, 1, 1);
            blackTexture.SetData([Color.Black]);
        });
        
        base.Load();
    }

    public override void SetDefaults()
    {
        Item.accessory = true;
        base.SetDefaults();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<SilverWolfPlayer>().silverWolf = true;
        base.UpdateAccessory(player, hideVisual);
    }

    public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
    {
        var value = Language.GetTextValue(Tooltip.Key);
        var maxl = value.Split("\r\n").MaxBy(a => a.Length);
        var v2 = FontAssets.MouseText.Value.MeasureString(maxl);

        //204 * 204
        var rect = new Rectangle(x - 20, y - 20, (int)(v2.X * 2.3), (int)(v2.X * 2.3));
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, UIScaleMatrix);
        spriteBatch.Draw(blackTexture, rect, null, White, 0f, Zero, SpriteEffects.None, 1f);
        spriteBatch.Draw(silverWolfBack.Value, rect, null, White, 0f, Zero, SpriteEffects.None, 1f);
        ReLogic.Graphics.DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.CombatText[0].Value, value, new Vector2(x, y), Color.Aqua, 0f, Zero, 2f, SpriteEffects.None, 1f);
        ReLogic.Graphics.DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.CombatText[0].Value, value, new Vector2(x+4, y+4), Color.Purple, 0f, Zero, 2f, SpriteEffects.None, 1f);
        ReLogic.Graphics.DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.CombatText[0].Value, value, new Vector2(x+2, y+2), Color.Black, 0f, Zero, 2f, SpriteEffects.None, 1f);
        ReLogic.Graphics.DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.CombatText[0].Value, value, new Vector2(x+ 3, y+3), Color.Pink, 0f, Zero, 2f, SpriteEffects.None, 1f);
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
        return false;
    }

    public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
    {
        return false;
    }

    public override void AddRecipes()
    {
        Recipe.Create(Type)
            .AddIngredient(ItemID.Zenith)
            .Register();
        base.AddRecipes();
    }
}

public class SilverWolfPlayer : ModPlayer
{
    public bool silverWolf = false;
    public override void ResetInfoAccessories()
    {
        silverWolf = false;
        base.ResetInfoAccessories();
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        var gnpc = target.GetGlobalNPC<ToughnessNPC>();
        var item = Player.HeldItem;
        if(!gnpc.ContainToughness(item, out var type)) {
            gnpc.types.Add(type);
        }

        base.ModifyHitNPC(target, ref modifiers);
    }
}
