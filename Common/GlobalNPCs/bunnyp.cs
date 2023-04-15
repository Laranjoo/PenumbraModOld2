using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Common.DropConditions;
using System.Linq;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Accessories;

namespace PenumbraMod.Common.GlobalNPCs
{
	// This file shows numerous examples of what you can do with the extensive NPC Loot lootable system.
	// You can find more info on the wiki: https://github.com/tModLoader/tModLoader/wiki/Basic-NPC-Drops-and-Loot-1.4
	// Despite this file being GlobalNPC, everything here can be used with a ModNPC as well! See examples of this in the Content/NPCs folder.
	public class bunnyp : GlobalNPC
	{
		
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {


            if (npc.type == NPCID.Bunny)
            {
                
                bunnypaw exampleDropCondition = new bunnypaw();
                IItemDropRule conditionalRule = new LeadingConditionRule(exampleDropCondition);

                int itemType = ModContent.ItemType<BunnysPaw>();
                if (npc.type == NPCID.Bunny)
                {
                    itemType = ModContent.ItemType<BunnysPaw>();
                }
                // 33% chance to drop other corresponding item in addition
                IItemDropRule rule = ItemDropRule.Common(itemType, chanceDenominator: 17);

                // Apply our item drop rule to the conditional rule
                conditionalRule.OnSuccess(rule);
                // Add the rule
                npcLoot.Add(conditionalRule);
                // It will result in the drop being shown in the bestiary, but only drop if the condition is true.
            }
        }
	}
}
