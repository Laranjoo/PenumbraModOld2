using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.Items;

namespace PenumbraMod.Content.Buffs
{
    public class EnergyConductor : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Conductor");
            Description.SetDefault("The Energy Conductor eill fight for you");

            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<EnergyConductorM>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }


}
