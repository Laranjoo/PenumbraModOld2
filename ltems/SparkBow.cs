using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using PenumbraMod.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;

namespace PenumbraMod.Content.Items
{
    public class SparkBow : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Spark Bow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Converts Wooden Arrows into lightning arrows, they inflicts Low voltage"
				+ "\n''Good for zapping people!''");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 29;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 36;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.shootSpeed = 20f;
			Item.noMelee = true;
			Item.crit = 16;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Spark>();
           
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 60f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }


	}
    public class Spark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Arrow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 600;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }
        private static VertexStrip _vertexStrip = new VertexStrip();

        public void Draw(Projectile proj)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(0f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        private Color StripColors(float progressOnStrip)
        {
            Color value = Main.hslToRgb((progressOnStrip * 1.6f - Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.5f);
            Color result = Color.Lerp(Color.Blue, value, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = 0;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
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
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            // Retrieve reference to shader
            var EOLShader = GameShaders.Armor.GetShaderFromItemId(1065);

            // Call Apply to apply the shader to the SpriteBatch. Only 1 shader can be active at a time.
            EOLShader.Apply(null);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If collide with tile, reduce the penetrate.
            // So the projectile can reflect at most 5 times
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }

            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueTorch, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.005f;

            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.78f);
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.6f);
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

    }
}