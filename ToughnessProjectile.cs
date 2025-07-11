using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ModJam;

public class ToughnessProjectile : GlobalProjectile
{
    public ToughnessTypes Type = ToughnessTypes.物理;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if(source is EntitySource_ItemUse itemSource) {
            var projSpawnItem = itemSource.Item;
            var toughessType = projSpawnItem.GetGlobalItem<ToughnessItem>().Type;
            Type = toughessType;
        }
        base.OnSpawn(projectile, source);
    }

    public override bool InstancePerEntity => true;
}
