using Dungeonator;
using ItemAPI;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class StoneAmmolet : PassiveItem
    {
        public static void Init()
        {
            string name = "Stone Ammolet";
            string resourcePath = "katmod/Resources/DecaySet/stoneammolet.png";
            GameObject gameObject = new GameObject(name);
            StoneAmmolet item = gameObject.AddComponent<StoneAmmolet>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Bonebender Charm";
            string longDesc = "Makes your enemies wither upon blank.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(321);
        }
        private void OnBlank(PlayerController player, int integer)
        {
            List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null && Owner != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    {
                        activeEnemies[i].ApplyEffect(wither, 1f, null);
                    }
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnUsedBlank += this.OnBlank;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedBlank -= this.OnBlank;
            return base.Drop(player);
        }

        public GameActorWitherEffect wither = new GameActorWitherEffect
        {
            duration = 20,
            TintColor = Color.black,
            AppliesTint = true
        };

        public static UnityEngine.GameObject overheadder = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
    }
}
