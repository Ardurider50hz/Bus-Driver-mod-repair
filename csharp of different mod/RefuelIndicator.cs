using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200000F RID: 15
	public class RefuelIndicator : MonoBehaviour
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000077C8 File Offset: 0x000059C8
		public void Setup()
		{
			base.transform.name = "CITROENAX Gasoline";
			base.transform.SetParent(GameObject.Find("GUI").transform.Find("Indicators/Fluids"));
			base.transform.localPosition = Vector3.zero;
			base.transform.localEulerAngles = Vector3.zero;
			this.fuelBar = base.transform.GetChild(1);
			Object.Destroy(this.fuelBar.GetComponent<PlayMakerFSM>());
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00007851 File Offset: 0x00005A51
		private void Update()
		{
			this.fuelBar.localScale = new Vector3(this.fuelTank.fuelLevel / this.fuelTank.fuelCapacity, 1f, 1f);
		}

		// Token: 0x040000AE RID: 174
		private Transform fuelBar;

		// Token: 0x040000AF RID: 175
		public FuelTank fuelTank;
	}
}
