using System;
using HutongGames.PlayMaker;

namespace BUS
{
	// Token: 0x0200002B RID: 43
	public class CheckSeatbeltBool : FsmStateAction
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000AC30 File Offset: 0x00008E30
		public override void Reset()
		{
			this.everyFrame = false;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000AC3C File Offset: 0x00008E3C
		public override void OnEnter()
		{
			bool flag = !this.DoSetup();
			if (flag)
			{
				base.Finish();
			}
			else
			{
				this.CheckSeatbeltStatus();
				bool flag2 = !this.everyFrame;
				if (flag2)
				{
					base.Finish();
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000AC80 File Offset: 0x00008E80
		public override void OnUpdate()
		{
			bool flag = this.everyFrame;
			if (flag)
			{
				this.CheckSeatbeltStatus();
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		private bool DoSetup()
		{
			return true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		private void CheckSeatbeltStatus()
		{
			bool flag = this.seatbeltScript != null;
			if (flag)
			{
				bool seatbeltOn = this.seatbeltScript.seatbeltOn;
				if (seatbeltOn)
				{
					base.Fsm.Event("RESET");
				}
			}
		}

		// Token: 0x04000182 RID: 386
		public bool everyFrame;

		// Token: 0x04000183 RID: 387
		public Seatbelt seatbeltScript;
	}
}
