using System;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000020 RID: 32
	public class DashVolumeFSM : MonoBehaviour
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00009180 File Offset: 0x00007380
		private void Start()
		{
			this.ignition.transform.SetParent(BUS.bus.transform);
			this.ignition.transform.localPosition = Vector3.zero;
			this.ignition.transform.localEulerAngles = Vector3.zero;
			this.Electricity = BUS.busparts.transform.Find("Electricity").gameObject;
			PlayMakerFSM playMakerFSM = this.ignition.AddComponent<PlayMakerFSM>();
			playMakerFSM.FsmName = "Use";
			FsmFloat fsmFloat = new FsmFloat
			{
				Name = "DashVolume",
				Value = 0f,
				UseVariable = false
			};
			playMakerFSM.Fsm.Variables.FloatVariables = new FsmFloat[]
			{
				fsmFloat
			};
			FsmState fsmState = new FsmState(playMakerFSM.Fsm)
			{
				Name = "State 1"
			};
			playMakerFSM.Fsm.States = new FsmState[]
			{
				fsmState
			};
			playMakerFSM.Fsm.StartState = fsmState.Name;
			PlayMakerExtensions.InitializeFSM(playMakerFSM);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00009294 File Offset: 0x00007494
		private void Update()
		{
			PlayMakerExtensions.GetPlayMaker(this.ignition, "Use").FsmVariables.GetFsmFloat("DashVolume").Value = (float)(this.Electricity.activeInHierarchy ? 1 : 0);
		}

		// Token: 0x04000115 RID: 277
		public GameObject ignition;

		// Token: 0x04000116 RID: 278
		private GameObject Electricity;
	}
}
