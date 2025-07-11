using Terraria.ModLoader;

namespace ModJam;

public class ToughnessItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    public ToughnessTypes Type = ToughnessTypes.物理;
}