using System;
using System.Collections;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000015 RID: 21
	public class Blinkers : MonoBehaviour
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00008372 File Offset: 0x00006572
		private void TurnOn()
		{
			base.StartCoroutine(this.HazardLights());
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00008382 File Offset: 0x00006582
		private IEnumerator HazardLights()
		{
			while (this.hazards)
			{
				this.lightL.SetActive(true);
				this.lightR.SetActive(true);
				this.dashIndicator.SetActive(true);
				MasterAudio.PlaySound3DAndForget("CarFoley", base.transform, false, 0.5f, null, 0f, "turn_signal_1");
				yield return this.seconds;
				this.lightL.SetActive(false);
				this.lightR.SetActive(false);
				this.dashIndicator.SetActive(false);
				MasterAudio.PlaySound3DAndForget("CarFoley", base.transform, false, 0.5f, null, 0f, "turn_signal_2");
				yield return this.seconds;
			}
			yield break;
		}

		// Token: 0x040000D9 RID: 217
		public bool hazards = false;

		// Token: 0x040000DA RID: 218
		public bool CarON = false;

		// Token: 0x040000DB RID: 219
		private WaitForSeconds seconds = new WaitForSeconds(0.4f);

		// Token: 0x040000DC RID: 220
		public GameObject lightR;

		// Token: 0x040000DD RID: 221
		public GameObject lightL;

		// Token: 0x040000DE RID: 222
		public GameObject dashIndicator;
	}
}
