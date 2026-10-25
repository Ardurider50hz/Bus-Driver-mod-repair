using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000012 RID: 18
	public class EngineWarning : MonoBehaviour
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00007E68 File Offset: 0x00006068
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			bool flag = !this.on;
			if (flag)
			{
				this.warning.SetActive(false);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00007EC8 File Offset: 0x000060C8
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = (this.on ? "Ignition Engine: ON" : "Ignition Engine: OFF");
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool flag = this.on;
					if (flag)
					{
						MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_in");
						this.on = false;
						this.warning.SetActive(false);
					}
					else
					{
						bool flag2 = !this.on;
						if (flag2)
						{
							MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_out");
							this.on = true;
							this.warning.SetActive(true);
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

		// Token: 0x040000C4 RID: 196
		public bool on = true;

		// Token: 0x040000C5 RID: 197
		public GameObject warning;

		// Token: 0x040000C6 RID: 198
		public GameObject trigger;

		// Token: 0x040000C7 RID: 199
		public Raycast raycast;

		// Token: 0x040000C8 RID: 200
		private FsmString GUIinteraction;

		// Token: 0x040000C9 RID: 201
		private FsmBool GUIuse;

		// Token: 0x040000CA RID: 202
		private bool mouseOver;
	}
}
