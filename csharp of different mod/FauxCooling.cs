using System;
using System.Collections;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200000B RID: 11
	public class FauxCooling : MonoBehaviour
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00006A00 File Offset: 0x00004C00
		private void Start()
		{
			this.cooldownTick = BUS.busparts.transform.Find("Simulation/CooldownTick").GetComponent<AudioSource>();
			this.cooldownTick.clip = GameObject.Find("block(Clone)").transform.Find("CooldownTick").GetComponent<AudioSource>().clip;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00006A5C File Offset: 0x00004C5C
		public void EngineStart()
		{
			this.cooldownTick.gameObject.SetActive(false);
			this.engineOn = true;
			bool flag = !this.coolingOn;
			if (flag)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.Cooling());
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00006AA8 File Offset: 0x00004CA8
		public void EngineStop()
		{
			this.engineOn = false;
			bool flag = this.engineTemperature >= 50f;
			if (flag)
			{
				base.StartCoroutine(this.CooldownTick());
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00006AE0 File Offset: 0x00004CE0
		private void OnEnable()
		{
			bool flag = this.cooldownTick != null;
			if (flag)
			{
				this.cooldownTick.gameObject.SetActive(false);
				base.StartCoroutine(this.Cooling());
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00006B1F File Offset: 0x00004D1F
		private void OnDisable()
		{
			this.coolingOn = false;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00006B29 File Offset: 0x00004D29
		private IEnumerator Cooling()
		{
			this.coolingOn = true;
			while (this.engineTemperature >= 25f)
			{
				this.temperatureIncrease = ((this.engineTemperature > 80f && this.engineOn) ? (this.drivetrain.rpm * this.rpmFactor - this.coolingFactor) : (this.drivetrain.rpm * this.rpmFactor)) * Time.deltaTime;
				this.engineTemperature = Mathf.Clamp(this.engineTemperature + this.temperatureIncrease, 20f, 135f);
				yield return null;
			}
			this.engineTemperature = 25f;
			this.coolingOn = false;
			yield break;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00006B38 File Offset: 0x00004D38
		private IEnumerator CooldownTick()
		{
			this.cooldownTick.gameObject.SetActive(true);
			while (this.engineTemperature >= 50f && !this.engineOn)
			{
				this.cooldownTick.volume = Mathf.Clamp(this.engineTemperature / 170f, 0f, 1f);
				this.cooldownTick.pitch = Mathf.Clamp(this.engineTemperature / 90f, 0.8f, 1.1f);
				yield return null;
			}
			this.cooldownTick.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000067 RID: 103
		public float engineTemperature = 26f;

		// Token: 0x04000068 RID: 104
		public float coolingFactor = 0.75f;

		// Token: 0x04000069 RID: 105
		public float rpmFactor = 0.0001f;

		// Token: 0x0400006A RID: 106
		public float temperatureIncrease = 0f;

		// Token: 0x0400006B RID: 107
		public bool coolingOn = false;

		// Token: 0x0400006C RID: 108
		public bool engineOn = false;

		// Token: 0x0400006D RID: 109
		public Drivetrain drivetrain;

		// Token: 0x0400006E RID: 110
		public AudioSource cooldownTick;
	}
}
