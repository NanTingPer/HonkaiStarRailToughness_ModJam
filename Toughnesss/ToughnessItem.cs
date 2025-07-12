using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace ModJam.Toughnesss;

public class ToughnessItem : GlobalItem
{
    public const string TOUGHNESSTYPEKEY = "ToughnessTypeItem";
    public override bool InstancePerEntity => true;
    public ToughnessTypes type = ToughnessTypes.未设置;

    public override void SetDefaults(Item entity)
    {
        if (type == ToughnessTypes.未设置) {
            SetToughnessType();
        }
        base.SetDefaults(entity);
    }

    public override void PostReforge(Item item)
    {
        SetToughnessType();
        ToughnessPlayer.postUpdateAction.Add(p => {
            if(p.whoAmI == Main.myPlayer) {
                CombatText.clearAll();
                var pos = p.position + new Vector2(-90f, 0f);
                var rect = new Rectangle((int)pos.X, (int)pos.Y, 10, 10);
                CombatText.NewText(rect, Red, type.ToString());
            }
        });

        base.PostReforge(item);
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

    public override void SaveData(Item item, TagCompound tag)
    {
        tag[TOUGHNESSTYPEKEY] = (int)type;
        base.SaveData(item, tag);
    }

    public override void LoadData(Item item, TagCompound tag)
    {
        if(tag.TryGet<int>(TOUGHNESSTYPEKEY, out var value)) {
            type = (ToughnessTypes)value;
        }
        base.LoadData(item, tag);
    }

    public void SetToughnessType()
    {
        var enumType = typeof(ToughnessTypes);
        var values = Enum.GetValues(enumType);
        ToughnessTypes vtype = ToughnessTypes.量子;
        var entor = values.GetEnumerator();
        for (int i = 0; i < rand.Next(0, values.Length - 1); i++) {
            entor.MoveNext();
            vtype = (ToughnessTypes)entor.Current;
        }
        type = vtype;
    }
}