using PenumbraMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;

using System;
using static System.Net.Mime.MediaTypeNames;
using Terraria.ModLoader;

namespace PenumbraMod
{
    internal class ReaperUI : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the Mod class.
        private UIText text;
        private UIElement area;
        private UIImage barFrame;
        private Color gradientA;
        private Color gradientB;

        public override void OnInitialize()
        {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding. You can use a UIPanel if you wish for a background.
            area = new UIElement();
            area.Width.Set(90, 0); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(0, 0);
            area.HAlign = area.VAlign = 0.5f;
            barFrame = new UIImage(Request<Texture2D>("PenumbraMod/ReaperClassBar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame.Left.Set(21, 0f);
            barFrame.Top.Set(42, 0f);
            barFrame.Width.Set(44, 0f);
            barFrame.Height.Set(22, 0f);
            text = new UIText("", 0.8f); // text to show stat
            text.Width.Set(46, 0f);
            text.Height.Set(22, 0f);
            text.Top.Set(64, 0f);
            text.Left.Set(0, 0f);
            area.Append(text);
            gradientA = new Color(119, 0, 128); // A dark purple
            gradientB = new Color(238, 0, 255); // A light purple
            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleDamageItem
            if (Main.LocalPlayer.HeldItem.DamageType != GetInstance<ReaperClass>())
                return;

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();
            // Calculate quotient
            float quotient = (float)ReaperClassPlayer.ReaperEnergy / ReaperClassPlayer.ReaperEnergyMax; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
            quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 1;
            hitbox.Width += 1;
            hitbox.Y += 6;
            hitbox.Height += 14;

            // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                //float percent = (float)i / steps; // Alternate Gradient Approach
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.HeldItem.DamageType != GetInstance<ReaperClass>())
                return;
            if (GetInstance<PenumbraConfig>().UITEXT == false)
            {
                return;
            }
            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();

            // Setting the text per tick to update and show our resource values.
            float b = (float)ReaperClassPlayer.ReaperEnergy / 20f;

            text.SetText($"Reaper Energy: {Math.Round(b * 10f) / 10f} / {(float)ReaperClassPlayer.ReaperEnergyMax / 20f}");


            base.Update(gameTime);
        }
    }
}
