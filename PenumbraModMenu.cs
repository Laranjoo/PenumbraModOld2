using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace PenumbraMod.Content
{
	public class PenumbraModMenu : ModMenu
	{
		public override Asset<Texture2D> Logo => base.Logo;
        public override void Update(bool isOnTitleScreen)
        {
            Main.dayTime = false;
            Main.time = 40000;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/TitleScreen");

		public override string DisplayName => "Penumbra Mod";

		
	}
}
