using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using PenumbraMod.Content.Buffs;
using static Humanizer.In;
using System.Diagnostics.Metrics;

namespace PenumbraMod.Content.Items.MeltedBlaster
{
    public class MeltedBlaster : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Melted Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("This gun has 2 modes:" +
                "\n[c/ffde38:First Mode:] Hold the gun to charge it up to 5 times" +
                "\nIt shoots Melted Tridents that inflicts 'On Fire!'" +
                "\n1th to 3rd charge scales the trident damage, 4th charge makes the trident pass through tiles" +
                "\nThe 5th charge shoots a high velocity trident that explodes on enemies hit, inflicting the Melted debuff" +
                "\nBut be careful in holding the 5th charge too much, the gun will start to get [c/740038:Overcharged]" +
                "\nWhen [c/740038:Overcharged], the gun explodes, inflicting damage on the player plus the melted debuff" +
                "\nYou can't use the gun when melting" +
                "\n[c/ff6a00:Second Mode:] The gun will shoot fire, inflicting 'On Fire!'" +
                "\nThe fire velocity depends on cursor position"
                + "\n[c/793a80:'Grand things requer grand responsability']");

        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 78;
            Item.height = 40;
            Item.useTime = 9999;
            Item.useAnimation = 9999;
            Item.useStyle = 5;
            Item.knockBack = 3;
            Item.value = 11200;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 25;
            Item.channel = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.itemAnimation = 0;
                player.itemTime = 0;
                Item.shoot = ModContent.ProjectileType<MeltedBlasterProj2>();
                Item.autoReuse = true;
            }
            else
            {
                player.itemAnimation = 0;
                player.itemTime = 0;
                Item.autoReuse = false;
                Item.shoot = ModContent.ProjectileType<MeltedBlasterProj>();
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<MeltedBlasterProj>()] < 1;
        }
        public int TwistedStyle = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                if (TwistedStyle == 0)
                {

                    float launchSpeed = 120f;
                    float launchSpeed2 = 0f;
                    float launchSpeed3 = 0f;

                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 direction = Vector2.Normalize(mousePosition - player.Center);
                    Vector2 Gun = direction * launchSpeed2;
                    Vector2 Disk = direction * launchSpeed3;
                    Vector2 muzzleOffset = Vector2.Normalize(new Vector2(-10, -25)) * 0f;
                    position = new Vector2(position.X, position.Y);
                    if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                    {
                        position += muzzleOffset;
                    }

                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, Gun.X, Gun.Y, ModContent.ProjectileType<MeltedBlasterProj>(), 0, knockback, player.whoAmI);


                }

                if (TwistedStyle == 1)
                {

                    float launchSpeed = 120f;
                    float launchSpeed2 = 0f;
                    float launchSpeed3 = 0f;

                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 direction = Vector2.Normalize(mousePosition - player.Center);
                    Vector2 Gun = direction * launchSpeed2;
                    Vector2 Disk = direction * launchSpeed3;
                    Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 0f;
                    position = new Vector2(position.X, position.Y);
                    if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                    {
                        position += muzzleOffset;
                    }

                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, Gun.X, Gun.Y, ModContent.ProjectileType<MeltedBlasterProj2>(), 0, knockback, player.whoAmI);


                }

            }

            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.myPlayer == player.whoAmI && player.altFunctionUse == 2 && TwistedStyle == 0)
            {
                TwistedStyle++;
                if (TwistedStyle > 0)
                {
                    TwistedStyle = 1;
                }
                //Item.shoot = TwistedStyle + 120;
                Vector2 velocity = new Vector2(0, 0);
                int basic = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, velocity, ModContent.ProjectileType<GunMode>(), 0, 0, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item149);
                player.itemAnimation = 0;
                player.itemTime = 0;
            }
            else
            {
                if (TwistedStyle == 1 && player.altFunctionUse == 2)
                {

                    TwistedStyle = 0;
                    Vector2 velocity = new Vector2(0, 0);
                    int basic = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, velocity, ModContent.ProjectileType<Charging>(), 0, 0, player.whoAmI);
                    SoundEngine.PlaySound(SoundID.Item149);
                    player.itemAnimation = 0;
                    player.itemTime = 0;
                }

            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 17);
            recipe.AddIngredient(ItemID.Obsidian, 25);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }


    public class MeltedBlasterProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Melted Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 999999;
            Projectile.ownerHitCheck = true;
        }
        public int counter;
        public bool boom = false;
        public bool bep = false;
        public bool bep2 = false;
        public bool text = false;
        public bool text1 = false;
        public float movement
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            counter++;
            Vector2 ownedMountedCenter = owner.RotatedRelativePoint(owner.MountedCenter, true);
            Projectile.direction = owner.direction;
            owner.heldProj = Projectile.whoAmI;
            Projectile.position.X = ownedMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownedMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!owner.frozen)
            {
                if (movement == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movement = 0.4f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (owner.itemAnimation < owner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    //movementFactor -= 2.4f;
                }
                else // Otherwise, increase the movement factor
                {
                    //movementFactor += 2.4f;
                }
            }
            if (boom == true)
            {
                for (int k = 0; k < 25; k++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                    Main.dust[dust].velocity *= 6.0f;
                }

                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bop"));

            }
            if (bep == true)
            {

                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bop"));

            }
            if (bep2 == true)
            {

                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bopboom"));

            }
            if (text == true)
            {
                Vector2 velocity9 = (Main.MouseWorld - Projectile.Center) / 99f;
                int basic23456 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity9, ModContent.ProjectileType<OVERCHARGING>(), 0, 0, owner.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bop"));
            }


            Projectile.position += Projectile.velocity * movement;
            // When we reach the end of the animation, we can kill the spear projectile

            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)

            Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();

            if (Main.MouseWorld.X == -1)
            {//Adjust when facing the other direction

                Projectile.rotation = Vector2.Normalize(owner.Center - Main.MouseWorld).ToRotation() + MathHelper.ToRadians(0f);
            }
            else
            {

                Projectile.rotation = Vector2.Normalize(owner.Center - Main.MouseWorld).ToRotation() + MathHelper.ToRadians(180f);
            }
            owner.ChangeDir(Main.MouseWorld.X > owner.Center.X ? 1 : -1);

            if (counter > 30 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 8f, ModContent.ProjectileType<MeltedTrident>(), 40, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 30 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 31 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 70 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 10f, ModContent.ProjectileType<MeltedTrident2>(), 50, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 70 && owner.channel || owner.CCed)
            {
                boom = true;
                Projectile.frame = 1;
            }
            if (counter > 71 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 110 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 14f, ModContent.ProjectileType<MeltedTrident3>(), 60, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 110 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 111 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 150 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 18f, ModContent.ProjectileType<MeltedTrident4>(), 80, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 150 && owner.channel || owner.CCed)
            {
                boom = true;
                Projectile.frame = 2;
            }
            if (counter > 151 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 190 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 22f, ModContent.ProjectileType<MeltedTrident5>(), 100, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 190 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 191 && owner.channel || owner.CCed)
            {
                boom = false;
            }

            if (counter > 230 && owner.channel || owner.CCed)
            {

                bep = true;

            }
            if (counter > 231 && owner.channel || owner.CCed)
            {

                bep = false;

            }
            if (counter > 240 && owner.channel || owner.CCed)
            {
                bep2 = true;
            }
            if (counter > 241 && owner.channel || owner.CCed)
            {
                bep2 = false;
            }
            if (counter > 250 && owner.channel || owner.CCed)
            {
                Projectile.frame = 3;
                text = true;
            }
            if (counter > 251 && owner.channel || owner.CCed)
            {
                text = false;
            }
            if (counter > 255 && owner.channel || owner.CCed)
            {
                bep = true;
            }
            if (counter > 256 && owner.channel || owner.CCed)
            {
                bep = false;
            }
            if (counter > 260 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 261 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 265 && owner.channel || owner.CCed)
            {
                bep = true;

            }
            if (counter > 266 && owner.channel || owner.CCed)
            {
                bep = false;
            }
            if (counter > 280)
            {
                Vector2 velocity3 = (Main.MouseWorld - Projectile.Center) / 8f;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity3, ModContent.ProjectileType<MeltedTridentHostile>(), 80, 0, owner.whoAmI);
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;
                SoundEngine.PlaySound(SoundID.Item74);
                for (int k = 0; k < 50; k++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.2f);
                    Main.dust[dust].velocity *= 7.0f;
                }
                owner.AddBuff(ModContent.BuffType<Melting>(), 360);
            }
            if (!owner.channel || owner.CCed)
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;

                return;
            }
            if (owner.HasBuff(ModContent.BuffType<Melting>()))
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;
            }

        }
    }
    public class MeltedBlasterProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Melted Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 999999;
            Projectile.ownerHitCheck = true;
        }
        public int counter;

        public float movement
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Projectile.frame = 0;
            Player owner = Main.player[Projectile.owner];
            counter++;
            Vector2 ownedMountedCenter = owner.RotatedRelativePoint(owner.MountedCenter, true);
            Projectile.direction = owner.direction;
            Projectile.timeLeft = 10;
            owner.heldProj = Projectile.whoAmI;
            Projectile.position.X = ownedMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownedMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!owner.frozen)
            {
                if (movement == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movement = 0.4f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (owner.itemAnimation < owner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    //movementFactor -= 2.4f;
                }
                else // Otherwise, increase the movement factor
                {
                    //movementFactor += 2.4f;
                }
            }


            Projectile.position += Projectile.velocity * movement;
            // When we reach the end of the animation, we can kill the spear projectile

            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)

            Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();

            if (Main.MouseWorld.X == -1)
            {//Adjust when facing the other direction

                Projectile.rotation = Vector2.Normalize(owner.Center - Main.MouseWorld).ToRotation() + MathHelper.ToRadians(0f);
            }
            else
            {

                Projectile.rotation = Vector2.Normalize(owner.Center - Main.MouseWorld).ToRotation() + MathHelper.ToRadians(180f);
            }
            owner.ChangeDir(Main.MouseWorld.X > owner.Center.X ? 1 : -1);

            if (counter > 10 && owner.channel || owner.CCed)
            {
                Vector2 velocity = (Main.MouseWorld - Projectile.Center) / 9f;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity, ModContent.ProjectileType<MeltedFire>(), 40, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item109);
                counter = 5;
            }


            if (!owner.channel || owner.CCed)
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;

                return;
            }
            if (owner.HasBuff(ModContent.BuffType<Melting>()))
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;
            }

        }
    }



}
