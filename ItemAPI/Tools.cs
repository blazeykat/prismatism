﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using UnityEngine;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Collections.ObjectModel;
using System.Collections;

namespace ItemAPI
{
    //Utility methods
    public static class Tools
    {
        public static bool verbose = true;
        private static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "psm.txt");
        public static string modID = "Prismatism";

        private static Dictionary<string, float> timers = new Dictionary<string, float>();

        public static void Init()
        {
            if (File.Exists(defaultLog)) File.Delete(defaultLog);
        }

    
        public static void Print<T>(T obj, string color = "FFFFFF", bool force = false)
        {
            if (verbose || force)
            {
                string[] lines = obj.ToString().Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
            }

            Log(obj.ToString());
        }


        public static void PrintRaw<T>(T obj, bool force = false)
        {
            if (verbose || force)
                LogToConsole(obj.ToString());

            Log(obj.ToString());
        }

        public static void PrintError<T>(T obj, string color = "FF0000")
        {
            string[] lines = obj.ToString().Split('\n');
            foreach (var line in lines)
                LogToConsole($"<color=#{color}>[{modID}] {line}</color>");

            Log(obj.ToString());
        }

        public static void PrintException(Exception e, string color = "FF0000")
        {
            string message = e.Message + "\n" + e.StackTrace;
            {
                string[] lines = message.Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
            }

            Log(e.Message);
            Log("\t" + e.StackTrace);
        }

