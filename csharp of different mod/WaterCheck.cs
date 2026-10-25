using System;
using System.Collections;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200000A RID: 10
	public class WaterCheck : MonoBehaviour
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000069A0 File Offset: 0x00004BA0
		private void Start()
		{
			this.waterCheck1 = GameObject.Find("WaterCheck1").transform;
			this.lamore = BUS.bus.transform;
			base.StartCoroutine(this.CheckWaterLevel());
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000069D5 File Offset: 0x00004BD5
		private IEnumerator CheckWaterLevel()
		{
			for (;;)
			{
				this.inWater = (base.transform.position.y < this.waterLevel && Vector3.Distance(this.lamore.position, this.waterCheck1.position) > 500f);
				yield return new WaitForSeconds(2f);
			}
			yield break;
		}

		// Token: 0x04000063 RID: 99
		public bool inWater = true;

		// Token: 0x04000064 RID: 100
		private float waterLevel = -10.5f;

		// Token: 0x04000065 RID: 101
		private Transform waterCheck1;

		// Token: 0x04000066 RID: 102
		private Transform lamore;
	}
}
