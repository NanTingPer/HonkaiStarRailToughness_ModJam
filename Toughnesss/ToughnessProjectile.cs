using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ModJam.Toughnesss;

public class ToughnessProjectile : GlobalProjectile
{
    public ToughnessTypes type = ToughnessTypes.物理;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if(source is EntitySource_ItemUse itemSource) {
            var projSpawnItem = itemSource.Item;
            var toughessType = projSpawnItem.GetGlobalItem<ToughnessItem>().type;
            type = toughessType;
        }
        base.OnSpawn(projectile, source);
    }

    public override bool InstancePerEntity => true;
}
