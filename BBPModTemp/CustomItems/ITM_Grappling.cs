using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_Grappling : ITM_Hook
    {
        public override Sprite GetSprite()
        {
            strength = 85f;
            hitWindows = false;
            return BasePlugin.AssetMan.Get<Sprite>("GrapplingIcon_Entity");
        }
    }
}
