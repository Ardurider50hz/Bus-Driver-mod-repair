using System;
using System.Linq;
using BUS.Properties;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MSCLoader;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000002 RID: 2
	public class BUS : Mod
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override string ID
		{
			get
			{
				return "BUSRemaked";
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public override string Name
		{
			get
			{
				return "Drivable Bus";
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205E File Offset: 0x0000025E
		public override string Author
		{
			get
			{
				return "Bosorkanamettle";
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002065 File Offset: 0x00000265
		public override string Version
		{
			get
			{
				return "1.06";
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206C File Offset: 0x0000026C
		public override string Description
		{
			get
			{
				return "Thanks to BrennFuchS for gear indicator!";
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002073 File Offset: 0x00000273
		public override byte[] Icon
		{
			get
			{
				return Resources.Drivable_bus;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000207A File Offset: 0x0000027A
		public override void ModSetup()
		{
			base.SetupFunction(3, new Action(this.Mod_OnLoad));
			base.SetupFunction(7, new Action(this.Mod_OnUpdate));
			base.SetupFunction(5, new Action(this.Mod_OnSave));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020B9 File Offset: 0x000002B9
		public override void ModSettings()
		{
			Settings.AddCheckBox(this, BUS.mirrors);
			Settings.AddCheckBox(this, BUS.CZTexts);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020D4 File Offset: 0x000002D4
		private void Mod_OnLoad()
		{
			AssetBundle assetBundle = AssetBundle.CreateFromMemoryImmediate(Resources.bus);
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("bus_parts.prefab");
			Texture2D mainTexture = assetBundle.LoadAsset<Texture2D>("ATLAS_HOME.png");
			Texture2D mainTexture2 = assetBundle.LoadAsset<Texture2D>("fabric7_curtains.png");
			Texture2D mainTexture3 = assetBundle.LoadAsset<Texture2D>("ryijy3.png");
			Texture2D mainTexture4 = assetBundle.LoadAsset<Texture2D>("bus_interior.png");
			this.lightsoff = assetBundle.LoadAsset<Material>("CarLightsOff.mat");
			this.lightson = assetBundle.LoadAsset<Material>("CarLightsOn.mat");
			bool flag = ModLoader.IsModPresent("Better_Graphics");
			if (flag)
			{
				this.lightson.SetFloat("_Intensity", 6.5f);
				this.lightson.SetColor("_Color", new Color(0.9705882f, 0.8865231f, 0.5352508f));
			}
			BUS.bus = Object.Instantiate<GameObject>(GameObject.Find("BUS").gameObject);
			this.saveData = SaveUtility.Load<SaveData>();
			BUS.bus.name = "BUS(11500kg, 580)";
			BUS.bus.transform.localPosition = this.saveData.buspos;
			BUS.bus.transform.localEulerAngles = this.saveData.busrot;
			BUS.bus.transform.Find("LOD").gameObject.SetActive(true);
			BUS.busparts = Object.Instantiate<GameObject>(gameObject);
			BUS.busparts.transform.SetParent(BUS.bus.transform);
			BUS.busparts.transform.localPosition = new Vector3(0f, -0.398f, 0f);
			BUS.busparts.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
			Object.Destroy(BUS.bus.GetComponent<MobileCarController>());
			foreach (PlayMakerFSM playMakerFSM in BUS.bus.GetComponents<PlayMakerFSM>())
			{
				Object.Destroy(playMakerFSM);
			}
			Object.Destroy(BUS.bus.transform.Find("LOD/Steering/TruckSteeringPivot").GetComponent<PlayMakerFSM>());
			this.playerCurrentVehicle = PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle");
			this.acc = BUS.bus.AddComponent<AxisCarController>();
			BUS.raycast = BUS.bus.AddComponent<Raycast>();
			this.PlayerInCar = false;
			GameObject gameObject2 = Object.Instantiate<GameObject>(GameObject.Find("NPC_CARS").transform.Find("Amikset/KYLAJANI").Find("LOD/PlayerFunctions").gameObject);
			gameObject2.SetActive(true);
			gameObject2.name = "PlayerFunctions";
			PlayMakerExtensions.InitializeFSM(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"));
			PlayMakerExtensions.GetAction<SetStringValue>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Press return"), 1).stringValue = "ENTER DRIVING MODE";
			PlayMakerExtensions.GetAction<SetBoolValue>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Press return"), 2).boolVariable = FsmVariables.GlobalVariables.FindFsmBool("GUIdrive");
			PlayMakerExtensions.GetAction<SetBoolValue>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Player in car"), 1).boolVariable = FsmVariables.GlobalVariables.FindFsmBool("GUIdrive");
			PlayMakerExtensions.GetAction<SetGameObject>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Player in car"), 4).gameObject.Value = BUS.bus;
			PlayMakerExtensions.GetAction<SetGameObject>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Player in car"), 4).variable = BUS.bus;
			PlayMakerExtensions.GetAction<SetBoolValue>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Wait for player"), 1).boolVariable = FsmVariables.GlobalVariables.FindFsmBool("GUIdrive");
			gameObject2.transform.Find("PlayerTrigger").GetComponent<BoxCollider>().center = new Vector3(0.5f, 0f, -4.5f);
			gameObject2.transform.Find("PlayerTrigger").GetComponent<BoxCollider>().size = new Vector3(2.5f, 2f, 11f);
			gameObject2.transform.SetParent(BUS.bus.transform);
			Object.Destroy(gameObject2.transform.Find("PlayerColliders").gameObject);
			gameObject2.transform.localPosition = new Vector3(-0.45f, 0.5f, 4.5f);
			gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			gameObject2.transform.Find("DriverHeadPivot").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("DeadBody").Value = BUS.bus.transform.Find("DeadBody").gameObject;
			gameObject2.transform.Find("PlayerTrigger/DriveTrigger").localPosition = new Vector3(0f, 0f, 0f);
			gameObject2.transform.Find("PlayerTrigger/DriveTrigger").localScale = new Vector3(5f, 5f, 5f);
			gameObject2.transform.Find("DriverHeadPivot").GetComponent<ConfigurableJoint>().connectedBody = BUS.bus.GetComponent<Rigidbody>();
			FsmStateAction fsmStateAction = PlayMakerExtensions.GetState(gameObject2.transform.Find("PlayerTrigger/DriveTrigger").gameObject.GetComponent<PlayMakerFSM>(), "Player in car").Actions[3];
			this.PlayerCheck = gameObject2.transform.Find("DriverHeadPivot/CameraPivot/Pivot").gameObject;
			(fsmStateAction as SetStringValue).stringValue.Value = "Bus";
			this.acc.gameObject.SetActive(false);
			PlayMakerExtensions.GetAction<GetVelocity>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Check speed"), 0).gameObject.GameObject = BUS.bus;
			BUS.bus.transform.Find("LOD/Passengers").gameObject.SetActive(false);
			Object.Destroy(BUS.bus.transform.Find("LOD/Driver").gameObject);
			Object.Destroy(BUS.bus.transform.Find("LOD/HeadTarget").gameObject);
			BUS.bus.transform.Find("BusInteractions").GetComponent<PlayMakerFSM>().enabled = false;
			SteeringWheel steeringWheel = BUS.bus.transform.Find("LOD/Steering/TruckSteeringPivot").gameObject.AddComponent<SteeringWheel>();
			steeringWheel.carController = this.acc;
			steeringWheel.rotateAroundY = true;
			BUS.bus.transform.Find("LOD/Gearstick/GearStickPivot/gearstick").GetComponent<MeshFilter>().mesh = assetBundle.LoadAsset<Mesh>("bus_gearstick.obj");
			BUS.bus.transform.Find("MESH/bus_interior").GetComponent<MeshFilter>().mesh = assetBundle.LoadAsset<Mesh>("bus_interior.obj");
			Object.Destroy(BUS.bus.transform.Find("CarLightsAI").GetComponent<PlayMakerFSM>());
			Object.Destroy(BUS.bus.transform.Find("NavigationAI").gameObject);
			Object.Destroy(BUS.bus.transform.Find("Route").gameObject);
			Object.Destroy(BUS.bus.transform.Find("Latanen").gameObject);
			bool flag2 = GameObject.Find("GIFU(750/450psi)").transform.Find("LOD/Dashboard/ButtonHorn") != null;
			if (flag2)
			{
				GameObject gameObject3 = Object.Instantiate<GameObject>(GameObject.Find("GIFU(750/450psi)").transform.Find("LOD/Dashboard/ButtonHorn").gameObject);
				gameObject3.name = "ButtonHorn";
				gameObject3.transform.SetParent(BUS.bus.transform.Find("LOD/Steering/TruckSteeringPivot"));
				gameObject3.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			GameObject gameObject4 = Object.Instantiate<Transform>(GameObject.Find("GIFU(750/450psi)").transform.Find("LOD/Dashboard/ButtonInteriorLight")).gameObject;
			gameObject4.transform.SetParent(BUS.bus.transform);
			gameObject4.name = "ButtonLights";
			gameObject4.transform.localPosition = new Vector3(-0.99f, 1.039f, 5.33f);
			gameObject4.AddComponent<LightSwitch>().beamsShort = BUS.bus.transform.Find("CarLightsAI/DrivingLights").gameObject;
			Object.Destroy(gameObject4.GetComponent<PlayMakerFSM>());
			gameObject4.GetComponent<LightSwitch>().LightMeshes = BUS.busparts.transform.Find("LightMeshes").gameObject;
			BUS.busparts.transform.Find("LightMeshes").transform.SetParent(BUS.bus.transform.Find("CarLightsAI"));
			BUS.busparts.transform.Find("ReverseLights").transform.SetParent(BUS.bus.transform.Find("CarLightsAI"));
			DashVolumeFSM dashVolumeFSM = BUS.bus.AddComponent<DashVolumeFSM>();
			dashVolumeFSM.ignition = new GameObject("Ignition");
			GameObject gameObject5 = Object.Instantiate<GameObject>(GameObject.Find("HAYOSIKO(1500kg, 250)").transform.Find("LOD/Dashboard/TurnSignals").gameObject);
			gameObject5.transform.SetParent(BUS.bus.transform.transform);
			PlayMakerFSM playMaker = PlayMakerExtensions.GetPlayMaker(gameObject5, "Usage");
			gameObject5.name = "TurnSignals";
			gameObject5.transform.localPosition = new Vector3(0f, 0f, 5f);
			gameObject5.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			playMaker.FsmVariables.GetFsmGameObject("LightsLeft").Value = BUS.busparts.transform.Find("indicator_blinker_left").gameObject;
			playMaker.FsmVariables.GetFsmGameObject("LightsRight").Value = BUS.busparts.transform.Find("indicator_blinker_right").gameObject;
			playMaker.FsmVariables.GetFsmFloat("Delay").Value = 0.45f;
			PlayMakerExtensions.GetAction<StringCompare>(PlayMakerExtensions.GetState(playMaker, "Check player"), 2).compareTo.Value = "Bus";
			PlayMakerExtensions.GetAction<StringCompare>(PlayMakerExtensions.GetState(playMaker, "State 2"), 2).compareTo.Value = "Bus";
			PlayMakerExtensions.GetAction<GetProperty>(PlayMakerExtensions.GetState(playMaker, "On"), 2).targetProperty.TargetObject = BUS.bus.GetComponent<AxisCarController>();
			PlayMakerExtensions.GetAction<ActivateGameObject>(PlayMakerExtensions.GetState(playMaker, "On"), 3).gameObject.GameObject = BUS.busparts.transform.Find("indicator_blinker_right").gameObject;
			PlayMakerExtensions.GetAction<GetProperty>(PlayMakerExtensions.GetState(playMaker, "Off"), 1).targetProperty.TargetObject = BUS.bus.GetComponent<AxisCarController>();
			PlayMakerExtensions.GetAction<ActivateGameObject>(PlayMakerExtensions.GetState(playMaker, "Off"), 2).gameObject.GameObject = BUS.busparts.transform.Find("indicator_blinker_right").gameObject;
			PlayMakerExtensions.GetAction<MasterAudioPlaySound>(PlayMakerExtensions.GetState(playMaker, "Activate dash"), 0).gameObject.GameObject = BUS.busparts.transform.Find("lever_Blinkers/Pivot/lever").gameObject;
			PlayMakerExtensions.GetAction<SetRotation>(PlayMakerExtensions.GetState(playMaker, "Activate dash"), 1).gameObject.GameObject = BUS.busparts.transform.Find("lever_Blinkers/Pivot/lever").gameObject;
			PlayMakerExtensions.GetAction<MasterAudioPlaySound>(PlayMakerExtensions.GetState(playMaker, "State 3"), 0).gameObject.GameObject = BUS.busparts.transform.Find("lever_Blinkers/Pivot/lever").gameObject;
			PlayMakerExtensions.GetAction<ActivateGameObject>(PlayMakerExtensions.GetState(playMaker, "State 3"), 1).gameObject.GameObject = BUS.busparts.transform.Find("indicator_blinker_right").gameObject;
			PlayMakerExtensions.GetAction<ActivateGameObject>(PlayMakerExtensions.GetState(playMaker, "State 3"), 2).gameObject.GameObject = BUS.busparts.transform.Find("indicator_blinker_left").gameObject;
			PlayMakerExtensions.GetAction<GetProperty>(PlayMakerExtensions.GetState(playMaker, "On 2"), 2).targetProperty.TargetObject = BUS.bus.GetComponent<AxisCarController>();
			PlayMakerExtensions.GetAction<ActivateGameObject>(PlayMakerExtensions.GetState(playMaker, "On 2"), 3).gameObject.GameObject = BUS.busparts.transform.Find("indicator_blinker_left").gameObject;
			PlayMakerExtensions.GetAction<GetProperty>(PlayMakerExtensions.GetState(playMaker, "Off 2"), 1).targetProperty.TargetObject = BUS.bus.GetComponent<AxisCarController>();
			PlayMakerExtensions.GetAction<ActivateGameObject>(PlayMakerExtensions.GetState(playMaker, "Off 2"), 2).gameObject.GameObject = BUS.busparts.transform.Find("indicator_blinker_left").gameObject;
			PlayMakerExtensions.GetAction<MasterAudioPlaySound>(PlayMakerExtensions.GetState(playMaker, "Activate dash 2"), 0).gameObject.GameObject = BUS.busparts.transform.Find("lever_Blinkers/Pivot/lever").gameObject;
			PlayMakerExtensions.GetAction<SetRotation>(PlayMakerExtensions.GetState(playMaker, "Activate dash 2"), 1).gameObject.GameObject = BUS.busparts.transform.Find("lever_Blinkers/Pivot/lever").gameObject;
			PlayMakerExtensions.GetAction<SetRotation>(PlayMakerExtensions.GetState(playMaker, "Check steering"), 0).gameObject.GameObject = BUS.busparts.transform.Find("lever_Blinkers/Pivot/lever").gameObject;
			PlayMakerExtensions.GetAction<GetFsmFloat>(PlayMakerExtensions.GetState(playMaker, "On"), 0).gameObject.GameObject = dashVolumeFSM.ignition;
			PlayMakerExtensions.GetAction<GetFsmFloat>(PlayMakerExtensions.GetState(playMaker, "On 2"), 0).gameObject.GameObject = dashVolumeFSM.ignition;
			BUS.busparts.transform.Find("mirror_left/BusMirrorLeft").GetComponent<Camera>().cullingMask = BUS.bus.transform.Find("BusInteractions/cam").GetComponent<Camera>().cullingMask;
			BUS.busparts.transform.Find("mirror_right/BusMirrorRight").GetComponent<Camera>().cullingMask = BUS.bus.transform.Find("BusInteractions/cam").GetComponent<Camera>().cullingMask;
			BUS.bus.transform.Find("BusInteractions/cam").gameObject.SetActive(false);
			bool flag3 = !(bool)BUS.mirrors.Value;
			if (flag3)
			{
				BUS.bus.transform.Find("BusInteractions/MIRROR").GetComponent<MeshRenderer>().sharedMaterial = BUS.bus.transform.Find("LOD/Gauges/Speedometer/needle").GetComponent<MeshRenderer>().sharedMaterial;
			}
			bool flag4 = (bool)BUS.mirrors.Value;
			if (flag4)
			{
				BUS.bus.AddComponent<Mirrors>();
			}
			BUS.busparts.transform.Find("mirror_left").gameObject.SetActive(false);
			BUS.busparts.transform.Find("mirror_right").gameObject.SetActive(false);
			BUS.bus.transform.Find("BusInteractions/cam").gameObject.SetActive(false);
			BUS.bus.transform.Find("BusInteractions/cam").transform.localPosition = new Vector3(0.9539642f, 1.966231f, 4.992018f);
			BUS.bus.transform.Find("BusInteractions/cam").transform.localEulerAngles = new Vector3(9.878098f, 206.5144f, 21.07451f);
			Seatbelt seatbelt = BUS.busparts.transform.Find("SeatbeltDriver").gameObject.AddComponent<Seatbelt>();
			seatbelt.fastened = BUS.busparts.transform.Find("SeatbeltDriver/Fastened_mesh").gameObject;
			seatbelt.fastenCollider = BUS.busparts.transform.Find("SeatbeltDriver/SeatbeltLock").GetComponent<Collider>();
			seatbelt.handleParent = BUS.busparts.transform.Find("SeatbeltDriver/HandleUpPivot");
			seatbelt.handle = BUS.busparts.transform.Find("SeatbeltDriver/HandleUpPivot/SeatbeltHandle");
			seatbelt.handleCollider = BUS.busparts.transform.Find("SeatbeltDriver/HandleUpPivot/SeatbeltHandle").GetComponent<Collider>();
			seatbelt.interaction = BUS.raycast;
			PlayMakerExtensions.AddAction(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Check speed"), new CheckSeatbeltBool
			{
				seatbeltScript = seatbelt,
				everyFrame = false
			});
			PlayMakerExtensions.AddAction(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject2.transform.Find("PlayerTrigger/DriveTrigger"), "PlayerTrigger"), "Player in car"), new SetRotationZ
			{
				gameObject = new FsmOwnerDefault
				{
					OwnerOption = 1,
					GameObject = new FsmGameObject
					{
						Value = GameObject.Find("PLAYER")
					}
				},
				zAngle = 0f,
				space = 1
			});
			this.handbrake = BUS.busparts.transform.Find("HandbrakePivot/handbrake").gameObject.AddComponent<Handbrake>();
			this.handbrake.HandbrakeWheels = new Wheel[]
			{
				BUS.bus.transform.Find("wheelRL1").GetComponent<Wheel>(),
				BUS.bus.transform.Find("wheelRR1").GetComponent<Wheel>()
			};
			this.handbrake.drivetrain = this.dt;
			this.handbrake.CarController = this.acc;
			this.handbrake.Interaction = BUS.raycast;
			this.handbrake.HandbrakeIndicator = BUS.busparts.transform.Find("Electricity/indicator_brake").gameObject;
			this.handbrake.handbrakeRot = this.saveData.HandbrakeRot;
			this.dt = BUS.bus.GetComponent<Drivetrain>();
			this.dt.carController = this.acc;
			this.dt.revLimiter = false;
			this.dt.gear = 1;
			BUS.gearIndicator = BUS.bus.AddComponent<GearIndicator>().gearIndicator;
			BUS.bus.transform.Find("LOD/Colliders").gameObject.AddComponent<LaneCheckFSM>();
			GameObject gameObject6 = Object.Instantiate<GameObject>(GameObject.Find("FERNDALE(1630kg)").transform.Find("LOD/Wipers").gameObject);
			gameObject6.transform.SetParent(BUS.bus.transform.Find("LOD"));
			gameObject6.transform.localPosition = new Vector3(-0.19f, 1.05f, 5.709002f);
			gameObject6.transform.localEulerAngles = new Vector3(298.0014f, 3.000004f, 180.0018f);
			gameObject6.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
			gameObject6.name = "Wipers";
			GameObject gameObject7 = Object.Instantiate<GameObject>(gameObject6.transform.Find("Left").gameObject);
			gameObject7.transform.SetParent(gameObject6.transform);
			gameObject7.name = "Middle";
			gameObject7.transform.localPosition = new Vector3(-0.764022f, -0.02295654f, 0.01499537f);
			gameObject7.transform.localEulerAngles = new Vector3(319.0005f, 14.00005f, 84.0004f);
			gameObject7.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject7.transform.Find("WiperLeftPivot").name = "WiperMiddlePivot";
			gameObject6.transform.Find("Left").localPosition = new Vector3(-0.09702229f, 0.02704346f, 0.03499537f);
			gameObject6.transform.Find("Left").localEulerAngles = new Vector3(317f, 1.400864E-05f, 102.0002f);
			gameObject6.transform.Find("Left").localScale = new Vector3(1f, 1f, 1f);
			gameObject6.transform.Find("Right").localPosition = new Vector3(-0.1640224f, 0.02704346f, 0.03499537f);
			gameObject6.transform.Find("Right").localEulerAngles = new Vector3(309.0002f, -1.627982E-05f, 84.00014f);
			gameObject6.transform.Find("Right").localScale = new Vector3(1f, -1f, 1f);
			BUS.bus.SetActive(true);
			GameObject gameObject8 = Object.Instantiate<GameObject>(GameObject.Find("HAYOSIKO(1500kg, 250)").transform.Find("LOD/Dashboard/Knobs/ButtonWipers").gameObject);
			gameObject8.transform.SetParent(BUS.bus.transform.Find("LOD"));
			gameObject8.name = "ButtonWipers";
			gameObject8.transform.localPosition = new Vector3(-1.099f, 1.05f, 4.732999f);
			gameObject8.GetComponent<SphereCollider>().radius = 0.015f;
			gameObject8.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject8.GetComponents<PlayMakerFSM>()[1].FsmVariables.FindFsmGameObject("WipersLeft").Value = gameObject6.transform.Find("Left/WiperLeftPivot").gameObject;
			gameObject8.GetComponents<PlayMakerFSM>()[1].FsmVariables.FindFsmGameObject("WipersRight").Value = gameObject6.transform.Find("Right/WiperRightPivot").gameObject;
			PlayMakerExtensions.GetAction<StringCompare>(PlayMakerExtensions.GetState(gameObject8.GetComponent<PlayMakerFSM>(), "Check car"), 0).compareTo.Value = "Bus";
			PlayMakerExtensions.GetAction<EaseFloat>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject8, "Function"), "Move"), 6).toValue.Value = 65f;
			PlayMakerExtensions.GetAction<EaseFloat>(PlayMakerExtensions.GetState(PlayMakerExtensions.GetPlayMaker(gameObject8, "Function"), "Move 2"), 3).fromValue.Value = 65f;
			PlayMakerExtensions.GetAction<MasterAudioPlaySound>(PlayMakerExtensions.GetState(gameObject8.GetComponents<PlayMakerFSM>()[1], "Move"), 0).gameObject.GameObject = gameObject6.transform.Find("Left/WiperLeftPivot/wipers_tap/wipers_rod").gameObject;
			BUS.bus.transform.Find("LOD/bus_curtains").GetComponent<MeshRenderer>().material.mainTexture = mainTexture2;
			BUS.bus.transform.Find("LOD/MESH/bus_roofrack").GetComponent<MeshRenderer>().materials[1].mainTexture = mainTexture3;
			BUS.bus.transform.Find("LOD/MESH/bus_seats").GetComponent<MeshRenderer>().materials[0].mainTexture = mainTexture3;
			BUS.bus.transform.Find("LOD/MESH/bus_seats").GetComponent<MeshRenderer>().materials[1].mainTexture = mainTexture;
			BUS.bus.transform.Find("MESH/bus_interior").GetComponent<MeshRenderer>().material.mainTexture = mainTexture4;
			BUS.bus.transform.Find("LOD/bus_windows_in").GetComponent<MeshRenderer>().reflectionProbeUsage = 0;
			BUS.bus.transform.Find("CarLightsAI/DrivingLights").transform.GetChild(0).gameObject.SetActive(true);
			BUS.bus.transform.Find("CarLightsAI/DrivingLights").transform.GetChild(1).gameObject.SetActive(true);
			BUS.bus.transform.Find("CarLightsAI/DrivingLights").transform.GetChild(2).gameObject.SetActive(true);
			BUS.bus.transform.Find("CarLightsAI/DrivingLights").transform.GetChild(3).gameObject.SetActive(true);
			BUS.busparts.transform.Find("Electricity/indicator_lights").SetParent(BUS.bus.transform.Find("CarLightsAI/DrivingLights/Low"));
			BUS.bus.transform.Find("MESH/bus_lights_on").GetComponent<MeshRenderer>().material = this.lightsoff;
			DoorOpen doorOpen = BUS.bus.transform.Find("LOD/Door").gameObject.AddComponent<DoorOpen>();
			BUS.bus.transform.Find("LOD/Door/Pivot/coll").gameObject.layer = LayerMask.NameToLayer("Dashboard");
			BUS.bus.transform.Find("LOD/Door/Pivot/coll").gameObject.tag = "Untagged";
			doorOpen.door = BUS.bus.transform.Find("LOD/Door/Pivot").GetComponent<Animation>();
			doorOpen.triggerDoor = BUS.bus.transform.Find("LOD/Door/Pivot/coll").GetComponent<Collider>();
			doorOpen.triggerButtonDoor = BUS.busparts.transform.Find("trigger_doorInterior").GetComponent<Collider>();
			doorOpen.raycast = BUS.raycast;
			BUS.busparts.transform.Find("Pedals").gameObject.AddComponent<Pedals>();
			this.townSignBus = BUS.busparts.transform.Find("PivotStopSign").gameObject.AddComponent<TownSign>();
			this.townSignBus.Lever = BUS.busparts.transform.Find("lever_StopSign/Pivot");
			this.townSignBus.StopSignPos = BUS.busparts.transform.Find("PivotStopSign/Pivot");
			this.townSignBus.Use = BUS.busparts.transform.Find("UseLever").GetComponent<Collider>();
			this.townSignBus.CurrentStage = this.saveData.CurrentStageTown;
			this.busIntLight = BUS.bus.AddComponent<InteriorLight>();
			this.busIntLight.light = BUS.busparts.transform.Find("Electricity/indicator_interior_lights").gameObject;
			this.busIntLight.trigger = BUS.busparts.transform.Find("interior_lights_trigger").gameObject;
			this.busIntLight.raycast = BUS.raycast;
			this.busIntLight.on = this.saveData.busIntLight;
			this.busFogs = BUS.bus.AddComponent<Foglights>();
			this.busFogs.foglight = BUS.busparts.transform.Find("Electricity/indicator_foglights").gameObject;
			this.busFogs.trigger = BUS.busparts.transform.Find("foglights_trigger").gameObject;
			this.busFogs.raycast = BUS.raycast;
			this.busFogs.on = this.saveData.foglightBus;
			BUS.busLowGear = BUS.bus.AddComponent<LowGearHighGear>();
			BUS.busLowGear.highgear = BUS.busparts.transform.Find("Electricity/highgear_light").gameObject;
			BUS.busLowGear.lowgear = BUS.busparts.transform.Find("Electricity/lowgear_light").gameObject;
			BUS.busLowGear.indicatorgear = BUS.busparts.transform.Find("Electricity/indicator_lowgear").gameObject;
			BUS.busLowGear.drivetrain = this.dt;
			BUS.busLowGear.trigger = BUS.busparts.transform.Find("trigger_gears").gameObject;
			BUS.busLowGear.raycast = BUS.raycast;
			BUS.busLowGear.isLowGear = BUS.busLowGear;
			this.brakelights = BUS.bus.transform.Find("BrakeLights").gameObject;
			BUS.bus.AddComponent<BrakeLights>().BrakeLightsDefault = this.brakelights;
			this.beamsshort = BUS.bus.transform.Find("CarLightsAI/DrivingLights/Low").gameObject;
			this.reverselights = BUS.bus.transform.Find("CarLightsAI/ReverseLights").gameObject;
			BUS.bus.transform.Find("LOD/Colliders").GetChild(18).transform.localPosition = new Vector3(0f, 1.18f, 0f);
			BUS.bus.transform.Find("LOD/Colliders").GetChild(18).gameObject.layer = LayerMask.NameToLayer("PlayerOnlyColl");
			BUS.bus.transform.GetChild(5).gameObject.SetActive(false);
			this.exhaust = BUS.bus.transform.Find("LOD/Exhaust").gameObject;
			this.preheat = BUS.busparts.transform.Find("Electricity/indicator_preheat").gameObject;
			this.suspensionwarn = BUS.busparts.transform.Find("Electricity/indicator_brakes").gameObject;
			this.enginewarn = BUS.busparts.transform.Find("Electricity/indicator_overheat").gameObject;
			this.suspWarn = BUS.bus.AddComponent<SuspensionWarning>();
			this.suspWarn.brakething = this.suspensionwarn;
			this.suspWarn.trigger = BUS.busparts.transform.Find("trigger_brakes").gameObject;
			this.suspWarn.raycast = BUS.raycast;
			this.suspWarn.on = this.saveData.suspWarnOn;
			this.engineWarn = BUS.bus.AddComponent<EngineWarning>();
			this.engineWarn.warning = this.enginewarn;
			this.engineWarn.trigger = BUS.busparts.transform.Find("trigger_engine").gameObject;
			this.engineWarn.raycast = BUS.raycast;
			this.engineWarn.on = this.saveData.engineWarnOn;
			this.preheatSC = BUS.bus.AddComponent<Preheat>();
			this.preheatSC.preheatthing = this.preheat;
			this.preheatSC.trigger = BUS.busparts.transform.Find("trigger_preheat").gameObject;
			this.preheatSC.raycast = BUS.raycast;
			this.preheatSC.on = this.saveData.preheatSCOn;
			FauxCooling fauxCooling = BUS.busparts.transform.Find("Simulation/FauxColling").gameObject.AddComponent<FauxCooling>();
			fauxCooling.drivetrain = BUS.bus.GetComponent<Drivetrain>();
			WaterCheck waterCheck = BUS.busparts.transform.Find("Simulation/WaterCheck").gameObject.AddComponent<WaterCheck>();
			this.busStarter = BUS.bus.AddComponent<StarterNew>();
			this.busStarter.drivetrain = this.dt;
			this.busStarter.lightObject = BUS.busparts.transform.Find("BatteryGaugePivot").gameObject;
			this.busStarter.oilObject = BUS.busparts.transform.Find("Electricity/indicator_engine").gameObject;
			this.busStarter.waterCheck = waterCheck;
			this.busStarter.cooling = fauxCooling;
			this.busStarter.exhaust = this.exhaust.transform.Find("FromEngine").GetComponent<EllipsoidParticleEmitter>();
			this.busStarter.keyhole = BUS.busparts.transform.Find("bus_dash_keyhole/bus_dash_keyhole");
			this.busStarter.key = BUS.busparts.transform.Find("bus_dash_keyhole/bus_dash_keyhole/key_bus").gameObject;
			this.busStarter.Interaction = BUS.raycast;
			this.busStarter.ignitionCollider = BUS.busparts.transform.Find("bus_dash_keyhole/bus_dash_keyhole").GetComponent<BoxCollider>();
			this.busStarter.busparts = BUS.busparts;
			this.busStarter.electricity = BUS.busparts.transform.Find("Electricity").gameObject;
			this.busStarter.heat = this.preheatSC;
			this.busStarter.engine = this.engineWarn;
			this.busStarter.EngineSound = BUS.bus.transform.Find("LOD/EngineSound").gameObject;
			this.busStarter.EngineSound.SetActive(false);
			this.dt.canStall = true;
			GameObject gameObject9 = BUS.busparts.transform.Find("FuelTank").gameObject;
			gameObject9.GetComponent<FixedJoint>().connectedBody = BUS.bus.GetComponent<Rigidbody>();
			FuelTank fuelTank = gameObject9.AddComponent<FuelTank>();
			fuelTank.drivetrain = this.dt;
			fuelTank.CarController = this.acc;
			fuelTank.starter = this.busStarter;
			fuelTank.fueltankRigidbody = gameObject9.GetComponent<Rigidbody>();
			fuelTank.indicatorFuel = BUS.busparts.transform.Find("Electricity/indicator_hazards").gameObject;
			this.busStarter.fuelTank = fuelTank;
			Transform transform = BUS.bus.transform.Find("CarLightsAI/DrivingLights");
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
			bool flag5 = ModLoader.IsModPresent("Worse_Audio_Out_of_car");
			if (flag5)
			{
				VehicleSounds vehicleSounds = BUS.bus.AddComponent<VehicleSounds>();
				vehicleSounds.start1 = BUS.busparts.transform.Find("start1").GetComponent<AudioLowPassFilter>();
				vehicleSounds.start2 = BUS.busparts.transform.Find("start2").GetComponent<AudioLowPassFilter>();
				vehicleSounds.start3 = BUS.busparts.transform.Find("start3").GetComponent<AudioLowPassFilter>();
				vehicleSounds.shutoff = BUS.busparts.transform.Find("shutOff").GetComponent<AudioLowPassFilter>();
				vehicleSounds.engineSound = BUS.bus.transform.Find("LOD/EngineSound").gameObject.AddComponent<AudioLowPassFilter>();
				vehicleSounds.CooldownTick = BUS.busparts.transform.Find("Simulation/CooldownTick").GetComponent<AudioLowPassFilter>();
			}
			GameObject gameObject10 = Object.Instantiate<GameObject>(GameObject.Find("HAYOSIKO(1500kg, 250)").transform.Find("RadioPivot").gameObject);
			gameObject10.transform.SetParent(BUS.bus.transform);
			gameObject10.transform.localPosition = new Vector3(-0.9f, 2.15f, 5.035f);
			gameObject10.transform.localEulerAngles = new Vector3(270f, 180f, 0f);
			gameObject10.name = "RadioPivot";
			gameObject10.transform.Find("Speaker").GetComponent<PlayMakerFSM>().FsmVariables.FindFsmBool("HighPass").Value = false;
			gameObject10.transform.Find("Speaker").GetComponent<PlayMakerFSM>().FsmVariables.FindFsmBool("LowPass").Value = true;
			this.exhaust.SetActive(false);
			BUS.bus.transform.Find("LOD/RegPlateGen").name = "RegPlateGen 1";
			BUS.bus.transform.Find("LOD/RegPlateGen 1").GetComponent<RegPlateGen>().PlateString = "VYH-745";
			BUS.bus.transform.Find("LOD/RegPlateGen").GetComponent<RegPlateGen>().PlateString = "VYH-745";
			GameObject gameObject11 = Object.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("busdepot.prefab"));
			gameObject11.transform.localPosition = new Vector3(-606.9965f, 4.512314f, 1680.838f);
			gameObject11.transform.localEulerAngles = new Vector3(0.6999999f, 114.0001f, 359.6f);
			gameObject11.name = "BusDepot";
			DoorScript door = gameObject11.transform.Find("DoorSingle").gameObject.AddComponent<DoorScript>();
			OpeningHours openingHours = gameObject11.AddComponent<OpeningHours>();
			openingHours.door = door;
			openingHours.Open = gameObject11.transform.Find("Open").gameObject;
			gameObject11.transform.Find("show_tag_card/Time").gameObject.AddComponent<DigitalClock>();
			ShowTagCart showTagCart = gameObject11.transform.Find("show_tag_card").gameObject.AddComponent<ShowTagCart>();
			showTagCart.UpperText = gameObject11.transform.Find("show_tag_card/UpperText").GetComponent<TextMesh>();
			showTagCart.TriggerCheckBalance = gameObject11.transform.Find("show_tag_card/trigger_CheckBalance").GetComponent<Collider>();
			showTagCart.TriggerOut = gameObject11.transform.Find("show_tag_card/trigger_QuitShift").GetComponent<Collider>();
			showTagCart.TriggerIn = gameObject11.transform.Find("show_tag_card/trigger_StartShift").GetComponent<Collider>();
			showTagCart.BusDoor = doorOpen;
			this.GameOptions();
			BUS.bus.GetComponent<GearIndicator>().lowGear = BUS.busLowGear;
			this.ClaimZone = gameObject11.transform.Find("Open/EnterCollision").gameObject.AddComponent<OnEnterZone>();
			this.ClaimZone.Envelope = gameObject11.transform.Find("Open/envelope").gameObject;
			this.ClaimZone.GuyAnimation = gameObject11.transform.Find("Open/Guy/skeleton/pelvis/spine_middle").GetComponent<Animation>();
			this.ClaimZone.busInZone = gameObject11.transform.Find("BusDropOffZone").gameObject.AddComponent<BusInZone>();
			this.ClaimZone.TotalMoneyMade = this.saveData.TotalMoneyMade;
			this.ClaimZone.showTagCart = showTagCart;
			showTagCart.onEnterZone = this.ClaimZone;
			openingHours.moneyZone = this.ClaimZone;
			BUS.bus.AddComponent<Beeper>();
			TicketPurchase ticketPurchase = BUS.bus.transform.Find("Ticket").gameObject.AddComponent<TicketPurchase>();
			BUS.bus.transform.Find("Ticket").gameObject.layer = 13;
			BUS.bus.transform.Find("Ticket").tag = "Untagged";
			Transform[] array = (from t in GameObject.Find("MAP").GetComponentsInChildren<Transform>(true)
			where t.name == "BusStop"
			select t).ToArray<Transform>();
			GameObject gameObject12 = Object.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("BusZone0"));
			gameObject12.transform.SetParent(array[0].Find("bus_stop"));
			gameObject12.name = "BusZone0";
			gameObject12.transform.localPosition = new Vector3(1.6f, 1.1f, 0.03f);
			gameObject12.transform.localEulerAngles = Vector3.zero;
			PassengerSystem passengerSystem = gameObject12.AddComponent<PassengerSystem>();
			passengerSystem.PeopleBusZone = BUS.busparts.transform.Find("PeopleBusZone0").gameObject;
			passengerSystem.dt = this.dt;
			passengerSystem.Tickets = ticketPurchase;
			passengerSystem.ZoneIndex = 0;
			passengerSystem.doorOpen = doorOpen;
			passengerSystem.IsSmallBusStop = true;
			PassengerExit passengerExit = gameObject12.AddComponent<PassengerExit>();
			passengerExit.CurrentPassengerSystem = passengerSystem;
			passengerExit.ClaimMoney = this.ClaimZone;
			GameObject gameObject13 = Object.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("BusZone1"));
			gameObject13.transform.SetParent(array[1].Find("bus_stop"));
			gameObject13.name = "BusZone1";
			gameObject13.transform.localPosition = new Vector3(1.6f, 1.1f, 0.03f);
			gameObject13.transform.localEulerAngles = Vector3.zero;
			PassengerSystem passengerSystem2 = gameObject13.AddComponent<PassengerSystem>();
			passengerSystem2.PeopleBusZone = BUS.busparts.transform.Find("PeopleBusZone1").gameObject;
			passengerSystem2.dt = this.dt;
			passengerSystem2.Tickets = ticketPurchase;
			passengerSystem2.ZoneIndex = 1;
			passengerSystem2.doorOpen = doorOpen;
			passengerSystem2.IsSmallBusStop = true;
			PassengerExit passengerExit2 = gameObject13.AddComponent<PassengerExit>();
			passengerExit2.CurrentPassengerSystem = passengerSystem2;
			passengerExit2.ClaimMoney = this.ClaimZone;
			BUS.busparts.transform.Find("PeopleBusZone1/Stranger5").gameObject.AddComponent<CiganRyhat>();
			BUS.busparts.transform.Find("PeopleBusZone1/Stranger5").gameObject.AddComponent<CiganTalkEngine>().UseCZText = (bool)BUS.CZTexts.Value;
			BUS.busparts.transform.Find("PeopleBusZone1/Stranger4/ThrowUp").gameObject.AddComponent<HomelessPuke>();
			AnnounceEndOfRoute announceEndOfRoute = BUS.busparts.transform.Find("trigger_EndRoute").gameObject.AddComponent<AnnounceEndOfRoute>();
			announceEndOfRoute.door = doorOpen;
			GameObject gameObject14 = Object.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("BusZone2"));
			gameObject14.transform.SetParent(array[2].Find("bus_stop"));
			gameObject14.name = "BusZone2";
			gameObject14.transform.localPosition = new Vector3(1.6f, 1.1f, 0.03f);
			gameObject14.transform.localEulerAngles = Vector3.zero;
			PassengerSystem passengerSystem3 = gameObject14.AddComponent<PassengerSystem>();
			passengerSystem3.PeopleBusZone = BUS.busparts.transform.Find("PeopleBusZone2").gameObject;
			passengerSystem3.dt = this.dt;
			passengerSystem3.Tickets = ticketPurchase;
			passengerSystem3.ZoneIndex = 2;
			passengerSystem3.doorOpen = doorOpen;
			PassengerExit passengerExit3 = gameObject14.AddComponent<PassengerExit>();
			passengerExit3.CurrentPassengerSystem = passengerSystem3;
			passengerExit3.ClaimMoney = this.ClaimZone;
			GameObject gameObject15 = Object.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("SmallBusZone0"));
			gameObject15.transform.SetParent(GameObject.Find("MAP").transform.Find("TrafficSigns/5/sign_pole 21/sign_bus"));
			gameObject15.name = "BusZone3";
			gameObject15.transform.localPosition = new Vector3(-5.700001f, -2.400001f, -1.35f);
			gameObject15.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			PassengerSystem passengerSystem4 = gameObject15.AddComponent<PassengerSystem>();
			passengerSystem4.PeopleBusZone = BUS.busparts.transform.Find("PeopleBusZone3").gameObject;
			passengerSystem4.dt = this.dt;
			passengerSystem4.Tickets = ticketPurchase;
			passengerSystem4.ZoneIndex = 3;
			passengerSystem4.doorOpen = doorOpen;
			passengerSystem4.IsSignStop = true;
			PassengerExit passengerExit4 = gameObject15.AddComponent<PassengerExit>();
			passengerExit4.CurrentPassengerSystem = passengerSystem4;
			passengerExit4.ClaimMoney = this.ClaimZone;
			GameObject gameObject16 = Object.Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("SmallBusZone1"));
			gameObject16.transform.SetParent(GameObject.Find("MAP").transform.Find("TrafficSigns/5/sign_pole 22/sign_bus"));
			gameObject16.name = "BusZone4";
			gameObject16.transform.localPosition = new Vector3(-4.700001f, -0.400001f, -1.5f);
			gameObject16.transform.localEulerAngles = new Vector3(0f, 0f, 162.0002f);
			PassengerSystem passengerSystem5 = gameObject16.AddComponent<PassengerSystem>();
			passengerSystem5.PeopleBusZone = BUS.busparts.transform.Find("PeopleBusZone4").gameObject;
			passengerSystem5.dt = this.dt;
			passengerSystem5.Tickets = ticketPurchase;
			passengerSystem5.ZoneIndex = 4;
			passengerSystem5.doorOpen = doorOpen;
			passengerSystem5.IsSignStop = true;
			PassengerExit passengerExit5 = gameObject16.AddComponent<PassengerExit>();
			passengerExit5.CurrentPassengerSystem = passengerSystem5;
			passengerExit5.ClaimMoney = this.ClaimZone;
			BUS.busparts.transform.Find("PeopleBusZone0").Find("Stranger1/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone0").Find("Stranger2/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone0").Find("Stranger3/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone1").Find("Stranger1/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone1").Find("Stranger2/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone1").Find("Stranger3/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone1").Find("Stranger4/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 2;
			BUS.busparts.transform.Find("PeopleBusZone1").Find("Stranger5/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 8;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger1/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger2/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger3/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger4/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger5/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger6/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone2").Find("Stranger7/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone3").Find("Stranger1/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			BUS.busparts.transform.Find("PeopleBusZone3").Find("Stranger2/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 3;
			BUS.busparts.transform.Find("PeopleBusZone4").Find("Stranger1/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 2;
			BUS.busparts.transform.Find("PeopleBusZone4").Find("Stranger2/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 5;
			BUS.busparts.transform.Find("PeopleBusZone4").Find("Stranger5/KickPassenger").gameObject.AddComponent<KickPassenger>().Kicks = 1;
			gameObject12.transform.SetParent(null);
			gameObject13.transform.SetParent(null);
			gameObject14.transform.SetParent(null);
			ticketPurchase.money = this.ClaimZone;
			assetBundle.Unload(false);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000051BC File Offset: 0x000033BC
		private void Mod_OnSave()
		{
			SaveUtility.Save<SaveData>(new SaveData
			{
				buspos = BUS.bus.transform.position,
				busrot = BUS.bus.transform.eulerAngles,
				TotalMoneyMade = this.ClaimZone.TotalMoneyMade,
				suspWarnOn = this.suspWarn.on,
				preheatSCOn = this.preheatSC.on,
				engineWarnOn = this.engineWarn.on,
				busIntLight = this.busIntLight.on,
				busLowGear = BUS.busLowGear.isLowGear,
				foglightBus = this.busFogs.on,
				HandbrakeRot = this.handbrake.handbrakeRot,
				CurrentStageTown = this.townSignBus.CurrentStage
			});
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00005298 File Offset: 0x00003498
		public void GameOptions()
		{
			BUS.optionAdjuster = GameObject.Find("Systems").transform.Find("OptionsMenu").gameObject.AddComponent<AdjustCarOptions>();
			BUS.optionAdjuster.Options = GameObject.Find("Systems/Options").GetComponents<PlayMakerFSM>()[0];
			BUS.optionAdjuster.CarController = this.acc;
			BUS.optionAdjuster.drivetrain = this.dt;
			BUS.optionAdjuster.Dynamics = BUS.bus.GetComponent<CarDynamics>();
			BUS.optionAdjuster.Graphics = GameObject.Find("Systems/Options").GetComponents<PlayMakerFSM>()[1];
			BUS.optionAdjuster.driverHeadPivot = BUS.bus.transform.Find("PlayerFunctions/DriverHeadPivot").GetComponent<ConfigurableJoint>();
			BUS.optionAdjuster.SteeringWheel = BUS.bus.transform.Find("LOD/Steering/TruckSteeringPivot").GetComponent<SteeringWheel>();
			BUS.optionAdjuster.SetupFSMS();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000538C File Offset: 0x0000358C
		private void Mod_OnUpdate()
		{
			bool flag = !this.PlayerInCar;
			if (flag)
			{
				bool flag2 = this.PlayerCheck.transform.Find("PLAYER") != null;
				if (flag2)
				{
					this.PlayerInCar = true;
				}
			}
			bool flag3 = this.PlayerCheck.transform.Find("PLAYER") == null;
			if (flag3)
			{
				this.PlayerInCar = false;
			}
			bool flag4 = !this.PlayerInCar;
			if (flag4)
			{
				this.acc.steerAxis = "null";
				this.acc.brakeAxis = "null";
				this.acc.shiftUpButton = "null";
				this.acc.shiftDownButton = "null";
				this.acc.throttleAxis = "null";
				this.acc.clutchAxis = "null";
				this.acc.handbrakeAxis = "null";
				BUS.gearIndicator.SetActive(false);
				BUS.bus.transform.Find("LOD/TrafficTrigger").gameObject.SetActive(false);
			}
			bool playerInCar = this.PlayerInCar;
			if (playerInCar)
			{
				this.acc.steerAxis = "Horizontal";
				this.acc.brakeAxis = "Brake";
				this.acc.shiftUpButton = "ShiftUp";
				this.acc.shiftDownButton = "ShiftDown";
				this.acc.throttleAxis = "Throttle";
				this.acc.clutchAxis = "Clutch";
				this.acc.handbrakeAxis = "Handbrake";
				BUS.gearIndicator.SetActive(true);
				BUS.bus.transform.Find("LOD/TrafficTrigger").gameObject.SetActive(true);
			}
			bool activeInHierarchy = BUS.bus.transform.Find("CarLightsAI/DrivingLights/Low").gameObject.activeInHierarchy;
			if (activeInHierarchy)
			{
				BUS.bus.transform.Find("MESH/bus_lights_on").GetComponent<MeshRenderer>().material = this.lightson;
			}
			else
			{
				BUS.bus.transform.Find("MESH/bus_lights_on").GetComponent<MeshRenderer>().material = this.lightsoff;
			}
			this.reverselights.gameObject.SetActive(this.dt.gear == 0);
		}

		// Token: 0x04000001 RID: 1
		public AxisCarController acc;

		// Token: 0x04000002 RID: 2
		public bool PlayerInCar;

		// Token: 0x04000003 RID: 3
		public GameObject PlayerCheck;

		// Token: 0x04000004 RID: 4
		private FsmString playerCurrentVehicle;

		// Token: 0x04000005 RID: 5
		public SaveData saveData;

		// Token: 0x04000006 RID: 6
		public OnEnterZone ClaimZone;

		// Token: 0x04000007 RID: 7
		public SuspensionWarning suspWarn;

		// Token: 0x04000008 RID: 8
		public EngineWarning engineWarn;

		// Token: 0x04000009 RID: 9
		public Preheat preheatSC;

		// Token: 0x0400000A RID: 10
		public Foglights busFogs;

		// Token: 0x0400000B RID: 11
		public InteriorLight busIntLight;

		// Token: 0x0400000C RID: 12
		public static LowGearHighGear busLowGear;

		// Token: 0x0400000D RID: 13
		public Handbrake handbrake;

		// Token: 0x0400000E RID: 14
		public TownSign townSignBus;

		// Token: 0x0400000F RID: 15
		public AudioSource enginesoundclip;

		// Token: 0x04000010 RID: 16
		public GameObject brakelights;

		// Token: 0x04000011 RID: 17
		public static GameObject bus;

		// Token: 0x04000012 RID: 18
		public GameObject beamsshort;

		// Token: 0x04000013 RID: 19
		public Material lightson;

		// Token: 0x04000014 RID: 20
		public Material lightsoff;

		// Token: 0x04000015 RID: 21
		public GameObject reverselights;

		// Token: 0x04000016 RID: 22
		public static GameObject gearIndicator;

		// Token: 0x04000017 RID: 23
		public GameObject preheat;

		// Token: 0x04000018 RID: 24
		private static AdjustCarOptions optionAdjuster;

		// Token: 0x04000019 RID: 25
		public GameObject suspensionwarn;

		// Token: 0x0400001A RID: 26
		public GameObject enginewarn;

		// Token: 0x0400001B RID: 27
		public Drivetrain dt;

		// Token: 0x0400001C RID: 28
		public static GameObject busparts;

		// Token: 0x0400001D RID: 29
		public GameObject exhaust;

		// Token: 0x0400001E RID: 30
		public StarterNew busStarter;

		// Token: 0x0400001F RID: 31
		public static Raycast raycast;

		// Token: 0x04000020 RID: 32
		public static int KeyBus = 0;

		// Token: 0x04000021 RID: 33
		public static Settings mirrors = new Settings("Mirrors", "Adds mirrors for bus.", true);

		// Token: 0x04000022 RID: 34
		public static Settings CZTexts = new Settings("CZTexts", "CZ Translation for Cigan NPC.", true);
	}
}
