using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Common.DropConditions;
using System.Linq;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Accessories;
using PenumbraMod.Content.NPCs.Bosses;

namespace PenumbraMod.Common.GlobalNPCs
{
	
	public class BossesDrops : GlobalNPC
	{
       
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

            if (npc.type == NPCID.KingSlime)
            { 
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InconsistentJelly>(), 1, 5, 10));

            }
            if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Witcherster>(), 6, 1));

            }
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrokenWoodyThing>(), 2, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ReaperEmblem>(), 5, 1));

            }

        }
	}
}
