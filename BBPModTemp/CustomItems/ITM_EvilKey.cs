using MTM101BaldAPI.Reflection;
using System.Collections;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_EvilKey : Item
    {
        [SerializeField]
        private PlayerManager player;

        [SerializeField]
        private float timer = 0f;

        [SerializeField]
        private float setTime = 10f;

        [SerializeField]
        private IEnumerator enumerator = null;

        [SerializeField]
        private HudGauge gauge;

        public override bool Use(PlayerManager pm)
        {
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out RaycastHit hitinfo, pm.pc.Reach, pm.pc.ClickLayers))
            {
                if (hitinfo.transform.TryGetComponent(out StandardDoor door))
                {
                    DetentionRoomFunction function = door.transform.parent.parent.parent.GetComponentInChildren<DetentionRoomFunction>();
                    if (door.locked && function != null)
                    {
                        player = pm;
                        function.ReflectionSetVariable("time", 0f);
                        foreach (StandardDoor door1 in door.transform.parent.parent.parent.GetComponentsInChildren<StandardDoor>())
                        {
                            door1.Unlock();
                        }
                        HudGauge detentionGauge = function.ReflectionGetVariable("gauge") as HudGauge;
                        if (detentionGauge != null)
                        {
                            if (detentionGauge.Active)
                            {
                                detentionGauge.Deactivate();
                            }
                        }
                        timer = 0f;
                        enumerator = IFeelNotGood();
                        gauge = Other.ActivateNewGaugeIThink(pm.playerNumber, "EvilKeyIcon_Gauge", setTime);
                        StartCoroutine(enumerator);
                        return true;
                    }
                }
            }
            Destroy(gameObject);
            return false;
        }

        public IEnumerator IFeelNotGood()
        {
            timer = 0f;
            while (timer < 3f)
            {
                if (player == null)
                {
                    Destroy(gameObject);
                    enumerator = null;
                    yield break;
                }
                player.ClearGuilt();
                player.RuleBreak("Escaping", 0.25f, 0.25f);
                timer += Time.deltaTime;
                if (gauge != null)
                {
                    gauge.SetValue(setTime, setTime - timer);
                }
                yield return null;
            }
            enumerator = null;
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
