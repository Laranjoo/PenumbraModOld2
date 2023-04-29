using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod
{
    internal class ReaperUI : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the Mod class.

        private UIText text;
        private UIImage barFrame;
        private Color gradientA;
        private Color gradientB;
        private DraggableUI panel;
        public override void OnInitialize()
        {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding. You can use a UIPanel if you wish for a background.
            panel = new DraggableUI();
            panel.Width.Set(90, 0);
            panel.Height.Set(22, 0);
            panel.Top.Set(25, 0f);
            panel.Left.Set(800, 0f);
            panel.BackgroundColor = new Color(0, 0, 0, 255) * 0f;
            panel.BorderColor = new Color(0, 0, 0, 255) * 0f;

            barFrame = new UIImage(Request<Texture2D>("PenumbraMod/ReaperClassBar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame.Left.Set(-13, 0f);
            barFrame.Top.Set(-10, 0f);
            barFrame.Width.Set(100, 0f);
            barFrame.Height.Set(22, 0f);
            text = new UIText("", 0.8f); // text to show stat
            text.Width.Set(46, 0f);
            text.Height.Set(22, 0f);
            text.Top.Set(22, 0f);
            text.Left.Set(-38, 0f);
            panel.Append(text);
            gradientA = new Color(199, 60, 10); // A orange
            gradientB = new Color(134, 15, 46); // A dark pink
            panel.Append(barFrame);
            Append(panel);
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
            hitbox.X += 14;
            hitbox.Width += 1;
            hitbox.Y += 4;
            hitbox.Height += 12;

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
            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();

            // Setting the text per tick to update and show our resource values.
            float b = (float)ReaperClassPlayer.ReaperEnergy / 20f;
            if (GetInstance<PenumbraConfig>().UITEXT)
                text.SetText($"Reaper Energy {ReaperClassPlayer.ReaperEnergy} / {ReaperClassPlayer.ReaperEnergyMax}");


            base.Update(gameTime);
        }
    }
    public class DraggableUI : UIPanel
    {
        // Stores the offset from the top left of the UIPanel while dragging
        private Vector2 offset;
        // A flag that checks if the panel is currently being dragged
        private bool dragging;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.LeftMouseDown(evt);
            // When the mouse button is down, then we start dragging
            DragStart(evt);
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            // When the mouse button is up, then we stop dragging
            DragEnd(evt);
        }

        private void DragStart(UIMouseEvent evt)
        {
            // The offset variable helps to remember the position of the panel relative to the mouse position
            // So no matter where you start dragging the panel, it will move smoothly
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 endMousePosition = evt.MousePosition;
            dragging = false;

            Left.Set(endMousePosition.X - offset.X, 0f);
            Top.Set(endMousePosition.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Checking ContainsPoint and then setting mouseInterface to true is very common
            // This causes clicks on this UIElement to not cause the player to use current items
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            // Here we check if the DragableUIPanel is outside the Parent UIElement rectangle
            // (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
            // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }
}
