using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000026 RID: 38
	public class BusInZone : MonoBehaviour
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00009F27 File Offset: 0x00008127
		private void Start()
		{
			this.bus = BUS.bus;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00009F38 File Offset: 0x00008138
		private void OnTriggerEnter(Collider other)
		{
			bool flag = this.IsBus(other.gameObject) && !this.BusInTrigger;
			if (flag)
			{
				this.BusInTrigger = true;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00009F70 File Offset: 0x00008170
		private void OnTriggerExit(Collider other)
		{
			bool flag = this.IsBus(other.gameObject);
			if (flag)
			{
				this.BusInTrigger = false;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00009F98 File Offset: 0x00008198
		private bool IsBus(GameObject obj)
		{
			return obj == this.bus || obj.transform.IsChildOf(this.bus.transform);
		}

		// Token: 0x0400014B RID: 331
		public bool BusInTrigger = false;

		// Token: 0x0400014C RID: 332
		private GameObject bus;
	}
}
