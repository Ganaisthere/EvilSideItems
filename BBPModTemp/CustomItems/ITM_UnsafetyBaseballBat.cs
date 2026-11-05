using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace EvilSideItems.CustomItems
{
    public class ITM_UnsafetyBaseballBat : Item
    {
        [SerializeField]
        internal float strength = 100f;

        public override bool Use(PlayerManager pm)
        {
            if (FindObjectsOfType<SweepAni>().Length > 0)
            {
                Destroy(gameObject);
                return false;
            }

            GameObject sweepMask_Obj = new GameObject("SweepMask");
            sweepMask_Obj.transform.SetParent(Singleton<CoreGameManager>.Instance.GetHud(pm.playerNumber).transform);
            RawImage sweepMask = sweepMask_Obj.AddComponent<RawImage>();
            sweepMask.rectTransform.sizeDelta = new Vector2(160f, 64f);
            sweepMask.rectTransform.anchorMax = new Vector2(0.5f, 0f);
            sweepMask.rectTransform.anchorMin = new Vector2(0.5f, 0f);
            sweepMask.rectTransform.anchoredPosition3D = new Vector3(192f, 64f, 0f);
            sweepMask.rectTransform.localScale = Vector3.one;
            sweepMask.rectTransform.SetAsFirstSibling();
            Mask mask = sweepMask_Obj.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            GameObject sweep_Obj = new GameObject("Sweep");
            sweep_Obj.transform.SetParent(sweepMask_Obj.transform);
            Image sweep = sweep_Obj.AddComponent<Image>();
            sweep.rectTransform.sizeDelta = new Vector2(192f, 64f);
            sweep.rectTransform.anchoredPosition3D = new Vector3(-192f, 0f, 0f);
            sweep.rectTransform.localScale = Vector3.one;
            sweep.sprite = BasePlugin.AssetMan.Get<Sprite>("Sweep");
            sweep.maskable = true;

            SweepAni sweepAni = sweepMask_Obj.AddComponent<SweepAni>();
            sweepAni.sweepMask = sweepMask;
            sweepAni.sweep = sweep;

            Other.PlaySingleIThink("NoSquee");

            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out RaycastHit hitinfo, pm.pc.Reach))
            {
                int hitSomething = 0;
                if (hitinfo.transform.TryGetComponent(out Entity entity))
                {
                    if (!hitinfo.transform.TryGetComponent<PlayerManager>(out _))
                    {
                        if (entity.TryGetComponent(out AudioManager audioMan))
                        {
                            audioMan.PlaySingle(AssetFinder.FindOfTypeWithName<SoundObject>("Bang", true));
                        }
                        else
                        {
                            Other.PlaySingleIThink("Bang");
                        }
                        entity.AddForce(new Force(pm.transform.forward, strength, -strength));
                        pm.RuleBreak("Bullying", 0.5f, 0.3f);
                        hitSomething += 1;
                    }
                }
                if (hitinfo.transform.TryGetComponent(out Window window))
                {
                    if (!(bool)window.ReflectionGetVariable("broken"))
                    {
                        Other.PlaySingleIThink("Bang");
                        window.Break(true);
                        pm.RuleBreak("Bullying", 0.5f, 0.3f);
                        hitSomething += 1;
                    }
                }
                if (hitSomething > 0)
                {
                    int random = Random.Range(0, 4);
                    if (random < hitSomething)
                    {
                        Destroy(gameObject);
                        return true;
                    }
                }
            }
            Destroy(gameObject);
            return false;
        }
    }

    public class SweepAni : MonoBehaviour
    {
        [SerializeField]
        internal float timer = 0f;

        [SerializeField]
        internal RawImage sweepMask;

        [SerializeField]
        internal Image sweep;

        public void Update()
        {
            if (sweepMask == null || sweep == null)
            {
                return;
            }
            sweepMask.rectTransform.anchoredPosition3D = new Vector3(192f - 384f * (timer / 0.27f), 64f, 0f);
            sweep.rectTransform.anchoredPosition3D = new Vector3(-192f + 384f * (timer / 0.27f), 0f, 0f);
            timer += Time.deltaTime;
            if (timer > 0.27f)
            {
                Destroy(sweep.gameObject);
                Destroy(sweepMask.gameObject);
            }
        }
    }
}
