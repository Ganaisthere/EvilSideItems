using MTM101BaldAPI.AssetTools;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_Esoda : Item
    {
        [SerializeField]
        private PlayerManager player;

        [SerializeField]
        private MovementModifier[] moveMods = new MovementModifier[2];

        [SerializeField]
        private float time;

        [SerializeField]
        private float speed = 48f;

        public override bool Use(PlayerManager pm)
        {
            Other.PlaySingleIThink("BsodaSpray");

            player = pm;
            time = 10f;

            transform.position = pm.transform.position;
            transform.forward = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward;
            transform.position -= transform.forward * 0.75f;

            GameObject spriteRenderer_Obj = new GameObject("spriteRenderer")
            {
                layer = LayerMask.NameToLayer("NPCs")
            };
            spriteRenderer_Obj.transform.SetParent(transform);
            spriteRenderer_Obj.transform.position = transform.position;
            spriteRenderer_Obj.layer = LayerMask.NameToLayer("Billboard");
            SpriteRenderer spriteRenderer = spriteRenderer_Obj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = BasePlugin.AssetMan.Get<Sprite>("EsodaSpray_Sprite_Transparent");
            spriteRenderer.SetSpriteRotation(Random.Range(0f, 360f));
            spriteRenderer.material = AssetFinder.FindOfTypeWithName<Material>("SpriteStandard_Billboard", true);

            moveMods[0] = new MovementModifier(transform.forward * speed, 10f);
            moveMods[1] = new MovementModifier(Vector3.zero, 0f);
            pm.plm.Entity.ExternalActivity.moveMods.Add(moveMods[0]);
            pm.plm.am.moveMods.Add(moveMods[1]);
            pm.RuleBreak("Drinking", 0.8f, 0.25f);

            return true;
        }

        public void Remove()
        {
            if (player == null)
            {
                return;
            }
            if (moveMods[0] != null)
            {
                player.plm.Entity.ExternalActivity.moveMods.Remove(moveMods[0]);
                moveMods[0] = null;
            }
            if (moveMods[1] != null)
            {
                player.plm.am.moveMods.Remove(moveMods[1]);
                moveMods[1] = null;
            }
        }

        public void Update()
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 1f)
            {
                Remove();
            }

            transform.position += transform.forward * speed * player.ec.EnvironmentTimeScale * Time.deltaTime;
            time -= Time.deltaTime * player.ec.EnvironmentTimeScale;

            transform.GetComponentInChildren<SpriteRenderer>().transform.position = transform.position;

            if (time > 0f)
            {
                return;
            }

            Remove();

            Destroy(gameObject);
        }

        public void OnDestroy()
        {
            Remove();
        }
    }
}
