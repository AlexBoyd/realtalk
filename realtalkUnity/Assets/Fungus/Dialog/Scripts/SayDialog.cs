using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{

	[ExecuteInEditMode]
	public class SayDialog : Dialog 
	{
		public Image continueImage;

		static public List<SayDialog> activeDialogs = new List<SayDialog>();

		protected virtual void OnEnable()
		{
			if (!activeDialogs.Contains(this))
			{
				activeDialogs.Add(this);
			}
		}
		
		protected virtual void OnDisable()
		{
			activeDialogs.Remove(this);
		}

		public virtual void Say(string text, Action onComplete, float timeoutDuration = 0)
		{
			Clear();
			
			if (storyText != null)
			{
				storyText.text = text;
			}

			Action onExitTag = delegate {
				Clear();					
				if (onComplete != null)
				{
					onComplete();
				}
			};

			StartCoroutine(WriteText(text, null, onExitTag));
			StartCoroutine (WaitForTimeout (timeoutDuration, onComplete));

		}

		protected override void Clear()
		{
			base.Clear();
			ShowContinueImage(false);
		}

		protected override void OnWaitForInputTag(bool waiting)
		{
			ShowContinueImage(waiting);
		}

		protected virtual void ShowContinueImage(bool visible)
		{
			if (continueImage != null)
			{
				continueImage.enabled = visible;
			}
		}

		protected virtual IEnumerator WaitForTimeout(float timeoutDuration, Action onTimeout)
		{
			float elapsedTime = 0;
            yield return null;
			while (elapsedTime < timeoutDuration)
			{
				elapsedTime += Time.deltaTime;
//				if(Input.GetKeyDown(KeyCode.Space))
//                {
//                    break;
//                }
				yield return null;
			}
			
			Clear();
			
			if (onTimeout != null)
			{
				onTimeout();
			}
		}
	}

}
