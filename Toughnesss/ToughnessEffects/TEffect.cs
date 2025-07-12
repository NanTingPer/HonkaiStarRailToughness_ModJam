using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ModJam.Toughnesss.ToughnessEffects;

public abstract class TEffect : GlobalNPC
{
    public static Dictionary<ToughnessTypes, Func<NPC, TEffect>> Applys { get; } = new()
    {
        { ToughnessTypes.物理, (npc) => npc.GetGlobalNPC<PhysicalEffect>() }
    };


    public int time = -1;
    /// <summary>
    /// 陷入瘫痪时候武器的攻击力
    /// </summary>
    public int damage = 0;

    /// <summary>
    /// 此方法需要由实现类初始化各种属性
    /// </summary>
    public void Apply(NPC npc, Projectile proj)
    {
        SetTime(npc);
        damage = proj.damage;
        SelfApply(npc, proj);
    }
    public void Apply(NPC npc, Item item)
    {
        SetTime(npc);
        damage = item.damage;
        SelfApply(npc, item);
    }


    protected abstract void SelfApply(NPC npc, Projectile proj);
    protected abstract void SelfApply(NPC npc, Item item);


    public sealed override void AI(NPC npc)
    {
        if(time > 0) {
            time -= 1;
            SelfAI(npc);
            if(time <= 0) { //退出效果时
                var tougNPC = npc.GetGlobalNPC<ToughnessNPC>();
                tougNPC.currentLenght = tougNPC.lengthMax;
                EndEffect(npc);
            }
        }
        base.AI(npc);
    }

    public sealed override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (time > 0) {
            SelfDraw(npc, spriteBatch, screenPos, drawColor);
        }
        base.PostDraw(npc, spriteBatch, screenPos, drawColor);
    }

    /// <summary>
    /// 绘制被击破时的效果
    /// </summary>
    protected virtual void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {

    }

    /// <summary>
    /// 击破效果被触发时的AI
    /// </summary>
    protected virtual void SelfAI(NPC npc)
    {

    }

    /// <summary>
    /// 击破效果失效时调用一次
    /// </summary>
    protected virtual void EndEffect(NPC npc)
    {

    }

    private void SetTime(NPC npc)
    {
        var gnpc = npc.GetGlobalNPC<ToughnessNPC>();
        time = gnpc.paralysisTime;
    }
}
