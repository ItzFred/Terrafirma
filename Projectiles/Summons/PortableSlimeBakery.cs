﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using TerrafirmaRedux.Projectiles.Ranged.Boomerangs;
using System.Collections.Generic;
using Terraria.ID;

namespace TerrafirmaRedux.Projectiles.Summons
{
    internal class PortableSlimeBakery : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.height = 36;
            Projectile.width = 40;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.tileCollide = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.sentry = true;
            Projectile.hide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.5f;
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 160 == 0 && Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust newdust = Dust.NewDustDirect(Projectile.Center, 5, 5, DustID.Smoke, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 1, Color.White, 1f);
                    if (i % 2 == 0)
                    {
                        Dust newslimedust = Dust.NewDustDirect(Projectile.Center, 5, 5, DustID.Water, Main.rand.NextFloat(-1.4f, 1.4f), Main.rand.NextFloat(-1.4f, 0.7f), 0, Color.White, 1f);
                    }
                }
                SoundEngine.PlaySound(SoundID.Item21, Projectile.Center);
                if (Projectile.ai[0] % (160 * 3) == 0)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center, new Vector2(0, -2f),ModContent.ProjectileType<OrangeSlimeFriend>(),Projectile.damage,Projectile.knockBack,Projectile.owner);
                else
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -2f), ModContent.ProjectileType<BlueSlimeFriend>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            
        }
    }
}
