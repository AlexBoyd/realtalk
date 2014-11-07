using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	[CommandInfo("Dialog", 
	             "Set Real Talk Dialog", 
	             "Sets the active dialog to use for displaying story text with the Real Talk command.")]
	public class SetRealTalkChooseDialog : Command 
	{
		public RealTalkChooseDialog realTalkDialog;	
		static public RealTalkChooseDialog activeDialog;
		
		public override void OnEnter()
		{
			activeDialog = realTalkDialog;
			Continue();
		}
		
		public override string GetSummary()
		{
			if (realTalkDialog == null)
			{
				return "Error: No dialog selected";
			}
			
			return realTalkDialog.name;
		}
		
		public override Color GetButtonColor()
		{
			return new Color32(184, 210, 235, 255);
		}
	}
}
