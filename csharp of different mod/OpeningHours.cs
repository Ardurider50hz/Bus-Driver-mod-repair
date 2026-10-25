using System;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000025 RID: 37
	public class OpeningHours : MonoBehaviour
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00009EB8 File Offset: 0x000080B8
		private void OnEnable()
		{
			this.Hours = PlayMakerExtensions.GetPlayMaker(GameObject.Find("MAP").transform.Find("SUN/Pivot/SUN"), "Clock").FsmVariables.GetFsmInt("Time");
			this.player = GameObject.Find("PLAYER").transform;
		}

		// Token: 0x04000145 RID: 325
		private FsmInt Hours;

		// Token: 0x04000146 RID: 326
		public GameObject Open;

		// Token: 0x04000147 RID: 327
		public DoorScript door;

		// Token: 0x04000148 RID: 328
		public OnEnterZone moneyZone;

		// Token: 0x04000149 RID: 329
		public float Distance = 250f;

		// Token: 0x0400014A RID: 330
		private Transform player;
	}
}
