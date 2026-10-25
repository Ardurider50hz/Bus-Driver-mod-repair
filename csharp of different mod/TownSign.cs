using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200002A RID: 42
	public class TownSign : MonoBehaviour
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000A9A6 File Offset: 0x00008BA6
		private void Start()
		{
			this.raycast = BUS.raycast;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		private void Update()
		{
			bool hit = this.raycast.GetHit(this.Use);
			if (hit)
			{
				this.guiInteraction.Value = "SWITCH TOWN  *" + this.Towns[this.CurrentStage] + "*";
				this.guiUse.Value = true;
				this.mouseOver = true;
				float axis = Input.GetAxis("Mouse ScrollWheel");
				bool flag = axis > 0f;
				if (flag)
				{
					this.IncreaseStage();
					MasterAudio.PlaySound3DAtTransformAndForget("HouseFoley", base.transform, 0.4f, null, 0f, "sauna_stove_knob");
				}
				else
				{
					bool flag2 = axis < 0f;
					if (flag2)
					{
						this.DecreaseStage();
						MasterAudio.PlaySound3DAtTransformAndForget("HouseFoley", base.transform, 0.4f, null, 0f, "sauna_stove_knob");
					}
				}
			}
			else
			{
				bool flag3 = this.mouseOver;
				if (flag3)
				{
					this.mouseOver = false;
					this.guiInteraction.Value = string.Empty;
					this.guiUse.Value = false;
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000AAD8 File Offset: 0x00008CD8
		private void IncreaseStage()
		{
			this.CurrentStage = Mathf.Clamp(this.CurrentStage + 1, 0, 3);
			this.UpdateLeverAndSign();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000AAF7 File Offset: 0x00008CF7
		private void DecreaseStage()
		{
			this.CurrentStage = Mathf.Clamp(this.CurrentStage - 1, 0, 3);
			this.UpdateLeverAndSign();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000AB18 File Offset: 0x00008D18
		private void UpdateLeverAndSign()
		{
			Vector3 localEulerAngles = this.Lever.localEulerAngles;
			localEulerAngles.y = this.PosLeverStages[this.CurrentStage];
			this.Lever.localEulerAngles = localEulerAngles;
			Vector3 localEulerAngles2 = this.StopSignPos.localEulerAngles;
			localEulerAngles2.x = this.StopSignPivotStages[this.CurrentStage];
			this.StopSignPos.localEulerAngles = localEulerAngles2;
		}

		// Token: 0x04000177 RID: 375
		private FsmBool guiUse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");

		// Token: 0x04000178 RID: 376
		private FsmString guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");

		// Token: 0x04000179 RID: 377
		private bool mouseOver = false;

		// Token: 0x0400017A RID: 378
		private Raycast raycast;

		// Token: 0x0400017B RID: 379
		public Transform Lever;

		// Token: 0x0400017C RID: 380
		public Transform StopSignPos;

		// Token: 0x0400017D RID: 381
		public Collider Use;

		// Token: 0x0400017E RID: 382
		public int CurrentStage = 0;

		// Token: 0x0400017F RID: 383
		private string[] Towns = new string[]
		{
			"RAITTIMAA",
			"PERAJARVI",
			"ALIVIESKA",
			"LUKKA"
		};

		// Token: 0x04000180 RID: 384
		private float[] PosLeverStages = new float[]
		{
			104f,
			119f,
			140f,
			160f
		};

		// Token: 0x04000181 RID: 385
		private float[] StopSignPivotStages = new float[]
		{
			0f,
			90f,
			180f,
			270f
		};
	}
}
