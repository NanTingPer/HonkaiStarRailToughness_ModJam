using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using static ModJam.Toughnesss.ToughnessTextures;
using System;
using System.Linq;

namespace ModJam.Toughnesss;
/// <summary>
/// 带有韧性条的NPC
/// </summary>
public class ToughnessNPC : GlobalNPC
{
    /// <summary>
    /// 非破韧伤害只生效比例
    /// </summary>
    public const float NONBREAKINGDAMAGEIMMUNITY = 0.2F;

    public override void SetDefaults(NPC entity)
    {
        var tougnpc = entity.GetGlobalNPC<ToughnessNPC>();
        if (entity.friendly) {
            tougnpc.isResilient = false;
            return;
        }

        //给予三个弱点
        var enumType = typeof(ToughnessTypes);
        var values = Enum.GetValues(enumType);
        IEnumerable<int> list = [];
        for (int i = 0; list.Count() < 3; i++) {
            list = list.Append(rand.Next(0, values.Length));
            list = list.Distinct();
        }
        tougnpc.types.Clear();
        tougnpc.types.AddRange(list.Select(i => (ToughnessTypes)i));
        currentLenght = lengthMax;
        base.SetDefaults(entity);
    }
    
    /// <summary>
    /// 是否是含有弱点的NPC
    /// </summary>
    public bool isResilient = true;
    /// <summary>
    /// 此NPC拥有的弱点
    /// </summary>
    public readonly List<ToughnessTypes> types = [];
    /// <summary>
    /// 最大韧性长度
    /// </summary>
    public int lengthMax = 10;
    /// <summary>
    /// 当前韧性长度
    /// </summary>
    public int currentLenght = 10;
    /// <summary>
    /// 瘫痪时间
    /// </summary>
    public int paralysisTime = 60;
    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (isResilient) {
            float imageSize = 20f;
            float imageFontRatio = 100f; //图片缩放比例
            var toughnessDrawPostition = npc.position + new Vector2(-40, -npc.height);
            foreach (var toughnessType in types) {
                if (Textures.TryGetValue(toughnessType, out var texture)) {
                    toughnessDrawPostition.X += 20f;
                    spriteBatch.Draw(texture.Value, toughnessDrawPostition - screenPos, null, White, 0f, Zero, imageSize / imageFontRatio, SpriteEffects.None, 1f);
                }
            }


            //条的宽度 => 100
            //    高度 => 5
            var bareakTheBarDrawPostition = npc.position + new Vector2(-20, -20) - screenPos;
            var whiteRectangle = new Rectangle((int)bareakTheBarDrawPostition.X, (int)bareakTheBarDrawPostition.Y, 1, 1);
            whiteRectangle.Width = (int)(BreakTheBar.Width() / 100f * (currentLenght / (float)lengthMax));
            whiteRectangle.Height = BreakTheBar.Height() / 100;
            spriteBatch.Draw(WhiteTexture, whiteRectangle, White);
            spriteBatch.Draw(BreakTheBar.Value, bareakTheBarDrawPostition, null, White, 0f, Zero, 0.01f, SpriteEffects.None, 1f);
        }
        return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
    }

    public bool ContainToughness(Item item, out ToughnessTypes type)
    {
        var tougItem = item.GetGlobalItem<ToughnessItem>();
        type = tougItem.type;
        return types.Contains(type);
    }
    public bool ContainToughness(Projectile projectile, out ToughnessTypes type)
    {
        var tougProject = projectile.GetGlobalProjectile<ToughnessProjectile>();
        type = tougProject.Type;
        return types.Contains(type);
    }

    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if (!ContainToughness(item, out _) && isResilient) {
            modifiers.FinalDamage *= NONBREAKINGDAMAGEIMMUNITY;
        }
        base.ModifyHitByItem(npc, player, item, ref modifiers);
    }

    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if (!ContainToughness(projectile, out _) && isResilient) {
            modifiers.FinalDamage *= NONBREAKINGDAMAGEIMMUNITY;
        }
        base.ModifyHitByProjectile(npc, projectile, ref modifiers);
    }
    public override bool InstancePerEntity => true;
}
