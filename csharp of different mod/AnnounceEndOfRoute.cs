using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000008 RID: 8
	public class AnnounceEndOfRoute : MonoBehaviour
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0000651B File Offset: 0x0000471B
		private void Start()
		{
			this.Use = BUS.busparts.transform.Find("trigger_EndRoute").GetComponent<Collider>();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00006540 File Offset: 0x00004740
		private void Update()
		{
			bool flag = !this.isExiting;
			if (flag)
			{
				this.CacheActivePassengers();
			}
			bool flag2 = this.activePassengers.Count > 0 && this.raycast.GetHit(this.Use) && !this.isExiting;
			if (flag2)
			{
				this.guiUse.Value = true;
				this.guiInteraction.Value = "ANNOUNCE END OF ROUTE";
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool open = this.door.open;
					if (open)
					{
						base.StartCoroutine(this.DisablePassengersOneByOne());
					}
				}
			}
			else
			{
				bool flag3 = this.mouseOver;
				if (flag3)
				{
					this.guiUse.Value = false;
					this.guiInteraction.Value = string.Empty;
					this.mouseOver = false;
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000661B File Offset: 0x0000481B
		private IEnumerator DisablePassengersOneByOne()
		{
			this.isExiting = true;
			foreach (Transform passenger in new List<Transform>(this.activePassengers))
			{
				bool flag = passenger != null && passenger.gameObject.activeSelf;
				if (flag)
				{
					passenger.gameObject.SetActive(false);
					this.activePassengers.Remove(passenger);
					yield return new WaitForSeconds(this.delayBetweenExits);
				}
				passenger = null;
			}
			List<Transform>.Enumerator enumerator = default(List<Transform>.Enumerator);
			this.isExiting = false;
			yield break;
			yield break;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000662C File Offset: 0x0000482C
		private void CacheActivePassengers()
		{
			this.activePassengers.Clear();
			for (int i = 0; i <= 4; i++)
			{
				Transform transform = BUS.busparts.transform.Find(string.Format("PeopleBusZone{0}", i));
				bool flag = transform != null;
				if (flag)
				{
					Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>(true);
					foreach (Transform transform2 in componentsInChildren)
					{
						bool flag2 = transform2.gameObject.activeSelf && transform2.name.StartsWith("Stranger") && !this.activePassengers.Contains(transform2);
						if (flag2)
						{
							this.activePassengers.Add(transform2);
						}
					}
				}
			}
		}

		// Token: 0x04000052 RID: 82
		public float delayBetweenExits = 0.9f;

		// Token: 0x04000053 RID: 83
		private bool isExiting = false;

		// Token: 0x04000054 RID: 84
		private List<Transform> activePassengers = new List<Transform>();

		// Token: 0x04000055 RID: 85
		private Raycast raycast = BUS.raycast;

		// Token: 0x04000056 RID: 86
		private FsmBool guiUse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");

		// Token: 0x04000057 RID: 87
		private FsmString guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");

		// Token: 0x04000058 RID: 88
		private bool mouseOver = false;

		// Token: 0x04000059 RID: 89
		private Collider Use;

		// Token: 0x0400005A RID: 90
		public DoorOpen door;
	}
}
