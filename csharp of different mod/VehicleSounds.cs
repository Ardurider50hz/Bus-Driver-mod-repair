using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200001F RID: 31
	public class VehicleSounds : MonoBehaviour
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000904A File Offset: 0x0000724A
		public void Start()
		{
			this.playerInCar = GameObject.Find("PLAYER").GetComponents<PlayMakerFSM>()[1].FsmVariables.GetFsmBool("PlayerInCar");
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00009074 File Offset: 0x00007274
		public void Update()
		{
			bool value = this.playerInCar.Value;
			if (value)
			{
				this.start1.cutoffFrequency = this.disortion;
				this.start2.cutoffFrequency = this.disortion;
				this.start3.cutoffFrequency = this.disortion;
				this.shutoff.cutoffFrequency = this.disortion;
				this.engineSound.cutoffFrequency = this.disortion;
				this.CooldownTick.cutoffFrequency = this.disortion;
			}
			else
			{
				this.start1.cutoffFrequency = 25000f;
				this.start2.cutoffFrequency = 25000f;
				this.start3.cutoffFrequency = 25000f;
				this.shutoff.cutoffFrequency = 25000f;
				this.engineSound.cutoffFrequency = 25000f;
				this.CooldownTick.cutoffFrequency = 25000f;
			}
		}

		// Token: 0x0400010D RID: 269
		private FsmBool playerInCar;

		// Token: 0x0400010E RID: 270
		public float disortion = 400f;

		// Token: 0x0400010F RID: 271
		public AudioLowPassFilter start1;

		// Token: 0x04000110 RID: 272
		public AudioLowPassFilter start2;

		// Token: 0x04000111 RID: 273
		public AudioLowPassFilter start3;

		// Token: 0x04000112 RID: 274
		public AudioLowPassFilter shutoff;

		// Token: 0x04000113 RID: 275
		public AudioLowPassFilter engineSound;

		// Token: 0x04000114 RID: 276
		public AudioLowPassFilter CooldownTick;
	}
}
