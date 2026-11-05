using MTM101BaldAPI.AssetTools;
using System.Collections;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_SuperDirtyChalkEraser : Item
    {

        [SerializeField]
        private float timer = 10f;

        [SerializeField]
        private float setTime = 10f;

        [SerializeField]
        private IEnumerator enumerator;

        [SerializeField]
        private HudGauge gauge;

        private static Sprite DustCloudSprite = null;
        public static Sprite GetOrCreateSprite()
        {
            if (DustCloudSprite == null)
            {
                Texture2D texture = AssetFinder.FindOfTypeWithName<Texture2D>("DustCloud", true);
                DustCloudSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 50f);
            }
            return DustCloudSprite;
        }

        public override bool Use(PlayerManager pm)
        {
            base.pm = pm;
            enumerator = Action();
            timer = setTime;
            Other.PlaySingleIThink("ChalkEraser");
            gauge = Other.ActivateNewGaugeIThink(pm.playerNumber, "SuperDirtyChalkEraserIcon_Gauge", timer);
            StartCoroutine(enumerator);
            return true;
        }

        public IEnumerator Action()
        {
            float timer2 = 0f;
            pm.SetHidden(true);
            while (timer > 0f)
            {
                timer -= Time.deltaTime * pm.PlayerTimeScale;
                if (timer2 < 0.1f)
                {
                    timer2 += Time.deltaTime * pm.PlayerTimeScale;
                }
                else
                {
                    timer2 = 0f;
                    CreateCloud(pm.transform);
                    CreateCloud(pm.transform);
                }
                if (gauge != null)
                {
                    gauge.SetValue(setTime, setTime - timer);
                }
                yield return null;
            }
            pm.SetHidden(value: false);
            enumerator = null;
            Destroy(gameObject);
            //ITM_InvisibilityElixir
            yield break;
        }

        public void OnDestroy()
        {
            if (enumerator != null)
            {
                StopCoroutine(enumerator);
            }
            if (gauge != null)
            {
                gauge.Deactivate();
            }
            if (pm != null)
            {
                pm.SetHidden(value: false);
            }
        }

        public static void CreateCloud(Transform playerTransform, bool offset = true)
        {
            GameObject cloud_Obj = new GameObject("SuperDirtyCloud")
            {
                layer = LayerMask.NameToLayer("Overlay")
            };
            SpriteRenderer spriteRenderer = cloud_Obj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetOrCreateSprite();
            spriteRenderer.material = AssetFinder.FindOfTypeWithName<Material>("SpriteStandard_Billboard", true);
            SuperDirtyCloud superDirtyCloud = cloud_Obj.AddComponent<SuperDirtyCloud>();
            superDirtyCloud.playerTransform = playerTransform;
            if (offset)
            {
                superDirtyCloud.offset = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            }
        }
    }

    public class SuperDirtyCloud : MonoBehaviour
    {
        [SerializeField]
        private float timer = 1f;

        [SerializeField]
        private float minus = -1f;

        [SerializeField]
        public Transform playerTransform;

        [SerializeField]
        public Vector3 offset;

        public void Start()
        {
            transform.position = playerTransform.position + offset;
            if (Random.Range(0, 1) == 0)
            {
                minus = 1f;
            }
        }

        public void Update()
        {
            transform.position = playerTransform.position + offset;
            timer -= Time.deltaTime;
            GetComponent<SpriteRenderer>().SetSpriteRotation(timer * minus * 5f);
            if (timer > 0)
            {
                return;
            }
            Destroy(gameObject);
        }
    }
}
