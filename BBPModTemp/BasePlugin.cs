using BepInEx;
using BepInEx.Configuration;
using EvilSideItems.CustomItems;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static BepInEx.BepInDependency;

namespace EvilSideItems
{
    [BepInPlugin("ganaisthere.plus.evilsideitems", "Evil Side Items", "0.1.0.0")]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi")]

    [BepInDependency("mtm101.rulerp.baldiplus.levelstudioloader", DependencyFlags.SoftDependency)]
    [BepInDependency("mtm101.rulerp.baldiplus.levelstudio", DependencyFlags.SoftDependency)]
    [BepInDependency("mrsasha5.baldi.basics.plus.advanced", DependencyFlags.SoftDependency)]
    [BepInDependency("com.crafthammer.item", DependencyFlags.SoftDependency)]

    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; internal set; }
        public static Harmony harmony = new Harmony("ganaisthere.plus.evilsideitems");
        public static AssetManager AssetMan = new AssetManager();
        public static List<string> AddedItemKey = new List<string>();

        public static List<PosterObject> AddedPosters = new List<PosterObject>();

        //----------------------------------------

        public ConfigEntry<bool> ConfigEasyMode;

        public void Awake()
        {
            /*ConfigEasyMode = Config.Bind
            (
                "General",
                "Easy Mode",
                false,
                "If true, Mrs Pomp and Gotta Sweep can not kill you in F3."
            );*/

            Instance = this;
            harmony.PatchAllConditionals();

            AddLocalization("Subtitles_English.json");

            ModdedSaveGame.AddSaveHandler(Info);

            LoadingEvents.RegisterOnAssetsLoaded(Info, LoadAssets(), LoadingEventOrder.Pre);

            GeneratorManagement.Register(this, GenerationModType.Addend, ChangeGenerator);
        }

        internal IEnumerator LoadAssets()
        {
            yield return 1225;

            AddedItemKey.Clear();
            string large;
            string small;

            void OneStepToAddIcon(string name, float smallPixelsPerUnit = 25f, float largePixelsPerUnit = 50f)
            {
                large = name + "Icon_Large";
                small = name + "Icon_Small";
                AddTexture2DAndToSprite(large + ".png", "Textures/Items", largePixelsPerUnit);
                AddTexture2DAndToSprite(small + ".png", "Textures/Items", smallPixelsPerUnit);
            }

            string postersPath = Path.Combine(AssetLoader.GetModPath(this), "Textures", "Posters");
            if (Directory.Exists(postersPath))
            {
                AddedPosters.Clear();
                foreach (string posterFile in Directory.GetFiles(postersPath, "*.png", SearchOption.TopDirectoryOnly))
                {
                    yield return "Building Posters...";
                    PosterObject poster = ObjectCreators.CreatePosterObject(AssetLoader.TextureFromFile(posterFile), new PosterTextData[0] { });
                    AssetMan.Add(Path.GetFileNameWithoutExtension(posterFile), poster);
                    AddedPosters.Add(poster);
                }
            }

            yield return "Building Item - EvilQuarter";
            OneStepToAddIcon("EvilQuarter");
            ItemObject evilQuarter = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_EvilQuarter", "Desc_EvilQuarter")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("EvilQuarter")
                .SetShopPrice(300)
                .SetGeneratorCost(50)
                .SetItemComponent<ITM_EvilQuarter>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "EvilQuarter",
                        "RMS_Trading_Common",
                        "quarter",
                        "crmp_iou_sticker_nooverride",
                        "crmp_iou_sticker_nodisguise"
                    })
                .Build();
            AssetMan.Add("EvilQuarter", evilQuarter);
            AddedItemKey.Add("EvilQuarter");

            yield return "Building Item - Esoda";
            OneStepToAddIcon("Esoda");
            AddTexture2DAndToSprite("EsodaSpray_Sprite_Transparent.png", "Textures/Entity", 20f);
            ItemObject eSODA = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_Esoda", "Desc_Esoda")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("Esoda")
                .SetShopPrice(325)
                .SetGeneratorCost(60)
                .SetItemComponent<ITM_Esoda>()
                .SetMeta(
                    ItemFlags.CreatesEntity,
                    new string[]
                    {
                        "ESODA",
                        "RMS_Trading_Common",
                        "food"
                    })
                .Build();
            AssetMan.Add("Esoda", eSODA);
            AddedItemKey.Add("Esoda");

            yield return "Building Item - EvilZestyBar";
            OneStepToAddIcon("EvilZestyBar");
            AddTexture2DAndToSprite("EvilZestyBarIcon_Gauge.png", "Textures/GUI", 20f);
            ItemObject evilZestyBar = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_EvilZestyBar", "Desc_EvilZestyBar")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("EvilZestyBar")
                .SetShopPrice(400)
                .SetGeneratorCost(60)
                .SetItemComponent<ITM_EvilZestyBar>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "EvilZestyBar",
                        "RMS_Trading_Common",
                        "food"
                    })
                .Build();
            AssetMan.Add("EvilZestyBar", evilZestyBar);
            AddedItemKey.Add("EvilZestyBar");

            yield return "Building Item - Grappling";
            OneStepToAddIcon("Grappling");
            AddTexture2DAndToSprite("GrapplingIcon_Entity.png", "Textures/Entity", 20f);
            ItemObject Grappling = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_Grappling", "Desc_Grappling")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("Grappling")
                .SetShopPrice(300)
                .SetGeneratorCost(55)
                .SetItemComponent<ITM_Grappling>()
                .SetMeta(
                    ItemFlags.CreatesEntity,
                    new string[]
                    {
                        "Grappling",
                        "RMS_Trading_Common"
                    })
                .Build();
            AssetMan.Add("Grappling", Grappling);
            AddedItemKey.Add("Grappling");

            yield return "Building Item - Hook";
            OneStepToAddIcon("Hook");
            AddTexture2DAndToSprite("HookIcon_Entity.png", "Textures/Entity", 20f);
            ItemObject hook = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_Hook", "Desc_Hook")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("Hook")
                .SetShopPrice(400)
                .SetGeneratorCost(70)
                .SetItemComponent<ITM_Hook>()
                .SetMeta(
                    ItemFlags.CreatesEntity,
                    new string[]
                    {
                        "Hook",
                        "RMS_Trading_Common"
                    })
                .Build();
            AssetMan.Add("Hook", hook);
            AddedItemKey.Add("Hook");

            yield return "Building Item - A Banana for Student";
            OneStepToAddIcon("Banana");
            AddSoundObject("Student_EatBanana.ogg", "SoundObjects", "Vfx_Student_EatBanana", SoundType.Voice, Color.white, -1f);
            ItemObject banana = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_BananaForStudent", "Desc_BananaForStudent")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("BananaForStudent")
                .SetShopPrice(300)
                .SetGeneratorCost(60)
                .SetItemComponent<ITM_BananaForStudent>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "BananaForStudent",
                        "RMS_Trading_Uncommon",
                        "food"
                    })
                .Build();
            AssetMan.Add("BananaForStudent", banana);
            AddedItemKey.Add("BananaForStudent");

            yield return "Building Item - Evil Key";
            OneStepToAddIcon("EvilKey");
            AddTexture2DAndToSprite("EvilKeyIcon_Gauge.png", "Textures/GUI", 20f);
            ItemObject evilKey = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_EvilKey", "Desc_EvilKey")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("EvilKey")
                .SetShopPrice(350)
                .SetGeneratorCost(60)
                .SetItemComponent<ITM_EvilKey>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "EvilKey",
                        "RMS_Trading_Uncommon",
                        "key"
                    })
                .Build();
            AssetMan.Add("EvilKey", evilKey);
            AddedItemKey.Add("EvilKey");

            yield return "Building Item - Bully Nametag";
            OneStepToAddIcon("Bullytag");
            AddTexture2DAndToSprite("BullytagIcon_Gauge.png", "Textures/GUI", 20f);
            ItemObject bullyTag = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_Bullytag", "Desc_Bullytag")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("Bullytag")
                .SetShopPrice(425)
                .SetGeneratorCost(60)
                .SetItemComponent<ITM_Bullytag>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "Bullytag",
                        "RMS_Trading_Uncommon",
                        "nametag"
                    })
                .Build();
            AssetMan.Add("Bullytag", bullyTag);
            AddedItemKey.Add("Bullytag");

            yield return "Building Item - Evil Notebook";
            OneStepToAddIcon("EvilNotebook", 25f, 100f);
            ItemObject evilNotebook = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_EvilNotebook", "Desc_EvilNotebook")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("EvilNotebook")
                .SetShopPrice(1225)
                .SetGeneratorCost(60)
                .SetAsInstantUse()
                .SetItemComponent<ITM_EvilNotebook>()
                .SetPickupSound(AssetFinder.FindOfTypeWithName<SoundObject>("NotebookCollect", true))
                .SetMeta(
                    ItemFlags.InstantUse,
                    new string[]
                    {
                        "EvilNotebook",
                        "RMS_Trading_Rare",
                        "notebook"
                    })
                .Build();
            AssetMan.Add("EvilNotebook", evilNotebook);
            AddedItemKey.Add("EvilNotebook");

            yield return "Building Item - Broken Map";
            OneStepToAddIcon("BrokenMap", 25f, 50f);
            ItemObject brokenMap = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_BrokenMap", "Desc_BrokenMap")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("BrokenMap")
                .SetShopPrice(1225)
                .SetGeneratorCost(70)
                .SetAsInstantUse()
                .SetItemComponent<ITM_BrokenMap>()
                .SetPickupSound(AssetFinder.FindOfTypeWithName<SoundObject>("BAL_Break", true))
                .SetMeta(
                    ItemFlags.InstantUse,
                    new string[]
                    {
                        "BrokenMap",
                        "RMS_Trading_Rare"
                    })
                .Build();
            AssetMan.Add("BrokenMap", brokenMap);
            AddedItemKey.Add("BrokenMap");

            yield return "Building Item - Super Dirty Chalk Eraser";
            OneStepToAddIcon("SuperDirtyChalkEraser", 25f, 50f);
            AddTexture2DAndToSprite("SuperDirtyChalkEraserIcon_Gauge.png", "Textures/GUI", 20f);
            ItemObject superDirtyChalkEraser = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_SuperDirtyChalkEraser", "Desc_SuperDirtyChalkEraser")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("SuperDirtyChalkEraser")
                .SetShopPrice(550)
                .SetGeneratorCost(80)
                .SetItemComponent<ITM_SuperDirtyChalkEraser>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "SuperDirtyChalkEraser",
                        "RMS_Trading_Rare"
                    })
                .Build();
            AssetMan.Add("SuperDirtyChalkEraser", superDirtyChalkEraser);
            AddedItemKey.Add("SuperDirtyChalkEraser");

            yield return "Building Item - Unsafety Baseball Bat";
            OneStepToAddIcon("UnsafetyBaseballBat", 25f, 50f);
            AddTexture2DAndToSprite("Sweep.png", "Textures/GUI", 20f);
            ItemObject unsafetyBaseballBat = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_UnsafetyBaseballBat", "Desc_UnsafetyBaseballBat")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("UnsafetyBaseballBat")
                .SetShopPrice(650)
                .SetGeneratorCost(85)
                .SetItemComponent<ITM_UnsafetyBaseballBat>()
                .SetMeta(
                    ItemFlags.None,
                    new string[]
                    {
                        "UnsafetyBaseballBat",
                        "RMS_Trading_Rare"
                    })
                .Build();
            AssetMan.Add("UnsafetyBaseballBat", unsafetyBaseballBat);
            AddedItemKey.Add("UnsafetyBaseballBat");

            yield return "Building Item - Evil YTP";
            OneStepToAddIcon("EvilYTP", 25f, 50f);
            ItemObject evilYTP = new ItemBuilder(Info)
                .SetNameAndDescription("Itm_EvilYTP", "Desc_EvilYTP")
                .SetSprites(AssetMan.Get<Sprite>(small), AssetMan.Get<Sprite>(large))
                .SetEnum("EvilYTP")
                .SetShopPrice(1225)
                .SetGeneratorCost(5)
                .SetAsInstantUse()
                .SetItemComponent<ITM_EvilYTP>()
                .SetPickupSound(AssetFinder.FindOfTypeWithName<SoundObject>("WaterSlurp", true))
                .SetMeta(
                    ItemFlags.InstantUse,
                    new string[]
                    {
                        "EvilYTP",
                        "RMS_Trading_Common"
                    })
                .Build();
            AssetMan.Add("EvilYTP", evilYTP);
            AddedItemKey.Add("EvilYTP");

            if (Other.CLC("mrsasha5.baldi.basics.plus.advanced"))
            {
                yield return "Adding Advanced Edition Support...";
                AdvancedSupport.AddRecipes();
                postersPath = Path.Combine(AssetLoader.GetModPath(this), "Compats", "Advanced", "Textures", "Posters");
                if (Directory.Exists(postersPath))
                {
                    AddedPosters.Clear();
                    foreach (string posterFile in Directory.GetFiles(postersPath, "*.png", SearchOption.TopDirectoryOnly))
                    {
                        yield return "Building Posters...";
                        PosterObject poster = ObjectCreators.CreatePosterObject(AssetLoader.TextureFromFile(posterFile), new PosterTextData[0] { });
                        AssetMan.Add(Path.GetFileNameWithoutExtension(posterFile), poster);
                        AddedPosters.Add(poster);
                    }
                }
            }
            if (Other.CLC("com.crafthammer.item"))
            {
                yield return "Adding Craft Hammer Support...";
                CraftHammerSupport.AddSupport();
            }
            if (Other.CLC("mtm101.rulerp.baldiplus.levelstudioloader"))
            {
                yield return "Adding Level Loader Support...";
                LoaderSupport.AddSupport();
            }
            if (Other.CLC("mtm101.rulerp.baldiplus.levelstudio"))
            {
                yield return "Adding Level Studio Support...";
                StudioSupport.AddSupport();
            }

            yield break;
        }

        internal void ChangeGenerator(string floorName, int floorNumber, SceneObject sceneObject)
        {
            void AddItem(CustomLevelObject clo, string selection, int spawnWeight, int shopWeight = -1)
            {
                ItemObject item = AssetMan.Get<ItemObject>(selection);
                if (item != null)
                {
                    clo.potentialItems = clo.potentialItems.AddToArray(
                            new WeightedItemObject()
                            {
                                selection = item,
                                weight = spawnWeight
                            }
                        );
                    if (shopWeight != 1225)
                    {
                        int realshopWeight = shopWeight;
                        if (shopWeight < 0)
                        {
                            realshopWeight = spawnWeight;
                        }
                        sceneObject.shopItems = sceneObject.shopItems.AddToArray(
                            new WeightedItemObject()
                            {
                                selection = item,
                                weight = realshopWeight
                            }
                        );
                    }
                }
            }
            void AddForcedItem(CustomLevelObject clo, string selection)
            {
                ItemObject item = AssetMan.Get<ItemObject>(selection);
                if (item != null)
                {
                    clo.forcedItems.Add(item);
                }
            }
            void AddPoster(CustomLevelObject clo, PosterObject poster, int spawnWeight = 80)
            {
                if (poster != null)
                {
                    clo.posters = clo.posters.AddToArray(new WeightedPosterObject(){selection = poster, weight = spawnWeight});
                }
            }

            if (floorName.StartsWith("F"))
            {
                CustomLevelObject[] customLevelObjects = CustomLevelObjectExtensions.GetCustomLevelObjects(sceneObject);
                if (floorNumber < 2)
                {
                    foreach (CustomLevelObject customLevelObject in customLevelObjects)
                    {
                        AddItem(customLevelObject, "EvilQuarter", 90);
                        AddItem(customLevelObject, "Esoda", 85);
                        AddItem(customLevelObject, "EvilZestyBar", 80);
                        AddItem(customLevelObject, "EvilKey", 75);
                        AddItem(customLevelObject, "SuperDirtyChalkEraser", 65, 70);
                        AddItem(customLevelObject, "UnsafetyBaseballBat", 50, 55);
                    }
                }
                else
                {
                    foreach (CustomLevelObject customLevelObject in customLevelObjects)
                    {
                        AddItem(customLevelObject, "EvilQuarter", 80, 85);
                        AddItem(customLevelObject, "Esoda", 77);
                        AddItem(customLevelObject, "EvilZestyBar", 75, 80);
                        AddItem(customLevelObject, "EvilKey", 70);
                        AddItem(customLevelObject, "SuperDirtyChalkEraser", 65, 70);
                        AddItem(customLevelObject, "UnsafetyBaseballBat", 60, 65);
                    }
                }
                foreach (CustomLevelObject customLevelObject in customLevelObjects)
                {
                    foreach (PosterObject poster in AddedPosters)
                    {
                        AddPoster(customLevelObject, poster);
                    }
                    AddItem(customLevelObject, "Grappling", 90, 75);
                    AddItem(customLevelObject, "Hook", 80);
                    AddItem(customLevelObject, "BananaForStudent", 70);
                    AddItem(customLevelObject, "Bullytag", 75, 70);
                    AddItem(customLevelObject, "EvilNotebook", 65, 1225);
                    AddItem(customLevelObject, "EvilYTP", 200, 1225);
                    AddForcedItem(customLevelObject, "BrokenMap");
                }
            }
        }

        internal void AddTexture2DAndToSprite(string fileNameWithExtension, string chlidPath = null, float pixelsPerUnit = 50f)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                Texture2D texture2D = AssetLoader.TextureFromFile(filePath);
                Sprite sprite = AssetLoader.SpriteFromTexture2D(texture2D, pixelsPerUnit);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), texture2D);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), sprite);
            }
            else
            {
                Log("File Not Exists: " + filePath);
            }
        }
        internal void AddAudioClip(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AudioClip audioClip = AssetLoader.AudioClipFromFile(filePath);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), audioClip);
            }
            else
            {
                Log("File Not Exists: " + filePath);
            }
        }
        internal void AddSoundObject(string fileNameWithExtension, string chlidPath = null, string subtitle = "Nothing", SoundType soundType = SoundType.Voice, Color color = default, float subtitletime = 0f)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AudioClip audioClip = AssetLoader.AudioClipFromFile(filePath);
                SoundObject soundObject = ObjectCreators.CreateSoundObject(audioClip, subtitle, soundType, color, subtitletime);
                soundObject.name = Path.GetFileNameWithoutExtension(filePath);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), soundObject);
            }
            else
            {
                Log("File Not Exists: " + filePath);
            }
        }
        internal void AddMidi(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AssetLoader.MidiFromFile(filePath, Path.GetFileNameWithoutExtension(filePath));
            }
            else
            {
                Log("File Not Exists: " + filePath);
            }
        }
        internal void AddLocalization(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AssetLoader.LocalizationFromFile(filePath, Language.English);
            }
            else
            {
                Log("File Not Exists: " + filePath);
            }
        }

        internal static void Log(object data, int level = 0)
        {
            if (level == 1)
            {
                Instance.Logger.LogWarning(data);
            }
            else if (level == 2)
            {
                Instance.Logger.LogError(data);
            }
            else
            {
                Instance.Logger.LogInfo(data);
            }
        }
    }
}
