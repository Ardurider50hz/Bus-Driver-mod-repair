using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000013 RID: 19
	public class SuspensionWarning : MonoBehaviour
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00008024 File Offset: 0x00006224
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			bool flag = this.on;
			if (flag)
			{
				this.brakething.SetActive(false);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00008080 File Offset: 0x00006280
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = (this.on ? "IGNITION ELECTRICS: ON" : "IGNITION ELECTRICS: OFF");
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool flag = !this.on;
					if (flag)
					{
						MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_in");
						this.on = true;
						this.brakething.SetActive(false);
					}
					else
					{
						bool flag2 = this.on;
						if (flag2)
						{
							MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_out");
							this.on = false;
							this.brakething.SetActive(true);
						}
					}
				}
			}
			else
			{
				bool flag3 = this.mouseOver;
				if (flag3)
				{
					this.GUIinteraction.Value = string.Empty;
					this.GUIuse.Value = false;
					this.mouseOver = false;
				}
			}
		}

		// Token: 0x040000CB RID: 203
		public bool on;

		// Token: 0x040000CC RID: 204
		public GameObject brakething;

		// Token: 0x040000CD RID: 205
		public GameObject trigger;

		// Token: 0x040000CE RID: 206
		public Raycast raycast;

		// Token: 0x040000CF RID: 207
		private FsmString GUIinteraction;

		// Token: 0x040000D0 RID: 208
		private FsmBool GUIuse;

		// Token: 0x040000D1 RID: 209
		private bool mouseOver;
	}
}
