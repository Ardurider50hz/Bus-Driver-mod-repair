using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000009 RID: 9
	public class KickPassenger : MonoBehaviour
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000677B File Offset: 0x0000497B
		private void Start()
		{
			this.Use = base.GetComponent<Collider>();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000678C File Offset: 0x0000498C
		private void Update()
		{
			bool flag = this.raycast.GetHit(this.Use) && !this.isKicking;
			if (flag)
			{
				this.guiUse.Value = true;
				this.guiInteraction.Value = "KICK PASSENGER";
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					this.AttemptKick();
				}
				else
				{
					bool keyDown = cInput.GetKeyDown("Hit");
					if (keyDown)
					{
						this.AttemptKick();
						string text = "PlayerMisc";
						Transform transform = base.transform;
						float num = 1f;
						string text2 = this.CurrentHit[Random.Range(0, this.CurrentHit.Length)];
						MasterAudio.PlaySound3DFollowTransform(text, transform, num, null, 0f, text2, false, false);
					}
				}
			}
			else
			{
				bool flag2 = this.mouseOver;
				if (flag2)
				{
					this.guiUse.Value = false;
					this.guiInteraction.Value = string.Empty;
					this.mouseOver = false;
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00006888 File Offset: 0x00004A88
		private void AttemptKick()
		{
			this.isKicking = true;
			int num = Random.Range(0, this.Kicks);
			bool flag = num >= this.Kicks - 2;
			if (flag)
			{
				base.transform.parent.gameObject.SetActive(false);
				this.guiInteraction.Value = "KICK PASSENGER";
			}
			else
			{
				this.guiInteraction.Value = "FAILING TO KICK";
			}
			this.isKicking = false;
		}

		// Token: 0x0400005B RID: 91
		private bool isKicking = false;

		// Token: 0x0400005C RID: 92
		private Raycast raycast = BUS.raycast;

		// Token: 0x0400005D RID: 93
		private FsmBool guiUse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");

		// Token: 0x0400005E RID: 94
		private FsmString guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");

		// Token: 0x0400005F RID: 95
		private bool mouseOver = false;

		// Token: 0x04000060 RID: 96
		public int Kicks = 4;

		// Token: 0x04000061 RID: 97
		private Collider Use;

		// Token: 0x04000062 RID: 98
		private string[] CurrentHit = new string[]
		{
			"punch0",
			"punch1",
			"punch2",
			"punch3",
			"punch4"
		};
	}
}
