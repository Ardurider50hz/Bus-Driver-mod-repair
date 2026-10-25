using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000024 RID: 36
	public class DoorScript : MonoBehaviour
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00009B4C File Offset: 0x00007D4C
		public void UnlockDoor()
		{
			this.Locked = false;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00009B58 File Offset: 0x00007D58
		public void LockDoor()
		{
			this.Locked = true;
			bool doorOpen = this.DoorOpen;
			if (doorOpen)
			{
				this.animation.Play("door_close");
				this.DoorOpen = false;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00009B94 File Offset: 0x00007D94
		private void Start()
		{
			this.raycast = BUS.raycast;
			this.Pivot = base.transform.GetChild(0).gameObject;
			this.trigger = this.Pivot.transform.Find("Handle").GetComponent<Collider>();
			this.animation = this.Pivot.GetComponent<Animation>();
			bool doorOpen = this.DoorOpen;
			if (doorOpen)
			{
				this.OpenDoor();
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00009C08 File Offset: 0x00007E08
		private void Update()
		{
			bool hit = this.raycast.GetHit(this.trigger);
			if (hit)
			{
				bool flag = !this.animation.isPlaying;
				if (flag)
				{
					this.mouseOver = true;
					this.GUIuse.Value = true;
					bool flag2 = Input.GetMouseButtonDown(0) || cInput.GetKeyDown("Use");
					if (flag2)
					{
						bool flag3 = !this.Locked;
						if (flag3)
						{
							bool flag4 = !this.DoorOpen;
							if (flag4)
							{
								this.OpenDoor();
								bool flag5 = !this.isStoreDoor;
								if (flag5)
								{
									bool flag6 = this.UseSound && !this.IsGate;
									if (flag6)
									{
										MasterAudio.PlaySound3DAndForget(this.isOutsideDoor ? "Store" : "HouseFoley", base.transform, false, 1f, null, 0f, "door_open");
									}
								}
							}
							else
							{
								bool doorOpen = this.DoorOpen;
								if (doorOpen)
								{
									this.CloseDoor();
									bool flag7 = !this.isStoreDoor;
									if (flag7)
									{
										bool flag8 = this.UseSound && !this.IsGate;
										if (flag8)
										{
											MasterAudio.PlaySound3DAndForget(this.isOutsideDoor ? "Store" : "HouseFoley", base.transform, false, 1f, null, 0f, "door_close");
										}
									}
								}
							}
						}
						else
						{
							bool flag9 = this.Locked && !this.DoorOpen;
							if (flag9)
							{
								this.LockedDoor();
							}
						}
					}
				}
			}
			else
			{
				bool flag10 = this.mouseOver;
				if (flag10)
				{
					this.mouseOver = false;
					this.GUIuse.Value = false;
				}
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00009DD4 File Offset: 0x00007FD4
		private void LockedDoor()
		{
			MasterAudio.PlaySound3DAndForget("Store", this.Pivot.transform, false, 1f, null, 0f, "door_locked");
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00009E10 File Offset: 0x00008010
		public void OpenDoor()
		{
			this.GUIuse.Value = false;
			this.animation.Play("door_open");
			this.DoorOpen = true;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00009E38 File Offset: 0x00008038
		public void CloseDoor()
		{
			this.GUIuse.Value = false;
			this.animation.Play("door_close");
			this.DoorOpen = false;
		}

		// Token: 0x04000139 RID: 313
		public GameObject Pivot;

		// Token: 0x0400013A RID: 314
		private Collider trigger;

		// Token: 0x0400013B RID: 315
		public bool DoorOpen = false;

		// Token: 0x0400013C RID: 316
		[SerializeField]
		public bool Locked = false;

		// Token: 0x0400013D RID: 317
		private Animation animation;

		// Token: 0x0400013E RID: 318
		private FsmBool GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");

		// Token: 0x0400013F RID: 319
		public bool isOutsideDoor = true;

		// Token: 0x04000140 RID: 320
		public bool UseSound = true;

		// Token: 0x04000141 RID: 321
		private bool mouseOver;

		// Token: 0x04000142 RID: 322
		public bool IsGate = false;

		// Token: 0x04000143 RID: 323
		public bool isStoreDoor = false;

		// Token: 0x04000144 RID: 324
		private Raycast raycast;
	}
}
