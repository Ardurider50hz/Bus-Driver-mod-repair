using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000011 RID: 17
	public class IgnitionOLD : MonoBehaviour
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00007BF8 File Offset: 0x00005DF8
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			bool flag = !this.keyout;
			if (flag)
			{
				this.electricity.SetActive(true);
				this.key.SetActive(true);
				this.keyhole.transform.localEulerAngles = new Vector3(0f, -57f, 0f);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00007C88 File Offset: 0x00005E88
		public void Update()
		{
			bool flag = this.keyout && this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
			if (flag)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "IGNITION";
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "ignition_keys");
					MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_in");
					this.keyout = false;
					this.key.SetActive(true);
					this.keyhole.transform.localEulerAngles = new Vector3(0f, -57f, 0f);
					this.electricity.SetActive(true);
				}
			}
			else
			{
				bool hit = this.raycast.GetHit(this.trigger.GetComponent<SphereCollider>());
				if (hit)
				{
					this.GUIuse.Value = true;
					this.GUIinteraction.Value = "IGNITION";
					bool mouseButtonDown2 = Input.GetMouseButtonDown(0);
					if (mouseButtonDown2)
					{
						MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "carkeys_out");
						this.keyout = true;
						this.key.SetActive(false);
						this.keyhole.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
						this.electricity.SetActive(false);
					}
				}
			}
		}

		// Token: 0x040000BC RID: 188
		public bool keyout = true;

		// Token: 0x040000BD RID: 189
		public GameObject key;

		// Token: 0x040000BE RID: 190
		public GameObject keyhole;

		// Token: 0x040000BF RID: 191
		public GameObject electricity;

		// Token: 0x040000C0 RID: 192
		public GameObject trigger;

		// Token: 0x040000C1 RID: 193
		public Raycast raycast;

		// Token: 0x040000C2 RID: 194
		private FsmString GUIinteraction;

		// Token: 0x040000C3 RID: 195
		private FsmBool GUIuse;
	}
}
