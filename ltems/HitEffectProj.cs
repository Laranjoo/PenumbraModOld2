using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class HitEffectProj : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().HitEffect;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HitEffectProj");
        }

        public override void SetDefaults()
        {
            Projectile.width = 19;
            Projectile.height = 17;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            
        }
        public override void AI()
        {
            Projectile.alpha += 25;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return ModContent.GetInstance<PenumbraConfig>().HitEffectColor * Projectile.Opacity;
        }
        
    }
}