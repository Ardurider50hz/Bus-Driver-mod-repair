using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BUS
{
	// Token: 0x0200001B RID: 27
	public class CiganTalkEngine : MonoBehaviour
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00008A70 File Offset: 0x00006C70
		private void Start()
		{
			this.ciganAnim = base.transform.Find("skeleton").GetComponent<Animation>();
			this.texts = new string[]
			{
				"\"Nemas pujcit aspon dvacku? Potrebuju na chleba, nevidis?\"",
				"\"Jak to, ze ja musim platit, kdyz nemam ani na deravi boty!?\"",
				"\"Tyhle lidi by nemeli delat ridice, zmetci jeden vedle druhyho jak ty.\"",
				"\"Cely den zobru lidi o desitku, a co? Ani korunu! Vsichni jen koukaj a delaj, ze me nevidi!\"",
				"\"Tenhle autobus ma tak tvrdy sedacky, ze mam pocit, ze si sedim na sutru\"",
				"\"Proc ten ridic vecne brzdi tak prudce? Chce nas vsechny vysypat nebo co?\"",
				"\"Co to smrdi? hmmmm.. aha! to asi bude Pan Autobusar! smrdis tak, ze se ani poradne nadechnout nejde.\"",
				"\"Uz treti zastavka a furt zadna vetsi baba z BA na zoznameni.. to je den.\"",
				"\"Zalez do díry perinbaba\""
			};
			this.textsEN = new string[]
			{
				"\"Do you have extra twenty bucks? I need money for bread, can't you see?\",",
				"\"How come I have to pay when I don't even have money for torn shoes!?\",",
				"\"These people shouldn't be drivers, scumbags one after another like you.\",",
				"\"I beg people for a tenner all day, and what? Not a single coin! Everyone just stares and pretends I don't exist!\",",
				"\"This bus has such hard seats, I feel like I'm sitting on a rock.\",",
				"\"Why does the driver keep braking so hard? Does he want to throw us all out or what?\",",
				"\"What's that smell? Hmmmm.. oh! Must be Mr. Bus Driver! You stink so bad I can't even breathe properly.\",",
				"\"Third stop already, and still no proper lady from BA to meet.. what a day.\",",
				"\"Go back to the hole grandpa\""
			};
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00008B44 File Offset: 0x00006D44
		private void Update()
		{
			this.Delay += 0.01f;
			bool flag = (double)this.Delay >= 8.5;
			if (flag)
			{
				bool flag2 = !this.ciganAnim.isPlaying;
				if (flag2)
				{
					this.CurrentTextID = Random.Range(0, 14);
					this.guiSubtitle.Value = (this.UseCZText ? this.texts[this.CurrentTextID] : this.textsEN[this.CurrentTextID]);
					string text = "Drunks";
					Transform transform = base.transform;
					float num = 1f;
					string text2 = this.voiceLines[this.CurrentTextID];
					MasterAudio.PlaySound3DFollowTransform(text, transform, num, null, 0f, text2, false, false);
					this.Delay = 0f;
				}
			}
		}

		// Token: 0x040000FB RID: 251
		public string[] voiceLines = new string[]
		{
			"jokke13",
			"jokke18",
			"jokke20",
			"jokke06",
			"jokke11",
			"jokke01",
			"jokke02",
			"jokke10",
			"jokke20"
		};

		// Token: 0x040000FC RID: 252
		public string[] texts;

		// Token: 0x040000FD RID: 253
		public string[] textsEN;

		// Token: 0x040000FE RID: 254
		public int CurrentTextID;

		// Token: 0x040000FF RID: 255
		private float Delay;

		// Token: 0x04000100 RID: 256
		public bool UseCZText = false;

		// Token: 0x04000101 RID: 257
		private FsmString guiSubtitle = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIsubtitle");

		// Token: 0x04000102 RID: 258
		private Animation ciganAnim;
	}
}
