using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Buffs;
using Microsoft.Xna.Framework;

namespace PenumbraMod.Content.Items
{
	public class Buriti : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Buriti"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Buriti" +
				"\nBuriti" +
				"\nBuriti" +
				"\nBuriti" +
				"\nBuriti" +
				"\nIts Buriti");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 450;
			Item.DamageType = DamageClass.Melee;
			Item.width = 228;
			Item.height = 227;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 1000;
			Item.rare = 11;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}
      
	}
}