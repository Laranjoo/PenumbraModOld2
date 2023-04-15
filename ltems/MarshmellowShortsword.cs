using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowShortsword : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Marshmellow Shortsword");
			Tooltip.SetDefault("[c/ffe5d9:'It pulses with it power...']" +
                "\n[c/ffe5d9:'The power of being fluffy!']");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

		public override void SetDefaults() {
			Item.damage = 7;
			Item.knockBack = 4f;
			Item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.width = 19;
			Item.height = 19;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.autoReuse = true;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 0, 10);

			Item.shoot = ModContent.ProjectileType<MarshmellowShortswordProjectile>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 2.1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Wood, 5)
				.AddIngredient<Marshmellow>(6)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
