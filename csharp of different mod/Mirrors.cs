using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000018 RID: 24
	public class Mirrors : MonoBehaviour
	{
		// Token: 0x06000073 RID: 115 RVA: 0x0000881D File Offset: 0x00006A1D
		private void Start()
		{
			this.playerCurrentVehicle = PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle");
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000883C File Offset: 0x00006A3C
		private void Update()
		{
			bool active = this.playerCurrentVehicle.Value == "Bus";
			bool flag = this.playerCurrentVehicle.Value == "Bus";
			if (flag)
			{
				BUS.busparts.transform.Find("mirror_left").gameObject.SetActive(active);
				BUS.busparts.transform.Find("mirror_right").gameObject.SetActive(active);
				BUS.bus.transform.Find("BusInteractions/cam").gameObject.SetActive(active);
			}
		}

		// Token: 0x040000F1 RID: 241
		private FsmString playerCurrentVehicle;
	}
}
