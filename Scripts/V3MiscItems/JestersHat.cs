using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using Dungeonator;
using System.Reflection;
using Brave.BulletScript;

namespace katmod
{
    class JestersHat : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crown of Chaos";
            string resourceName = "katmod/Resources/V3MiscItems/catseye";
            GameObject obj = new GameObject();
            JestersHat item = obj.AddComponent<JestersHat>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "So Many Nights";
            string longDesc = "Entering a secret room heals you for 1 heart.\n\nA pin fashioned with the logo of a cat's eye.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.S;
            Hook hook = new Hook(
                typeof(Bullet).GetMethod("Fire", new Type[] { typeof(Offset), typeof(Direction), typeof(Speed), typeof(Bullet) }),
                typeof(JestersHat).GetMethod("FireHook")
            );
        }

        public static void FireHook(Action<Bullet, Offset, Direction, Speed, Bullet> orig, Bullet self, Offset offset, Direction direction, Speed speed, Bullet bullet)
        {
            ETGModConsole.Log("yeppers");
            if (GameManager.Instance.PrimaryPlayer.HasMTGConsoleID("psm:crown_of_chaos") || (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.HasMTGConsoleID("psm:crown_of_chaos")))
            {
                bullet.Projectile.collidesWithEnemies = true;
            }
            orig(self, offset, direction, speed, bullet);
        }

        public delegate void Action<T, T2, T3, T4, T5>(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }
}