using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000019 RID: 25
	public class BrakeLights : MonoBehaviour
	{
		// Token: 0x06000076 RID: 118 RVA: 0x000088DB File Offset: 0x00006ADB
		private void Start()
		{
			this.brakeLights = BUS.busparts.transform.Find("BrakeLights").gameObject;
			this.acc = BUS.bus.GetComponent<AxisCarController>();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00008910 File Offset: 0x00006B10
		private void Update()
		{
			this.brakeLights.SetActive(this.acc.brakeInput >= 0.04f);
			this.BrakeLightsDefault.SetActive(this.acc.brakeInput >= 0.04f);
		}

		// Token: 0x040000F2 RID: 242
		private GameObject brakeLights;

		// Token: 0x040000F3 RID: 243
		private AxisCarController acc;

		// Token: 0x040000F4 RID: 244
		public GameObject BrakeLightsDefault;
	}
}
