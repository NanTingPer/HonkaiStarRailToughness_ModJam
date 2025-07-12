using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ModLoader;

namespace ModJam.Toughnesss;

public class ToughnessItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    public ToughnessTypes type = ToughnessTypes.冰;

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "ModJam-ToughbessItemImage", "q"));
        base.ModifyTooltips(item, tooltips);
    }

    public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
    {
        if (line.Text == "q") {
            spriteBatch.Draw(ToughnessTextures.Textures[type].Value, new Vector2(line.X, line.Y), null, White, 0f, Zero, 0.1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            return false;
        }
        return base.PreDrawTooltipLine(item, line, ref yOffset);
    }
}