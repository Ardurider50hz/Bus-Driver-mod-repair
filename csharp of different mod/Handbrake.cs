using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000022 RID: 34
	public class Handbrake : MonoBehaviour
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00009728 File Offset: 0x00007928
		private void Start()
		{
			base.transform.localEulerAngles = new Vector3(this.handbrakeRot, 180f, 0f);
			this.handbrakeOn = (this.handbrakeRot > 275f && this.handbrakeRot < 285f);
			this.handbrakeCollider = base.GetComponent<Collider>();
			this.guiUse = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse");
			this.guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000097BC File Offset: 0x000079BC
		private void Update()
		{
			bool hit = this.Interaction.GetHit(this.handbrakeCollider);
			if (hit)
			{
				this.mouseOver = true;
				this.guiUse.Value = true;
				this.guiInteraction.Value = this.interaction;
				bool flag = (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !this.handbrakeInAction;
				if (flag)
				{
					bool flag2 = this.currentHandbrake != null;
					if (flag2)
					{
						base.StopCoroutine(this.currentHandbrake);
					}
					this.currentHandbrake = base.StartCoroutine(Input.GetMouseButtonDown(0) ? this.HandbrakeAction("handbrake_on", 0, 100f) : this.HandbrakeAction("handbrake_off", 1, -100f));
				}
			}
			else
			{
				bool flag3 = this.mouseOver;
				if (flag3)
				{
					this.mouseOver = false;
					this.guiUse.Value = false;
					this.guiInteraction.Value = string.Empty;
				}
			}
			this.HandbrakeIndicator.SetActive(this.handbrakeOn);
			bool flag4 = this.handbrakeOn;
			if (flag4)
			{
				foreach (Wheel wheel in this.HandbrakeWheels)
				{
					wheel.handbrake = Mathf.Clamp(this.handbrakeRot * 0.05f, 0f, 1f);
				}
				this.CarController.handbrakeInput = Mathf.Clamp(this.handbrakeRot * 0.05f, 0f, 1f);
			}
			bool flag5 = this.handbrakeRot <= 271f;
			if (flag5)
			{
				this.handbrakeOn = false;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000995D File Offset: 0x00007B5D
		private IEnumerator HandbrakeAction(string sound, int mouseButton, float direction)
		{
			this.handbrakeInAction = true;
			MasterAudio.PlaySound3DAndForget("CarFoley", base.transform, false, 1f, null, 0f, sound);
			while (this.handbrakeInAction)
			{
				bool mouseButtonUp = Input.GetMouseButtonUp(mouseButton);
				if (mouseButtonUp)
				{
					break;
				}
				this.handbrakeRot = Mathf.Clamp(this.handbrakeRot + direction * Time.deltaTime, this.handbrakeDown, this.handbrakeUp);
				base.transform.localEulerAngles = new Vector3(this.handbrakeRot, 180f, 0f);
				this.handbrakeOn = (this.handbrakeRot > 271f);
				yield return null;
			}
			this.handbrakeInAction = false;
			yield break;
		}

		// Token: 0x04000125 RID: 293
		public Wheel[] HandbrakeWheels;

		// Token: 0x04000126 RID: 294
		public Raycast Interaction;

		// Token: 0x04000127 RID: 295
		public AxisCarController CarController;

		// Token: 0x04000128 RID: 296
		public Drivetrain drivetrain;

		// Token: 0x04000129 RID: 297
		public GameObject HandbrakeIndicator;

		// Token: 0x0400012A RID: 298
		private Collider handbrakeCollider;

		// Token: 0x0400012B RID: 299
		private bool handbrakeInAction = false;

		// Token: 0x0400012C RID: 300
		private bool handbrakeOn = false;

		// Token: 0x0400012D RID: 301
		public float handbrakeRot = 290f;

		// Token: 0x0400012E RID: 302
		private float handbrakeUp = 290f;

		// Token: 0x0400012F RID: 303
		private float handbrakeDown = 270f;

		// Token: 0x04000130 RID: 304
		private bool mouseOver = false;

		// Token: 0x04000131 RID: 305
		private FsmBool guiUse;

		// Token: 0x04000132 RID: 306
		private FsmString guiInteraction;

		// Token: 0x04000133 RID: 307
		private string interaction = " Handbrake ";

		// Token: 0x04000134 RID: 308
		private Coroutine currentHandbrake;
	}
}
