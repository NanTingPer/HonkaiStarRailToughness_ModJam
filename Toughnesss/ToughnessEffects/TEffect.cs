using Microsoft.Xna.Framework.Graphics;
using ModJam.Nets;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using static Terraria.ID.NetmodeID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModJam.Toughnesss.ToughnessEffects;

public abstract class TEffect : GlobalNPC
{
    public static Dictionary<ToughnessTypes, Func<NPC, TEffect>> Applys { get; } = new()
    {
        { ToughnessTypes.物理, (npc) => npc.GetGlobalNPC<PhysicalEffect>() },
        { ToughnessTypes.冰, (npc) => npc.GetGlobalNPC<IceEffect>() },
        { ToughnessTypes.风, (npc) => npc.GetGlobalNPC<WindEffect>() },
        { ToughnessTypes.火, (npc) => npc.GetGlobalNPC<FirEffect>() },
        { ToughnessTypes.量子, (npc) => npc.GetGlobalNPC<QuantumEffect>() },
        { ToughnessTypes.虚数, (npc) => npc.GetGlobalNPC<ImaginaryEffect>() },
        { ToughnessTypes.雷, (npc) => npc.GetGlobalNPC<LightningEffect>() }
    };

    private static Dictionary<Type, ToughnessTypes> TTT { get; } = new()
    {
        { typeof(PhysicalEffect), ToughnessTypes.物理 },
        { typeof(IceEffect), ToughnessTypes.冰 },
        { typeof(WindEffect), ToughnessTypes.风 },
        { typeof(FirEffect), ToughnessTypes.火 },
        { typeof(QuantumEffect), ToughnessTypes.量子 },
        { typeof(ImaginaryEffect), ToughnessTypes.虚数 },
        { typeof(LightningEffect), ToughnessTypes.雷 },
    };

    [NetField]
    public int time = -1;
    /// <summary>
    /// 陷入瘫痪时候武器的攻击力
    /// </summary>
    [NetField]
    public int damage = 0;

    /// <summary>
    /// 此方法需要由实现类初始化各种属性
    /// </summary>
    public void Apply(NPC npc, Projectile proj)
    {
        //if (netMode != Server && netMode != SinglePlayer) {
        //    var type = TTT[this.GetType()];
        //    Mod.GetPacket().SendApplyEffect(type, npc);
        //    Logging.PublicLogger.Info("Client-SendApplyEffect :  " + type.ToString());
        //}
        SetTime(npc);
        damage = proj.damage;
        SelfApply(npc, proj);
        npc.netUpdate = true;
    }
    public void Apply(NPC npc, Item item)
    {
        //if (netMode != Server && netMode != SinglePlayer) {
        //    var type = TTT[this.GetType()];
        //    Mod.GetPacket().SendApplyEffect(type, npc);
        //    Logging.PublicLogger.Info("Client-SendApplyEffect :  " + type.ToString());
        //}
        SetTime(npc);
        damage = item.damage;
        SelfApply(npc, item);
        npc.netUpdate = true;
    }

    protected abstract void SelfApply(NPC npc, Projectile proj);
    protected abstract void SelfApply(NPC npc, Item item);


    public sealed override void AI(NPC npc)
    {
        if(time > 0) {
            SelfAI(npc);
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

    public sealed override void PostAI(NPC npc)
    {
        if (time > 0) {
            SelfPostAI(npc);
            time -= 1;
            if (time <= 0) { //退出效果时
                var tougNPC = npc.GetGlobalNPC<ToughnessNPC>();
                tougNPC.currentLenght = tougNPC.lengthMax;
                EndEffect(npc);
            }
        }
        base.PostAI(npc);
    }

    public sealed override bool PreAI(NPC npc)
    {
        if (time > 0) {
            return SelfPreAI(npc);
        }
        return base.PreAI(npc);
    }

    public sealed override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if(time > 0) {
            SelfModifyHitByItem(npc, player, item, ref modifiers);
        }
        base.ModifyHitByItem(npc, player, item, ref modifiers);
    }

    public sealed override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if(time > 0) {
            SelfModifyHitByProjectile(npc, projectile, ref modifiers);
        }
        base.ModifyHitByProjectile(npc, projectile, ref modifiers);
    }


    protected virtual void SelfModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {

    }

    protected virtual void SelfModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {

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
    /// <returns>是否执行原有AI</returns>
    protected virtual void SelfAI(NPC npc)
    {
    }

    /// <summary>
    /// 如果不要AI执行，请手动设置 PreAI返回false
    /// </summary>
    protected virtual void SelfPostAI(NPC npc)
    {

    }

    protected virtual bool SelfPreAI(NPC npc)
    {
        return true;
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

    public sealed override bool InstancePerEntity => true;

    public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
    {
        base.ReceiveExtraAI(npc, bitReader, binaryReader);
    }

    public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        base.SendExtraAI(npc, bitWriter, binaryWriter);
    }
}
