using HonkaiStarRailToughness.Toughnesss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HonkaiStarRailToughness.Items.UpToughnessTypes;

public abstract class UpToughnessType : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 9999;
        Item.value = 0;
        base.SetDefaults();
    }
    protected abstract ToughnessTypes ToughnessType { get; }
    public override bool CanRightClick()
    {
        return true;
    }

    public override void RightClick(Player player)
    {
        var entity = player.HeldItem;
        if (entity.createTile == -1 && entity.damage != -1) {
            entity.GetGlobalItem<ToughnessItem>().type = ToughnessType;
        }
        base.RightClick(player);
    }

    public override void AddRecipes()
    {
        Recipe.Create(Type)
            .AddRecipeGroup(HonkaiStarRailToughnessSystem.Fairy, 2)
            .Register();
        base.AddRecipes();
    }
}

public class UpQuantum : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.量子;
}

public class UpPhysical : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.物理;
}

public class UpLightning : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.雷;
}

public class UpImaginary : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.虚数;
}

public class UpWind : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.风;
}

public class UpFir : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.火;
}

public class UpIce : UpToughnessType
{
    protected override ToughnessTypes ToughnessType => ToughnessTypes.冰;
}