using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000003 RID: 3
	public class LightSwitch : MonoBehaviour
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00005634 File Offset: 0x00003834
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.raycast = BUS.raycast;
			this.trigger = base.GetComponent<Collider>();
			this.playerCurrentVehicle = PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000056A8 File Offset: 0x000038A8
		public void Update()
		{
			this.CheckLightStage();
			bool flag = this.playerCurrentVehicle.Value == "Bus";
			if (flag)
			{
				this.KeyInput();
			}
			bool hit = this.raycast.GetHit(this.trigger);
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "LIGHTS";
				this.MouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "dash_button");
					bool flag2 = this.LightStage == 0;
					if (flag2)
					{
						this.LightStage = 1;
					}
					else
					{
						bool flag3 = this.LightStage == 1;
						if (flag3)
						{
							this.LightStage = 2;
						}
						else
						{
							bool flag4 = this.LightStage == 2;
							if (flag4)
							{
								this.LightStage = 0;
							}
						}
					}
				}
			}
			else
			{
				bool mouseOver = this.MouseOver;
				if (mouseOver)
				{
					this.GUIuse.Value = false;
					this.GUIinteraction.Value = string.Empty;
					this.MouseOver = false;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000057DC File Offset: 0x000039DC
		private void KeyInput()
		{
			bool buttonDown = cInput.GetButtonDown("HighBeam");
			if (buttonDown)
			{
				this.holdTime = 0.5f;
				this.holdButton = true;
			}
			else
			{
				bool flag = cInput.GetButton("HighBeam") && this.holdButton;
				if (flag)
				{
					this.holdTime -= Time.deltaTime;
					bool flag2 = this.holdTime < 0f;
					if (flag2)
					{
						this.holdButton = false;
						bool flag3 = this.LightStage < 2;
						if (flag3)
						{
							this.LightStage = 2;
						}
						else
						{
							this.LightStage = 0;
						}
						this.CheckLightStage();
						MasterAudio.PlaySound3DAndForget("CarFoley", base.transform, false, 0.4f, null, 0f, "dash_button");
					}
				}
				else
				{
					bool buttonUp = cInput.GetButtonUp("HighBeam");
					if (buttonUp)
					{
						this.holdButton = false;
						bool flag4 = this.holdTime > 0f;
						if (flag4)
						{
							this.highBeam = !this.highBeam;
							string text = "CarFoley";
							Transform transform = base.transform;
							bool flag5 = false;
							float num = 0.5f;
							string text2 = this.highBeam ? "turn_signal_lever_on" : "turn_signal_lever_off";
							MasterAudio.PlaySound3DAndForget(text, transform, flag5, num, null, 0f, text2);
							this.LightMeshes.transform.Find("HighBeam").gameObject.SetActive(this.highBeam);
							this.LightMeshes.transform.Find("LowBeam").gameObject.SetActive(!this.highBeam);
						}
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00005980 File Offset: 0x00003B80
		private void CheckLightStage()
		{
			switch (this.LightStage)
			{
			case 0:
				this.beamsShort.SetActive(false);
				this.beamsShort.transform.GetChild(0).GetComponent<Light>().range = 30f;
				this.beamsShort.transform.GetChild(1).GetComponent<Light>().range = 30f;
				this.LightMeshes.SetActive(false);
				this.LightMeshes.transform.Find("HighBeam").gameObject.SetActive(false);
				this.LightMeshes.transform.Find("LowBeam").gameObject.SetActive(false);
				break;
			case 1:
				this.beamsShort.SetActive(true);
				this.beamsShort.transform.GetChild(0).GetComponent<Light>().range = 30f;
				this.beamsShort.transform.GetChild(1).GetComponent<Light>().range = 30f;
				this.LightMeshes.SetActive(true);
				this.LightMeshes.transform.Find("HighBeam").gameObject.SetActive(false);
				this.LightMeshes.transform.Find("LowBeam").gameObject.SetActive(true);
				break;
			case 2:
				this.beamsShort.SetActive(true);
				this.beamsShort.transform.GetChild(0).GetComponent<Light>().range = 60f;
				this.beamsShort.transform.GetChild(1).GetComponent<Light>().range = 60f;
				this.LightMeshes.SetActive(true);
				this.LightMeshes.transform.Find("HighBeam").gameObject.SetActive(true);
				this.LightMeshes.transform.Find("LowBeam").gameObject.SetActive(true);
				break;
			}
		}

		// Token: 0x04000023 RID: 35
		public int LightStage = 0;

		// Token: 0x04000024 RID: 36
		public GameObject beamsShort;

		// Token: 0x04000025 RID: 37
		private Collider trigger;

		// Token: 0x04000026 RID: 38
		private Raycast raycast;

		// Token: 0x04000027 RID: 39
		private FsmString GUIinteraction;

		// Token: 0x04000028 RID: 40
		private FsmBool GUIuse;

		// Token: 0x04000029 RID: 41
		private bool MouseOver = false;

		// Token: 0x0400002A RID: 42
		public bool highBeam = false;

		// Token: 0x0400002B RID: 43
		private bool holdButton = false;

		// Token: 0x0400002C RID: 44
		private float holdTime = 0.5f;

		// Token: 0x0400002D RID: 45
		public GameObject LightMeshes;

		// Token: 0x0400002E RID: 46
		private FsmString playerCurrentVehicle;
	}
}
