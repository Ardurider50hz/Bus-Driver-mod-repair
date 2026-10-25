using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x02000021 RID: 33
	public class Seatbelt : MonoBehaviour
	{
		// Token: 0x06000093 RID: 147 RVA: 0x000092D0 File Offset: 0x000074D0
		private void Start()
		{
			this.fpsCamera = GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/FPSCamera");
			this.PlayerSeatbeltsOn = PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerSeatbeltsOn");
			this.guiUse = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse");
			this.guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
			this.playerCurrentVehicle = PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle");
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00009368 File Offset: 0x00007568
		private void Update()
		{
			bool flag = this.playerCurrentVehicle.Value == "Bus";
			if (flag)
			{
				bool flag2 = !this.seatbeltOn;
				if (flag2)
				{
					bool flag3 = !this.seatbeltHeld && this.interaction.GetHit(this.handleCollider);
					if (flag3)
					{
						this.guiUse.Value = true;
						this.guiInteraction.Value = " Take Seatbelt ";
						bool mouseButtonDown = Input.GetMouseButtonDown(0);
						if (mouseButtonDown)
						{
							this.seatbeltHeld = true;
							this.handle.parent = this.fpsCamera;
							this.handleCollider.enabled = false;
							this.guiInteraction.Value = " Take Seatbelt ";
						}
					}
					else
					{
						bool flag4 = this.seatbeltHeld && this.interaction.GetHit(this.fastenCollider);
						if (flag4)
						{
							this.guiInteraction.Value = " Fasten Seatbelt ";
							this.guiUse.Value = true;
							bool mouseButtonUp = Input.GetMouseButtonUp(0);
							if (mouseButtonUp)
							{
								this.seatbeltHeld = false;
								this.fastened.SetActive(true);
								this.handleParent.gameObject.SetActive(false);
								this.seatbeltOn = true;
								this.PlayerSeatbeltsOn.Value = true;
								this.ReturnSeatbelt(false);
								MasterAudio.PlaySound3DFollowTransform("CarFoley", this.fastened.transform, 1f, null, 0f, "seatbelt_fasten", false, false);
								this.guiInteraction.Value = " Fasten Seatbelt ";
							}
						}
						else
						{
							bool flag5 = this.seatbeltHeld && Input.GetMouseButtonUp(0);
							if (flag5)
							{
								this.seatbeltHeld = false;
								this.ReturnSeatbelt(true);
							}
							else
							{
								bool flag6 = this.mouseOver;
								if (flag6)
								{
									this.guiUse = true;
									this.guiInteraction = " Unfasten Seatbelt ";
								}
							}
						}
					}
				}
				else
				{
					bool hit = this.interaction.GetHit(this.fastenCollider);
					if (hit)
					{
						this.guiInteraction.Value = " Unfasten Seatbelt ";
						this.guiUse.Value = true;
						bool mouseButtonDown2 = Input.GetMouseButtonDown(0);
						if (mouseButtonDown2)
						{
							this.fastened.SetActive(false);
							this.handleParent.gameObject.SetActive(true);
							this.seatbeltOn = false;
							this.PlayerSeatbeltsOn.Value = false;
							MasterAudio.PlaySound3DFollowTransform("CarFoley", this.fastened.transform, 1f, null, 0f, "seatbelt_unfasten", false, false);
							MasterAudio.PlaySound3DFollowTransform("CarFoley", this.handleParent, 1f, null, 0.2f, "seatbelt_retract", false, false);
							this.mouseOver = false;
							this.guiUse.Value = false;
							this.guiInteraction.Value = "";
						}
					}
					else
					{
						bool flag7 = this.mouseOver;
						if (flag7)
						{
						}
					}
				}
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00009684 File Offset: 0x00007884
		private void ReturnSeatbelt(bool sound = false)
		{
			this.handleCollider.enabled = true;
			this.handle.parent = this.handleParent;
			this.handle.localPosition = Vector3.zero;
			this.handle.localEulerAngles = new Vector3(0f, 0f, -14f);
			this.handle.localScale = Vector3.one;
			if (sound)
			{
				MasterAudio.PlaySound3DFollowTransform("CarFoley", this.handleParent, 1f, null, 0f, "seatbelt_retract", false, false);
			}
		}

		// Token: 0x04000117 RID: 279
		public Raycast interaction;

		// Token: 0x04000118 RID: 280
		[HideInInspector]
		public bool seatbeltOn;

		// Token: 0x04000119 RID: 281
		private FsmBool PlayerSeatbeltsOn;

		// Token: 0x0400011A RID: 282
		private Transform fpsCamera;

		// Token: 0x0400011B RID: 283
		[Space(10f)]
		public Transform handleParent;

		// Token: 0x0400011C RID: 284
		public Transform handle;

		// Token: 0x0400011D RID: 285
		public Collider handleCollider;

		// Token: 0x0400011E RID: 286
		public GameObject fastened;

		// Token: 0x0400011F RID: 287
		public Collider fastenCollider;

		// Token: 0x04000120 RID: 288
		private bool seatbeltHeld;

		// Token: 0x04000121 RID: 289
		private FsmBool guiUse;

		// Token: 0x04000122 RID: 290
		private FsmString guiInteraction;

		// Token: 0x04000123 RID: 291
		[Space(10f)]
		public FsmString playerCurrentVehicle;

		// Token: 0x04000124 RID: 292
		private bool mouseOver;
	}
}
