using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using PenumbraMod.Common.Base;
using ReLogic.Content;
using System.Security.Cryptography;
using PenumbraMod.Content.Items;
using Terraria.ID;

namespace PenumbraMod
{

    public partial class PenumbraMod : Mod
    {
        public static PenumbraMod Instance { get; private set; }
        internal UserInterface _ReaperbarUserInterface;
        internal ReaperUI ReaperBarUI;
        public PenumbraMod()
        {
            Instance = this;
        }
        public override void Load()
        {
            if (!Main.dedServ)
            {
                ReaperBarUI = new ReaperUI();
                _ReaperbarUserInterface = new UserInterface();
                _ReaperbarUserInterface.SetState(ReaperBarUI);
                Main.QueueMainThreadAction(() =>
                {
                    Texture2D brightness6 = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness6", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness6);
                    Texture2D brightness2 = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness2", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness2);
                    Texture2D brightness2D = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness2Death", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness2D);
                    Texture2D brightnessP2 = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/BrightnessP2", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightnessP2);
                    Texture2D brightness7 = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness7", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness7);
                });
            }
        }
        public static void GetTexturePremultiplied(ref Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }
        public override void PostSetupContent()
        {
            Common.Systems.ModIntegrationsSystem.PerformModSupport();
        }
        /// <summary>
        /// Eye of the storm glomask effect
        /// </summary>
        public static Color Eyestorm => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.White, Color.Blue * 0.6f, Color.White);
        /// <summary>
        /// Effect used for some storm based projectiles
        /// </summary>
        public static Color Storm => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Blue, Color.LightBlue * 0.6f, Color.Blue);
        /// <summary>
        /// Used for the Phantom's Penumbratic darkmatter scythe
        /// </summary>
        public static Color Phantom => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Purple, Color.Purple * 0.6f, Color.Gray);
        /// <summary>
        /// Used for the composite sword line
        /// </summary>
        public static Color CompositeSword => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Gray, Color.White * 0.6f, Color.Gray);
    }
    public class PenumbraModSystem : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            GetInstance<PenumbraMod>()._ReaperbarUserInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "PenumbraMod2: Reaper Energy Bar",
                    delegate
                    {
                        GetInstance<PenumbraMod>()._ReaperbarUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        public override void PostWorldGen()
        {
            int[] itemsToPlaceInIceChests = { ItemType<FlyingGume>() };
            int itemsToPlaceInIceChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                // If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 4 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == 0)
                        {
                            if (!WorldGen.genRand.NextBool(5)) break;
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInIceChests[itemsToPlaceInIceChestsChoice]);
                            itemsToPlaceInIceChestsChoice = (itemsToPlaceInIceChestsChoice + 1) % itemsToPlaceInIceChests.Length;
                            break;
                        }
                    }
                }
            }
        }
    }
}