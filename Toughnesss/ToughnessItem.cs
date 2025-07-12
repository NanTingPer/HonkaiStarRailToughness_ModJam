using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ModLoader;

namespace ModJam.Toughnesss;

public class ToughnessItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    public ToughnessTypes type = ToughnessTypes.火;

    public override void SetDefaults(Item entity)
    {
        var enumType = typeof(ToughnessTypes);
        var values = Enum.GetValues(enumType);
        ToughnessTypes vtype = ToughnessTypes.量子;
        var entor = values.GetEnumerator();
        for (int i = 0; i < rand.Next(0, values.Length); i++) {
            entor.MoveNext();
            vtype = (ToughnessTypes)entor.Current;
        }

        type = vtype;
        base.SetDefaults(entity);
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "ModJam-ToughbessItemImage", "q"));
        base.ModifyTooltips(item, tooltips);
    }

    public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
    {
        if (line.Text == "q") {
            spriteBatch.Draw(ToughnessTextures.Textures[type].Value, new Vector2(line.X, line.Y), null, White, 0f, Zero, 0.3f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            return false;
        }
        return base.PreDrawTooltipLine(item, line, ref yOffset);
    }
}