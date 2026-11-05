using System.Collections;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_Bullytag : Item
    {
        [SerializeField]
        private PlayerManager player = null;

        [SerializeField]
        private NPC npc = null;

        [SerializeField]
        private float timer = 0f;

        [SerializeField]
        private float setTime = 15f;

        [SerializeField]
        private IEnumerator enumerator = null;

        [SerializeField]
        private HudGauge gauge = null;

        public override bool Use(PlayerManager pm)
        {
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out RaycastHit hitinfo, pm.pc.Reach))
            {
                if (hitinfo.transform.TryGetComponent(out NPC npc))
                {
                    this.npc = npc;
                }
            }
            else
            {
                player = pm;
            }
            Other.PlaySingleIThink("Slap");
            gauge = Other.ActivateNewGaugeIThink(pm.playerNumber, "BullytagIcon_Gauge", setTime);
            enumerator = Action();
            StartCoroutine(enumerator);
            return true;
        }

        public IEnumerator Action()
        {
            timer = 0f;
            while (timer < setTime)
            {
                timer += Time.deltaTime;
                if (gauge != null)
                {
                    gauge.SetValue(setTime, timer);
                }
                if (player != null)
                {
                    player.RuleBreak("Bullying", 0.1f, 0.25f);
                }
                if (npc != null)
                {
                    Cell cell = npc.ec.CellFromPosition(npc.transform.position);
                    if (cell != null)
                    {
                        if (cell.room != null)
                        {
                            DetentionRoomFunction function = cell.room.GetComponentInChildren<DetentionRoomFunction>();
                            if (function == null)
                            {
                                npc.SetGuiltIThink(0.1f, "Bullying");
                            }
                        }
                    }
                }
                yield return null;
            }
            Destroy(gameObject);
            yield break;
        }

        public void OnDestroy()
        {
            if (enumerator != null)
            {
                StopCoroutine(enumerator);
                enumerator = null;
            }
            if (gauge != null)
            {
                gauge.Deactivate();
                gauge = null;
            }
        }
    }
}
