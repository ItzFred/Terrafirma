﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terrafirma.Systems.MageClass;
using Terrafirma.Items.Weapons.Summoner.Wrench;
using Terrafirma.Common.Items;
using Terrafirma.Items.Consumable;
using Terraria.GameContent.UI;
using Terrafirma.Systems.NewNPCQuests;
using Terrafirma.Common.Interfaces;
using Terrafirma.Items.Weapons.Melee.Knight;
using Terrafirma.Projectiles.Melee.Knight;
using Microsoft.Xna.Framework.Input;
using Terrafirma.Systems.Trees;
using Terrafirma.Items.Equipment;
using Terraria.DataStructures;
using Terrafirma.Common.Templates.Melee;
using System.Collections.Generic;


namespace Terrafirma.Common.Players
{
    public class TerrafirmaModPlayer : ModPlayer
    {
        //Accessories

        public bool PristineEmblem = false;
        public bool SapphireWard = false;
        public bool Foregrip = false;
        public bool DrumMag = false;
        public bool AmmoCan = false;
        public bool CanUseAmmo = true;
        public bool BoxOfHighPiercingRounds = false;

        public bool SpringBoots = true;

        //Movement Variables

        public Vector2 Momentum = Vector2.Zero;
        public int FloorTime = 0;
        public float JumpMultiplier = 1f;

        //UI Variables

        public static bool SpellUI = false;
        internal Item HeldMagicItem = new Item(0);

        public static bool SpellSideMenu = false;

        //

        public Quest[] playerquests = new Quest[] { };

        internal bool RightMouseSwitch = false;

        public override void ResetEffects()
        {
            Player.pickSpeed *= 0.8f;
            Player.autoJump = true;
            //Player.runAcceleration *= 2;

            PristineEmblem = false;
            SapphireWard = false;
            Foregrip = false;
            DrumMag = false;
            AmmoCan = false;
            CanUseAmmo = true;
            BoxOfHighPiercingRounds = false;

            SpringBoots = false;
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if(!mediumCoreDeath)
            {
                Player.ConsumedManaCrystals = 2;
            }
            return base.AddStartingItems(mediumCoreDeath);
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (!CanUseAmmo)
            {
                return false;
            }

            if (ammo.ammo == AmmoID.Bullet && Main.rand.NextBool(2, 3) && DrumMag)
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo);

        }

        public override void PostUpdateRunSpeeds()
        {
            if (SpringBoots)
            {
                Player.runSlowdown = 0.2f;

                if (Player.velocity.Y != 0) FloorTime = 0;
                else FloorTime++;

                if (FloorTime > 10) JumpMultiplier = 1f;

            }
            if (Player.ItemAnimationActive && Player.HeldItem.type != 0)
            {
                if (Player.HeldItem.Spell() != null && Player.HeldItem.Spell().GetSpellName() == "Mana Bloom")
                {
                    Player.accRunSpeed = 2f;
                }
            }
        }

