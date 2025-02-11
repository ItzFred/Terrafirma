﻿using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Audio;

namespace Terrafirma.Systems.MageClass
{
    [Autoload(Side = ModSide.Client)]
    public class SpellUISystem : ModSystem
    {
        internal SpellUI spellui;
        private UserInterface spellwheel;

        public Spell SelectedSpell;

        public override void Load()
        {
            spellui = new SpellUI();
            spellui.Activate();
            spellwheel = new UserInterface();
            spellwheel.SetState(spellui);
        }

        public void Hide()
        {
            spellwheel?.SetState(null);
        }

        public void Show()
        {
            spellwheel?.SetState(spellui);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (spellwheel?.CurrentState != null)
            {
                spellwheel?.Update(gameTime);
            }
        }

        public void Flush()
        {
            spellui?.Flush();
            spellui.UIOpen = false;
        }

        public void Create( int item)
        {
            spellui?.Create(item);
        }

        public void UpdateMana (float manacost)
        {
            spellui.manacost = manacost;
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Terrafirma: Spell Wheel",
                    delegate
                    {
                        if (spellwheel?.CurrentState != null)
                        {
                            spellwheel.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
