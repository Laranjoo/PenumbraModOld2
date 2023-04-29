using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Buffs;

namespace PenumbraMod.Content.NPCs
{
	// Party Zombie is a pretty basic clone of a vanilla NPC. To learn how to further adapt vanilla NPC behaviors, see https://github.com/tModLoader/tModLoader/wiki/Advanced-Vanilla-Code-Adaption#example-npc-npc-clone-with-modified-projectile-hoplite
	public class MeltedSkeleton : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Melted Skeleton");

			Main.npcFrameCount[Type] = 3;


			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					BuffID.OnFire,
					BuffID.OnFire3,
					ModContent.BuffType<MeltedEx>(),
					BuffID.Confused // Most NPCs have this
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 38;
			NPC.damage = 35;
			NPC.defense = 15;
			NPC.lifeMax = 720;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 3200f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3; // Fighter AI, important to choose the aiStyle that matches the NPCID that we want to mimic
			NPC.lavaImmune = true;
			AIType = NPCID.Skeleton; // Use vanilla zombie's type when executing AI code. (This also means it will try to despawn during daytime)
			AnimationType = NPCID.Zombie; // Use vanilla zombie's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
			Banner = Item.NPCtoBanner(NPCID.Skeleton);
			BannerItem = Item.BannerToItem(Banner); // Makes kills of this NPC go towards dropping the banner it's associated with.

		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Obsidian, 1, 3, 7));
			npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 5, 15));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeltedEmber>(), 1, 1, 4));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode)
				return SpawnCondition.Underworld.Chance * 0.2f;
			return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("This cursed skeleton fell into the hell and burned, he misteriously survived to the burns, suffering in the hell because of its unknown curse..."),


				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{

			if (NPC.life <= 0)
			{
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				int backGoreType = Mod.Find<ModGore>("MeltedSkeletonGore_Head").Type;
				int backGoreType2 = Mod.Find<ModGore>("MeltedSkeletonGore_Leg").Type;
				int frontGoreType = Mod.Find<ModGore>("MeltedSkeletonGore_Body").Type;

				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType2);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType);
				}


			}

		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			int buffType = BuffID.OnFire;
			int timeToAdd = 5 * 60; //This makes it 5 seconds, one second is 60 ticks
			target.AddBuff(buffType, timeToAdd);
		}
	}
}