        public override void PreUpdateMovement()
        {

            if (SpringBoots)
            {
                Player.frogLegJumpBoost = true;

                if (Player.justJumped)
                {

                    if (JumpMultiplier > 1)
                    {
                        SoundStyle boing = new SoundStyle("Terrafirma/Sounds/Boing", SoundType.Sound);
                        boing.Volume = 0.8f;
                        boing.PitchRange = (-0.1f, 0.1f);
                        boing.Pitch -= JumpMultiplier / 10;

                        SoundEngine.PlaySound(boing, Player.position);
                    }

                    JumpMultiplier = MathHelper.Clamp(JumpMultiplier * 1.25f, 1f, 3f);
                }
            }
        }
        public override void OnMissingMana(Item item, int neededMana)
        {
            Player.manaFlower = false;
        }
        public override void PostUpdate()
        {
            if (playerquests.Length == 0) playerquests = QuestID.quests;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //Check if Mouse
            Tile tile = Main.tile[(Main.MouseWorld / 16).ToPoint()];
            bool TileInteract = TileID.Sets.InteractibleByNPCs[tile.TileType] || TileID.Sets.HasOutlines[tile.TileType];

            if (HeldMagicItem != Player.HeldItem && triggersSet.MouseRight && Player.inventory[Player.selectedItem].damage != -1 && !Main.HoveringOverAnNPC && Main.SmartInteractTileCoordsSelected.Count == 0 && !TileInteract)
            {
                if (SpellID.itemcatalogue.ContainsKey(Player.inventory[Player.selectedItem].type))
                {
                    ModContent.GetInstance<SpellUISystem>().Create(Player.HeldItem.type);
                    HeldMagicItem = Player.HeldItem;
                }
                else ModContent.GetInstance<SpellUISystem>().Flush();
            }

            if (triggersSet.MouseRight && !Player.ItemAnimationActive)
            {
                HeldMagicItem = Player.HeldItem;
                if (!SpellUI && SpellID.itemcatalogue.ContainsKey(Player.inventory[Player.selectedItem].type) && Player.inventory[Player.selectedItem].damage != -1 && HeldMagicItem == Player.HeldItem && !Main.HoveringOverAnNPC && Main.SmartInteractTileCoordsSelected.Count == 0 && !TileInteract)
                {
                    ModContent.GetInstance<SpellUISystem>().Create(Player.HeldItem.type);
                    SpellUI = true;
                }
            }
            else
            {        
                if (SpellUI)
                {
                    ModContent.GetInstance<SpellUISystem>().Flush();
                    if (ModContent.GetInstance<SpellUISystem>().SelectedSpell != null && 
                        SpellUI == true &&
                        Player.HeldItem.type > 0 &&
                        SpellID.itemcatalogue.ContainsKey(Player.HeldItem.type))
                    {
                        SpellUI = false;
                        Player.HeldItem.GetGlobalItem<GlobalItemInstanced>().Spell =
                        ModContent.GetInstance<SpellUISystem>().SelectedSpell;
                        ModifyMagicSpellStats.ApplyDefaults(Player.HeldItem);
                    }
                }
            }

            //Item Right Click
            if (triggersSet.MouseRight)
            {
                if (Player.HeldItem.type == ModContent.ItemType<BookmarkerWrench>() &&
                    Player == Main.LocalPlayer)
                {
                    TFUtils.UpdateSentryPriority(null, Player);
                }
            }

            //Spell Side menu
            if (!SpellSideMenu && SpellID.itemcatalogue.ContainsKey(Player.HeldItem.type))
            {
                ModContent.GetInstance<SpellSideMenuUISystem>().Create(Player.HeldItem);
                SpellSideMenu = true;
            }
            if (!SpellID.itemcatalogue.ContainsKey(Player.HeldItem.type) || ModContent.GetInstance<SpellSideMenuUISystem>().spellitem.type != Player.HeldItem.type || Player.HeldItem.type == 0)
            {
                ModContent.GetInstance<SpellSideMenuUISystem>().Flush();
                SpellSideMenu = false;
            }

            //if (Main.keyState.IsKeyDown(Keys.P))
            //{
            //    Tile tree = Main.tile[(short)(Main.MouseWorld.X / 16), (short)(Main.MouseWorld.Y / 16)];
            //    tree.HasTile = true;
            //    tree.TileType = (ushort)ModContent.TileType<ModTree_Terrafirma>();
            //}
            if (Keybinds.tertiaryAttack.JustPressed)
            {
                if (Player.HeldItem.ModItem is IHasTertriaryFunction t && !Player.ItemAnimationActive && t.canUseTertriary(Player))
                {
                    Item i = Player.HeldItem;
                    Player.ApplyItemAnimation(Player.HeldItem);
                    Player.ApplyItemTime(Player.HeldItem);
                    SoundEngine.PlaySound(i.UseSound, Player.Center);
                    t.TertriaryShoot(Player,Player.GetSource_ItemUse_WithPotentialAmmo(i,Player.ChooseAmmo(i).type) as EntitySource_ItemUse_WithAmmo,Player.Center,Player.Center.DirectionTo(Main.MouseWorld) * i.shootSpeed, i.shoot,Player.GetWeaponDamage(i), Player.GetWeaponKnockback(i));
                }
            }
        }
        public override void OnEnterWorld()
        {
            SpellSideMenu = false;
            base.OnEnterWorld();
        }

        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            if (Main.mouseRight && !RightMouseSwitch)
            {
                if(Main.mouseItem.ModItem is IUseOnItemInInventoryItem item)
                {
                    if (item.canBeUsedOnThisItem(Player,Main.mouseItem, inventory[slot],context))
                        item.useOnItem(Player,Main.mouseItem,inventory[slot],context);
                }
                RightMouseSwitch = true;
            }
            if (!Main.mouseRight) RightMouseSwitch = false;
            return base.HoverSlot(inventory, context, slot);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if(PristineEmblem && target.life >= target.lifeMax * 0.66f)
            {
                modifiers.SourceDamage *= 1.1f;
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if(proj.reflected && proj.owner == Player.whoAmI && SapphireWard)
            {
                modifiers.ScalingArmorPenetration += -0.5f;
            }
        }
    }
}
