using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Buffs;
using System.Collections.Generic;
using Terraria.Audio;
using PenumbraMod.Common.Systems;
using PenumbraMod.Content.Items.Consumables;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;

namespace PenumbraMod.Content.NPCs.Bosses
{
    [AutoloadBossHead]
    public class BuritiBoss : ModNPC
	{
        public bool SecondStage
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Buriti");
            NPCID.Sets.TrailCacheLength[NPC.type] = 14; //How many copies of shadow/trail
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					ModContent.BuffType<StunnedNPC>(),
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 0.4f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            Main.npcFrameCount[Type] = 2;
        }

		public override void SetDefaults() {
			NPC.width = 228;
			NPC.height = 227;
			NPC.damage = 60;
			NPC.defense = 45;
			NPC.lifeMax = 44450;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.Roar;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 2;
			NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 5);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 200f;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Buriti");
        }
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (player.dead)
            {
               
                 int type = Main.rand.Next(1, 7);
                if (type == 1)
                {
                    Main.NewText("HAHAHAHAHAHAHAHAHAHAHAHAHAHAH STOOPID NEWB");
                }
                if (type == 2)
                {
                    Main.NewText("noob, doesnt know how to dodge");
                }
                if (type == 3)
                 {
                    Main.NewText("SKILL ISSUE");
                }
                if (type == 4)
                {
                    Main.NewText("skidibi dob dob dob dob yes yes yes yes");

                }
                if (type == 5)
                {
                    Main.NewText("Such a ridiculous act, dying?, stupid.");
                }
                if (type == 6)
                {
                    Main.NewText("VAI JOGAR NO VASCO KKKKKKKKK");
                }

                // If the targeted player is dead, flee
                NPC.velocity.Y -= 0.04f;
                // This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
                NPC.EncourageDespawn(10);
                return;
            }
            if (NPC.life < NPC.lifeMax / 3)
            {
                WalterWhite(player);
            } 
            else
            {
                FirstPhase(player);
            }
            
        }
        Vector2 QWERTY = Vector2.Zero;
        public void FirstPhase(Player player)
        {
            NPC.ai[0]++;
            NPC.ai[1]++;
            NPC.localAI[0]++;
            NPC.localAI[1]++;
            if (NPC.ai[0] == 1)
            {
                int type = Main.rand.Next(1, 6);
                if (type == 1)
                {
                    Main.NewText("hi noob");
                }
                if (type == 2)
                {
                    Main.NewText("ready to die?");
                }
                if (type == 3)
                {
                    Main.NewText("prove your skill issue to me");
                }
                if (type == 4)
                {
                    Main.NewText("wet owl is here to get some bitches");

                }
                if (type == 5)
                {
                    Main.NewText("hola amigo estupido");
                }
                NPC.netUpdate = true;
            }
            // TELEPORT WHEN PLAYER IS FAR AWAY
            if (NPC.localAI[0] > 3)
            {
                float range2 = 500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) > range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-100, -50);
                    int type = Main.rand.Next(1, 6);
                    if (type == 1)
                    {
                        Main.NewText("dont");
                    }
                    if (type == 2)
                    {
                        Main.NewText("why the hell u escaping?");
                    }
                    if (type == 3)
                    {
                        Main.NewText("nop aint escaping");
                    }
                    if (type == 4)
                    {
                        Main.NewText("no bitches?");

                    }
                    if (type == 5)
                    {
                        Main.NewText("RUN RUN LITTLE BABY");
                    }
                    // Teleport
                }
                NPC.netUpdate = true;

            }
            NPC.position += NPC.velocity;
            //NPC.velocity *= (float)(NPC.ai[3]/210) + 1;
            NPC.rotation = NPC.velocity.X / 16;
            // SPAWN MULTIPLE PROJECTILES
            if (NPC.ai[0] == 40 || NPC.ai[0] == 70 || NPC.ai[0] == 90)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] >= 120 && NPC.ai[0] <= 130)
            {
                Vector2 vel = new Vector2(0, 6);
                NPC.velocity = vel;
                NPC.dontTakeDamage = false;
                NPC.position += NPC.velocity;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 9;
                int type = ModContent.ProjectileType<BuritiFinger>();

                int damage = 30;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // TELEPORt
            if (NPC.localAI[1] == 170)
            {
                Main.NewText("Fun fact: my fingers are tasty");
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-720, -300);
                      // Teleport
                }
                Vector2 vel = new Vector2(10, 0);
                NPC.velocity = vel;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 0;
                int type = ModContent.ProjectileType<BrightnessDeath>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);

            }
            // MOVE
            if (NPC.localAI[0] >= 170 && NPC.localAI[0] <= 210)
            {
                NPC.friendly = false;
                Vector2 vel = new Vector2(8, 0);
                NPC.velocity = vel;
                NPC.dontTakeDamage = false;
                NPC.position += NPC.velocity;

            }
            if (NPC.localAI[0] == 180 || NPC.localAI[0] == 185 || NPC.localAI[0] == 190 || NPC.localAI[0] == 195 || NPC.localAI[0] == 200 || NPC.localAI[0] == 205)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity =  NPC.Center;
                Vector2 velocity2 = new Vector2(0, 12);
                int type = ModContent.ProjectileType<BuritiFinger2>();

                int damage = 30;
                Projectile.NewProjectile(entitySource, velocity, velocity2, type, damage, 0f, Main.myPlayer);
            }
            if (NPC.ai[0] == 215)
            {
                NPC.aiStyle = 2;
                NPC.netUpdate = true;
                int type = Main.rand.Next(1, 6);
                if (type == 1)
                {
                    Main.NewText("imagine if you touch the circle, such a skill issue");
                }
                if (type == 2)
                {
                    Main.NewText("i dare you to touch this");
                }
                if (type == 3)
                {
                    Main.NewText("stuck on the circle hahahaha (tip, instakill)");
                }
                if (type == 4)
                {
                    Main.NewText("try to run bro");

                }
                if (type == 5)
                {
                    Main.NewText("dont touch the circle or youll die :)");
                }
                
            }
            // SPIN MOVEMENT AND ALSO SHOOTING PROJECTILES
            if (NPC.localAI[0] >= 240 && NPC.localAI[0] <= 265)
            {
                NPC.aiStyle = -1;
                QWERTY = player.Center;
                NPC.Center = QWERTY + new Vector2(350, 0).RotatedBy(NPC.ai[0] / 4);

                NPC.dontTakeDamage = false;
            }
            // CIRCLE THINGY
            if (NPC.localAI[0] >= 241 && NPC.localAI[0] <= 266)
            {
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.position += NPC.velocity;
                    int velocity = 0;
                    int type = ModContent.ProjectileType<BuritiFinger>();
                    int type2 = ModContent.ProjectileType<not>();

                    int damage = 800;
                    Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
            }
                // LINE
                if (NPC.localAI[1] == 300)
                {
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.position += NPC.velocity;
                int velocity = 0;
                Vector2 vel1 = player.Center + new Vector2(0, 0);
                    Vector2 vel2 = player.Center + new Vector2(0, 150);
                    Vector2 vel3 = player.Center + new Vector2(0, 250);
                    Vector2 vel4 = player.Center + new Vector2(0, 350);
                    Vector2 vel5 = player.Center + new Vector2(0, -150);
                    Vector2 vel6 = player.Center + new Vector2(0, -250);
                    Vector2 vel7 = player.Center + new Vector2(0, -350);
                    int type = ModContent.ProjectileType<Line2>();
                    int damage = 0;
                    Projectile.NewProjectile(entitySource, vel1, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel2, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel3, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel4, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel5, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel6, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel7, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
            }
                // PROJECTILES
                if (NPC.localAI[1] == 330)
                {
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(-8, 0);
                Vector2 vel1 = player.Center + new Vector2(-400, 0);
                    Vector2 vel2 = player.Center + new Vector2(-400, 150);
                    Vector2 vel3 = player.Center + new Vector2(-400, 250);
                    Vector2 vel4 = player.Center + new Vector2(-400, 350);
                    Vector2 vel5 = player.Center + new Vector2(-400, -150);
                    Vector2 vel6 = player.Center + new Vector2(-400, -250);
                    Vector2 vel7 = player.Center + new Vector2(-400, -350);
                    int type = ModContent.ProjectileType<BuritiFinger3>();
                    int damage = 60;
                    Projectile.NewProjectile(entitySource, vel1, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel2, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel3, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel4, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel5, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel6, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(entitySource, vel7, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
            }
            // LINE
            if (NPC.localAI[1] == 400)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 0;
                Vector2 vel1 = player.Center + new Vector2(0, 0);
                Vector2 vel2 = player.Center + new Vector2(150, 0);
                Vector2 vel3 = player.Center + new Vector2(250, 0);
                Vector2 vel4 = player.Center + new Vector2(350, 0);
                Vector2 vel5 = player.Center + new Vector2(-150, 0);
                Vector2 vel6 = player.Center + new Vector2(-250, 0);
                Vector2 vel7 = player.Center + new Vector2(-350, 0);
                int type = ModContent.ProjectileType<Line>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, vel1, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel2, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel3, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel4, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel5, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel6, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel7, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
            }
            // PROJECTILES
            if (NPC.localAI[1] == 430)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(0, 1);
                Vector2 vel1 = player.Center + new Vector2(0, -400);
                Vector2 vel2 = player.Center + new Vector2(150, -400);
                Vector2 vel3 = player.Center + new Vector2(250, -400);
                Vector2 vel4 = player.Center + new Vector2(350, -400);
                Vector2 vel5 = player.Center + new Vector2(-150, -400);
                Vector2 vel6 = player.Center + new Vector2(-250, -400);
                Vector2 vel7 = player.Center + new Vector2(-350, -400);
                int type = ModContent.ProjectileType<BuritiFinger3>();
                int damage = 60;
                Projectile.NewProjectile(entitySource, vel1, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel2, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel3, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel4, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel5, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel6, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel7, NPC.DirectionTo(player.Center) * 2f * velocity, type, damage, 0f, Main.myPlayer);
            }
            // LINE
            if (NPC.localAI[1] == 500)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 0;
                Vector2 vel1 = player.Center + new Vector2(0, 0);
                Vector2 vel2 = player.Center + new Vector2(0, 150);
                Vector2 vel3 = player.Center + new Vector2(0, 250);
                Vector2 vel4 = player.Center + new Vector2(0, 350);
                Vector2 vel5 = player.Center + new Vector2(0, -150);
                Vector2 vel6 = player.Center + new Vector2(0, -250);
                Vector2 vel7 = player.Center + new Vector2(0, -350);
                Vector2 vel8 = player.Center + new Vector2(0, 0);
                Vector2 vel9 = player.Center + new Vector2(150, 0);
                Vector2 vel10 = player.Center + new Vector2(250, 0);
                Vector2 vel11 = player.Center + new Vector2(350, 0);
                Vector2 vel12 = player.Center + new Vector2(-150, 0);
                Vector2 vel13 = player.Center + new Vector2(-250, 0);
                Vector2 vel14 = player.Center + new Vector2(-350, 0);
                int type = ModContent.ProjectileType<Line2>();
                int type2 = ModContent.ProjectileType<Line>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, vel1, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel2, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel3, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel4, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel5, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel6, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel7, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel8, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel9, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel10, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel11, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel12, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel13, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel14, NPC.DirectionTo(player.Center) * 1f * velocity, type2, damage, 0f, Main.myPlayer);
            }
            // PROJECTILES
            if (NPC.localAI[1] == 530)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(-8, 0);
                Vector2 velocity2 = new Vector2(0, 1);
                Vector2 vel1 = player.Center + new Vector2(-400, 0);
                Vector2 vel2 = player.Center + new Vector2(-400, 150);
                Vector2 vel3 = player.Center + new Vector2(-400, 250);
                Vector2 vel4 = player.Center + new Vector2(-400, 350);
                Vector2 vel5 = player.Center + new Vector2(-400, -150);
                Vector2 vel6 = player.Center + new Vector2(-400, -250);
                Vector2 vel7 = player.Center + new Vector2(-400, -350);
                Vector2 vel8 = player.Center + new Vector2(0, -400);
                Vector2 vel9 = player.Center + new Vector2(150, -400);
                Vector2 vel10 = player.Center + new Vector2(250, -400);
                Vector2 vel11 = player.Center + new Vector2(350, -400);
                Vector2 vel12 = player.Center + new Vector2(-150, -400);
                Vector2 vel13 = player.Center + new Vector2(-250, -400);
                Vector2 vel14 = player.Center + new Vector2(-350, -400);
                int type = ModContent.ProjectileType<BuritiFinger3>();
                int damage = 60;
                Projectile.NewProjectile(entitySource, vel1, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel2, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel3, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel4, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel5, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel6, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel7, NPC.DirectionTo(player.Center) * 8f * velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel8, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel9, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel10, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel11, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel12, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel13, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel14, NPC.DirectionTo(player.Center) * 2f * velocity2, type, damage, 0f, Main.myPlayer);
            }
            // MORE PROJESCTILES
            if (NPC.localAI[0] == 360 || NPC.localAI[0] == 385 || NPC.localAI[0] == 410 || NPC.localAI[0] == 445 || NPC.localAI[0] == 460 || NPC.localAI[0] == 495 || NPC.localAI[0] == 531 || NPC.localAI[0] == 565 || NPC.localAI[0] == 590 || NPC.localAI[0] == 605)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = NPC.Center;
                Vector2 velocity2 = new Vector2(0, 1);
                int type = ModContent.ProjectileType<BuritiFinger3>();

                int damage = 30;
                Projectile.NewProjectile(entitySource, velocity, velocity2, type, damage, 0f, Main.myPlayer);
            }
            // APPEAR AT PLAYER POS
            if (NPC.ai[1] >= 310 && NPC.ai[1] <= 650)
            {
                    NPC.aiStyle = -1;
                    NPC.friendly = false;
                    float range2 = 2500f * 16f; // 100 tiles
                    if (NPC.DistanceSQ(player.Center) < range2 * range2)
                    {
                        NPC.position = player.Center + new Vector2(-120, -400);
                        // Teleport
                    }
                    NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            // MORE LINE ATTACK
            if (NPC.ai[0] == 690 || NPC.ai[0] == 710 || NPC.ai[0] == 730 || NPC.ai[0] == 750)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = player.Center;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Line>();
                int type2 = ModContent.ProjectileType<Line2>();

                int damage = 0;

                Projectile.NewProjectile(entitySource, velocity, velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, velocity, velocity2, type2, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // PROJECTILES
            if (NPC.ai[0] == 720 || NPC.ai[0] == 740 || NPC.ai[0] == 770 || NPC.ai[0] == 790)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(4, 0);
                Vector2 velocity2 = new Vector2(0, 1);
                Vector2 vel1 = player.Center + new Vector2(-400, 0);
                Vector2 vel2 = player.Center + new Vector2(0, -400);
                int type = ModContent.ProjectileType<BuritiFinger3>();

                int damage = 50;
                Projectile.NewProjectile(entitySource, vel1, velocity, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, vel2, velocity2, type, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // PROJECTILES AT CIRCLE FORM
            if (NPC.ai[0] == 705 || NPC.ai[0] == 722 || NPC.ai[0] == 745 || NPC.ai[0] == 762 || NPC.ai[0] == 785)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 5; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            // SPIN MOVEMENT AND ALSO SHOOTING PROJECTILES
            if (NPC.localAI[0] >= 800 && NPC.localAI[0] <= 950)
            {
                NPC.aiStyle = -1;
                QWERTY = player.Center;
                NPC.Center = QWERTY + new Vector2(350, 0).RotatedBy(NPC.ai[0] / 10);

                NPC.dontTakeDamage = false;
            }
            // BRIGHTNESS
            if (NPC.ai[1] == 800)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 0;
                int type = ModContent.ProjectileType<BrightnessDeath>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;

            }
            // MORE PROJESCTILES
            if (NPC.localAI[0] == 810 || NPC.localAI[0] == 820 || NPC.localAI[0] == 830 || NPC.localAI[0] == 840 || NPC.localAI[0] == 850 || NPC.localAI[0] == 860 || NPC.localAI[0] == 870 || NPC.localAI[0] == 880 || NPC.localAI[0] == 890 || NPC.localAI[0] == 900)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 6;
                int type = ModContent.ProjectileType<BuritiFinger>();

                int damage = 30;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
            }
            // MORE LINE ATTACK
            if (NPC.ai[0] == 845 || NPC.ai[0] == 878 || NPC.ai[0] == 899 || NPC.ai[0] == 914)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = player.Center;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Line>();
                int type2 = ModContent.ProjectileType<Line2>();

                int damage = 0;

                Projectile.NewProjectile(entitySource, velocity, velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, velocity, velocity2, type2, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // PROJECTILES
            if (NPC.ai[0] == 875 || NPC.ai[0] == 908 || NPC.ai[0] == 929 || NPC.ai[0] == 944)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(-4, 0);

                Vector2 velocity2 = new Vector2(0, 2);

                Vector2 velocity3 = new Vector2(0, -2);

                Vector2 velocity4 = new Vector2(4, 0);

                Vector2 vel1 = player.Center + new Vector2(400, 0);

                Vector2 vel2 = player.Center + new Vector2(0, -300);

                Vector2 vel3 = player.Center + new Vector2(0, 300);

                Vector2 vel4 = player.Center + new Vector2(-400, 0);

                
                int type = ModContent.ProjectileType<BuritiFinger3>();

                int damage = 50;
                Projectile.NewProjectile(entitySource, vel1, velocity, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel2, velocity2, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel3, velocity3, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel4, velocity4, type, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            //RESET
            if (NPC.ai[0] == 980)
            {
                NPC.ai[0] = 2;
                NPC.ai[1] = 2;
                NPC.localAI[0] = 2;
                NPC.localAI[1] = 2;
                NPC.aiStyle = 2;
                NPC.netUpdate = true;
            }
        }
        public void WalterWhite(Player player)
        {
            // SECOND PSHASE JESSE
            float speed = 2f;
            NPC.ai[2]++;
            NPC.ai[3]++;
            if (NPC.ai[2] == 1)
            {
                Main.NewText("STUPID IDIOT WHAT WERE YOU THINKING??????");
            }
            if (NPC.ai[2] == 52)
            {
                Main.NewText("ARE YOU DUBM OR WHAT???");
            }
            if (NPC.ai[2] == 100)
            {
                Main.NewText("IM JUST FRICKING TRYING TO KILL YOU");
            }
            if (NPC.ai[2] == 150)
            {
                Main.NewText("JUST STAY STILL UNTIL I SMASH YOUR BALLS");
            }
            if (NPC.ai[2] == 191)
            {
                Main.NewText("STUPID IDIOT");
            }
            if (NPC.ai[2] == 290)
            {
                Main.NewText("stop dodging my attacks!!!!!!!!!!!  I am going to suffere carfdiac failure!!!!!!!");
            }
            // MOVE
            if (NPC.ai[2] >= 2 && NPC.ai[2] <= 240)
            {
                NPC.life = NPC.lifeMax / 3 - 2;
                NPC.aiStyle = -1;
                NPC.friendly = false;
                Vector2 vel = new Vector2(0, 0);
                NPC.velocity = vel;
                NPC.dontTakeDamage = true;
                NPC.position += NPC.velocity;
                NPC.netUpdate = true;

            }
            // IF PLAYER IS FAR AWAY, TELEPORT
            if (NPC.ai[3] > 3)
            {
                float range2 = 500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) > range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-100, -50);
                    int type = Main.rand.Next(1, 6);
                    if (type == 1)
                    {
                        Main.NewText("DONT ESCAPE FROM ME");
                    }
                    if (type == 2)
                    {
                        Main.NewText("ESCAPING? SKILL ISSUE");
                    }
                    if (type == 3)
                    {
                        Main.NewText("AINT ESCAPING LITTLE BABY");
                    }
                    if (type == 4)
                    {
                        Main.NewText("STOP RUNNING FROM ME :(");

                    }
                    if (type == 5)
                    {
                        Main.NewText("YOU CAN RUN, YOU CAN HIDE, I WILL FIIIIIND YOU, AND EAT, YOU, SLOWLY");
                    }
                    // Teleport
                }
                NPC.netUpdate = true;

            }
            if (NPC.ai[3] == 249)
            {
                NPC.aiStyle = 2;
                NPC.friendly = false;
                Vector2 vel = new Vector2(0, -6);
                NPC.velocity = vel;
                NPC.dontTakeDamage = false;
                NPC.position += NPC.velocity;
                NPC.netUpdate = true;
            }
            // SPAWN MULTIPLE PROJECTILES
            if (NPC.ai[3] == 250 || NPC.ai[3] == 261 || NPC.ai[3] == 270)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 17; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] >= 319 && NPC.ai[2] <= 479)
            {
                NPC.aiStyle = -1;
                NPC.netUpdate = true;
            }
            // APPEAR AT PLAYER POS
            if (NPC.ai[2] == 320)
            {
                NPC.aiStyle = -1;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-100, -50);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            // MAKE THE HEXAGON PATTERN MOVE
            if (NPC.ai[2] == 350)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(100, 0) * d;
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 370)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(100, 50) * d;
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 390)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-100, 50) * d;
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 410)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-100, 0) * d;
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 430)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-100, -50) * d;
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 450)
            {
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(100, -50) * d;
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 480)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.friendly = false;
                float d = 8f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;

            }
            if (NPC.ai[2] == 540)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.aiStyle = 2;
                NPC.netUpdate = true;

            }
            // LINE ATTACK
            if (NPC.ai[2] == 565 || NPC.ai[2] == 578 || NPC.ai[2] == 599 || NPC.ai[2] == 614)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = player.Center;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Line>();
                int type2 = ModContent.ProjectileType<Line2>();

                int damage = 0;

                Projectile.NewProjectile(entitySource, velocity, velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, velocity, velocity2, type2, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 570 || NPC.ai[2] == 575 || NPC.ai[2] == 580 || NPC.ai[2] == 585 || NPC.ai[2] == 590 || NPC.ai[2] == 595 || NPC.ai[2] == 600 || NPC.ai[2] == 605)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            // PROJECTILES
            if (NPC.ai[2] == 595 || NPC.ai[2] == 608 || NPC.ai[2] == 629 || NPC.ai[2] == 644)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(-4, 0);

                Vector2 velocity2 = new Vector2(0, 2);

                Vector2 velocity3 = new Vector2(0, -2);

                Vector2 velocity4 = new Vector2(4, 0);

                Vector2 vel1 = player.Center + new Vector2(400, 0);

                Vector2 vel2 = player.Center + new Vector2(0, -300);

                Vector2 vel3 = player.Center + new Vector2(0, 300);

                Vector2 vel4 = player.Center + new Vector2(-400, 0);


                int type = ModContent.ProjectileType<BuritiFinger3>();

                int damage = 50;
                Projectile.NewProjectile(entitySource, vel1, velocity, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel2, velocity2, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel3, velocity3, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel4, velocity4, type, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] >= 680 && NPC.ai[2] <= 750)
            {
                NPC.rotation += 1.05f;
                NPC.friendly = false;
                NPC.aiStyle = -1;
                float d = 6f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;

            }
            if (NPC.ai[2] == 670 || NPC.ai[2] == 680 || NPC.ai[2] == 690 || NPC.ai[2] == 700 || NPC.ai[2] == 710 || NPC.ai[2] == 720 || NPC.ai[2] == 730 || NPC.ai[2] == 740 || NPC.ai[2] == 751 || NPC.ai[2] == 760)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<BuritiFinger>(), 30, 0f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] >= 820 && NPC.ai[2] <= 830)
            {
                Vector2 vel = new Vector2(0, 6);
                NPC.velocity = vel;
                NPC.dontTakeDamage = false;
                NPC.position += NPC.velocity;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int velocity = 9;
                int type = ModContent.ProjectileType<BuritiFinger>();

                int damage = 30;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // LINE ATTACK
            if (NPC.ai[2] == 840)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = player.Center;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Line>();
                int type2 = ModContent.ProjectileType<Line2>();

                int damage = 0;

                Projectile.NewProjectile(entitySource, velocity, velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, velocity, velocity2, type2, damage, 0f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // PROJECTILES
            if (NPC.ai[2] == 870)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(-4, 0);

                Vector2 velocity2 = new Vector2(0, 2);

                Vector2 velocity3 = new Vector2(0, -2);

                Vector2 velocity4 = new Vector2(4, 0);

                Vector2 vel1 = player.Center + new Vector2(400, 0);

                Vector2 vel2 = player.Center + new Vector2(0, -300);

                Vector2 vel3 = player.Center + new Vector2(0, 300);

                Vector2 vel4 = player.Center + new Vector2(-400, 0);


                int type = ModContent.ProjectileType<BuritiFinger3>();

                int damage = 50;
                Projectile.NewProjectile(entitySource, vel1, velocity, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel2, velocity2, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel3, velocity3, type, damage, 0f, Main.myPlayer);

                Projectile.NewProjectile(entitySource, vel4, velocity4, type, damage, 0f, Main.myPlayer);

                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 900)
            {
                NPC.aiStyle = 2;
                NPC.ai[2] = 248;
                NPC.ai[3] = 248;
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            NPC.position += NPC.velocity;
            //NPC.velocity *= (float)(NPC.ai[3]/210) + 1;
            NPC.rotation = NPC.velocity.X / 16;
        }
        public override void FindFrame(int frameHeight)
        {
            // This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
            // In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
            int startFrame = 0;
            int finalFrame = 0;
            if (NPC.life < NPC.lifeMax / 3)
            {
                startFrame = 1;
                finalFrame = 1;
            }
            int frameSpeed = 5;
            NPC.frameCounter += 0.7f;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= finalFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor) //PreDraw for trails
        {
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive);
            if (NPC.life < NPC.lifeMax / 3)
            {
                Main.instance.LoadProjectile(NPC.type);
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(lightColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin();
            return true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot) {
			// Finally, we can add additional drops. Many Zombie variants have their own unique drops: https://terraria.fandom.com/wiki/Zombie
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Buriti>()));
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }
        public override void OnKill()
        {
            // This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedBuritiBoss, -1);

            // Since this hook is only ran in singleplayer and serverside, we would have to sync it manually.
            // Thankfully, vanilla sends the MessageID.WorldData packet if a BOSS was killed automatically, shortly after this hook is ran

            // If your NPC is not a boss and you need to sync the world (which includes ModSystem, check DownedBossSystem), use this code:
            /*
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
			*/
        }
       
        public override void BossLoot(ref string name, ref int potionType)
        {
            // Here you'd want to change the potion type that drops when the boss is defeated. Because this boss is early pre-hardmode, we keep it unchanged
            // (Lesser Healing Potion). If you wanted to change it, simply write "potionType = ItemID.HealingPotion;" or any other potion type
            potionType = ItemID.GreaterHealingPotion;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface, // Plain black background
				new FlavorTextBestiaryInfoElement("Its buriti")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                int type = Main.rand.Next(1, 6);
                if (type == 1)
                {
                    Main.NewText("OH GOD I DIED TO THIS STUPID BEING?");
                }
                if (type == 2)
                {
                    Main.NewText("mine skill issue");
                }
                if (type == 3)
                {
                    Main.NewText("ok ok i admit you are good");
                }
                if (type == 4)
                {
                    Main.NewText("Why :(");

                }
                if (type == 5)
                {
                    Main.NewText("SEE YA NEXT TIME NEWB");
                }
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                ;
                
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }
        }
    }
}
