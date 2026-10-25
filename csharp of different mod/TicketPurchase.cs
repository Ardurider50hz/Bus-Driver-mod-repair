using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000005 RID: 5
	public class TicketPurchase : MonoBehaviour
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00005D48 File Offset: 0x00003F48
		public void Start()
		{
			this.GUIbuy = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIbuy");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.trigger = base.GetComponent<SphereCollider>();
			this.raycast = BUS.raycast;
			Object.Destroy(base.GetComponent<PlayMakerFSM>());
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger);
			if (hit)
			{
				bool flag = this.CanBuyTicket && this.ActiveZoneIndex != -1;
				if (flag)
				{
					PassengerSystem[] array = Object.FindObjectsOfType<PassengerSystem>();
					PassengerSystem passengerSystem = null;
					foreach (PassengerSystem passengerSystem2 in array)
					{
						bool flag2 = passengerSystem2.ZoneIndex == this.ActiveZoneIndex;
						if (flag2)
						{
							passengerSystem = passengerSystem2;
							break;
						}
					}
					bool flag3 = passengerSystem != null && passengerSystem.HasPassengers();
					if (flag3)
					{
						this.GUIbuy.Value = true;
						this.GUIinteraction.Value = string.Format("GIVE BUS TICKET {0} MK  (SALARY: 15)", this.Price);
						this.mouseOver = true;
						bool mouseButtonDown = Input.GetMouseButtonDown(0);
						if (mouseButtonDown)
						{
							this.money.TotalMoneyMade += this.Price - 10f;
							passengerSystem.MovePassenger();
						}
					}
					else
					{
						this.CanBuyTicket = false;
					}
				}
			}
			else
			{
				bool flag4 = this.mouseOver;
				if (flag4)
				{
					this.mouseOver = false;
					this.GUIbuy.Value = false;
					this.GUIinteraction.Value = string.Empty;
				}
			}
		}

		// Token: 0x04000036 RID: 54
		public bool CanBuyTicket = false;

		// Token: 0x04000037 RID: 55
		public int ActiveZoneIndex = -1;

		// Token: 0x04000038 RID: 56
		private Collider trigger;

		// Token: 0x04000039 RID: 57
		private Raycast raycast;

		// Token: 0x0400003A RID: 58
		private FsmString GUIinteraction;

		// Token: 0x0400003B RID: 59
		private bool mouseOver;

		// Token: 0x0400003C RID: 60
		private FsmBool GUIbuy;

		// Token: 0x0400003D RID: 61
		public OnEnterZone money;

		// Token: 0x0400003E RID: 62
		public float Price = 25f;
	}
}
