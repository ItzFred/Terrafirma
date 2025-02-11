﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terrafirma.Projectiles.Summon.Sentry.Hardmode;

namespace Terrafirma.Items.Weapons.Summoner.Sentry.Hardmode
{
    internal class CursedFlameSentryStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.knockBack = 1f;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.DD2_DefenseTowerSpawn;

            Item.width = 44;
            Item.height = 44;

            Item.autoReuse = true;
            Item.noMelee = true;

            Item.ArmorPenetration = 15;

            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.shoot = ModContent.ProjectileType<CursedFlameSentry>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
            .AddIngredient(ItemID.SoulofFright, 20)
            .AddIngredient(ItemID.EbonstoneBlock, 20)
            .AddIngredient(ItemID.CursedFlame, 10)
            .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player == Main.LocalPlayer)
            {
                int WorldX;
                int WorldY;
                int PushUpY;
                Main.LocalPlayer.FindSentryRestingSpot(type, out WorldX, out WorldY, out PushUpY);

                Projectile.NewProjectile(source, new Vector2(WorldX, WorldY - PushUpY + 15), Vector2.Zero, type, damage, 0, player.whoAmI, 0, 0, 0);
                player.UpdateMaxTurrets();
            }
            return false;
        }
    }
}
