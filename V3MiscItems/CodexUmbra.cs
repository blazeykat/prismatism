using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace katmod
{
    class CodexUmbra : PassiveItem
    {
        public static void Init()
        {
            string name = "Codex Umbra";
            string resource = "katmod/Resources/V3MiscItems/bettercodex";
            GameObject obj = new GameObject(name);
            CodexUmbra item = obj.AddComponent<CodexUmbra>();
            ItemBuilder.AddSpriteToObject(name, resource, obj);
            string shortd = "Book of Shadows";
            string longd = "Conjure the shadows to your will, giving bonus pickups.";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f);
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(487);
            Hook hook = new Hook(typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(CodexUmbra).GetMethod("VeryShady"));
            Hook hook2 = new Hook(typeof(SilencerItem).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(CodexUmbra).GetMethod("VeryBlank"));
            Hook hook3 = new Hook(typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(CodexUmbra).GetMethod("Healing"));
        }

        public static void VeryShady(Action<KeyBulletPickup, PlayerController> acshon, KeyBulletPickup key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in GameManager.Instance.GetActivePlayerClosestToPoint(key.specRigidbody.UnitCenter, true).passiveItems)
            {
                if (passives is CodexUmbra && BoxOTools.BasicRandom(.5f))
                {
                    GiveRandomItem(player);
                }
            }
        }


        public static void VeryBlank(Action<SilencerItem, PlayerController> acshon, SilencerItem key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is CodexUmbra && BoxOTools.BasicRandom(.5f))
                {
                    GiveRandomItem(player);
                }
            }
        }

        public static void Healing(Action<HealthPickup, PlayerController> acshon, HealthPickup key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is CodexUmbra && BoxOTools.BasicRandom(.5f))
                {
                    GiveRandomItem(player);
                }
            }
        }

        public static void GiveRandomItem(PlayerController player)
        {
            switch ((int)UnityEngine.Random.Range(1, 11))
            {
                default:
                    ETGModConsole.Log("you buffoon");
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    LootEngine.SpawnCurrency(player.specRigidbody.UnitCenter, 8, false);
                    break;
                case 9:
                    player.carriedConsumables.KeyBullets += 1;
                    player.BloopItemAboveHead(KeyDoubler.itemator.sprite);
                    break;
                case 10:
                    player.Blanks += 1;
                    player.BloopItemAboveHead(BlankDoubler.itemator.sprite);
                    break;
            }
        }
    }
}
