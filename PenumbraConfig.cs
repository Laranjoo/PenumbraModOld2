using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace PenumbraMod
{
    [Label("Penumbra Mod Config")]
    [BackgroundColor(20, 0, 38)]
    public class PenumbraConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        // The "$" character before a name means it should interpret the name as a translation key and use the loaded translation with the same key.
        // The things in brackets are known as "Attributes".
        #region Visuals
        [Header("[i:4001] Visuals")] // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category.
        [Label("Enable or disable hit effect")]// A label is the text displayed next to the option. This should usually be a short description of what it does./*/
        [BackgroundColor(255, 0, 0)]// color
        [Tooltip("Enables or disables the hit effect when htting an NPC")] // A tooltip is a description showed when you hover your mouse over the option. It can be used as a more in-depth explanation of the option.
        [DefaultValue(true)] // This sets the configs default value.
        [ReloadRequired] // Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool HitEffect;

        [Label("Change the color of the hit effect")]
        [BackgroundColor(255, 0, 0)]
        [DefaultValue("255, 0, 0, 0")]
        [SliderColor(65, 19, 105)]
        public Color HitEffectColor;

        [Label("Change the velocity of the hit effect")]
        [BackgroundColor(255, 0, 0)]
        [SliderColor(65, 19, 105)]
        [DefaultValue(typeof(Vector2), "1, 1")]
        [Range(0f, 12f)]
        public Vector2 HitEffectVelocity { get; set; }

        [Label("Enable or disable Reaper energy bar text")]// A label is the text displayed next to the option. This should usually be a short description of what it does./*/
        [BackgroundColor(255, 0, 0)]// color
        [DefaultValue(true)] // This sets the configs default value.
        [ReloadRequired]
        public bool UITEXT { get; set; }
        #endregion

        #region Player
        [Header("[i:4] Player")]
        [Label("Enable or disable item vanilla changes")]
        [BackgroundColor(130, 0, 155)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool VanillaChanges { get; set; }

        [Label("Enable or disable player rotation on item use")]
        [Tooltip("Whenever the player do a 'Use Turn' when using a item")]
        [BackgroundColor(130, 0, 155)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool UseTurn { get; set; }

        [Label("Enable or disable player to auto reuse items")]
        [BackgroundColor(130, 0, 155)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool Autouse { get; set; }


        #endregion


    }

}
