using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000014 RID: 20
	public class Preheat : MonoBehaviour
	{
		// Token: 0x06000067 RID: 103 RVA: 0x000081CC File Offset: 0x000063CC
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			bool flag = this.on;
			if (flag)
			{
				this.preheatthing.SetActive(false);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00008228 File Offset: 0x00006428
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = (this.on ? "Ignition Glowplugs: ON" : "Ignition Gluwplugs: OFF");
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool flag = !this.on;
					if (flag)
					{
						MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_in");
						this.on = true;
						this.preheatthing.SetActive(false);
					}
					else
					{
						bool flag2 = this.on;
						if (flag2)
						{
							MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_out");
							this.on = false;
							this.preheatthing.SetActive(true);
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

		// Token: 0x040000D2 RID: 210
		public bool on;

		// Token: 0x040000D3 RID: 211
		public GameObject preheatthing;

		// Token: 0x040000D4 RID: 212
		public GameObject trigger;

		// Token: 0x040000D5 RID: 213
		public Raycast raycast;

		// Token: 0x040000D6 RID: 214
		private FsmString GUIinteraction;

		// Token: 0x040000D7 RID: 215
		private FsmBool GUIuse;

		// Token: 0x040000D8 RID: 216
		private bool mouseOver;
	}
}
