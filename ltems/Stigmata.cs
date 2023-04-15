using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PenumbraMod.Content.Items
{
	public class Stigmata : ModItem
	{
        public const int MaxExampleLifeFruits = 10;
        public const int LifePerFruit = 10;
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Stigmata");
			Tooltip.SetDefault("You shouldn't have done this..." +
				"\nOnly fools use this" +
				"\nPermanently increases damage by 10%" +
				"\nBut it can be only used in a special way, and once...");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
            
        }

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.maxStack = 1;
			Item.value = 0;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item109;
		}
        public override bool CanUseItem(Player player)
        {
            if (Main.bloodMoon)
                // Any mod that changes statLifeMax to be greater than 500 is broken and needs to fix their code.
                // This check also prevents this item from being used before vanilla health upgrades are maxed out.
                return player.GetModPlayer<ExampleLifeFruitPlayer>().exampleLifeFruits < LifePerFruit;

            return Main.bloodMoon;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.bloodMoon)
            {
               player.GetDamage(DamageClass.Generic) += 0.10f;
               // This is very important. This is what makes it permanent.
               player.GetModPlayer<ExampleLifeFruitPlayer>().exampleLifeFruits++;
            }
            // Do not do this: player.statLifeMax += 2;
           
            // This handles the 2 achievements related to using any life increasing item or getting to exactly 500 hp and 200 mp.
            // Ignored since our item is only useable after this achievement is reached
            // AchievementsHelper.HandleSpecialEvent(player, 2);
            //TODO re-add this when ModAchievement is merged?
            return true;
        }
        public override void AddRecipes()
        {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BrokenWoodyThing>(), 1);
			recipe.AddIngredient(ItemID.Wood, 15);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
        }
        public class ExampleLifeFruitPlayer : ModPlayer
        {
            public int exampleLifeFruits;

            public override void ResetEffects()
            {
                // Increasing health in the ResetEffects hook in particular is important so it shows up properly in the player select menu
                // and so that life regeneration properly scales with the bonus health
                Player.GetDamage(DamageClass.Generic) += 0.10f;
            }

            public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)PenumbraMod.MessageType.ExamplePlayerSyncPlayer);
                packet.Write((byte)Player.whoAmI);
                packet.Write(exampleLifeFruits);
                packet.Send(toWho, fromWho);
            }

            // NOTE: The tag instance provided here is always empty by default.
            // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
            public override void SaveData(TagCompound tag)
            {
                tag["DamagePlayer"] = exampleLifeFruits;
            }

            public override void LoadData(TagCompound tag)
            {
                exampleLifeFruits = (int)tag["DamagePlayer"];
            }
        }

    }
}