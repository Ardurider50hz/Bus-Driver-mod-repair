using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200002F RID: 47
	public class DoorOpen : MonoBehaviour
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x0000B3C8 File Offset: 0x000095C8
		public void Start()
		{
			this.GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse");
			this.GUIinteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.door.Play("bus_door_close");
			bool flag = this.open;
			if (flag)
			{
				this.door.Play("bus_door_open");
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000B438 File Offset: 0x00009638
		public void Update()
		{
			bool flag = this.raycast.GetHit(this.triggerDoor) || this.raycast.GetHit(this.triggerButtonDoor);
			if (flag)
			{
				this.GUIuse.Value = true;
				this.GUIinteraction.Value = (this.open ? "CLOSE DOOR" : "OPEN DOOR");
				this.mouseOver = true;
				bool mouseButtonDown = Input.GetMouseButtonDown(0);
				if (mouseButtonDown)
				{
					bool flag2 = !this.door.isPlaying;
					if (flag2)
					{
						bool flag3 = BUS.KeyBus == 1;
						if (flag3)
						{
							MasterAudio.PlaySound3DAndForget("CarFoley", base.gameObject.transform, false, 1f, null, 0f, "bus_door_open");
							bool flag4 = !this.open;
							if (flag4)
							{
								this.OpenDoor();
							}
							else
							{
								bool flag5 = this.open;
								if (flag5)
								{
									this.CloseDoor();
								}
							}
						}
						else
						{
							MasterAudio.PlaySound3DAndForget("Store", base.transform, false, 1f, null, 0f, "door_locked");
						}
					}
				}
			}
			else
			{
				bool flag6 = this.mouseOver;
				if (flag6)
				{
					this.GUIinteraction.Value = string.Empty;
					this.GUIuse.Value = false;
					this.mouseOver = false;
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000B5A1 File Offset: 0x000097A1
		public void OpenDoor()
		{
			this.open = true;
			this.door.Play("bus_door_open");
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000B5BC File Offset: 0x000097BC
		public void CloseDoor()
		{
			this.open = false;
			this.door.Play("bus_door_close");
		}

		// Token: 0x040001A3 RID: 419
		public bool open;

		// Token: 0x040001A4 RID: 420
		public Animation door;

		// Token: 0x040001A5 RID: 421
		public Collider triggerDoor;

		// Token: 0x040001A6 RID: 422
		public Collider triggerButtonDoor;

		// Token: 0x040001A7 RID: 423
		public Raycast raycast;

		// Token: 0x040001A8 RID: 424
		private FsmString GUIinteraction;

		// Token: 0x040001A9 RID: 425
		private FsmBool GUIuse;

		// Token: 0x040001AA RID: 426
		private bool mouseOver;
	}
}
