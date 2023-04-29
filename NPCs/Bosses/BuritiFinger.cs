using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.NPCs.Bosses
{
	public class BuritiFinger : ModProjectile
	{
		public override void SetStaticDefaults()
		{
             // DisplayName.SetDefault("Buriti Finger"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			
        }

        public override void SetDefaults()
		{
            Projectile.damage = 50;
            Projectile.width = 88;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
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
        }
    }
    public class BuritiFinger3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Buriti Finger"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

        }

        public override void SetDefaults()
        {
            Projectile.damage = 50;
            Projectile.width = 101;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            Projectile.velocity *= 1.05f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }
    }
    public class BuritiFinger2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Buriti Finger"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

        }

        public override void SetDefaults()
        {
            Projectile.damage = 50;
            Projectile.width = 88;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        int t;
        public override void AI()
        {
            Projectile.alpha -= 20;
            t++;
            if (t == 10)
            {
                Projectile.velocity.Y += 1.1f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }
    }
    public class not : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Buriti Finger"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

        }

        public override void SetDefaults()
        {
            Projectile.damage = 500;
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
    }
    public class Line : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Line"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 1900;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
       
        double deg;

        public override void AI()
        {
            
            Projectile.alpha -= 50;
          
        }
            
    }
    public class Line2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Line"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

        }

        public override void SetDefaults()
        {
            Projectile.width = 1900;
            Projectile.height = 5;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
        }
        double deg;

        public override void AI()
        {
            Projectile.alpha -= 50;
           
        }
    }
}