using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000010 RID: 16
	public class Refueling : MonoBehaviour
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00007890 File Offset: 0x00005A90
		private void Start()
		{
			GameObject gameObject = GameObject.Find("diesel(itemx)/FluidTrigger");
			this.jerrycanTrigger = gameObject.GetComponent<Collider>();
			PlayMakerFSM component = gameObject.GetComponent<PlayMakerFSM>();
			this.jerrycanPouring = component.FsmVariables.FindFsmBool("Pouring");
			this.jerrycanLevel = component.FsmVariables.FindFsmFloat("Fluid");
			this.jerrycanPouringSoundParent = GameObject.Find("Systems").transform.Find("DisabledAudioParent");
			this.jerrycanPouringSound = this.jerrycanPouringSoundParent.Find("FuelPouringSound");
			Transform transform = GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/FPSCamera/Fuel/Pivot/hand/Armature/Bone/Bone_001/Pistol/Pistol D/FuelNozzle");
			this.nozzleTrigger = transform.GetComponent<Collider>();
			this.refuelIndicator = Object.Instantiate<GameObject>(GameObject.Find("GUI").transform.Find("Indicators/Fluids/GasolineCar").gameObject);
			this.refuelIndicator.AddComponent<RefuelIndicator>().fuelTank = this.fuelTank;
			this.refuelIndicator.GetComponent<RefuelIndicator>().Setup();
			this.fuelTank.Indi = this.refuelIndicator.GetComponent<RefuelIndicator>();
			PlayMakerFSM component2 = transform.GetComponent<PlayMakerFSM>();
			this.nozzlePour = component2.FsmVariables.FindFsmBool("Pour");
			this.nozzleFlow = component2.FsmVariables.FindFsmFloat("FuelFlow");
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000079EC File Offset: 0x00005BEC
		private void OnTriggerEnter(Collider other)
		{
			bool flag = this.fuelMode == 0;
			if (flag)
			{
				bool flag2 = other == this.jerrycanTrigger && this.jerrycanPouring.Value && this.jerrycanLevel.Value > 0f && this.jerrycanTrigger.transform.parent.parent != null;
				if (flag2)
				{
					base.StartCoroutine(this.JerryCanFueling());
				}
				else
				{
					bool flag3 = other == this.nozzleTrigger;
					if (flag3)
					{
						base.StartCoroutine(this.NozzleFueling());
					}
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00007A84 File Offset: 0x00005C84
		private void OnTriggerExit(Collider other)
		{
			bool flag = other == this.jerrycanTrigger && this.fuelMode == 1;
			if (flag)
			{
				this.fuelMode = 0;
				base.StopAllCoroutines();
				this.refuelIndicator.SetActive(false);
				this.jerrycanPouringSound.SetParent(this.jerrycanPouringSoundParent);
				this.jerrycanPouringSound.localPosition = Vector3.zero;
				this.jerrycanPouringSound.localEulerAngles = Vector3.zero;
			}
			bool flag2 = other == this.nozzleTrigger && this.fuelMode == 2;
			if (flag2)
			{
				this.fuelMode = 0;
				base.StopAllCoroutines();
				this.refuelIndicator.SetActive(false);
				this.nozzlePour.Value = false;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00007B4C File Offset: 0x00005D4C
		private void OnDisable()
		{
			bool flag = this.fuelMode != 0;
			if (flag)
			{
				this.fuelMode = 0;
				this.nozzlePour.Value = false;
				this.jerrycanPouringSound.SetParent(this.jerrycanPouringSoundParent);
				this.jerrycanPouringSound.localPosition = Vector3.zero;
				this.jerrycanPouringSound.localEulerAngles = Vector3.zero;
				this.refuelIndicator.SetActive(false);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00007BBE File Offset: 0x00005DBE
		private IEnumerator JerryCanFueling()
		{
			this.fuelMode = 1;
			this.refuelIndicator.SetActive(true);
			this.jerrycanPouringSound.SetParent(base.transform);
			this.jerrycanPouringSound.localPosition = Vector3.zero;
			this.jerrycanPouringSound.localEulerAngles = Vector3.zero;
			while (this.fuelMode == 1 && this.jerrycanTrigger.enabled && this.jerrycanTrigger.transform.parent.parent != null && this.jerrycanPouring.Value && this.jerrycanLevel.Value > 0f && this.fuelTank.fuelLevel < this.fuelTank.fuelCapacity)
			{
				this.fuelTank.fuelLevel = Mathf.Clamp(this.fuelTank.fuelLevel + this.jerrycanFuelRate * Time.deltaTime, 0f, this.fuelTank.fuelCapacity);
				this.jerrycanLevel.Value -= this.jerrycanFuelRate * Time.deltaTime;
				this.fuelTank.UpdateFuelTankMass();
				yield return null;
			}
			this.fuelMode = 0;
			this.refuelIndicator.SetActive(false);
			this.jerrycanPouringSound.SetParent(this.jerrycanPouringSoundParent);
			this.jerrycanPouringSound.localPosition = Vector3.zero;
			this.jerrycanPouringSound.localEulerAngles = Vector3.zero;
			yield break;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00007BCD File Offset: 0x00005DCD
		private IEnumerator NozzleFueling()
		{
			this.fuelMode = 2;
			this.nozzlePour.Value = true;
			this.refuelIndicator.SetActive(true);
			while (this.fuelMode == 2 && this.nozzlePour.Value && this.nozzleTrigger.gameObject.activeInHierarchy && this.fuelTank.fuelLevel < this.fuelTank.fuelCapacity)
			{
				this.fuelTank.fuelLevel = Mathf.Clamp(this.fuelTank.fuelLevel + this.nozzleFlow.Value * Time.deltaTime, 0f, this.fuelTank.fuelCapacity);
				this.fuelTank.UpdateFuelTankMass();
				bool flag = this.fuelTank.fuelLevel >= this.fuelTank.fuelCapacity;
				if (flag)
				{
					break;
				}
				yield return null;
			}
			this.fuelMode = 0;
			this.nozzlePour.Value = false;
			this.refuelIndicator.SetActive(false);
			yield break;
		}

		// Token: 0x040000B0 RID: 176
		private int fuelMode = 0;

		// Token: 0x040000B1 RID: 177
		public FuelTank fuelTank;

		// Token: 0x040000B2 RID: 178
		public GameObject refuelIndicator;

		// Token: 0x040000B3 RID: 179
		public Collider jerrycanTrigger;

		// Token: 0x040000B4 RID: 180
		public FsmBool jerrycanPouring;

		// Token: 0x040000B5 RID: 181
		public FsmFloat jerrycanLevel;

		// Token: 0x040000B6 RID: 182
		public Transform jerrycanPouringSound;

		// Token: 0x040000B7 RID: 183
		public Transform jerrycanPouringSoundParent;

		// Token: 0x040000B8 RID: 184
		public float jerrycanFuelRate = 0.7f;

		// Token: 0x040000B9 RID: 185
		public Collider nozzleTrigger;

		// Token: 0x040000BA RID: 186
		public FsmBool nozzlePour;

		// Token: 0x040000BB RID: 187
		public FsmFloat nozzleFlow;
	}
}
