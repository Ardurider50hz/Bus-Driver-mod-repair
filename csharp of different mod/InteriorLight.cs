using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000017 RID: 23
	public class InteriorLight : MonoBehaviour
	{
		// Token: 0x06000070 RID: 112 RVA: 0x000086A8 File Offset: 0x000068A8
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			bool flag = this.on;
			if (flag)
			{
				this.light.SetActive(false);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00008704 File Offset: 0x00006904
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "INSIDE LIGHTS";
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "dash_button");
					bool flag = !this.on;
					if (flag)
					{
						this.on = true;
						this.light.SetActive(true);
					}
					else
					{
						bool flag2 = this.on;
						if (flag2)
						{
							this.on = false;
							this.light.SetActive(false);
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

		// Token: 0x040000EA RID: 234
		public bool on;

		// Token: 0x040000EB RID: 235
		public GameObject light;

		// Token: 0x040000EC RID: 236
		public GameObject trigger;

		// Token: 0x040000ED RID: 237
		public Raycast raycast;

		// Token: 0x040000EE RID: 238
		private FsmString GUIinteraction;

		// Token: 0x040000EF RID: 239
		private FsmBool GUIuse;

		// Token: 0x040000F0 RID: 240
		private bool mouseOver = false;
	}
}
