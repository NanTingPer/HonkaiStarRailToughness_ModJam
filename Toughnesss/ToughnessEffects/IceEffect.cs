using Microsoft.Xna.Framework.Graphics;
using HonkaiStarRailToughness.Nets;
using Terraria;

namespace HonkaiStarRailToughness.Toughnesss.ToughnessEffects;

/// <summary>
/// 冰破韧效果
/// <para>冻住敌人2秒</para>
/// </summary>
public class IceEffect : TEffect
{
    /// <summary>
    /// 被冻住的位置
    /// </summary>
    [NetField]
    public Vector2 nailPosition = Zero;

    protected override void SelfApply(NPC npc, Projectile proj)
    {
        time = 120;
        nailPosition = npc.Center;
    }

    protected override void SelfApply(NPC npc, Item item)
    {
        time = 120;
        nailPosition = npc.Center;
    }

    protected override void SelfDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var rect = new Rectangle();
        var pos = npc.position - screenPos;
        rect.X = (int)pos.X;
        rect.Y = (int)pos.Y;
        rect.Height = npc.height;
        rect.Width = npc.width;

        spriteBatch.Draw(ToughnessTextures.冰.Value, rect, null, White, 0f, Zero,/*0.5f,*/ SpriteEffects.None, 1f);
        base.SelfDraw(npc, spriteBatch, screenPos, drawColor);
    }

    protected override bool SelfPreAI(NPC npc)
    {
        return false;
        return base.SelfPreAI(npc);
    }

    protected override void SelfPostAI(NPC npc)
    {
        npc.Center = nailPosition;
        base.SelfPostAI(npc);
    }

    //public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
    //{
    //    binaryWriter.WriteVector2(nailPosition);
    //    binaryWriter.Write(time);
    //    binaryWriter.Write(damage);
    //}
    //
    //public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
    //{
    //    nailPosition = binaryReader.ReadVector2();
    //    time = binaryReader.ReadInt32();
    //    damage = binaryReader.ReadInt32();
    //}
}
