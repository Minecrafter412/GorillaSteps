using System;
using BepInEx;
using GorillaTagModTemplateProject.Patches;
using UnityEngine;
using UnityEngine.UI;
using Utilla;

namespace GorillaTagModTemplateProject
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	/* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		bool inRoom;
		public GameObject StepsCounterObject = null;
		public Text steps = null;

		void Start()
		{
			/* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

			Utilla.Events.GameInitialized += OnGameInitialized;
            HarmonyPatches.ApplyHarmonyPatches();
        }

		void OnEnable()
		{
			/* Set up your mod here */
			/* Code here runs at the start and whenever your mod is enabled*/
		}

		void OnDisable()
		{
			/* Undo mod setup here */
			/* This provides support for toggling mods with ComputerInterface, please implement it :) */
			/* Code here runs whenever your mod is disabled (including if it disabled on startup)*/
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			/* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
			// create a canvas parented to the main camera
			// don forgor semicolons!!1!11!!!!!111!111!!
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
        }

		void Update()
		{
            /* Code here runs every frame when the mod is enabled */
            steps.text = $"Steps: {HandTapPatch.stepsCount}";
        }

		/* This attribute tells Utilla to call this method when a modded room is joined */
		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			/* Activate your mod here */
			/* This code will run regardless of if the mod is enabled*/

			inRoom = true;
		}

		/* This attribute tells Utilla to call this method when a modded room is left */
		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			/* Deactivate your mod here */
			/* This code will run regardless of if the mod is enabled*/

			inRoom = false;
		}
	}
}
