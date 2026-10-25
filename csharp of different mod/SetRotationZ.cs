using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200002C RID: 44
	public class SetRotationZ : FsmStateAction
	{
		// Token: 0x060000CB RID: 203 RVA: 0x0000AD03 File Offset: 0x00008F03
		public override void Reset()
		{
			this.gameObject = null;
			this.zAngle = 0f;
			this.space = 1;
			this.everyFrame = false;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000AD2C File Offset: 0x00008F2C
		public override void OnEnter()
		{
			this.ApplyRotation();
			bool flag = !this.everyFrame;
			if (flag)
			{
				base.Finish();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000AD58 File Offset: 0x00008F58
		public override void OnUpdate()
		{
			bool flag = this.everyFrame;
			if (flag)
			{
				this.ApplyRotation();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000AD7C File Offset: 0x00008F7C
		private void ApplyRotation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			bool flag = ownerDefaultTarget != null;
			if (flag)
			{
				Vector3 localEulerAngles = ownerDefaultTarget.transform.localEulerAngles;
				localEulerAngles.z = this.zAngle.Value;
				bool flag2 = this.space == 1;
				if (flag2)
				{
					ownerDefaultTarget.transform.localEulerAngles = localEulerAngles;
				}
				else
				{
					ownerDefaultTarget.transform.eulerAngles = localEulerAngles;
				}
			}
		}

		// Token: 0x04000184 RID: 388
		public FsmOwnerDefault gameObject;

		// Token: 0x04000185 RID: 389
		public FsmFloat zAngle;

		// Token: 0x04000186 RID: 390
		public Space space = 1;

		// Token: 0x04000187 RID: 391
		public bool everyFrame;
	}
}
