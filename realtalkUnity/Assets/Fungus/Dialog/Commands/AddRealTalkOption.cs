using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	[CommandInfo("Dialog", 
	             "Add Real Talk Option", 
	             "Adds a Real Talk option for the player to select, displayed by the next Say command.")]

	public class AddRealTalkOption : Command 
	{
		public string optionTitle;
		public Sequence targetSequence;
        public float valence;
		
		public bool hideOnSelected;
		
		public override void OnEnter()
		{
			if (hideOnSelected &&
			    targetSequence.GetExecutionCount() > 0)
			{
				Continue();
				return;
			}
			
			RealTalk.Option option = new RealTalk.Option();
			option.optionTitle = optionTitle;
			option.targetSequence = targetSequence;
            option.valence = valence;
			RealTalk.realTalkOptionsList.Add(option);
			
			Continue();
		}
		
//		public override string GetSummary()
//		{
//			string summaryText = optionTitle;
//			
//			if (targetSequence == null)
//			{
//				summaryText += " ( <Continue> )";
//			}
//			else
//			{
//				summaryText += " (" + targetSequence.name + ")";
//			}
//			
//			return summaryText;
//		}
		
		public override void GetConnectedSequences(ref List<Sequence> connectedSequences)
		{
			if (targetSequence != null)
			{
				connectedSequences.Add(targetSequence);
			}
		}
		
		public override Color GetButtonColor()
		{
			return new Color32(184, 210, 235, 255);
		}
	}

}
