using MTM101BaldAPI;
using PlusLevelStudio;
using PlusLevelStudio.Editor;
using PlusLevelStudio.Editor.Tools;

namespace EvilSideItems
{
    [ConditionalPatchMod("mtm101.rulerp.baldiplus.levelstudio")]
    public class StudioSupport
    {
        internal static void AddSupport()
        {
            EditorInterfaceModes.AddModeCallback(Add);
        }

        internal static void Add(EditorMode mode, bool vanillaCompliant)
        {
            void AddItemObject(string item)
            {
                EditorInterfaceModes.AddToolToCategory(mode, "items", new ItemTool(item, BasePlugin.AssetMan.Get<ItemObject>(item).itemSpriteSmall, true));
            }
            foreach (string key in BasePlugin.AddedItemKey)
            {
                AddItemObject(key);
            }
            void AddPoster(PosterObject poster)
            {
                EditorInterfaceModes.AddToolToCategory(mode, "posters", new PosterTool(poster.name));
            }
            foreach (PosterObject poster in BasePlugin.AddedPosters)
            {
                AddPoster(poster);
            }
        }
    }
}
