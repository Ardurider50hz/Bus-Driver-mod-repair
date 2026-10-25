using System;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000032 RID: 50
	public class Raycast : MonoBehaviour
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0000B867 File Offset: 0x00009A67
		private void Start()
		{
			this.layerMask = LayerMask.GetMask(new string[]
			{
				"Dashboard"
			});
			this.hitInfo = default(RaycastHit);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000B890 File Offset: 0x00009A90
		private void FixedUpdate()
		{
			bool flag = Camera.main != null;
			if (flag)
			{
				this.hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), ref this.hitInfo, this.rayDistance, this.layerMask);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000B8D9 File Offset: 0x00009AD9
		public bool GetHit(Collider collider)
		{
			return this.hasHit && this.hitInfo.collider == collider;
		}

		// Token: 0x040001B8 RID: 440
		public RaycastHit hitInfo;

		// Token: 0x040001B9 RID: 441
		public bool hasHit = false;

		// Token: 0x040001BA RID: 442
		public float rayDistance = 2f;

		// Token: 0x040001BB RID: 443
		public int layerMask;
	}
}
