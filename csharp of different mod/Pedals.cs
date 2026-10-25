using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000023 RID: 35
	public class Pedals : MonoBehaviour
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000099DC File Offset: 0x00007BDC
		private void Start()
		{
			this.PedalGas = base.transform.Find("pedal_throttle");
			this.PedalBrake = base.transform.Find("pedal_brake");
			this.PedalClutch = base.transform.Find("pedal_clutch");
			this.acc = BUS.bus.GetComponent<AxisCarController>();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00009A3C File Offset: 0x00007C3C
		private void Update()
		{
			this.PedalGas.localEulerAngles = new Vector3(Mathf.Lerp(this.PedalGas.localEulerAngles.x, Mathf.Lerp(0f, 15f, -this.acc.throttleInput), Time.deltaTime * 12f), 180f, 180f);
			this.PedalBrake.localEulerAngles = new Vector3(Mathf.Lerp(this.PedalBrake.localEulerAngles.x, Mathf.Lerp(0f, 15f, -this.acc.brakeInput), Time.deltaTime * 12f), 180f, 180f);
			this.PedalClutch.localEulerAngles = new Vector3(Mathf.Lerp(this.PedalClutch.localEulerAngles.x, Mathf.Lerp(0f, 15f, -this.acc.clutchInput), Time.deltaTime * 12f), 180f, 180f);
		}

		// Token: 0x04000135 RID: 309
		public Transform PedalGas;

		// Token: 0x04000136 RID: 310
		public Transform PedalBrake;

		// Token: 0x04000137 RID: 311
		public Transform PedalClutch;

		// Token: 0x04000138 RID: 312
		private AxisCarController acc;
	}
}
