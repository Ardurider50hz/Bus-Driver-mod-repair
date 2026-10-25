using System;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200001A RID: 26
	public class LaneCheckFSM : MonoBehaviour
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00008960 File Offset: 0x00006B60
		private void Start()
		{
			this.car = BUS.bus;
			this.rig = BUS.bus.GetComponent<Rigidbody>();
			this.blank = this.car.AddComponent<PlayMakerFSM>();
			this.blank.FsmName = "Steering";
			PlayMakerExtensions.AddVariable(this.blank, new FsmFloat("Lane"));
			this.LanePos = this.blank.FsmVariables.FindFsmFloat("Lane");
			PlayMakerExtensions.AddVariable(this.blank, new FsmFloat("SpeedKMH"));
			this.SpeedKMH = this.blank.FsmVariables.FindFsmFloat("SpeedKMH");
			this.lanepos = FsmVariables.GlobalVariables.FindFsmFloat("PlayerRoadLane");
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00008A24 File Offset: 0x00006C24
		private void Update()
		{
			this.LanePos.Value = this.lanepos.Value;
			this.SpeedKMH.Value = this.rig.velocity.magnitude * 3.6f;
		}

		// Token: 0x040000F5 RID: 245
		public GameObject car;

		// Token: 0x040000F6 RID: 246
		public Rigidbody rig;

		// Token: 0x040000F7 RID: 247
		private PlayMakerFSM blank;

		// Token: 0x040000F8 RID: 248
		private FsmFloat LanePos;

		// Token: 0x040000F9 RID: 249
		private FsmFloat SpeedKMH;

		// Token: 0x040000FA RID: 250
		private FsmFloat lanepos;
	}
}
