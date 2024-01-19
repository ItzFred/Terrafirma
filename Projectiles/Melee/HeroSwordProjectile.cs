﻿using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using TerrafirmaRedux.Items.Weapons.Melee.Swords;
using TerrafirmaRedux.Particles;

namespace TerrafirmaRedux.Projectiles.Melee
{
    public class HeroSwordProjectile: ModProjectile
    {
        public override string Texture => "TerrafirmaRedux/Items/Weapons/Melee/Swords/HeroSword";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 800;
            DrawOriginOffsetX = -9;
            DrawOriginOffsetY = -9;
            Projectile.friendly = true;
            Projectile.scale = 0.1f;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.CritChance = 4;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
             
            if (Projectile.ai[0] > 30)
            {
                Projectile.scale = 1f;
                Projectile.rotation += 0.6f * (Projectile.ai[0] / 2f);
                if (Projectile.ai[0] > 600) Projectile.velocity = Vector2.Normalize(Main.player[Projectile.owner].MountedCenter - Projectile.Center) * (Projectile.ai[0] * 0.1f);
                else Projectile.velocity += Vector2.Normalize(Main.player[Projectile.owner].MountedCenter - Projectile.Center);
                //Projectile.velocity += Vector2.Lerp(Projectile.velocity, Main.player[Projectile.owner].MountedCenter - Projectile.Center, 0.5f);
                if (Projectile.Hitbox.Intersects(Main.player[Projectile.owner].Hitbox) || Projectile.Center.Distance(Main.player[Projectile.owner].MountedCenter) < Projectile.ai[0] / 4f) Projectile.Kill();
            }
            else
            {
                Projectile.scale = Math.Clamp(Projectile.scale + (1f - Projectile.scale) / 4f, 0f, 1f);
            }

            Projectile.velocity *= Projectile.ai[0] > 30 ? 1f : 0.9f ;
            if (Projectile.ai[0] == 30) Projectile.velocity *= -1;

            if (Projectile.Center.Distance(Main.player[Projectile.owner].MountedCenter) > 1000f) Projectile.Kill();

            if (Projectile.ai[0] % 8 == 0) SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);

            if (Projectile.ai[0] % 10 == 0 && Main.LocalPlayer == Main.player[Projectile.owner])
            {
                Projectile newproj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(20f, 0f).RotatedBy(Projectile.Center.DirectionTo(Main.MouseWorld).ToRotation()), ModContent.ProjectileType<HeroSwordShot>(), Projectile.damage + Projectile.damage * (int)Math.Clamp(Projectile.ai[0] / 150f, 0f, 4f), Projectile.knockBack, Projectile.owner, 0, 0, 0);
                newproj.scale = 1f + Math.Clamp(Projectile.ai[0] / 600f, 0f, 1f);
            }


            if (Main.player[Projectile.owner].HeldItem.type != ModContent.ItemType<HeroSword>()) Projectile.Kill();
            

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ParticleSystem.AddParticle(new BigSparkle(), Projectile.Center, Vector2.Zero, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 0), 0, 10, 1, 1f, Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2));
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            ParticleSystem.AddParticle(new BigSparkle(), Projectile.Center, Vector2.Zero, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 0), 0, 10, 1, 2f, Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D SwordTexture = ModContent.Request<Texture2D>("TerrafirmaRedux/Items/Weapons/Melee/Swords/HeroSword").Value;
            Texture2D BlurTexture = ModContent.Request<Texture2D>("TerrafirmaRedux/Projectiles/Melee/HeroSwordBlur").Value;

            Main.EntitySpriteDraw(SwordTexture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 56, 56), new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 0) * 0.2f, Projectile.rotation, new Vector2(28), Projectile.scale * 2f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(SwordTexture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 56, 56), new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 0), Projectile.rotation, new Vector2(28), Projectile.scale * 1.2f, SpriteEffects.None, 0);
            for (int i = 0; i < 10; i++)
            {
                Main.EntitySpriteDraw(SwordTexture, Projectile.oldPos[i] + Projectile.Size/2 - Main.screenPosition, new Rectangle(0, 0, 56, 56), new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 0) * (1f - (i / 10f)), Projectile.oldRot[i], new Vector2(28), Projectile.scale * 1.2f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(SwordTexture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 56, 56), Color.White, Projectile.rotation, new Vector2(28), Projectile.scale * 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(SwordTexture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 56, 56), new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 0) * 0.5f, Projectile.rotation, new Vector2(28), Projectile.scale * 0.8f, SpriteEffects.None, 0);


            return false;
        }

    }
}
