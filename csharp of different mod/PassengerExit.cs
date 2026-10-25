using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000007 RID: 7
	public class PassengerExit : MonoBehaviour
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000062BB File Offset: 0x000044BB
		private void Start()
		{
			this.systems = Object.FindObjectsOfType<PassengerSystem>();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000062CC File Offset: 0x000044CC
		public void Update()
		{
			this.CheckForZoneChange();
			bool flag = this.busInDifferentZone && !this.isWaitingForExit;
			if (flag)
			{
				bool flag2 = this.CurrentPassengerSystem != null && this.CurrentPassengerSystem.doorOpen.open && this.CurrentPassengerSystem.dt.differentialSpeed <= 0.4f;
				if (flag2)
				{
					this.isWaitingForExit = true;
					base.StartCoroutine(this.ExitPassengerAfterDelay());
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00006354 File Offset: 0x00004554
		private void CheckForZoneChange()
		{
			foreach (PassengerSystem passengerSystem in this.systems)
			{
				bool busInTrigger = passengerSystem.BusInTrigger;
				if (busInTrigger)
				{
					bool flag = this.CurrentPassengerSystem != passengerSystem;
					if (flag)
					{
						this.busInDifferentZone = true;
					}
					else
					{
						this.busInDifferentZone = false;
					}
					break;
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000063B1 File Offset: 0x000045B1
		private IEnumerator ExitPassengerAfterDelay()
		{
			yield return new WaitForSeconds(1f);
			bool flag = Random.value < this.exitChance;
			if (flag)
			{
				this.RemoveRandomPassengers();
			}
			this.isWaitingForExit = false;
			yield break;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000063C0 File Offset: 0x000045C0
		private void RemoveRandomPassengers()
		{
			bool flag = this.CurrentPassengerSystem == null;
			if (!flag)
			{
				Transform[] componentsInChildren = this.CurrentPassengerSystem.PeopleBusZone.GetComponentsInChildren<Transform>(true);
				bool flag2 = this.activePassengers.Count == 0;
				if (flag2)
				{
					this.activePassengers.Clear();
					foreach (Transform transform in componentsInChildren)
					{
						bool flag3 = transform.gameObject.activeSelf && transform.name.StartsWith("Stranger");
						if (flag3)
						{
							this.activePassengers.Add(transform);
						}
					}
				}
				int num = Mathf.Min(this.activePassengers.Count, Random.Range(1, 4));
				for (int j = 0; j < num; j++)
				{
					int index = Random.Range(0, this.activePassengers.Count);
					this.activePassengers[index].gameObject.SetActive(false);
					this.activePassengers.RemoveAt(index);
					this.ClaimMoney.TotalMoneyMade += 40f;
				}
			}
		}

		// Token: 0x0400004B RID: 75
		public PassengerSystem CurrentPassengerSystem;

		// Token: 0x0400004C RID: 76
		private bool busInDifferentZone = false;

		// Token: 0x0400004D RID: 77
		private bool isWaitingForExit = false;

		// Token: 0x0400004E RID: 78
		private PassengerSystem[] systems;

		// Token: 0x0400004F RID: 79
		private List<Transform> activePassengers = new List<Transform>();

		// Token: 0x04000050 RID: 80
		private float exitChance = 0.1f;

		// Token: 0x04000051 RID: 81
		public OnEnterZone ClaimMoney;
	}
}
