using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200002D RID: 45
	public class AdjustCarOptions : MonoBehaviour
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000AE04 File Offset: 0x00009004
		public void SetupFSMS()
		{
			this.forceFeedback = this.drivetrain.gameObject.GetComponent<ForceFeedback>();
			this.FFBFactor = this.Options.FsmVariables.GetFsmInt("FFBFactor");
			this.FFBMultiplier = this.Options.FsmVariables.GetFsmFloat("FFBMultiplier");
			this.FFBClamp = this.Options.FsmVariables.GetFsmInt("FFBClamp");
			this.FFBInverted = this.Options.FsmVariables.GetFsmBool("FFBInverted");
			this.SteeringAid = this.Options.FsmVariables.GetFsmBool("SteeringAid");
			this.SteeringAidMinVelo = this.Options.FsmVariables.GetFsmFloat("SteeringAidMinVelo");
			this.SteeringTime = this.Options.FsmVariables.GetFsmFloat("SteeringTime");
			this.SteeringVeloTime = this.Options.FsmVariables.GetFsmFloat("SteeringVeloTime");
			this.SteeringRotation = this.Options.FsmVariables.GetFsmFloat("SteeringRotation");
			this.AutoClutch = this.Options.FsmVariables.GetFsmBool("AutoClutch");
			this.HShifter = this.Options.FsmVariables.GetFsmBool("HShifter");
			this.HeadBobDrive = this.Graphics.FsmVariables.GetFsmInt("HeadBobDrive");
			this.AdjustOptions();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000AF73 File Offset: 0x00009173
		private void OnDisable()
		{
			this.AdjustOptions();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000AF80 File Offset: 0x00009180
		public void AdjustOptions()
		{
			try
			{
				this.Dynamics.enableForceFeedback = true;
				this.forceFeedback.factor = this.FFBFactor.Value * 100;
				this.forceFeedback.multiplier = this.FFBMultiplier.Value;
				this.forceFeedback.clampValue = this.FFBClamp.Value;
				this.forceFeedback.invertForceFeedback = this.FFBInverted.Value;
				this.CarController.steerAssistance = this.SteeringAid.Value;
				this.CarController.smoothInput = this.SteeringAid.Value;
				this.CarController.SteerAssistanceMinVelocity = this.SteeringAidMinVelo.Value;
				this.CarController.steerTime = this.SteeringTime.Value;
				this.CarController.steerReleaseTime = this.SteeringTime.Value;
				this.CarController.veloSteerTime = this.SteeringVeloTime.Value;
				this.CarController.veloSteerReleaseTime = this.SteeringVeloTime.Value;
				this.SteeringWheel.maxSteeringAngle = this.SteeringRotation.Value;
				this.drivetrain.autoClutch = this.AutoClutch.Value;
				this.drivetrain.shifter = this.HShifter.Value;
				this.driverHeadPivot.yMotion = ((this.HeadBobDrive.Value == 2) ? 1 : 0);
				this.driverHeadPivot.angularXMotion = ((this.HeadBobDrive.Value > 0) ? 1 : 0);
				Debug.Log("Oltcit: Car settings applied!");
			}
			catch
			{
			}
		}

		// Token: 0x04000188 RID: 392
		public CarDynamics Dynamics;

		// Token: 0x04000189 RID: 393
		public PlayMakerFSM Options;

		// Token: 0x0400018A RID: 394
		public AxisCarController CarController;

		// Token: 0x0400018B RID: 395
		public Drivetrain drivetrain;

		// Token: 0x0400018C RID: 396
		public PlayMakerFSM Graphics;

		// Token: 0x0400018D RID: 397
		public ConfigurableJoint driverHeadPivot;

		// Token: 0x0400018E RID: 398
		public SteeringWheel SteeringWheel;

		// Token: 0x0400018F RID: 399
		private ForceFeedback forceFeedback;

		// Token: 0x04000190 RID: 400
		private FsmInt FFBFactor;

		// Token: 0x04000191 RID: 401
		private FsmFloat FFBMultiplier;

		// Token: 0x04000192 RID: 402
		private FsmInt FFBClamp;

		// Token: 0x04000193 RID: 403
		private FsmBool FFBInverted;

		// Token: 0x04000194 RID: 404
		private FsmBool SteeringAid;

		// Token: 0x04000195 RID: 405
		private FsmFloat SteeringAidMinVelo;

		// Token: 0x04000196 RID: 406
		private FsmFloat SteeringTime;

		// Token: 0x04000197 RID: 407
		private FsmFloat SteeringVeloTime;

		// Token: 0x04000198 RID: 408
		private FsmFloat SteeringRotation;

		// Token: 0x04000199 RID: 409
		private FsmBool AutoClutch;

		// Token: 0x0400019A RID: 410
		private FsmBool HShifter;

		// Token: 0x0400019B RID: 411
		private FsmInt HeadBobDrive;
	}
}
