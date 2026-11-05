using BaldisBasicsPlusAdvanced.API;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using System.IO;

namespace EvilSideItems
{
    [ConditionalPatchMod("mrsasha5.baldi.basics.plus.advanced")]
    public class AdvancedSupport
    {
        public static void AddRecipes()
        {
            ApiManager.LoadKitchenStoveRecipesFromFolder(BasePlugin.Instance.Info, Path.Combine(AssetLoader.GetModPath(BasePlugin.Instance), "Compats", "Advanced", "Recipes"), false);
        }
    }
}
