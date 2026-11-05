using MTM101BaldAPI.Components;
using MTM101BaldAPI.PlusExtensions;
using System.Collections;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_EvilZestyBar : Item
    {
        [SerializeField]
        private PlayerManager player = null;

        [SerializeField]
        private float setTime = 10f;

        [SerializeField]
        private ValueModifier[] valueModifiers = new ValueModifier[2];

        [SerializeField]
        public IEnumerator enumerator = null;

        [SerializeField]
        private HudGauge gauge;

        public override bool Use(PlayerManager pm)
        {
            foreach (var i in FindObjectsOfType<ITM_EvilZestyBar>())
            {
                if (i.player == pm && i != this)
                {
                    Destroy(gameObject);
                    return false;
                }
            }

            Other.PlaySingleIThink("ChipCrunch");
            setTime = 10f;
            player = pm;
            gauge = Other.ActivateNewGaugeIThink(pm.playerNumber, "EvilZestyBarIcon_Gauge", setTime);
            enumerator = Action();
            StartCoroutine(enumerator);

            return true;
        }

        public IEnumerator Action()
        {
            if (player == null)
            {
                yield break;
            }

            float timer = 0f;
            PlayerMovementStatModifier statModifier = player.GetMovementStatModifier();
            valueModifiers[0] = new ValueModifier(2f, 0f);
            valueModifiers[1] = new ValueModifier(2f, 0f);
            statModifier.AddModifier("walkSpeed", valueModifiers[0]);
            statModifier.AddModifier("runSpeed", valueModifiers[1]);

            while (timer < setTime)
            {
                if (player == null)
                {
                    yield break;
                }
                else
                {
                    yield return null;
                }
                if (gauge != null)
                {
                    gauge.SetValue(setTime, setTime - timer);
                }
                timer += Time.deltaTime * player.PlayerTimeScale;
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
            }
            if (player != null)
            {
                PlayerMovementStatModifier statModifier = player.GetMovementStatModifier();
                if (valueModifiers[0] != null)
                {
                    statModifier.RemoveModifier(valueModifiers[0]);
                    valueModifiers[0] = null;
                }
                if (valueModifiers[1] != null)
                {
                    statModifier.RemoveModifier(valueModifiers[1]);
                    valueModifiers[1] = null;
                }
            }
            if (gauge != null)
            {
                gauge.Deactivate();
            }
        }
    }
}
