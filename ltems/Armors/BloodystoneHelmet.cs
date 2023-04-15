﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.Items.Placeable;


namespace PenumbraMod.Content.Items.Armors
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class BloodystoneHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Increases damage by 7%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			// ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawBackHair[Item.headSlot] = true;
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true; 
		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Orange; // The rarity of the item
			Item.defense = 21; // The amount of defense the item will give when equipped
			
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<BloodystoneBreastplate>() && legs.type == ModContent.ItemType<BloodystoneLeggings>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Greately increased Life Regeneration and 15% Melee damage"; // This is the setbonus tooltip
			player.lifeRegen += 20;
			player.GetDamage(DamageClass.Melee) += 0.15f;
			player.GetDamage(DamageClass.Generic) += 0.07f;

        }

		
		public override void AddRecipes() 
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 18);
            recipe.AddIngredient(ItemID.CrimsonHelmet, 1);
            recipe.AddIngredient(ItemID.Bone, 7);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
	}
}
