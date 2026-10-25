using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200002E RID: 46
	public class GearIndicator : MonoBehaviour
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x0000B140 File Offset: 0x00009340
		public void Awake()
		{
			this.gearIndicator = Object.Instantiate<GameObject>(GameObject.Find("GUI/Indicators").transform.Find("Gear").gameObject);
			this.gearIndicator.name = "BUSgear";
			this.gearIndicator.transform.SetParent(GameObject.Find("GUI/Indicators").transform);
			this.gearIndicator.transform.localPosition = new Vector3(12f, 0.21f, 0f);
			this.gearIndicator.transform.localEulerAngles = Vector3.zero;
			this.gearIndicator.transform.localScale = Vector3.one;
			Object.Destroy(this.gearIndicator.GetComponent<PlayMakerFSM>());
			this.gearIndicatorText = this.gearIndicator.GetComponentsInChildren<TextMesh>(true);
			this.drivetrain = BUS.bus.GetComponent<Drivetrain>();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000B22C File Offset: 0x0000942C
		private void Update()
		{
			bool flag = BUS.gearIndicator == null;
			if (flag)
			{
				BUS.gearIndicator = this.gearIndicator;
			}
			this.tempGear = this.drivetrain.gear;
			this.gearIndicatorText[0].text = this.gears[this.tempGear] + " +" + this.lowGear.CurrentGear;
			this.gearIndicatorText[1].text = this.gears[this.tempGear] + " +" + this.lowGear.CurrentGear;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000B2C8 File Offset: 0x000094C8
		private void OnEnable()
		{
			this.gearIndicator.SetActive(this.gearIndicatorOn.Value);
			this.tempGear = this.drivetrain.gear;
			this.gearIndicatorText[0].text = this.gears[this.tempGear] + " +" + this.lowGear.CurrentGear;
			this.gearIndicatorText[1].text = this.gears[this.tempGear] + " +" + this.lowGear.CurrentGear;
		}

		// Token: 0x0400019C RID: 412
		public Drivetrain drivetrain;

		// Token: 0x0400019D RID: 413
		public GameObject gearIndicator;

		// Token: 0x0400019E RID: 414
		private TextMesh[] gearIndicatorText;

		// Token: 0x0400019F RID: 415
		public LowGearHighGear lowGear;

		// Token: 0x040001A0 RID: 416
		private string[] gears = new string[]
		{
			"R",
			"N",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6"
		};

		// Token: 0x040001A1 RID: 417
		private int tempGear = -1;

		// Token: 0x040001A2 RID: 418
		public FsmBool gearIndicatorOn;
	}
}
