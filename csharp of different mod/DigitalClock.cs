using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000028 RID: 40
	public class DigitalClock : MonoBehaviour
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x0000A3B8 File Offset: 0x000085B8
		private void Start()
		{
			this.RawHours = GameObject.Find("MAP").transform.Find("SUN/Pivot/SUN").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("Time");
			this.isOddHour = GameObject.Find("YARD").transform.Find("Building/Dynamics/SuomiClock/Clock").GetComponents<PlayMakerFSM>()[1].FsmVariables.GetFsmBool("SecondHour");
			this.RawMinutes = PlayMakerGlobals.Instance.Variables.GetFsmFloat("ClockMinutes");
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000A448 File Offset: 0x00008648
		private void Update()
		{
			this.Wait = Mathf.Clamp(this.Wait -= Time.deltaTime, 0f, float.PositiveInfinity);
			bool flag = (double)this.Wait <= 0.0;
			if (flag)
			{
				this.secondindi = ((!this.issecond) ? " : " : " : ");
				this.issecond = !this.issecond;
				this.Wait = this.WaitToNextSecond;
			}
			this.Hours = (float)((!this.isOddHour.Value) ? this.RawHours.Value : (this.RawHours.Value + 1));
			this.Hours = Mathf.Clamp(this.Hours, 0f, 23f);
			bool flag2 = !this.HasPower;
			if (!flag2)
			{
				this.Minutes = Mathf.Clamp(Mathf.RoundToInt(this.RawMinutes.Value - 0.5f), 0, 59);
				this.DigiTime = this.Hours.ToString("00") + this.secondindi + this.Minutes.ToString("00");
				base.gameObject.GetComponent<TextMesh>().text = this.DigiTime;
			}
		}

		// Token: 0x0400015C RID: 348
		public bool HasPower = true;

		// Token: 0x0400015D RID: 349
		public string DigiTime = "00 : 00";

		// Token: 0x0400015E RID: 350
		public float WaitToNextSecond = 0.5f;

		// Token: 0x0400015F RID: 351
		private FsmInt RawHours;

		// Token: 0x04000160 RID: 352
		private float Hours;

		// Token: 0x04000161 RID: 353
		private int Minutes;

		// Token: 0x04000162 RID: 354
		private FsmFloat RawMinutes;

		// Token: 0x04000163 RID: 355
		private string secondindi = ":";

		// Token: 0x04000164 RID: 356
		private bool issecond;

		// Token: 0x04000165 RID: 357
		private FsmBool isOddHour;

		// Token: 0x04000166 RID: 358
		private float Wait;
	}
}