        public static void Log<T>(T obj)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, defaultLog), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void Log<T>(T obj, string fileName)
        {
            if (!verbose) return;
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, fileName), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void LogToConsole(string message)
        {
            message.Replace("\t", "    ");
            ETGModConsole.Log(message);
        }

        private static void BreakdownComponentsInternal(this GameObject obj, int lvl = 0)
        {
            string space = "";
            for (int i = 0; i < lvl; i++)
            {
                space += "\t";
            }

            Log(space + obj.name + "...");
            foreach (var comp in obj.GetComponents<Component>())
            {
                Log(space + "    -" + comp.GetType());
            }

            foreach (var child in obj.GetComponentsInChildren<Transform>())
            {
                if (child != obj.transform)
                    child.gameObject.BreakdownComponentsInternal(lvl + 1);
            }
        }

        public static void BreakdownComponents(this GameObject obj)
        {
            BreakdownComponentsInternal(obj, 0);
        }

        public static void ExportTexture(Texture texture, string folder = "")
        {
            string path = Path.Combine(ETGMod.ResourcesDirectory, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(Path.Combine(path, texture.name + ".png"), ((Texture2D)texture).EncodeToPNG());
        }

        public static T GetEnumValue<T>(string val) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), val.ToUpper());
        }


        public static void LogPropertiesAndFields<T>(T obj, string header = "")
        {
            Log(header);
            Log("=======================");
            if (obj == null) { Log("LogPropertiesAndFields: Null object"); return; }
            Type type = obj.GetType();
            Log($"Type: {type}");
            PropertyInfo[] pinfos = type.GetProperties();
            Log($"{typeof(T)} Properties: ");
            foreach (var pinfo in pinfos)
            {
                try
                {
                    var value = pinfo.GetValue(obj, null);
                    string valueString = value.ToString();
                    bool isList = obj?.GetType().GetGenericTypeDefinition() == typeof(List<>);
                    if (isList)
                    {
                        var list = value as List<object>;
                        valueString = $"List[{list.Count}]";
                        foreach (var subval in list)
                        {
                            valueString += "\n\t\t" + subval.ToString();
                        }
                    }
                    Log($"\t{pinfo.Name}: {valueString}");
                }
                catch { }
            }
            Log($"{typeof(T)} Fields: ");
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                Log($"\t{finfo.Name}: {finfo.GetValue(obj)}");
            }
        }
        public static void StartTimer(string name)
        {
            string key = name.ToLower();
            bool flag = Tools.timers.ContainsKey(key);
            if (flag)
            {
                Tools.PrintError<string>("Timer " + name + " already exists.", "FF0000");
            }
            else
            {
                Tools.timers.Add(key, Time.realtimeSinceStartup);
            }
        }

        // Token: 0x06000084 RID: 132 RVA: 0x00006310 File Offset: 0x00004510
        public static void StopTimerAndReport(string name)
        {
            string key = name.ToLower();
            bool flag = !Tools.timers.ContainsKey(key);
            if (flag)
            {
                Tools.PrintError<string>("Could not stop timer " + name + ", no such timer exists", "FF0000");
            }
            else
            {
                float num = Tools.timers[key];
                int num2 = (int)((Time.realtimeSinceStartup - num) * 1000f);
                Tools.timers.Remove(key);
                Tools.Print<string>(name + " finished in " + num2.ToString() + "ms", "FFFFFF", false);
            }
        }


        public static ReadOnlyCollection<OverrideBehavior> overrideBehaviors = null;
        static bool hasInit = false;

        public static void EnemyInit()
        {
            try
            {
                List<OverrideBehavior> obs = new List<OverrideBehavior>();
                foreach (Type type in
                Assembly.GetAssembly(typeof(OverrideBehavior)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(OverrideBehavior))))
                {
                    obs.Add((OverrideBehavior)Activator.CreateInstance(type));
                }
                overrideBehaviors = new ReadOnlyCollection<OverrideBehavior>(obs);
                hasInit = true;
            }
            catch (Exception e)
            {
                ETGModConsole.Log("Failed to init EnemyAPI! Please contact spcreat!\n\n" + e);
            }
        }

        public static void ManualAddOB(Type ob)
        {
            if (ob.IsClass && !ob.IsAbstract && ob.IsSubclassOf(typeof(OverrideBehavior)))
            {
                var l = new List<OverrideBehavior>(overrideBehaviors);
                l.Add((OverrideBehavior)Activator.CreateInstance(ob));
                overrideBehaviors = new ReadOnlyCollection<OverrideBehavior>(l);
            }
        }

        public static void DebugInformation(BehaviorSpeculator behavior, string path = "")
        {
            List<string> logs = new List<string>();

            logs.Add("Enemy report for enemy '" + behavior.aiActor.ActorName + "' with ID " + behavior.aiActor.EnemyGuid + ":");
            logs.Add("");

            logs.Add("--- Beginning behavior report");
            foreach (var b in behavior.AttackBehaviors)
            {
                if (b is AttackBehaviorGroup)
                {
                    logs.Add("Note: This actor has an AttackBehaviorGroup. The nicknames and probabilities are as follows:");
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(" - " + be.NickName + " | " + be.Probability);
                    }
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(ReturnPropertiesAndFields(be.Behavior, "Logging AttackBehaviorGroup behavior " + be.Behavior.GetType().Name + " with nickname " + be.NickName + " and probability " + be.Probability));
                    }
                }
                else
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging attack behavior " + b.GetType().Name));
                }
            }
            logs.Add("-----");
            foreach (var b in behavior.MovementBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging movement behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OtherBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging other behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OverrideBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging override behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.TargetBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging target behavior " + b.GetType().Name));
            }
            logs.Add("--- End of behavior report");
            logs.Add("");

            logs.Add("Components attached to the actor object are listed below.");
            foreach (var c in behavior.aiActor.gameObject.GetComponents(typeof(object)))
            {
                logs.Add(c.GetType().Name);
            }

            logs.Add("");
            if (behavior.bulletBank)
            {
                logs.Add("--- Beginning bullet bank report");

                foreach (var b in behavior.bulletBank.Bullets)
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging bullet " + b.Name));
                }
                logs.Add("--- End of bullet bank report");
            }
            else
            {
                logs.Add("--- Actor does not have a bullet bank.");
            }

            var retstr = string.Join("\n", logs.ToArray());
            if (string.IsNullOrEmpty(path))
            {
                ETGModConsole.Log(retstr);
            }
            else
            {
                File.WriteAllText(path, retstr);
            }
        }

        public static void DebugInformationNoAIActor(BehaviorSpeculator behavior, string path = "")
        {
            List<string> logs = new List<string>();

            logs.Add("Enemy report");
            logs.Add("");

            logs.Add("--- Beginning behavior report");
            foreach (var b in behavior.AttackBehaviors)
            {
                if (b is AttackBehaviorGroup)
                {
                    logs.Add("Note: This actor has an AttackBehaviorGroup. The nicknames and probabilities are as follows:");
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(" - " + be.NickName + " | " + be.Probability);
                    }
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(ReturnPropertiesAndFields(be.Behavior, "Logging AttackBehaviorGroup behavior " + be.Behavior.GetType().Name + " with nickname " + be.NickName + " and probability " + be.Probability));
                    }
                }
                else
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging attack behavior " + b.GetType().Name));
                }
            }
            logs.Add("-----");
            foreach (var b in behavior.MovementBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging movement behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OtherBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging other behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OverrideBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging override behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.TargetBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging target behavior " + b.GetType().Name));
            }
            logs.Add("--- End of behavior report");
            logs.Add("");

            logs.Add("Components attached to the object are listed below.");
            foreach (var c in behavior.gameObject.GetComponents(typeof(object)))
            {
                logs.Add(c.GetType().Name);
            }

            logs.Add("");
            if (behavior.bulletBank)
            {
                logs.Add("--- Beginning bullet bank report");

                foreach (var b in behavior.bulletBank.Bullets)
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging bullet " + b.Name));
                }
                logs.Add("--- End of bullet bank report");
            }
            else
            {
                logs.Add("--- Actor does not have a bullet bank.");
            }

            var retstr = string.Join("\n", logs.ToArray());
            if (string.IsNullOrEmpty(path))
            {
                ETGModConsole.Log(retstr);
            }
            else
            {
                File.WriteAllText(path, retstr);
            }
        }

        public static void DebugBulletBank(AIBulletBank bank, string path = "")
        {
            List<string> logs = new List<string>();

            logs.Add("bullet bank report");
            logs.Add("");

            logs.Add("");
            if (bank)
            {
                logs.Add("--- Beginning bullet bank report");

                foreach (var b in bank.Bullets)
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging bullet " + b.Name));
                }
                logs.Add("--- End of bullet bank report");
            }
            else
            {
                logs.Add("--- Actor does not have a bullet bank.");
            }

            var retstr = string.Join("\n", logs.ToArray());
            if (string.IsNullOrEmpty(path))
            {
                ETGModConsole.Log(retstr);
            }
            else
            {
                File.WriteAllText(path, retstr);
            }
        }

        public static string ReturnPropertiesAndFields<T>(T obj, string header = "")
        {
            string ret = "";
            ret += "\r\n" + (header);
            ret += "\r\n" + ("=======================");
            if (obj == null) { ret += "\r\n" + ("LogPropertiesAndFields: Null object"); return ret; }
            Type type = obj.GetType();
            ret += "\r\n" + ($"{typeof(T)} Fields: ");
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                try
                {
                    var value = finfo.GetValue(obj);
                    string valueString = value.ToString();
                    bool isArray = value.GetType().IsArray == true;
                    if (isArray)
                    {
                        var ar = (value as IEnumerable);
                        valueString = $"Array[]";
                        foreach (var subval in ar)
                        {
                            valueString += "\r\n\t\t" + subval.ToString();
                        }
                    }
                    else if (value is BulletScriptSelector)
                    {
                        valueString = (value as BulletScriptSelector).scriptTypeName;
                    }
                    ret += "\r\n" + ($"\t{finfo.Name}: {valueString}");
                }
                catch
                {
                    ret += "\r\n" + ($"\t{finfo.Name}: {finfo.GetValue(obj)}");
                }
            }

            return ret;
        }
    }
}
