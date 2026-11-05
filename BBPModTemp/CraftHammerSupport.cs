using CraftHammer;
using MTM101BaldAPI;
using MTM101BaldAPI.Registers;

namespace EvilSideItems
{
    public class CraftHammerSupport
    {
        [ConditionalPatchMod("com.crafthammer.item")]
        public static void AddSupport()
        {
            ItemObject BAGet(string key)
            {
                return BasePlugin.AssetMan.Get<ItemObject>(key);
            }
            ItemObject MRIIFBE(Items items)
            {
                return ItemMetaStorage.Instance.FindByEnum(items).value;
            }

            CraftHammerApi.AddRecipe(
                MRIIFBE(Items.GrapplingHook),
                new ItemObject[2] { BAGet("Grappling"), BAGet("Hook") }
                );

            CraftHammerApi.AddRecipe(
                BAGet("Esoda"),
                new ItemObject[2] { MRIIFBE(Items.DietBsoda), MRIIFBE(Items.PrincipalWhistle) }
                );

            CraftHammerApi.AddRecipe(
                BAGet("EvilQuarter"),
                new ItemObject[2] { MRIIFBE(Items.Quarter), MRIIFBE(Items.PrincipalWhistle) }
                );

            CraftHammerApi.AddRecipe(
                BAGet("EvilZestyBar"),
                new ItemObject[2] { MRIIFBE(Items.ZestyBar), MRIIFBE(Items.PrincipalWhistle) }
                );

            CraftHammerApi.AddRecipe(
                BAGet("SuperDirtyChalkEraser"),
                new ItemObject[2] { MRIIFBE(Items.ChalkEraser), MRIIFBE(Items.ChalkEraser) }
                );

            CraftHammerApi.AddRecipe(
                BAGet("BananaForStudent"),
                new ItemObject[2] { MRIIFBE(Items.ZestyBar), MRIIFBE(Items.ZestyBar) }
                );

            CraftHammerApi.AddRecipe(
                BAGet("BananaForStudent"),
                new ItemObject[2] { MRIIFBE(Items.ZestyBar), MRIIFBE(Items.NanaPeel) }
                );

            CraftHammerApi.AddRecipe(
                MRIIFBE(Items.NanaPeel),
                new ItemObject[1] { BAGet("BananaForStudent") }
                );

            CraftHammerApi.AddRecipe(
                BAGet("Bullytag"),
                new ItemObject[2] { MRIIFBE(Items.Nametag), MRIIFBE(Items.PrincipalWhistle) }
                );

            CraftHammerApi.AddRecipe(
                BAGet("EvilKey"),
                new ItemObject[2] { MRIIFBE(Items.DetentionKey), MRIIFBE(Items.DetentionKey) }
                );

            CraftHammerApi.AddRecipe(
                MRIIFBE(Items.DetentionKey),
                new ItemObject[1] { BAGet("EvilKey") }
                );
        }
    }
}
