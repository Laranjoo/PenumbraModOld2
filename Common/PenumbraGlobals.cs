﻿using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Common
{
    // This file contains ALL global changes from vanilla
    public class PenumbraGlobals
    {
        
    }
    public class PenumbraGlobalNPC : GlobalNPC
    {
        public static int eyeStorm = -1;
    }
    public class PenumbraGlobalTooltips : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().VanillaChanges;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            // Here we add a tooltip to the gel to let the player know what will happen
            if (item.type == ItemID.DeathSickle)
            {
                tooltips.Add(new(Mod, "", "[c/3b12d3:Special Ability:] Increases the swing speed of the scythe, shots faster extra projectiles, at cost of reaper energy not increases"));
            }
            if (item.type == ItemID.IceSickle)
            {
                tooltips.Add(new(Mod, "", "[c/00a4ff:Special Ability:] Increases the swing speed of the scythe, and shoots additional ice sickles"));
            }
            if (item.type == ItemID.BeamSword)
            {
                tooltips.Add(new(Mod, "", "Right Click to stand a shield in your front, at enemy hits, you will gain holy protection to dodge the next attack and gain 10+ defense"));
            }
        }
    }

    public class PenumbraGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().VanillaChanges;
        }
        public override bool InstancePerEntity => true;
        /// <summary>
        /// Useful to check if item hit NPC
        /// </summary>
        public bool ItemhitNPC;
        public override void SetDefaults(Item item)
        {
            ItemhitNPC = false;
            if (item.type == ItemID.DeathSickle)
            {
                item.DamageType = ModContent.GetInstance<ReaperClass>();
                item.useAnimation = 22;
                item.useTime = 22;
                item.shootSpeed = 11f;
            }
            if (item.type == ItemID.BeamSword)
            {
                item.useAnimation = 21;
                item.useTime = 21;
                item.shootSpeed = 14f;
                item.shoot = ModContent.ProjectileType<BeamSwordProj>();
                item.noUseGraphic = false;
                item.noMelee = false;
            }
            if (item.type == ItemID.IceSickle)
            {
                item.DamageType = ModContent.GetInstance<ReaperClass>();
                item.shoot = ModContent.ProjectileType<IceSickle>();
                item.shootSpeed = 14f;
            }
            if (item.type == ItemID.Gladius)
            {
                item.shoot = ModContent.ProjectileType<GladiusRework>();
                item.shootSpeed = 6f;
                item.useTime = 6;
                item.useAnimation = 14;
                item.reuseDelay = 10;
            }
        }
       
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.DeathSickle)
            {
                if (player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 9300)
                {
                    player.AddBuff(ModContent.BuffType<DeathSpeed>(), 360);

                }
            }
            return true;
        }
        public override void HoldItem(Item item, Player player)
        {
            if (player.HasBuff(BuffID.ShadowDodge))
            {
                if (player.HeldItem?.type == ItemID.BeamSword)
                {
                    player.statDefense += 10;
                }

            }
        }
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            ItemhitNPC = true;
            target.AddBuff(ModContent.BuffType<hitef>(), 10);
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            ItemhitNPC = true;
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.type == ItemID.BeamSword)
            {
                return true;
            }
            return false;
        }
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.HasBuff(ModContent.BuffType<DeathSpeed>()))
            {
                if (item.type == ItemID.DeathSickle)
                {
                    if (Main.rand.NextBool(3))
                    {

                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PurpleTorch);

                    }
                }
            }

            if (item.type == ItemID.IceSickle)
            {
                if (Main.rand.NextBool(8))
                {

                    Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.BlueTorch);

                }
            }

        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.IceSickle)
            {
                item.shoot = ModContent.ProjectileType<IceSickle>();
                if (player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 9000)
                {
                    item.shootSpeed = 18f;
                    const int NumProjectiles = 3;
                    for (int i = 0; i < NumProjectiles; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(100));
                        // Decrease velocity randomly for nicer visuals.
                        newVelocity *= 1f - Main.rand.NextFloat(0.1f);
                        // Create a projectile.
                        Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<IceSickle>(), damage, knockback, player.whoAmI);

                    }
                    return false;
                }
            }
            if (item.type == ItemID.Gladius)
            {
                item.shoot = ModContent.ProjectileType<GladiusRework>();
            }
            if (item.type == ItemID.BeamSword)
            {
                if (player.altFunctionUse == 2)
                {
                    const int NumProjectiles = 1;
                    for (int i = 0; i < NumProjectiles; i++)
                    {
                        Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 2f, ModContent.ProjectileType<BeamSwordShield>(), 100, knockback, player.whoAmI);

                    }

                    item.noUseGraphic = true;
                    item.noMelee = true;
                    item.useStyle = ItemUseStyleID.Shoot;
                }

                else
                {
                    Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 14f, ModContent.ProjectileType<BeamSwordProj>(), 70, knockback, player.whoAmI);
                    item.noUseGraphic = false;
                    item.noMelee = false;
                    item.useStyle = ItemUseStyleID.Swing;
                }
                return false;
            }
            if (player.HasBuff(ModContent.BuffType<DeathSpeed>()))
            {
                if (item.type == ItemID.DeathSickle)
                {
                    const int NumProjectiles = 1;
                    for (int i = 0; i < NumProjectiles; i++)
                    {
                        Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 16f, ModContent.ProjectileType<SwordBeam>(), 100, knockback, player.whoAmI);
                        Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 12f, ProjectileID.DeathSickle, damage, knockback, player.whoAmI);

                    }
                    return false;

                }
            }
            return true;

        }
    }
    public class HitEffect : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().HitEffect;
        }
        /// <summary>
        /// Useful to check if item hit NPC
        /// </summary>
        public bool ItemhitNPC;
        public override void SetDefaults(Item item)
        {
            ItemhitNPC = false;
        }
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            OnHit2(target, player);
            ItemhitNPC = true;
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            ItemhitNPC = true;
        }
        /// <summary>
        /// Made for the projectiles actually spawn when hitting npc (For the hit effect)
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        public void OnHit2(NPC npc, Player player)
        {
            if (ItemhitNPC)
            {
                Vector2 velocity = ModContent.GetInstance<PenumbraConfig>().HitEffectVelocity;
                const int NumProjectiles = 3;
                for (int i = 0; i < NumProjectiles; i++)
                {

                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));
                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 4f - Main.rand.NextFloat(0.1f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, newVelocity, ModContent.ProjectileType<HitEffectProj>(), 0, 0, player.whoAmI);

                }
            }
        }
    }
    public class UseTurn : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().UseTurn;
        }
        
        public override void SetDefaults(Item item)
        {
            if (item.type != ModContent.ItemType<ShockWave>() && item.type != ItemID.Gladius && item.type != ItemID.CopperShortsword && item.type != ItemID.GoldShortsword && item.type != ItemID.IronShortsword && item.type != ItemID.LeadShortsword
                && item.type != ItemID.PlatinumShortsword && item.type != ItemID.SilverShortsword && item.type != ItemID.TinShortsword && item.type != ItemID.TungstenShortsword)
            {
                item.useTurn = true;
            }
           
        }

    }
    public class AutoReuse : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().Autouse;
        }
        public override void SetDefaults(Item item)
        {
            if (item.type != ModContent.ItemType<Glock17>())
            {
                item.autoReuse = true;
            }
               
        }

    }
    public class PenumbraGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().VanillaChanges;
        }
        /// <summary>
        /// Useful to check if projectile hit NPC
        /// </summary>
        public bool ProjhitNPC;
        public override void SetDefaults(Projectile projectile)
        {
            ProjhitNPC = false;
            if (projectile.type == ProjectileID.DeathSickle)
            {
                projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            if (player.HeldItem?.type == ModContent.ItemType<OmniStaff>())
            {
                if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    target.AddBuff(ModContent.BuffType<HighVoltage>(), 120);
                }
            }
            if (player.HeldItem?.type == ItemID.InfluxWaver)
            {
                if (projectile.type == ProjectileID.InfluxWaver)
                {
                    target.AddBuff(ModContent.BuffType<MediumVoltage>(), 120);
                }
            }
        }
    }
    public class HitEffectProjectile : PenumbraGlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().HitEffect;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            ProjhitNPC = true;
            target.AddBuff(ModContent.BuffType<hitef>(), 10);
            Vector2 velocity = ModContent.GetInstance<PenumbraConfig>().HitEffectVelocity;
            const int NumProjectiles = 3;
            Player player = Main.player[projectile.owner];
            for (int i = 0; i < NumProjectiles; i++)
            {

                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));
                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 4f - Main.rand.NextFloat(0.1f);
                // Create a projectile.
                Projectile.NewProjectileDirect(Projectile.InheritSource(projectile), target.Center, newVelocity, ModContent.ProjectileType<HitEffectProj>(), 0, 0, player.whoAmI);

            }
        }

    } 
}
