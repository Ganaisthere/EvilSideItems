using MTM101BaldAPI.AssetTools;
using System.Collections;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_BananaForStudent : Item
    {
        [SerializeField]
        private PlayerManager player;

        [SerializeField]
        private float timer = 0f;

        [SerializeField]
        private MovementModifier moveMod;

        public override bool Use(PlayerManager pm)
        {
            player = pm;
            moveMod = new MovementModifier(Vector3.zero, 0f);
            pm.plm.am.moveMods.Add(moveMod);
            StartCoroutine(Eat());
            return true;
        }

        public void Remove()
        {
            if (moveMod != null && player != null)
            {
                player.plm.am.moveMods.Remove(moveMod);
                moveMod = null;
            }
        }

        public IEnumerator Eat()
        {
            timer = 0f;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Student_EatBanana"));
            WeightedSoundObject[] weightedSounds = new WeightedSoundObject[3]
                {
                    new WeightedSoundObject()
                    {
                        selection = AssetFinder.FindOfTypeWithName<SoundObject>("ChipCrunch", true),
                        weight = 2
                    },
                    new WeightedSoundObject()
                    {
                        selection = AssetFinder.FindOfTypeWithName<SoundObject>("AppleCrunch", true),
                        weight = 2
                    },
                    new WeightedSoundObject()
                    {
                        selection = AssetFinder.FindOfTypeWithName<SoundObject>("Student_Yay", true),
                        weight = 1
                    }
                };

            while (timer < 5f)
            {
                if (player == null)
                {
                    yield break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            float timer2 = 0f;
            while (timer < 10f)
            {
                if (player == null)
                {
                    yield break;
                }
                if (timer2 < 0.05f)
                {
                    timer2 += Time.deltaTime;
                }
                else
                {
                    timer2 = 0f;
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(weightedSounds));
                    player.plm.stamina += player.plm.StaminaMax / 50f;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }

        public void OnDestroy()
        {
            Remove();
        }
    }
}
