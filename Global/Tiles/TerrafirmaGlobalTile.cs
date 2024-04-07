﻿using Terrafirma.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrafirma.Global.Tiles
{
    public class TerrafirmaGlobalTile : GlobalTile
    {
        public override void Drop(int i, int j, int type)
        {
            if (type == TileID.LivingMahoganyLeaves && Main.rand.NextBool(3))
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<MahoganyLeaf>());
            }
            base.Drop(i, j, type);
        }
    }
}
