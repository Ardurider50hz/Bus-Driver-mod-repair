using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200000E RID: 14
	public class FuelTankHatch : MonoBehaviour
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00007369 File Offset: 0x00005569
		private void Start()
		{
			this.guiUse = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse");
			this.guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000073A0 File Offset: 0x000055A0
		private void Update()
		{
			this.HatchOpener();
			bool flag = this.hatchOpened;
			if (flag)
			{
				this.CapOpener();
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000073C8 File Offset: 0x000055C8
		private void HatchOpener()
		{
			bool hit = this.Interaction.GetHit(this.hatchCollider);
			if (hit)
			{
				this.mouseOverHatch = true;
				this.guiUse.Value = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					this.hatchOpened = !this.hatchOpened;
					base.transform.localEulerAngles = (this.hatchOpened ? this.hatchOpen : this.hatchClosed);
					MasterAudio.PlaySound3DAndForget("CarFoley", base.transform, false, 1f, new float?(this.hatchOpened ? 1f : 0.75f), 0f, "filler_hatch");
					this.capCollider.enabled = this.hatchOpened;
					bool flag = this.capStage == 12;
					if (flag)
					{
						this.fuelFiller.SetActive(this.hatchOpened);
					}
				}
			}
			else
			{
				bool flag2 = this.mouseOverHatch;
				if (flag2)
				{
					this.mouseOverHatch = false;
					this.guiUse.Value = false;
					this.guiInteraction.Value = string.Empty;
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000074E4 File Offset: 0x000056E4
		private void CapOpener()
		{
			this.inputDelay -= Time.deltaTime;
			bool hit = this.Interaction.GetHit(this.capCollider);
			if (hit)
			{
				this.mouseOverCap = true;
				this.guiUse.Value = true;
				this.guiInteraction.Value = this.interaction;
				bool flag = this.capStage < 12 && Input.GetAxis(this.mouseWheel) < 0f && this.inputDelay <= 0f;
				if (flag)
				{
					this.capStage++;
					this.capTransform.localEulerAngles = new Vector3(0f, this.capRotation * (float)this.capStage, 0f);
					this.inputDelay = this.capDelay;
					bool flag2 = this.capStage == 12;
					if (flag2)
					{
						MasterAudio.PlaySound3DAndForget("CarFoley", this.capTransform, false, 1f, null, 0f, "filler_cap");
						this.fuelFiller.SetActive(true);
						this.capTransform.gameObject.SetActive(false);
					}
				}
				else
				{
					bool flag3 = this.capStage > 0 && Input.GetAxis(this.mouseWheel) > 0f && this.inputDelay <= 0f;
					if (flag3)
					{
						this.capStage--;
						this.capTransform.localEulerAngles = new Vector3(0f, this.capRotation * (float)this.capStage, 0f);
						this.inputDelay = this.capDelay;
						bool flag4 = this.capStage == 11;
						if (flag4)
						{
							MasterAudio.PlaySound3DAndForget("CarFoley", this.capTransform, false, 1f, null, 0f, "filler_cap");
							this.fuelFiller.SetActive(false);
							this.capTransform.gameObject.SetActive(true);
						}
					}
				}
			}
			else
			{
				bool flag5 = this.mouseOverCap;
				if (flag5)
				{
					this.mouseOverCap = false;
					this.guiUse.Value = false;
					this.guiInteraction.Value = string.Empty;
				}
			}
		}

		// Token: 0x0400009C RID: 156
		public Raycast Interaction;

		// Token: 0x0400009D RID: 157
		public bool hatchOpened = false;

		// Token: 0x0400009E RID: 158
		public Collider hatchCollider;

		// Token: 0x0400009F RID: 159
		public Vector3 hatchOpen = new Vector3(270f, 120f, 0f);

		// Token: 0x040000A0 RID: 160
		public Vector3 hatchClosed = new Vector3(270f, 0f, 0f);

		// Token: 0x040000A1 RID: 161
		public int capStage = 0;

		// Token: 0x040000A2 RID: 162
		public float capRotation = 15f;

		// Token: 0x040000A3 RID: 163
		public float capDelay = 0.05f;

		// Token: 0x040000A4 RID: 164
		public Transform capTransform;

		// Token: 0x040000A5 RID: 165
		public Collider capCollider;

		// Token: 0x040000A6 RID: 166
		public GameObject fuelFiller;

		// Token: 0x040000A7 RID: 167
		public bool mouseOverHatch = false;

		// Token: 0x040000A8 RID: 168
		public bool mouseOverCap = false;

		// Token: 0x040000A9 RID: 169
		public FsmBool guiUse;

		// Token: 0x040000AA RID: 170
		public FsmString guiInteraction;

		// Token: 0x040000AB RID: 171
		public string interaction = "";

		// Token: 0x040000AC RID: 172
		public string mouseWheel = "Mouse ScrollWheel";

		// Token: 0x040000AD RID: 173
		public float inputDelay = 0f;
	}
}
