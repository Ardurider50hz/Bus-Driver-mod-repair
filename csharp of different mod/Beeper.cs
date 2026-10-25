using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200001E RID: 30
	public class Beeper : MonoBehaviour
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00008DDC File Offset: 0x00006FDC
		private void Start()
		{
			this.BusStops = new GameObject[]
			{
				GameObject.Find("MAP").transform.Find("BusStop").gameObject,
				GameObject.Find("MAP").transform.GetChild(21).gameObject,
				GameObject.Find("MAP").transform.Find("TrafficSigns/5/sign_pole 21/sign_bus").gameObject,
				GameObject.Find("MAP").transform.Find("TrafficSigns/5/sign_pole 22/sign_bus").gameObject
			};
			this.PeopleBus = new GameObject[]
			{
				BUS.busparts.transform.Find("PeopleBusZone0").gameObject,
				BUS.busparts.transform.Find("PeopleBusZone1").gameObject,
				BUS.busparts.transform.Find("PeopleBusZone2").gameObject
			};
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00008ED8 File Offset: 0x000070D8
		private void Update()
		{
			bool flag = this.CheckForActivePassenger();
			if (flag)
			{
				foreach (GameObject gameObject in this.BusStops)
				{
					float num = Vector3.Distance(base.transform.position, gameObject.transform.position);
					bool flag2 = num <= this.triggerDistance;
					if (flag2)
					{
						bool flag3 = this.lastTriggeredStop != gameObject;
						if (flag3)
						{
							this.PlaySound();
							this.lastTriggeredStop = gameObject;
						}
						return;
					}
				}
				this.lastTriggeredStop = null;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00008F6C File Offset: 0x0000716C
		private void PlaySound()
		{
			MasterAudio.PlaySound3DAndForget("CarFoley", BUS.busparts.transform, false, 1f, null, 0f, "bus_buzzer");
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00008FA8 File Offset: 0x000071A8
		private bool CheckForActivePassenger()
		{
			foreach (GameObject gameObject in this.PeopleBus)
			{
				Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(true);
				foreach (Transform transform in componentsInChildren)
				{
					bool flag = transform.gameObject.activeSelf && transform.name.StartsWith("Stranger");
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000109 RID: 265
		public GameObject[] BusStops;

		// Token: 0x0400010A RID: 266
		public GameObject[] PeopleBus;

		// Token: 0x0400010B RID: 267
		public float triggerDistance = 50f;

		// Token: 0x0400010C RID: 268
		private GameObject lastTriggeredStop = null;
	}
}
