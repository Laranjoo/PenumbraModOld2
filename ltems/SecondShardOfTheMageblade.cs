﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class SecondShardOfTheMageblade : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Second Shard Of The Mageblade");
			Tooltip.SetDefault("The Second piece of an ancient powerful blade...");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			
          
            
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = 2250;
			Item.rare = ItemRarityID.Cyan;	
		}



	}
}