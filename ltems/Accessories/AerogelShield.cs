using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Placeable;
using System;
using PenumbraMod.Content.NPCs;

namespace PenumbraMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]

    public class AerogelShield : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This slimy shield protects you from the slimes and keep them friendly" +
                "\nGrants immunity to slimed debuff" +
                "\nSpawns Slime Spikes when moving" +
                "\n''You feel like the king!''");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {

            Item.canBePlacedInVanityRegardlessOfConditions = true;
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.defense = 2;
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Slimed] = true;
            player.npcTypeNoAggro[NPCID.BlueSlime] = true;
            player.npcTypeNoAggro[NPCID.SlimeSpiked] = true;
            player.npcTypeNoAggro[NPCID.GoldenSlime] = true;
            player.npcTypeNoAggro[NPCID.IceSlime] = true;
            player.npcTypeNoAggro[NPCID.SlimeRibbonYellow] = true;
            player.npcTypeNoAggro[NPCID.SlimeRibbonWhite] = true;
            player.npcTypeNoAggro[NPCID.SlimeRibbonRed] = true;
            player.npcTypeNoAggro[NPCID.SlimeRibbonGreen] = true;
            player.npcTypeNoAggro[NPCID.Slimer] = true;
            player.npcTypeNoAggro[NPCID.SlimeMasked] = true;
            player.npcTypeNoAggro[NPCID.RainbowSlime] = true;
            player.npcTypeNoAggro[NPCID.CorruptSlime] = true;
            player.npcTypeNoAggro[NPCID.LavaSlime] = true;
            player.npcTypeNoAggro[NPCID.UmbrellaSlime] = true;
            player.npcTypeNoAggro[NPCID.Crimslime] = true;
            player.npcTypeNoAggro[NPCID.SpikedJungleSlime] = true;
            player.npcTypeNoAggro[ModContent.NPCType<MarshmellowSlime>()] = true;
            if (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f && !player.rocketFrame)
            {

                if (Main.rand.NextBool(16))
                {

                    Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(player.position.X + Main.rand.NextFloat(player.width), player.position.Y + Main.rand.NextFloat(player.height)), new Vector2(0f, 0f), ModContent.ProjectileType<SlimeSpike>(), 12, 0, Main.myPlayer);
                }

            }
        }


        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AerogelBar>(20)
                .AddIngredient(ItemID.RoyalGel, 1)
                .AddIngredient(ItemID.Gel, 35)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SlimeSpike : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Spike"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

        }

        public override void SetDefaults()
        {
            Projectile.damage = 9;
            Projectile.width = 18;
            Projectile.height = 46;
            Projectile.aiStyle = 68;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.scale = 0.7f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slimed, 300);
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 0.93f);

            int dust = Dust.NewDust(Projectile.Center, 2, 1, DustID.SlimeBunny, 0f, 0f, 0, Color.Blue, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1.0f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 150) * 0.005f;

        }

    }


}

   
