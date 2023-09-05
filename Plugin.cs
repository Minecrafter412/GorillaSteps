using BepInEx;
using GorillaTagModTemplateProject.Patches;
using GorillaTagModTemplateProject.Scripts;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


namespace GorillaTagModTemplateProject
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public Text steps = null;
        private bool ModInitialized;
        public bool lastPressed;
        public bool buttonPressed;
        public bool lastHeldDown;
        public GameObject StepsWatchObject = null;


        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
        void Start()
        /* A lot of Gorilla Tag systems will not be set up when start is called /*
		/* Put code in OnGameInitialized to avoid null references */
        {

            Utilla.Events.GameInitialized += OnGameInitialized;
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnEnable()
        {
            if (ModInitialized)
            {
                StepsWatchObject.SetActive(true);
            }
        }

        void OnDisable()
        {
            if (ModInitialized)
            {
                StepsWatchObject.SetActive(false);
            }
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
            // create a canvas parented to the main camera
            // don forgor semicolons!!1!11!!!!!111!111!!
            PlayerData playerData = DataSystem.GetPlayerData();
            HandTapPatch.stepsCount = playerData.steps;

            //-------------------------------------------------
            // The AssetBundle
            var bundle = LoadAssetBundle("GorillaSteps.Resources.stepboardassetbundle");
            Logger.LogMessage(bundle.GetAllAssetNames());
            var asset = Instantiate(bundle.LoadAsset<GameObject>("stepsboardwatch"));
            asset.transform.SetParent(GorillaTagger.Instance.rightHandTransform, false);
            asset.transform.localPosition = new Vector3(-0.0121f, -0.0667f, -0.1265f);
            asset.transform.localScale = new Vector3(0.0161f, 0.0161f, 0.0161f);
            asset.transform.localRotation = Quaternion.Euler(61.0003f, 0f, 9f);
            asset.AddComponent<MeshRenderer>();
            asset.GetComponent<MeshRenderer>().enabled = true;

            /*-------------------------------------------------*/
            /* Canvas Holder so it loads the text. */
            GameObject StepsWatchObject = new GameObject("StepsWatchCanvasHolder");
            StepsWatchObject.transform.SetParent(GorillaTagger.Instance.rightHandTransform, false);
            StepsWatchObject.transform.localPosition = new Vector3(-0.0415f, 0f, -0.2f);
            StepsWatchObject.transform.localScale = new Vector3(0.0026f, 0.0026f, 0.0026f);
            StepsWatchObject.transform.localRotation = Quaternion.Euler(0f, 90f, 62.0924f);

            StepsWatchObject.AddComponent<Canvas>();

            /*-------------------------------------------------*/
            /* Loads the Text. */
            GameObject StepsTextObject = new GameObject("StepsText");
            StepsTextObject.transform.SetParent(StepsWatchObject.transform, false);
            StepsTextObject.AddComponent<CanvasRenderer>();

            steps = StepsTextObject.AddComponent<Text>();
            steps.font = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/NameTagAnchor/NameTagCanvas/Text").GetComponent<Text>().font;


            ModInitialized = true;

        }
        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed);
            if (ModInitialized)
            {
                steps.text = $"      {HandTapPatch.stepsCount}";
            }

            if (ModInitialized && buttonPressed && StepsWatchObject.activeSelf && !lastHeldDown)
            {
                StepsWatchObject.SetActive(false);
            }

            else if (ModInitialized && buttonPressed && !StepsWatchObject.activeSelf && !lastHeldDown)
            {
                StepsWatchObject.SetActive(false);
            }

            lastHeldDown = buttonPressed;
        }
    }
}
