using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000016 RID: 22
	public class LowGearHighGear : MonoBehaviour
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000083B8 File Offset: 0x000065B8
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.drivetrain.maxPower = 2500f;
			this.drivetrain.maxPowerRPM = 4500f;
			this.drivetrain.maxTorque = 9200f;
			this.drivetrain.maxTorqueRPM = 2500f;
			bool flag = !this.isLowGear;
			if (flag)
			{
				this.lowgear.SetActive(true);
				this.highgear.SetActive(false);
				this.indicatorgear.SetActive(false);
			}
			else
			{
				this.lowgear.SetActive(false);
				this.highgear.SetActive(true);
				this.indicatorgear.SetActive(true);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000849C File Offset: 0x0000669C
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "RANGE";
				this.MouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool flag = !this.isLowGear;
					if (flag)
					{
						MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 0.4f, new float?(0.8f), 0f, "dash_button");
						MasterAudio.PlaySound3DFollowTransform("Motor", base.gameObject.transform, 1f, null, 0f, "truck_air", false, false);
						this.isLowGear = true;
						this.lowgear.SetActive(true);
						this.CurrentGear = "LOW";
						this.highgear.SetActive(false);
						this.indicatorgear.SetActive(false);
					}
					else
					{
						bool flag2 = this.isLowGear;
						if (flag2)
						{
							MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 0.4f, null, 0f, "dash_button");
							MasterAudio.PlaySound3DFollowTransform("Motor", base.gameObject.transform, 1f, null, 0f, "truck_air", false, false);
							this.isLowGear = false;
							this.lowgear.SetActive(false);
							this.CurrentGear = "HIGH";
							this.highgear.SetActive(true);
							this.indicatorgear.SetActive(true);
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

		// Token: 0x040000DF RID: 223
		public bool isLowGear = true;

		// Token: 0x040000E0 RID: 224
		public GameObject indicatorgear;

		// Token: 0x040000E1 RID: 225
		public GameObject lowgear;

		// Token: 0x040000E2 RID: 226
		public GameObject highgear;

		// Token: 0x040000E3 RID: 227
		public Drivetrain drivetrain;

		// Token: 0x040000E4 RID: 228
		public GameObject trigger;

		// Token: 0x040000E5 RID: 229
		public Raycast raycast;

		// Token: 0x040000E6 RID: 230
		private FsmString GUIinteraction;

		// Token: 0x040000E7 RID: 231
		private FsmBool GUIuse;

		// Token: 0x040000E8 RID: 232
		private bool MouseOver;

		// Token: 0x040000E9 RID: 233
		public string CurrentGear = "LOW";
	}
}
