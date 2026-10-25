using System;
using System.Collections;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200001C RID: 28
	public class CiganRyhat : MonoBehaviour
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00008CA2 File Offset: 0x00006EA2
		private void OnEnable()
		{
			base.StartCoroutine(this.CiganRyhatRandom());
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00008CB2 File Offset: 0x00006EB2
		private void Start()
		{
			base.StartCoroutine(this.CiganRyhatRandom());
			this.ciganAnim = base.transform.Find("skeleton").GetComponent<Animation>();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00008CDD File Offset: 0x00006EDD
		private IEnumerator CiganRyhatRandom()
		{
			while (this.ciganAnim != null)
			{
				yield return new WaitForSeconds(60f);
				bool flag = !this.ciganAnim.isPlaying;
				if (flag)
				{
					this.ciganAnim.Play("cigan");
				}
			}
			yield break;
		}

		// Token: 0x04000103 RID: 259
		private Animation ciganAnim;
	}
}
