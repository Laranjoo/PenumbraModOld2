﻿using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static PenumbraMod.Content.Items.Wakizashi;

namespace PenumbraMod.Content.Items
{
    public class CompositeSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Composite Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("A very old blacksmith forged this legendary blade..." +
                "\nThis sword can break apart and reach long distances, causing mass destruction.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Call this method to quickly set some of the properties below.
            //Item.DefaultToWhip(ModContent.ProjectileType<ExampleWhipProjectileAdvanced>(), 20, 2, 4);

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 350;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Purple;

            Item.shoot = ModContent.ProjectileType<CompositeSwordProj>();
            Item.shootSpeed = 4;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public int TwistedStyle = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.noUseGraphic = true;
            if (TwistedStyle == 0)
            {
                Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/CompositeSwordSwing1")
                {
                    Volume = 1.6f,
                    PitchVariance = 0.2f,
                    MaxInstances = 2,
                };
                TwistedStyle = 1;
            }
            else if (TwistedStyle == 1)
            {
                Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/CompositeSwordSwing2")
                {
                    Volume = 1.6f,
                    PitchVariance = 0.2f,
                    MaxInstances = 2,
                };
                TwistedStyle = 0;
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CompositeSwordHold>()] < 1)
            {//Equip animation.
                int index = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<CompositeSwordHold>(), 0, 0, player.whoAmI, 0f);
            }
        }
    }
    public class CompositeSwordHold : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Composite Sword");
        }
        public override void SetDefaults()
        {
            AIType = 0;
            Projectile.width = 71;
            Projectile.height = 71;
            Projectile.penetrate = -1;
            Projectile.light = 0.3f;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.rotation = 90;
        }
        bool firstSpawn = true;
        double deg;

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 2;
            float pos = 4.5f;
            if (firstSpawn)
            {
                firstSpawn = false;
            }
            Projectile.timeLeft = 2;

            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.ai[1] = 280;
                Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            }
            else
            {
                Projectile.ai[1] = 180;
                Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2 * pos;
            }

            Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(86f) + projOwner.velocity.X / -60;
            }
            else
            {

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(366f) + projOwner.velocity.X / -60;
            }

            Projectile.ai[0]++;

            if (projOwner.dead && !projOwner.active)
            {//Disappear when player dies
                Projectile.timeLeft = 0;
                Projectile.Kill();
                Projectile.alpha = 255;
            }

            if (projOwner.ownedProjectileCounts[ModContent.ProjectileType<CompositeSwordProj>()] >= 1)
            {
                Projectile.alpha = 255;
            }
            else
            {
                //Arms will hold the weapon.
                projOwner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (projOwner.Center -
                    new Vector2(Projectile.Center.X + (projOwner.velocity.X * 0.06f), Projectile.Center.Y + (projOwner.velocity.Y * 0.06f))
                    ).ToRotation() + MathHelper.PiOver2);
                Projectile.alpha = 0;
            }

            if (projOwner.HeldItem?.type != ModContent.ItemType<CompositeSword>())
            {
                Projectile.Kill();
            }
            //Orient projectile
            Projectile.direction = projOwner.direction;
            Projectile.spriteDirection = projOwner.direction;
            if (Projectile.ai[0] == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/CompositeSwordHold") { PitchVariance = 0.1f, MaxInstances = 1 }, Projectile.Center);
            }

        }

        public override void Kill(int timeLeft)
        {


        }
    }

}

