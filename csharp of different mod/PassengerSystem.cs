using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000006 RID: 6
	public class PassengerSystem : MonoBehaviour
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00005F20 File Offset: 0x00004120
		private void Start()
		{
			this.Hours = PlayMakerExtensions.GetPlayMaker(GameObject.Find("MAP").transform.Find("SUN/Pivot/SUN"), "Color").FsmVariables.GetFsmInt("Time");
			this.bus = BUS.bus;
			this.BusZone = base.transform.Find("People").gameObject;
			base.StartCoroutine(this.EnableRandomPassengers());
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00005F9C File Offset: 0x0000419C
		private void Update()
		{
			bool flag = this.BusInTrigger && this.dt.differentialSpeed <= 0.4f && this.doorOpen.open;
			if (flag)
			{
				this.Tickets.CanBuyTicket = true;
				this.Tickets.ActiveZoneIndex = this.ZoneIndex;
			}
			else
			{
				bool flag2 = this.Tickets.ActiveZoneIndex == this.ZoneIndex;
				if (flag2)
				{
					this.Tickets.CanBuyTicket = false;
					this.Tickets.ActiveZoneIndex = -1;
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00006028 File Offset: 0x00004228
		private void OnTriggerEnter(Collider other)
		{
			bool flag = this.IsPlayerCharacter(other.gameObject) && !this.BusInTrigger;
			if (flag)
			{
				this.BusInTrigger = true;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00006060 File Offset: 0x00004260
		private void OnTriggerExit(Collider other)
		{
			bool flag = this.IsPlayerCharacter(other.gameObject);
			if (flag)
			{
				this.BusInTrigger = false;
				bool flag2 = this.Tickets.ActiveZoneIndex == this.ZoneIndex;
				if (flag2)
				{
					this.Tickets.ActiveZoneIndex = -1;
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000060AC File Offset: 0x000042AC
		private bool IsPlayerCharacter(GameObject obj)
		{
			return obj == this.bus || obj.transform.IsChildOf(this.bus.transform);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000060E8 File Offset: 0x000042E8
		public void MovePassenger()
		{
			Transform[] componentsInChildren = this.BusZone.GetComponentsInChildren<Transform>(true);
			Transform[] componentsInChildren2 = this.PeopleBusZone.GetComponentsInChildren<Transform>(true);
			foreach (Transform transform in componentsInChildren)
			{
				bool flag = transform.gameObject.activeSelf && transform.name.StartsWith("Stranger");
				if (flag)
				{
					Transform transform2 = null;
					foreach (Transform transform3 in componentsInChildren2)
					{
						bool flag2 = transform3.name == transform.name;
						if (flag2)
						{
							transform2 = transform3;
							break;
						}
					}
					bool flag3 = transform2 != null;
					if (flag3)
					{
						transform.gameObject.SetActive(false);
						transform2.gameObject.SetActive(true);
					}
					break;
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000061C8 File Offset: 0x000043C8
		public bool HasPassengers()
		{
			Transform[] componentsInChildren = this.BusZone.GetComponentsInChildren<Transform>(true);
			foreach (Transform transform in componentsInChildren)
			{
				bool flag = transform.gameObject.activeSelf && transform.name.StartsWith("Stranger");
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00006230 File Offset: 0x00004430
		public void DisableAllStrangersInBusZone()
		{
			Transform[] componentsInChildren = this.BusZone.GetComponentsInChildren<Transform>(true);
			foreach (Transform transform in componentsInChildren)
			{
				bool flag = transform.name.StartsWith("Stranger");
				if (flag)
				{
					transform.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00006287 File Offset: 0x00004487
		private IEnumerator EnableRandomPassengers()
		{
			while (BUS.bus != null)
			{
				yield return new WaitForSeconds(140f);
				bool flag = this.Hours.Value >= 2 && this.Hours.Value <= 24;
				if (flag)
				{
					Transform[] busZoneChildren = this.BusZone.GetComponentsInChildren<Transform>(true);
					Transform[] peopleBusZoneChildren = this.PeopleBusZone.GetComponentsInChildren<Transform>(true);
					List<Transform> inactiveStrangers = new List<Transform>();
					Transform[] array = busZoneChildren;
					int i = 0;
					while (i < array.Length)
					{
						Transform child = array[i];
						bool flag2 = !child.gameObject.activeSelf && child.name.StartsWith("Stranger");
						if (!flag2)
						{
							goto IL_28F;
						}
						bool isInBus = false;
						foreach (Transform busPassenger in peopleBusZoneChildren)
						{
							bool flag3 = busPassenger.gameObject.activeSelf && busPassenger.name == child.name;
							if (flag3)
							{
								isInBus = true;
								break;
							}
							busPassenger = null;
						}
						Transform[] array2 = null;
						bool flag4 = isInBus;
						if (!flag4)
						{
							bool flag5 = child.name == "Stranger5";
							if (flag5)
							{
								inactiveStrangers.Add(child);
								this.IsRareNPC = true;
							}
							else
							{
								bool isSmallBusStop = this.IsSmallBusStop;
								if (isSmallBusStop)
								{
									bool flag6 = Random.Range(0, 100) >= 50;
									if (flag6)
									{
										inactiveStrangers.Add(child);
									}
								}
								else
								{
									bool isSignStop = this.IsSignStop;
									if (isSignStop)
									{
										bool flag7 = Random.Range(0, 100) >= 84;
										if (flag7)
										{
											inactiveStrangers.Add(child);
										}
									}
									else
									{
										inactiveStrangers.Add(child);
									}
								}
							}
							goto IL_28F;
						}
						IL_297:
						i++;
						continue;
						IL_28F:
						child = null;
						goto IL_297;
					}
					array = null;
					bool flag8 = inactiveStrangers.Count > 0;
					if (flag8)
					{
						Transform selectedStranger = inactiveStrangers[Random.Range(0, inactiveStrangers.Count)];
						bool flag9 = this.IsRareNPC && selectedStranger.name == "Stranger5";
						if (flag9)
						{
							selectedStranger.gameObject.SetActive(Random.Range(0, 100) >= 70);
						}
						else
						{
							selectedStranger.gameObject.SetActive(true);
						}
						selectedStranger = null;
					}
					this.IsRareNPC = false;
					busZoneChildren = null;
					peopleBusZoneChildren = null;
					inactiveStrangers = null;
				}
				else
				{
					this.DisableAllStrangersInBusZone();
				}
			}
			yield break;
		}

		// Token: 0x0400003F RID: 63
		public GameObject PeopleBusZone;

		// Token: 0x04000040 RID: 64
		private GameObject BusZone;

		// Token: 0x04000041 RID: 65
		public TicketPurchase Tickets;

		// Token: 0x04000042 RID: 66
		public int ZoneIndex;

		// Token: 0x04000043 RID: 67
		public bool BusInTrigger = false;

		// Token: 0x04000044 RID: 68
		private GameObject bus;

		// Token: 0x04000045 RID: 69
		public DoorOpen doorOpen;

		// Token: 0x04000046 RID: 70
		public Drivetrain dt;

		// Token: 0x04000047 RID: 71
		private bool IsRareNPC = false;

		// Token: 0x04000048 RID: 72
		public bool IsSmallBusStop = false;

		// Token: 0x04000049 RID: 73
		public bool IsSignStop = false;

		// Token: 0x0400004A RID: 74
		private FsmInt Hours;
	}
}
