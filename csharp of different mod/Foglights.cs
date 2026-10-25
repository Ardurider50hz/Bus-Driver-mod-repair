using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000004 RID: 4
	public class Foglights : MonoBehaviour
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00005BC4 File Offset: 0x00003DC4
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			bool flag = this.on;
			if (flag)
			{
				this.on = true;
				this.foglight.SetActive(true);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00005C28 File Offset: 0x00003E28
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "FOGLIGHTS";
				this.MouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "dash_button");
					bool flag = !this.on;
					if (flag)
					{
						this.on = true;
						this.foglight.SetActive(true);
					}
					else
					{
						bool flag2 = this.on;
						if (flag2)
						{
							this.on = false;
							this.foglight.SetActive(false);
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

		// Token: 0x0400002F RID: 47
		public bool on = false;

		// Token: 0x04000030 RID: 48
		public GameObject foglight;

		// Token: 0x04000031 RID: 49
		public GameObject trigger;

		// Token: 0x04000032 RID: 50
		public Raycast raycast;

		// Token: 0x04000033 RID: 51
		private FsmString GUIinteraction;

		// Token: 0x04000034 RID: 52
		private FsmBool GUIuse;

		// Token: 0x04000035 RID: 53
		private bool MouseOver = false;
	}
}
