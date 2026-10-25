using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000027 RID: 39
	public class OnEnterZone : MonoBehaviour
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00009FE4 File Offset: 0x000081E4
		private void Start()
		{
			this.EnvelopeColl = this.Envelope.transform.GetChild(0).GetComponent<Collider>();
			this.player = GameObject.Find("PLAYER").transform;
			this.money = FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney");
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000A038 File Offset: 0x00008238
		private void Update()
		{
			bool playedAnimation = this.PlayedAnimation;
			if (playedAnimation)
			{
				bool flag = Vector3.Distance(base.transform.position, this.player.position) >= 50f;
				if (flag)
				{
					this.PlayedAnimation = false;
					this.Envelope.SetActive(false);
				}
			}
			bool flag2 = this.TotalMoneyMade != 0f;
			if (flag2)
			{
				bool hit = BUS.raycast.GetHit(this.EnvelopeColl);
				if (hit)
				{
					this.guiUse.Value = true;
					this.guiInteraction.Value = string.Format("PAYDAY - BUS DRIVER: {0} MK", this.TotalMoneyMade);
					this.mouseOver = true;
					bool mouseButtonDown = Input.GetMouseButtonDown(0);
					if (mouseButtonDown)
					{
						this.money.Value += this.TotalMoneyMade;
						this.TotalMoneyMade = 0f;
						this.Envelope.SetActive(false);
					}
				}
			}
			else
			{
				bool flag3 = this.mouseOver;
				if (flag3)
				{
					this.guiInteraction.Value = string.Empty;
					this.guiUse.Value = false;
					this.mouseOver = false;
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000A16C File Offset: 0x0000836C
		private void OnTriggerEnter(Collider other)
		{
			bool flag = this.IsPlayerCharacter(other.gameObject) && !this.InTrigger;
			if (flag)
			{
				this.InTrigger = true;
				bool flag2 = this.busInZone;
				if (flag2)
				{
					bool flag3 = !this.showTagCart.IsInShift;
					if (flag3)
					{
						bool flag4 = this.showTagCart.onEnterZone.TotalMoneyMade != 0f;
						if (flag4)
						{
							base.StartCoroutine(this.GiveEnvelope());
						}
					}
					else
					{
						this.guiSubtitle.Value = "End your shift first!";
					}
				}
				else
				{
					this.guiSubtitle.Value = "What did you come here for? Come on, park the bus in one of the yellow marked areas and end your shift.";
				}
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000A220 File Offset: 0x00008420
		private IEnumerator GiveEnvelope()
		{
			this.guiSubtitle.Value = this.VoiceLinesEnter[Random.Range(0, this.VoiceLinesEnter.Length)];
			yield return new WaitForSeconds(1f);
			bool flag = !this.PlayedAnimation;
			if (flag)
			{
				this.GuyAnimation.Play("guy_giveEnvelope");
				yield return new WaitForSeconds(this.GuyAnimation.GetClip("guy_giveEnvelope").length - 1f);
				this.Envelope.SetActive(true);
				this.PlayedAnimation = true;
			}
			yield break;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000A230 File Offset: 0x00008430
		private void OnTriggerExit(Collider other)
		{
			bool flag = this.IsPlayerCharacter(other.gameObject);
			if (flag)
			{
				this.InTrigger = false;
				bool flag2 = !this.showTagCart.IsInShift;
				if (flag2)
				{
					bool flag3 = this.showTagCart.onEnterZone.TotalMoneyMade != 0f;
					if (flag3)
					{
						this.guiSubtitle.Value = "\"Make sure to collect your cash everyday after you're done!\"";
					}
					else
					{
						this.guiSubtitle.Value = "\"What are you waiting for? Get to work.\"";
					}
				}
				else
				{
					this.guiSubtitle.Value = "Oh, and don't crash please. Kari will kill me.";
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000A2C8 File Offset: 0x000084C8
		private bool IsPlayerCharacter(GameObject obj)
		{
			return obj == this.player || obj.transform.IsChildOf(this.player);
		}

		// Token: 0x0400014D RID: 333
		private Transform player;

		// Token: 0x0400014E RID: 334
		private bool InTrigger = false;

		// Token: 0x0400014F RID: 335
		private FsmString guiSubtitle = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle");

		// Token: 0x04000150 RID: 336
		public BusInZone busInZone;

		// Token: 0x04000151 RID: 337
		public Animation GuyAnimation;

		// Token: 0x04000152 RID: 338
		public GameObject Envelope;

		// Token: 0x04000153 RID: 339
		private bool PlayedAnimation = false;

		// Token: 0x04000154 RID: 340
		private Collider EnvelopeColl;

		// Token: 0x04000155 RID: 341
		private FsmBool guiUse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");

		// Token: 0x04000156 RID: 342
		private FsmString guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");

		// Token: 0x04000157 RID: 343
		private bool mouseOver = false;

		// Token: 0x04000158 RID: 344
		public float TotalMoneyMade = 0f;

		// Token: 0x04000159 RID: 345
		private FsmFloat money;

		// Token: 0x0400015A RID: 346
		public ShowTagCart showTagCart;

		// Token: 0x0400015B RID: 347
		private string[] VoiceLinesEnter = new string[]
		{
			"\"Hoping to see you again tommorow.\"",
			"\"I hope you parked the bus right. Otherwise Kari will fuck me up again.\"",
			"\"Good night and see you later.\"",
			"\"I hope the bus is alright. People can be pigs.\""
		};
	}
}
