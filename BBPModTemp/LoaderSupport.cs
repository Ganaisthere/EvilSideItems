using MTM101BaldAPI;
using PlusStudioLevelLoader;

namespace EvilSideItems
{
    [ConditionalPatchMod("mtm101.rulerp.baldiplus.levelstudioloader")]
    public class LoaderSupport
    {
        internal static void AddSupport()
        {
            foreach (string key in BasePlugin.AddedItemKey)
            {
                LevelLoaderPlugin.Instance.itemObjects.Add(key, BasePlugin.AssetMan.Get<ItemObject>(key));
            }
            foreach (PosterObject poster in BasePlugin.AddedPosters)
            {
                LevelLoaderPlugin.Instance.posterAliases.Add(poster.name, poster);
            }
        }
    }
}
