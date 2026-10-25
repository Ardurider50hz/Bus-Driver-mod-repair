using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000031 RID: 49
	public class SaveData
	{
		// Token: 0x040001AD RID: 429
		public Vector3 buspos = new Vector3(-601.278f, 5.064445f, 1669.975f);

		// Token: 0x040001AE RID: 430
		public Vector3 busrot = new Vector3(358.998f, 294.2497f, 0.4566539f);

		// Token: 0x040001AF RID: 431
		public float TotalMoneyMade = 0f;

		// Token: 0x040001B0 RID: 432
		public bool suspWarnOn = false;

		// Token: 0x040001B1 RID: 433
		public bool engineWarnOn = true;

		// Token: 0x040001B2 RID: 434
		public bool preheatSCOn = false;

		// Token: 0x040001B3 RID: 435
		public float HandbrakeRot = 290f;

		// Token: 0x040001B4 RID: 436
		public bool busIntLight = false;

		// Token: 0x040001B5 RID: 437
		public bool busLowGear = false;

		// Token: 0x040001B6 RID: 438
		public bool foglightBus = false;

		// Token: 0x040001B7 RID: 439
		public int CurrentStageTown = 0;
	}
}
