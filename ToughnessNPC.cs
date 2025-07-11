using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using static ModJam.ToughnessTextures;

namespace ModJam;
/// <summary>
/// 带有韧性条的NPC
/// </summary>
public class ToughnessNPC : GlobalNPC
{
    public override void SetDefaults(NPC entity)
    {
        var tougnpc = entity.GetGlobalNPC<ToughnessNPC>();
        tougnpc.Types = [
            ToughnessTypes.风,
            ToughnessTypes.冰,
            ToughnessTypes.物理
        ];
        base.SetDefaults(entity);
    }
    
    /// <summary>
    /// 是否是含有弱点的NPC
    /// </summary>
    public bool IsResilient = false;
    /// <summary>
    /// 此NPC拥有的弱点
    /// </summary>
    public List<ToughnessTypes> Types = [];
    /// <summary>
    /// 最大韧性长度
    /// </summary>
    public int LengthMax = 100;
    /// <summary>
    /// 当前韧性长度
    /// </summary>
    public int CurrentLenght = 100;
    /// <summary>
    /// 瘫痪时间
    /// </summary>
    public int ParalysisTime = 60;
    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        float imageSize = 20f;
        float imageFontRatio = 100f; //图片缩放比例
        float toughnessOffset = 0f;
        var toughnessDrawPostition = npc.position + new Vector2(-20, -40);
        foreach (var toughnessType in Types) {
            if(Textures.TryGetValue(toughnessType.ToString(), out var texture)) {
                toughnessDrawPostition += new Vector2(toughnessOffset * imageSize, 0);
                toughnessOffset += 1f;
                spriteBatch.Draw(texture.Value, toughnessDrawPostition - screenPos, null, White, 0f, Zero, imageSize / imageFontRatio, SpriteEffects.None, 1f);
            }
        }


        //条的宽度 => 100
        //    高度 => 5
        var bareakTheBarDrawPostition = (npc.position + new Vector2(-20, -20)) - screenPos;
        var whiteRectangle = new Rectangle((int)bareakTheBarDrawPostition.X, (int)bareakTheBarDrawPostition.Y, 1,1);
        whiteRectangle.Width = (int)((BreakTheBar.Width() / 100f) * ((float)CurrentLenght / (float)LengthMax));
        whiteRectangle.Height = BreakTheBar.Height() / 100;
        spriteBatch.Draw(WhiteTexture, whiteRectangle, White);
        spriteBatch.Draw(BreakTheBar.Value, bareakTheBarDrawPostition, null, White, 0f, Zero, 0.01f, SpriteEffects.None, 1f);

        return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
    }

    public bool ContainToughness(Item item)
    {
        var tougItem = item.GetGlobalItem<ToughnessItem>();
        return Types.Contains(tougItem.Type);
    }
    public bool ContainToughness(Projectile projectile)
    {
        var tougProject = projectile.GetGlobalProjectile<ToughnessProjectile>();
        return Types.Contains(tougProject.Type);
    }


    public override bool InstancePerEntity => true;
}
