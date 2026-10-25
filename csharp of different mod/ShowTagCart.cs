using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000029 RID: 41
	public class ShowTagCart : MonoBehaviour
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000A5C8 File Offset: 0x000087C8
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.raycast = BUS.raycast;
			this.PlayerName = PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerName");
			this.currentVehicle = PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle");
			base.StartCoroutine(this.AddHourlyMoney());
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000A656 File Offset: 0x00008856
		private IEnumerator AddHourlyMoney()
		{
			while (this.onEnterZone != null)
			{
				bool flag = this.IsInShift && this.currentVehicle.Value == "Bus";
				if (flag)
				{
					yield return new WaitForSeconds(120f);
					this.onEnterZone.TotalMoneyMade += 50f;
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000A668 File Offset: 0x00008868
		private void Update()
		{
			bool hit = this.raycast.GetHit(this.TriggerOut);
			if (hit)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "END SHIFT";
				this.mouseOverOut = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool isInShift = this.IsInShift;
					if (isInShift)
					{
						base.StartCoroutine(this.EndShift());
						MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
					}
				}
			}
			else
			{
				bool flag = this.mouseOverOut;
				if (flag)
				{
					this.mouseOverOut = false;
					this.GUIinteraction.Value = string.Empty;
					this.GUIuse.Value = false;
				}
			}
			bool hit2 = this.raycast.GetHit(this.TriggerIn);
			if (hit2)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "START SHIFT";
				this.mouseOver = true;
				bool mouseButtonDown2 = Input.GetMouseButtonDown(0);
				if (mouseButtonDown2)
				{
					bool flag2 = !this.IsInShift;
					if (flag2)
					{
						base.StartCoroutine(this.StartShift());
						MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
					}
				}
			}
			else
			{
				bool flag3 = this.mouseOver;
				if (flag3)
				{
					this.mouseOver = false;
					this.GUIinteraction.Value = string.Empty;
					this.GUIuse.Value = false;
				}
			}
			bool hit3 = this.raycast.GetHit(this.TriggerCheckBalance);
			if (hit3)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = "CHECK EARNINGS";
				this.mouseOverTotalBalance = true;
				bool mouseButtonDown3 = Input.GetMouseButtonDown(0);
				if (mouseButtonDown3)
				{
					bool isInShift2 = this.IsInShift;
					if (isInShift2)
					{
						bool flag4 = !this.CheckingBalance;
						if (flag4)
						{
							base.StartCoroutine(this.CheckBalance());
							MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
						}
						else
						{
							bool checkingBalance = this.CheckingBalance;
							if (checkingBalance)
							{
								this.UpperText.text = "SHOW TAG CARD";
								this.CheckingBalance = false;
							}
						}
					}
					else
					{
						bool flag5 = !this.IsInShift;
						if (flag5)
						{
							base.StartCoroutine(this.CheckBalanceUnregistered());
							MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
						}
					}
				}
			}
			else
			{
				bool flag6 = this.mouseOverTotalBalance;
				if (flag6)
				{
					this.mouseOverTotalBalance = false;
					this.GUIinteraction.Value = string.Empty;
					this.GUIuse.Value = false;
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000A953 File Offset: 0x00008B53
		private IEnumerator StartShift()
		{
			yield return new WaitForSeconds(0.5f);
			this.UpperText.text = "LOADING....";
			yield return new WaitForSeconds(2f);
			this.UpperText.text = string.Format("WELCOME {0}!", this.PlayerName);
			this.IsInShift = true;
			BUS.KeyBus = 1;
			yield return new WaitForSeconds(4f);
			this.UpperText.text = "PLEASE PROCEED..";
			yield return new WaitForSeconds(2f);
			this.UpperText.text = "SHOW TAG CARD";
			yield break;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000A962 File Offset: 0x00008B62
		private IEnumerator EndShift()
		{
			yield return new WaitForSeconds(0.5f);
			this.UpperText.text = "LOADING USERS..";
			yield return new WaitForSeconds(2f);
			this.UpperText.text = string.Format("WELCOME {0}!", this.PlayerName);
			yield return new WaitForSeconds(0.5f);
			MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
			this.UpperText.text = string.Format("ENDING SHIFT {0}...", this.PlayerName);
			this.IsInShift = false;
			this.BusDoor.CloseDoor();
			BUS.KeyBus = 0;
			yield return new WaitForSeconds(2f);
			this.UpperText.text = "SHOW TAG CARD";
			yield break;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000A971 File Offset: 0x00008B71
		private IEnumerator CheckBalance()
		{
			this.CheckingBalance = true;
			yield return new WaitForSeconds(0.5f);
			this.UpperText.text = "ENTER PIN";
			yield return new WaitForSeconds(0.7f);
			this.UpperText.text = "*";
			MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
			yield return new WaitForSeconds(0.4f);
			this.UpperText.text = "**";
			MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
			yield return new WaitForSeconds(0.7f);
			MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
			this.UpperText.text = "***";
			yield return new WaitForSeconds(0.4f);
			MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
			this.UpperText.text = "****";
			yield return new WaitForSeconds(1.2f);
			MasterAudio.PlaySound3DAndForget("386", base.transform, false, 0.1f, new float?(1.1f), 0f, "386_button");
			this.UpperText.text = "TODAY             WEEKLY";
			yield return new WaitForSeconds(1.2f);
			this.UpperText.text = string.Format("TODAY EARNINGS: {0}MK", this.onEnterZone.TotalMoneyMade);
			yield break;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000A980 File Offset: 0x00008B80
		private IEnumerator CheckBalanceUnregistered()
		{
			yield return new WaitForSeconds(1f);
			this.UpperText.text = "ERROR: NOT SIGNED IN";
			yield return new WaitForSeconds(1f);
			this.UpperText.text = "LOADING....";
			yield return new WaitForSeconds(2f);
			this.UpperText.text = "SHOW TAG CARD";
			yield break;
		}

		// Token: 0x04000167 RID: 359
		private Raycast raycast;

		// Token: 0x04000168 RID: 360
		private FsmString GUIinteraction;

		// Token: 0x04000169 RID: 361
		private FsmBool GUIuse;

		// Token: 0x0400016A RID: 362
		private bool mouseOver;

		// Token: 0x0400016B RID: 363
		private bool mouseOverOut;

		// Token: 0x0400016C RID: 364
		private bool mouseOverTotalBalance;

		// Token: 0x0400016D RID: 365
		public Collider TriggerOut;

		// Token: 0x0400016E RID: 366
		public Collider TriggerIn;

		// Token: 0x0400016F RID: 367
		public Collider TriggerCheckBalance;

		// Token: 0x04000170 RID: 368
		public TextMesh UpperText;

		// Token: 0x04000171 RID: 369
		public bool IsInShift = false;

		// Token: 0x04000172 RID: 370
		private FsmString PlayerName;

		// Token: 0x04000173 RID: 371
		public OnEnterZone onEnterZone;

		// Token: 0x04000174 RID: 372
		public DoorOpen BusDoor;

		// Token: 0x04000175 RID: 373
		private bool CheckingBalance = false;

		// Token: 0x04000176 RID: 374
		private FsmString currentVehicle;
	}
}
