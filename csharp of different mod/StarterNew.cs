using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200000C RID: 12
	public class StarterNew : MonoBehaviour
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00006B98 File Offset: 0x00004D98
		private void Start()
		{
			this.drivetrain.rpm = 0f;
			this.drivetrain.maxTorque = 0f;
			this.drivetrain.engineFrictionFactor = 5f;
			this.starterSound = BUS.busparts.transform.Find("start2").gameObject;
			this.lightObject.transform.localEulerAngles = new Vector3(-26f, 0f, 0f);
			this.guiUse = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse");
			this.guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.electricity = BUS.busparts.transform.Find("Electricity").gameObject;
			this.exhaust.gameObject.SetActive(true);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00006C80 File Offset: 0x00004E80
		private void Update()
		{
			this.playerHasKey = (BUS.KeyBus == 1);
			bool flag = this.playerHasKey && this.Interaction.GetHit(this.ignitionCollider);
			if (flag)
			{
				this.mouseOver = true;
				this.guiUse.Value = true;
				this.guiInteraction.Value = this.interaction;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					this.mouseHeld = true;
					this.startTime = Time.time;
					bool startAccordingtoTemp = this.StartAccordingtoTemp;
					if (startAccordingtoTemp)
					{
						this.engineStartTime = Mathf.Clamp(this.cooling.engineTemperature * -0.025f + 2.75f, 0.75f, 2f);
					}
					bool flag2 = !this.keyIgnition;
					if (flag2)
					{
						this.KeyIgnitionHandler(true);
					}
					else
					{
						this.keyIgnitionOut = !this.carEngineOn;
						bool flag3 = this.carEngineOn;
						if (flag3)
						{
							this.StopCarEngine();
							this.KeyIgnitionHandler(false);
						}
					}
				}
			}
			else
			{
				bool flag4 = this.mouseOver;
				if (flag4)
				{
					this.mouseOver = false;
					this.guiUse.Value = false;
					this.guiInteraction.Value = string.Empty;
				}
			}
			bool flag5 = this.mouseHeld;
			if (flag5)
			{
				this.MouseHeld();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00006DCC File Offset: 0x00004FCC
		private void KeyIgnitionHandler(bool keyIn)
		{
			if (keyIn)
			{
				this.PlaySound("CarFoley", this.keyhole, "carkeys_in", 0.5f, false);
				this.key.SetActive(true);
				this.keyhole.localEulerAngles = Vector3.zero;
				this.electricity.SetActive(true);
				this.oilObject.SetActive(true);
				this.keyIgnition = true;
				this.keyIgnitionOut = false;
				bool flag = !this.waterCheck.inWater;
				if (flag)
				{
					this.lightObject.transform.localEulerAngles = Vector3.zero;
				}
			}
			else
			{
				this.PlaySound("CarFoley", this.keyhole, "carkeys_out", 0.5f, false);
				this.key.SetActive(false);
				this.keyhole.localEulerAngles = new Vector3(0f, 26f, 0f);
				this.electricity.SetActive(false);
				this.oilObject.SetActive(false);
				this.keyIgnition = false;
				this.keyIgnitionOut = false;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00006EE8 File Offset: 0x000050E8
		private void MouseHeld()
		{
			bool mouseButtonUp = Input.GetMouseButtonUp(0);
			if (mouseButtonUp)
			{
				bool flag = this.keyIgnitionOut && this.startTime + this.holdTime >= Time.time;
				if (flag)
				{
					this.KeyIgnitionHandler(false);
				}
				else
				{
					bool flag2 = this.keyIgnition;
					if (flag2)
					{
						this.keyhole.localEulerAngles = new Vector3(0f, -15f, 0f);
					}
				}
				this.busparts.transform.Find("start3").gameObject.SetActive(false);
				this.startSoundPlaying = false;
				this.mouseHeld = false;
			}
			bool mouseButton = Input.GetMouseButton(0);
			if (mouseButton)
			{
				bool flag3 = !this.carEngineOn && this.keyIgnition && this.startTime + this.holdTime <= Time.time;
				if (flag3)
				{
					bool flag4 = this.drivetrain.gear != 1 && !this.waterCheck.inWater;
					if (flag4)
					{
						this.drivetrain.gear = 1;
					}
					bool flag5 = this.heat.on && this.engine.on;
					if (flag5)
					{
						bool flag6 = !this.startSoundPlaying;
						if (flag6)
						{
							this.startSoundPlaying = true;
							base.StartCoroutine(this.StartSound());
						}
					}
					bool flag7 = this.startTime + this.holdTime + this.engineStartTime <= Time.time && !this.waterCheck.inWater && this.fuelTank.fuelLevel > 0.4f;
					if (flag7)
					{
						this.carRunning = base.StartCoroutine(this.CarRunning());
					}
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000070A4 File Offset: 0x000052A4
		public void StopCarEngine()
		{
			this.carEngineOn = false;
			this.drivetrain.canStall = true;
			this.drivetrain.rpm = 0f;
			this.drivetrain.maxTorque = 0f;
			this.drivetrain.engineFrictionFactor = 5f;
			base.StopCoroutine(this.carRunning);
			this.exhaust.emit = false;
			this.busparts.transform.Find("start3").gameObject.SetActive(false);
			this.EngineSound.SetActive(false);
			this.cooling.EngineStop();
			this.fuelTank.StopConsumption();
			this.busparts.transform.Find("shutOff").gameObject.SetActive(true);
			bool inWater = this.waterCheck.inWater;
			if (inWater)
			{
				this.lightObject.transform.localEulerAngles = new Vector3(0f, -80f, 0f);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000071AA File Offset: 0x000053AA
		private IEnumerator CarRunning()
		{
			this.EngineSound.SetActive(true);
			this.drivetrain.canStall = false;
			this.drivetrain.maxTorque = this.maxTorque;
			this.drivetrain.engineFrictionFactor = 0.25f;
			this.carEngineOn = true;
			this.cooling.EngineStart();
			this.fuelTank.StartConsumption();
			this.exhaust.emit = true;
			this.electricity.SetActive(true);
			yield return new WaitForSeconds(1f);
			this.drivetrain.canStall = true;
			while (this.carEngineOn)
			{
				bool flag = this.drivetrain.rpm < this.stallRPM || this.waterCheck.inWater;
				if (flag)
				{
					break;
				}
				yield return null;
			}
			this.StopCarEngine();
			yield break;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000071B9 File Offset: 0x000053B9
		private IEnumerator StartSound()
		{
			this.PlaySound("CarFoley", this.keyhole, "ignition_keys", 0.5f, false);
			this.keyhole.localEulerAngles = new Vector3(0f, -58f, 0f);
			bool flag = !this.waterCheck.inWater;
			if (flag)
			{
				yield return new WaitForSeconds(0.2f);
				this.busparts.transform.Find("start1").gameObject.SetActive(true);
				this.busparts.transform.Find("shutOff").gameObject.SetActive(false);
				yield return new WaitForSeconds(0.3f);
				this.starterSound.SetActive(true);
				while (!this.carEngineOn && this.mouseHeld)
				{
					yield return null;
				}
				bool flag2 = this.carEngineOn;
				if (flag2)
				{
					this.busparts.transform.Find("start3").gameObject.SetActive(true);
				}
				this.oilObject.SetActive(false);
				this.starterSound.SetActive(false);
				this.busparts.transform.Find("start1").gameObject.SetActive(false);
				this.lightObject.transform.localEulerAngles = new Vector3(0f, -80f, 0f);
			}
			yield break;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000071C8 File Offset: 0x000053C8
		private void PlaySound(string type, Transform source, string sound = "", float volume = 1f, bool stopSound = false)
		{
			if (stopSound)
			{
				MasterAudio.StopAllOfSound(type);
			}
			else
			{
				MasterAudio.PlaySound3DAndForget(type, base.transform, false, volume, null, 0f, sound);
			}
		}

		// Token: 0x0400006F RID: 111
		public Drivetrain drivetrain;

		// Token: 0x04000070 RID: 112
		public Raycast Interaction;

		// Token: 0x04000071 RID: 113
		public FauxCooling cooling;

		// Token: 0x04000072 RID: 114
		public WaterCheck waterCheck;

		// Token: 0x04000073 RID: 115
		public FuelTank fuelTank;

		// Token: 0x04000074 RID: 116
		public Collider ignitionCollider;

		// Token: 0x04000075 RID: 117
		public GameObject key;

		// Token: 0x04000076 RID: 118
		public GameObject electricity;

		// Token: 0x04000077 RID: 119
		public GameObject busparts;

		// Token: 0x04000078 RID: 120
		public Transform keyhole;

		// Token: 0x04000079 RID: 121
		private bool playerHasKey = true;

		// Token: 0x0400007A RID: 122
		public Preheat heat;

		// Token: 0x0400007B RID: 123
		public EngineWarning engine;

		// Token: 0x0400007C RID: 124
		private Coroutine carRunning;

		// Token: 0x0400007D RID: 125
		public bool mouseHeld = false;

		// Token: 0x0400007E RID: 126
		public bool carEngineOn = false;

		// Token: 0x0400007F RID: 127
		public bool keyIgnition = false;

		// Token: 0x04000080 RID: 128
		public bool keyIgnitionOut = false;

		// Token: 0x04000081 RID: 129
		public float startTime = 4f;

		// Token: 0x04000082 RID: 130
		public float holdTime = 0.5f;

		// Token: 0x04000083 RID: 131
		public float engineStartTime = 4f;

		// Token: 0x04000084 RID: 132
		public float maxTorque = 886f;

		// Token: 0x04000085 RID: 133
		public float stallRPM = 200f;

		// Token: 0x04000086 RID: 134
		public bool startSoundPlaying = false;

		// Token: 0x04000087 RID: 135
		public GameObject starterSound;

		// Token: 0x04000088 RID: 136
		public GameObject lightObject;

		// Token: 0x04000089 RID: 137
		public GameObject oilObject;

		// Token: 0x0400008A RID: 138
		public EllipsoidParticleEmitter exhaust;

		// Token: 0x0400008B RID: 139
		public bool StartAccordingtoTemp = true;

		// Token: 0x0400008C RID: 140
		private bool mouseOver = false;

		// Token: 0x0400008D RID: 141
		private FsmBool guiUse;

		// Token: 0x0400008E RID: 142
		private FsmString guiInteraction;

		// Token: 0x0400008F RID: 143
		private string interaction = " Ignition ";

		// Token: 0x04000090 RID: 144
		public GameObject EngineSound;
	}
}
