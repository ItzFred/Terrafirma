﻿using Microsoft.Xna.Framework;
using System;
using Terrafirma.Buffs.Debuffs;
using Terrafirma.Data;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrafirma.Projectiles.Ranged.Arrows
{
    internal class AngryArrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileSets.CanBeReflected[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 17;
            Projectile.width = 4;
            Projectile.height = 4;
            DrawOffsetX = -Projectile.width / 2 - 5;
            Projectile.penetrate = 1;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.arrow = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.boss)
            {
                if(Main.rand.NextBool(11))
                target.target = Projectile.owner;
            }
            else
            {
                target.target = Projectile.owner;
            }

            for (int i = 0; i < 5; i++)
            {
                Dust TorchDust = Dust.NewDustPerfect(Projectile.position, DustID.Torch, new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.8f, 1.2f), Projectile.velocity.Y * Main.rand.NextFloat(0.8f, 1.2f)), 0, default, Main.rand.NextFloat(1.5f, 1.8f));
                TorchDust.noGravity = true;
            }
            for (int i = 0; i < 12; i++)
            {
                Dust SmokeDust = Dust.NewDustPerfect(Projectile.position, DustID.Smoke, new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.8f, 1.2f), Projectile.velocity.Y * Main.rand.NextFloat(0.8f, 1.2f)), 128, default, Main.rand.NextFloat(1.2f, 1.7f));
                SmokeDust.noGravity = true;
            }

        }

        public override void AI()
        {
            Projectile.ai[1]++;

            if (Projectile.ai[1] % 2 == 0)
            {
                Dust TorchDust = Dust.NewDustPerfect(Projectile.position, DustID.Torch, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.3f, 0.3f)), 0, default, Main.rand.NextFloat(1.2f, 1.5f));
                TorchDust.noGravity = true;
            }
            Dust SmokeDust = Dust.NewDustPerfect(Projectile.position, DustID.Smoke, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.3f, 0.3f)), 128, default, Main.rand.NextFloat(0.9f, 1.2f));
            SmokeDust.noGravity = true;

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.Kill();

            for (int i = 0; i < 5; i++)
            {
                Dust TorchDust = Dust.NewDustPerfect(Projectile.position, DustID.Torch, new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.8f, 1.2f), Projectile.velocity.Y * Main.rand.NextFloat(0.8f, 1.2f)), 0, default, Main.rand.NextFloat(1.5f, 1.8f));
                TorchDust.noGravity = true;
            }
            for (int i = 0; i < 12; i++)
            {
                Dust SmokeDust = Dust.NewDustPerfect(Projectile.position, DustID.Smoke, new Vector2(Projectile.velocity.X * Main.rand.NextFloat(0.8f, 1.2f), Projectile.velocity.Y * Main.rand.NextFloat(0.8f, 1.2f)), 128, default, Main.rand.NextFloat(1.2f, 1.7f));
                SmokeDust.noGravity = true;
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
