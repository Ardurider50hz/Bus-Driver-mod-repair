using System;
using System.Collections;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200001D RID: 29
	public class HomelessPuke : MonoBehaviour
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00008CEC File Offset: 0x00006EEC
		private void OnEnable()
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00008CF0 File Offset: 0x00006EF0
		private void Start()
		{
			this.ThrowUp1 = base.transform.Find("ThrowUp1").GetComponent<AudioSource>();
			this.ThrowUp2 = base.transform.Find("ThrowUp2").GetComponent<AudioSource>();
			this.ThrowUp = base.transform.Find("Particle").GetComponent<EllipsoidParticleEmitter>();
			this.rig = BUS.bus.GetComponent<Rigidbody>();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00008D60 File Offset: 0x00006F60
		private void Update()
		{
			bool flag = !this.PukedHimself;
			if (flag)
			{
				bool flag2 = this.rig.velocity.magnitude * 3.6f >= 100f;
				if (flag2)
				{
					base.StartCoroutine(this.HomelessPukeHimself());
					this.PukedHimself = true;
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00008DBA File Offset: 0x00006FBA
		private IEnumerator HomelessPukeHimself()
		{
			bool CurrentAudio = Random.Range(0, 100) >= 50;
			bool flag = CurrentAudio;
			if (flag)
			{
				this.ThrowUp2.Play();
			}
			else
			{
				this.ThrowUp1.Play();
			}
			this.ThrowUp.emit = true;
			yield return new WaitForSeconds(1.7f);
			this.ThrowUp.emit = false;
			yield break;
		}

		// Token: 0x04000104 RID: 260
		private AudioSource ThrowUp1;

		// Token: 0x04000105 RID: 261
		private AudioSource ThrowUp2;

		// Token: 0x04000106 RID: 262
		private EllipsoidParticleEmitter ThrowUp;

		// Token: 0x04000107 RID: 263
		private bool PukedHimself = false;

		// Token: 0x04000108 RID: 264
		private Rigidbody rig;
	}
}
