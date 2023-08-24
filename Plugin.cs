using System;
using BepInEx;
using GorillaTagModTemplateProject.Scripts;
using GorillaTagModTemplateProject.Patches;
using UnityEngine;
using UnityEngine.UI;

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
		public GameObject StepsCounterObject = null;
		public Text steps = null;
		private bool ModInitialized;


        void Start()
		{
			/* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

			Utilla.Events.GameInitialized += OnGameInitialized;
            HarmonyPatches.ApplyHarmonyPatches();
        }

		void OnEnable()
		{
            if (ModInitialized)
			{
				StepsCounterObject.SetActive(true);
			}
		}

		void OnDisable()
		{
            if (ModInitialized)
			{
				StepsCounterObject.SetActive(false);
			}
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			/* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
			// create a canvas parented to the main camera
			// don forgor semicolons!!1!11!!!!!111!111!!
			PlayerData playerData = DataSystem.GetPlayerData();
			HandTapPatch.stepsCount = playerData.steps;

			StepsCounterObject = new GameObject("StepsCounterObject");
            StepsCounterObject.transform.SetParent(GorillaTagger.Instance.mainCamera.transform, false);
            StepsCounterObject.transform.localPosition = new Vector3(0.1f, -0.05f, 0.9f);
            StepsCounterObject.transform.localScale = new Vector3(0.0075f, 0.0075f, 0.0075f);
            StepsCounterObject.AddComponent<Canvas>();

			GameObject StepsTextObject = new GameObject("StepsText");
            StepsTextObject.transform.SetParent(StepsCounterObject.transform, false);
            StepsTextObject.AddComponent<CanvasRenderer>();

            steps = StepsTextObject.AddComponent<Text>();
            steps.font = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/NameTagAnchor/NameTagCanvas/Text").GetComponent<Text>().font;

			ModInitialized = true;
        }

		void Update()
		{
            /* Code here runs every frame when the mod is enabled */
			if (ModInitialized)
			{
                steps.text = $"Steps: {HandTapPatch.stepsCount}";
            }
        }
	}
}
