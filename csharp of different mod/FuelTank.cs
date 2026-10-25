using System;
using System.Collections;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200000D RID: 13
	public class FuelTank : MonoBehaviour
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00007296 File Offset: 0x00005496
		public void Start()
		{
			this.fueltankRigidbody = base.GetComponent<Rigidbody>();
			this.UpdateFuelTankMass();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000072AC File Offset: 0x000054AC
		public void StartConsumption()
		{
			base.StartCoroutine(this.Consumption());
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000072BC File Offset: 0x000054BC
		public void StopConsumption()
		{
			base.StopAllCoroutines();
			this.fueltankRigidbody.mass = this.fuelLevel + 4f;
			this.lowFuelTriggered = false;
			this.drivetrain.revLimiterTriggered = false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000072F1 File Offset: 0x000054F1
		private IEnumerator Consumption()
		{
			base.StartCoroutine(this.FuelTankMass());
			while (this.fuelLevel > 0.2f)
			{
				this.fuelConsumption = this.drivetrain.rpm / this.consumptionFactor * ((this.drivetrain.rpm > 1500f && this.drivetrain.gear != 1 && this.drivetrain.clutchDragImpulse != 0f) ? this.drivetrain.throttle : 1f) * Time.deltaTime;
				this.fuelLevel = Mathf.Clamp(this.fuelLevel - this.fuelConsumption, 0f, this.fuelCapacity);
				bool flag = !this.lowFuelTriggered && this.fuelLevel < 0.6f;
				if (flag)
				{
					base.StartCoroutine(this.LowFuel());
				}
				yield return new WaitForFixedUpdate();
			}
			this.starter.StopCarEngine();
			yield break;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00007300 File Offset: 0x00005500
		private IEnumerator LowFuel()
		{
			this.lowFuelTriggered = true;
			while (this.fuelLevel < 0.6f)
			{
				this.drivetrain.revLimiterTriggered = true;
				this.indicatorFuel.SetActive(true);
				yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
			}
			this.lowFuelTriggered = false;
			this.drivetrain.revLimiterTriggered = false;
			this.indicatorFuel.SetActive(false);
			yield break;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000730F File Offset: 0x0000550F
		private IEnumerator FuelTankMass()
		{
			for (;;)
			{
				this.fueltankRigidbody.mass = this.fuelLevel + 4f;
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000731E File Offset: 0x0000551E
		public void UpdateFuelTankMass()
		{
			this.fueltankRigidbody.mass = this.fuelLevel + 4f;
		}

		// Token: 0x04000091 RID: 145
		public float fuelConsumption;

		// Token: 0x04000092 RID: 146
		public float consumptionFactor = 200000f;

		// Token: 0x04000093 RID: 147
		public float fuelLevel = 35f;

		// Token: 0x04000094 RID: 148
		public float fuelCapacity = 40f;

		// Token: 0x04000095 RID: 149
		public bool lowFuelTriggered = false;

		// Token: 0x04000096 RID: 150
		public Rigidbody fueltankRigidbody;

		// Token: 0x04000097 RID: 151
		public GameObject indicatorFuel;

		// Token: 0x04000098 RID: 152
		public Drivetrain drivetrain;

		// Token: 0x04000099 RID: 153
		public AxisCarController CarController;

		// Token: 0x0400009A RID: 154
		public StarterNew starter;

		// Token: 0x0400009B RID: 155
		public RefuelIndicator Indi;
	}
}
