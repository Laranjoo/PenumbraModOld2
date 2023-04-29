using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;
using PenumbraMod.Content.Items;

namespace PenumbraMod.Content.DamageClasses
{
    public class ReaperClass : DamageClass
    {
        // This is an example damage class designed to demonstrate all the current functionality of the feature and explain how to create one of your own, should you need one.
        // For information about how to apply stat bonuses to specific damage classes, please instead refer to ExampleMod/Content/Items/Accessories/ExampleStatBonusAccessory.
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            // This method lets you make your damage class benefit from other classes' stat bonuses by default, as well as universal stat bonuses.
            // To briefly summarize the two nonstandard damage class names used by DamageClass:
            // Default is, you guessed it, the default damage class. It doesn't scale off of any class-specific stat bonuses or universal stat bonuses.
            // There are a number of items and projectiles that use this, such as thrown waters and the Bone Glove's bones.
            // Generic, on the other hand, scales off of all universal stat bonuses and nothing else; it's the base damage class upon which all others that aren't Default are built.
            if (damageClass == Generic)
                return StatInheritanceData.Full;

            return new StatInheritanceData(
                damageInheritance: 0f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0f,
                knockbackInheritance: 0f
            );

        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            // This method allows you to make your damage class benefit from and be able to activate other classes' effects (e.g. Spectre bolts, Magma Stone) based on what returns true.
            // Note that unlike our stat inheritance methods up above, you do not need to account for universal bonuses in this method.
            // For this example, we'll make our class able to activate melee- and magic-specifically effects.
            if (damageClass == Melee)
                return true;

            return false;
        }

        public override void SetDefaultStats(Player player)
        {
            // This method lets you set default statistical modifiers for your example damage class.
            // Here, we'll make our example damage class have more critical strike chance and armor penetration than normal.
            player.GetCritChance<ReaperClass>() += 1;
            player.GetArmorPenetration<ReaperClass>() += 2;
            // These sorts of modifiers also exist for damage (GetDamage), knockback (GetKnockback), and attack speed (GetAttackSpeed).
            // You'll see these used all around in referencce to vanilla classes and our example class here. Familiarize yourself with them.
        }
        // This property lets you decide whether or not your damage class can use standard critical strike calculations.
        // Note that setting it to false will also prevent the critical strike chance tooltip line from being shown.
        // This prevention will overrule anything set by ShowStatTooltipLine, so be careful!
        public override bool UseStandardCritCalcs => true;


    }
    public class ReaperClassDPlayer : ModPlayer
    {
        public int ReaperEnergy;
        public int ReaperEnergyMax;
        public float ReaperEnergyMult;
        public override void PreUpdate()
        {
            if (ReaperEnergy > ReaperEnergyMax)
            {
                ReaperEnergy = ReaperEnergyMax;

                SoundEngine.PlaySound(SoundID.Item113, Player.position);
            }
            if (ReaperEnergy > 0 && ReaperEnergy < (int)(ReaperEnergyMax * 0.96f))
                ReaperEnergy -= 1;

        }

        public override void ResetEffects()
        {
            ReaperEnergyMax = 10000;
            ReaperEnergyMult = 0f;
        }
        public override void UpdateDead()
        {
            ReaperEnergy = 0;
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (target.type != NPCID.TargetDummy)
            {
                if (ReaperEnergy < ReaperEnergyMax)
                {
                    if (item.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>()))
                    {
                        if (Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                            ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult + 1) * 8);
                    }

                }
            }

            if (ReaperEnergy > ReaperEnergyMax)
            {
                if (ReaperEnergy > 10000)
                {

                    if (Player.HasItem(ModContent.ItemType<AerogelScythe>()))
                    {

                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SlimeHomingProj>())

                            proj.ai[1] = 1;

                    }

                    if (Player.HasItem(ModContent.ItemType<RichMahoganyScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item71, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<RichMahoganyProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<BorealWoodScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item71, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SnowBall>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<JungleScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item17, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SporeProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<EbonwoodScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item17, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<EbonwoodSpike>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<SilverScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item17, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SilverScytheProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<GoldenScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item17, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<GoldScytheProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<WoodenScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item71, Player.position);
                    }
                    if (Player.HasItem(ModContent.ItemType<IronScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item17, Player.position);
                        Projectile proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<IronScytheProj>())

                            proj.ai[1] = 1;
                    }
                    if (Player.HasItem(ModContent.ItemType<LeadScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item37, Player.position);
                    }

                }

                SoundEngine.PlaySound(SoundID.Item113, Player.position);
                ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult - 500));
                ReaperEnergy = 0;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.type != NPCID.TargetDummy)
            {
                if (ReaperEnergy < ReaperEnergyMax)
                {
                    if (proj.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>()))
                    {
                        if (Main.LocalPlayer.HeldItem.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>()))
                            ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult + 1) * 5);
                    }
                }

            }

            if (ReaperEnergy > ReaperEnergyMax)
            {
                if (ReaperEnergy > 10000)
                {

                    if (Player.HasItem(ModContent.ItemType<AerogelScythe>()))
                    {

                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SlimeHomingProj>())

                            proj.ai[1] = 1;


                    }
                    if (Player.HasItem(ModContent.ItemType<PhantomsPenumbraticDarkmatterScythe>()))
                    {

                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScythe.PhantomsPenumbraticDarkmatterScytheSwing>())

                            proj.ai[1] = 1;
                        if (!proj.active || proj.type != ModContent.ProjectileType<PhantomsPenumbraticDarkmatterScythe.PhantomsPenumbraticDarkmatterScytheSwing2>())

                            proj.ai[1] = 1;
                        if (!proj.active || proj.type != ModContent.ProjectileType<PhantomDarkMatter>())

                            proj.ai[1] = 1;
                        if (ReaperEnergy < ReaperEnergyMax)
                        {
                            if (Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                                ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult + 1) * 8);
                        }
                        if (ReaperEnergy > ReaperEnergyMax)
                        {
                            ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult - 70));
                        }
                    }
                    if (Player.HasItem(ModContent.ItemType<Kusarigama>()))
                    {

                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<KusarigamaProj>())

                            proj.ai[1] = 1;
                        if (ReaperEnergy < ReaperEnergyMax)
                        {
                            if (Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                                ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult + 1) * 8);
                        }
                        if (ReaperEnergy > ReaperEnergyMax)
                        {
                            ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult - 70));
                        }
                    }
                    if (Player.HasItem(ModContent.ItemType<RichMahoganyScythe>()))
                    {

                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<RichMahoganyProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<JungleScythe>()))
                    {

                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SporeProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<BorealWoodScythe>()))
                    {

                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SnowBall>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<EbonwoodScythe>()))
                    {
                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<EbonwoodSpike>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<WoodenScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item71, Player.position);
                    }
                    if (Player.HasItem(ModContent.ItemType<IronScythe>()))
                    {
                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<IronScytheProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<GoldenScythe>()))
                    {
                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<GoldScytheProj>())

                            proj.ai[1] = 1;

                    }
                    if (Player.HasItem(ModContent.ItemType<LeadScythe>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item37, Player.position);
                    }
                    if (Player.HasItem(ModContent.ItemType<SilverScythe>()))
                    {
                        proj = Main.projectile[1];
                        if (!proj.active || proj.type != ModContent.ProjectileType<SilverScytheProj>())

                            proj.ai[1] = 1;

                    }
                }
                ReaperEnergy = 0;
                SoundEngine.PlaySound(SoundID.Item113, Player.position);

            }
        }

    }
}