﻿using Terraria.ModLoader;
using Terrafirma.Common.Templates.Melee;
using Microsoft.Xna.Framework;

namespace Terrafirma.Projectiles.Melee.Paladin
{
    public class LeadThrowingHammer : PaladinHammer
    {
        public override int ThrownProjectile => ModContent.ProjectileType<LeadThrowingHammerThrown>();
    }
    public class LeadThrowingHammerThrown : PaladinHammerThrown
    {
        public override int FlightDuration => 40;
        public override string Texture => "Terrafirma/Projectiles/Melee/Paladin/LeadThrowingHammer";
    }
}
