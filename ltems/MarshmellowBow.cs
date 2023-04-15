using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marshmellow Bow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("'This fluffy bow doesn't seems to hurt someone...'");
				
			
		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 46;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.shootSpeed = 6f;
			Item.noMelee = true;
			Item.crit = 6;
		}


        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 4);
			recipe.AddIngredient(ModContent.ItemType<Consumables.Marshmellow>(), 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
		}


	}
}