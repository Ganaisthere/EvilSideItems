using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_EvilYTP : Item
    {
        [SerializeField]
        private int value = 25;

        public override bool Use(PlayerManager pm)
        {
            value = (int)(pm.plm.stamina / pm.plm.StaminaMax * 100f);
            pm.plm.stamina = 0f;
            Singleton<CoreGameManager>.Instance.AddPoints(value, pm.playerNumber, true, true, true);
            return true;
        }
    }
}
