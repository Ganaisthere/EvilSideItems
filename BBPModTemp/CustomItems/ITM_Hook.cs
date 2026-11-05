using MTM101BaldAPI.AssetTools;
using System.Collections.Generic;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_Hook : Item
    {
        [SerializeField]
        private EnvironmentController ec;

        [SerializeField]
        private float timer = 0f;

        [SerializeField]
        private float speed = 50f;

        [SerializeField]
        private float baseYPos = 5f;

        [SerializeField]
        private List<Entity> touchedNpcs = new List<Entity>();

        [SerializeField]
        internal float strength = 100f;

        [SerializeField]
        internal bool hitWindows = true;

        public override bool Use(PlayerManager pm)
        {
            baseYPos = 5f;
            pm.RuleBreak("Bullying", 0.8f, 0.25f);
            ec = pm.ec;
            timer = 0f;
            touchedNpcs.Clear();
            strength = 80f;
            hitWindows = true;

            transform.position = pm.transform.position;
            transform.forward = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward;

            GameObject spriteRenderer_Obj = new GameObject("spriteRenderer")
            {
                layer = LayerMask.NameToLayer("NPCs")
            };
            spriteRenderer_Obj.transform.SetParent(transform);
            spriteRenderer_Obj.transform.position = new Vector3(transform.position.x, baseYPos, transform.position.z);
            spriteRenderer_Obj.layer = LayerMask.NameToLayer("Billboard");
            SpriteRenderer spriteRenderer = spriteRenderer_Obj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetSprite();
            spriteRenderer.material = AssetFinder.FindOfTypeWithName<Material>("SpriteStandard_Billboard", true);

            Other.PlaySingleIThink("GrappleLaunch");

            return true;
        }

        public virtual Sprite GetSprite()
        {
            return BasePlugin.AssetMan.Get<Sprite>("HookIcon_Entity");
        }

        public void Update()
        {
            if (ec == null)
            {
                Destroy(gameObject);
            }
            timer += Time.deltaTime * ec.EnvironmentTimeScale;
            float edit = (timer - 1f) * timer;

            transform.position += transform.forward * speed * ec.EnvironmentTimeScale * Time.deltaTime;
            GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(transform.position.x, baseYPos - edit * 2f, transform.position.z);
            GetComponentInChildren<SpriteRenderer>().transform.rotation *= Quaternion.Euler(0f, 0f, Time.deltaTime * ec.EnvironmentTimeScale * 3f);

            if (Physics.Raycast(transform.position - transform.forward * 2f, transform.forward, out RaycastHit hitinfo, 4f))
            {
                if (hitinfo.transform.TryGetComponent(out Entity entity))
                {
                    if (!touchedNpcs.Contains(entity) && !hitinfo.transform.TryGetComponent<PlayerManager>(out _))
                    {
                        touchedNpcs.Add(entity);
                        timer = 0f;
                        baseYPos = GetComponentInChildren<SpriteRenderer>().transform.position.y;
                        if (entity.TryGetComponent(out AudioManager audioMan))
                        {
                            audioMan.PlaySingle(AssetFinder.FindOfTypeWithName<SoundObject>("Bang", true));
                        }
                        else
                        {
                            Other.PlaySingleIThink("Bang");
                        }
                        entity.AddForce(new Force(transform.forward, strength, -100f));
                    }
                }
                else if (hitinfo.transform.TryGetComponent(out Window window) && hitWindows)
                {
                    window.Break(true);
                }
            }

            if (GetComponentInChildren<SpriteRenderer>().transform.position.y < -2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
