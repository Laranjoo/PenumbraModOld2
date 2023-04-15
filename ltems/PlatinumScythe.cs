using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class PlatinumScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Pointy and shiny!" +
                "\n[c/f6d8eb:Special ability:] When used, the player throws a platinum scythe that sticks into ground/enemies, dealing damage in them");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 7850;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 10f;
			Item.noUseGraphic = false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 9500)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<PlatinumScytheProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 9500)
            {
                const int NumProjectiles = 2;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 2f - Main.rand.NextFloat(0.4f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<PlatinumScytheProj>(), damage, knockback, player.whoAmI);

                }

            }

            return true;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.PlatinumBar, 20)
			.AddIngredient(ItemID.Wood, 15)
			.AddTile(TileID.Anvils)
			.Register();
		}
        public class PlatinumScytheProj : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Platinum Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
                ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            }

            public override void SetDefaults()
            {
                Projectile.penetrate = -1;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.damage = 25;
                Projectile.scale = 1.1f;
                Projectile.hide = false;
                Projectile.timeLeft = 300;
                Projectile.aiStyle = 1;
                Projectile.width = 52;
                Projectile.height = 52;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }
            public override void AI()
            {
                float maxDetectRadius = 50f; // The maximum radius at which a projectile can detect a target
                float projSpeed = 8f; // The speed at which the projectile moves towards the target
                NPC closestNPC = FindClosestNPC(maxDetectRadius);
                if (closestNPC == null)
                    return;

                // If found, change the velocity of the projectile and turn it in the direction of the target
                // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
                Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;

            }
            public NPC FindClosestNPC(float maxDetectDistance)
            {
                NPC closestNPC = null;

                // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
                float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

                // Loop through all NPCs(max always 200)
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC target = Main.npc[k];
                    // Check if NPC able to be targeted. It means that NPC is
                    // 1. active (alive)
                    // 2. chaseable (e.g. not a cultist archer)
                    // 3. max life bigger than 5 (e.g. not a critter)
                    // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                    // 5. hostile (!friendly)
                    // 6. not immortal (e.g. not a target dummy)
                    if (target.CanBeChasedBy())
                    {
                        // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                        float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                        // Check if it is within the radius
                        if (sqrDistanceToTarget < sqrMaxDetectDistance)
                        {
                            sqrMaxDetectDistance = sqrDistanceToTarget;
                            closestNPC = target;
                        }
                    }
                }

                return closestNPC;
            }
            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                Vector2 vel = Vector2.Zero;
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation += 0f;
                return false;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }

                return true;
            }
        }
       
    }
}