using BepInEx.Bootstrap;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Reflection;
using System.Collections;
using UnityEngine;

namespace EvilSideItems
{
    public static class Other
    {
        public static string GLT(string key)
        {
            return Singleton<LocalizationManager>.Instance.GetLocalizedText(key);
        }

        public static bool CLC(string i)
        {
            return Chainloader.PluginInfos.ContainsKey(i);
        }

        public static void PlaySingleIThink(string name)
        {
            SoundObject sound = AssetFinder.FindOfTypeWithName<SoundObject>(name, true);
            if (sound == null)
            {
                return;
            }
            CoreGameManager CGM = Singleton<CoreGameManager>.Instance;
            if (CGM == null)
            {
                Singleton<MusicManager>.Instance.PlaySoundEffect(sound);
                return;
            }
            CGM.audMan.PlaySingle(sound);
        }

        public static IEnumerator GuiltTimerIThink(this NPC npc)
        {
            float guiltTime = (float)npc.ReflectionGetVariable("guiltTime");
            while (guiltTime > 0f)
            {
                guiltTime = (float)npc.ReflectionGetVariable("guiltTime");
                guiltTime -= Time.deltaTime * npc.ec.NpcTimeScale;
                npc.ReflectionSetVariable("guiltTime", guiltTime);
                yield return null;
            }

            npc.ClearGuilt();
        }

        public static void SetGuiltIThink(this NPC npc, float time, string rule)
        {
            if (npc.Disobeying)
            {
                npc.ReflectionSetVariable("guiltTime", time);
                npc.ReflectionSetVariable("brokenRule", rule);
                return;
            }

            npc.ReflectionSetVariable("disobeying", true);
            npc.ReflectionSetVariable("guiltTime", time);
            npc.ReflectionSetVariable("brokenRule", rule);
            IEnumerator guiltTimer = npc.ReflectionGetVariable("guiltTimer") as IEnumerator;
            npc.ReflectionSetVariable("guiltTimer", npc.GuiltTimerIThink());
            guiltTimer = npc.GuiltTimerIThink();
            npc.StartCoroutine(guiltTimer);
        }

        public static HudGauge ActivateNewGaugeIThink(int playerNumber, Sprite sprite, float setTime)
        {
            return Singleton<CoreGameManager>.Instance.GetHud(playerNumber).gaugeManager.ActivateNewGauge(sprite, setTime);
        }

        public static HudGauge ActivateNewGaugeIThink(int playerNumber, string SpriteName, float setTime)
        {
            return ActivateNewGaugeIThink(playerNumber, BasePlugin.AssetMan.Get<Sprite>(SpriteName), setTime);
        }

        public static void UnRevealIThink(this MapTile mapTile)
        {
            mapTile.ReflectionSetVariable("found", false);
            mapTile.gameObject.SetActive(false);
        }

        public static void HideIThink(this Map map, int posX, int posZ)
        {
            if (map.tiles[posX, posZ] != null)
            {
                map.tiles[posX, posZ].SpriteRenderer.sprite = null;
                map.tiles[posX, posZ].SpriteRenderer.color = Color.clear;

                map.tiles[posX, posZ].UnRevealIThink();
                map.foundTiles[posX, posZ] = false;
            }
        }

        public static void NotCompleteMapIThink(this Map map)
        {
            for (int i = 0; i < map.size.x; i++)
            {
                for (int j = 0; j < map.size.z; j++)
                {
                    if (!map.Ec.cells[i, j].Null && !map.Ec.cells[i, j].hideFromMap)
                    {
                        map.HideIThink(i, j);
                    }
                }
            }
        }
    }
}
