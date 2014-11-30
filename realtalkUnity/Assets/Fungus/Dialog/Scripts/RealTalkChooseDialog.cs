using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{

	[ExecuteInEditMode]
	public class RealTalkChooseDialog : Dialog 
	{
		public Slider timeoutSlider;
        public Slider emotionalSlider;

        public Toggle em1;
        public Toggle em2;
        public Toggle em3;

        public enum RealTalkMood
        {
            e1,
            e2,
            e3
        }

		public class Option
		{
			public string text;
			public UnityAction onSelect;
            public float valence;
		}

        public RealTalkMood CurrentMood = RealTalkMood.e2;

		public List<UnityEngine.UI.Button> optionButtons = new List<UnityEngine.UI.Button>();

        static public List<RealTalkChooseDialog> activeDialogs = new List<RealTalkChooseDialog>();

        public RealTalkMode RTMode;

        public enum RealTalkMode
        {
            Control,
            Slider,
            MoodMode
        }

		protected virtual void OnEnable()
		{
			if (!activeDialogs.Contains(this))
			{
				activeDialogs.Add(this);
			}
            emotionalSlider.gameObject.SetActive(false);
            em1.gameObject.SetActive(RTMode == RealTalkMode.MoodMode);
            em2.gameObject.SetActive(RTMode == RealTalkMode.MoodMode);
            em3.gameObject.SetActive(RTMode == RealTalkMode.MoodMode);
            timeoutSlider.gameObject.SetActive(false);

            Clear();

		}

        private void Update()
        {
            emotionalSlider.value += Time.deltaTime * Input.GetAxis ("Horizontal") * 2;

            if(Input.GetKeyDown (KeyCode.JoystickButton18))
            {
                em1.isOn = true;
            }
            if(Input.GetKeyDown (KeyCode.JoystickButton16))
            {
                em2.isOn = true;
            }
            if(Input.GetKeyDown (KeyCode.JoystickButton17))
            {
                em3.isOn = true;
            }

//            if (Input.GetKeyUp (KeyCode.Alpha1))
//            {
//                Application.LoadLevel (0);
//            }
//            if (Input.GetKeyUp (KeyCode.Alpha2))
//            {
//                Application.LoadLevel (1);
//            }
//            if (Input.GetKeyUp (KeyCode.Alpha3))
//            {
//                Application.LoadLevel (2);
//            }
        }

		public void onClickControl()
		{
			Application.LoadLevel (0);
		}
		
		public void onClickSlider ()
		{
			Application.LoadLevel (1);
		}
		
		public void onClickToggle()
		{
			Application.LoadLevel (2);
		}

		protected virtual void OnDisable()
		{
			activeDialogs.Remove(this);
		}

		public override void ShowDialog (bool visible)
		{
			base.ShowDialog (visible);
			timeoutSlider.gameObject.SetActive(false);

		}

		public virtual void Choose(string text, List<Option> options, float timeoutDuration, Action onTimeout)
		{
			Clear();
            emotionalSlider.gameObject.SetActive(RTMode == RealTalkMode.Slider);

			Action onWritingComplete = delegate {
				
			};

			StartCoroutine(WriteText(text, onWritingComplete, onTimeout));
            foreach (Option option in options)
            {
                AddOption(option.text, option.onSelect);
            }
            
            if (timeoutDuration > 0)
            {
                timeoutSlider.gameObject.SetActive(true);
                StartCoroutine(WaitForTimeout(timeoutDuration, onTimeout));
            }
		}

		protected virtual IEnumerator WaitForTimeout(float timeoutDuration, Action onTimeout)
		{
			float elapsedTime = 0;

			while (elapsedTime < timeoutDuration)
			{
				if (timeoutSlider != null)
				{
					float t = elapsedTime / timeoutDuration;
					timeoutSlider.value = t;
				}

				elapsedTime += Time.deltaTime;

				yield return null;
			}

			Clear();
			
			if (onTimeout != null)
			{
				onTimeout();
			}
		}

		protected override void Clear()
		{
			base.Clear();
			ClearOptions();
		}

		protected virtual void ClearOptions()
		{
			if (optionButtons == null)
			{
				return;
			}

			foreach (UnityEngine.UI.Button button in optionButtons)
			{
				button.onClick.RemoveAllListeners();
			}

			foreach (UnityEngine.UI.Button button in optionButtons)
			{
				if (button != null)
				{
					button.gameObject.SetActive(false);
				}
			}
		}
		
		protected virtual bool AddOption(string text, UnityAction action)
		{
			if (optionButtons == null)
			{
				return false;
			}
			
			bool addedOption = false;
			foreach (UnityEngine.UI.Button button in optionButtons)
			{
				if (!button.gameObject.activeSelf)
				{
					button.gameObject.SetActive(true);
					
					Text textComponent = button.GetComponentInChildren<Text>();
					if (textComponent != null)
					{
						textComponent.text = text;
					}

					UnityAction buttonAction = action;

					button.onClick.AddListener(delegate {
						StopAllCoroutines(); // Stop timeout
						Clear();
						if (buttonAction != null)
						{
							buttonAction();
						}
					});
					
					addedOption = true;
					break;
				}
			}
			
			return addedOption;
		}		
	}

}
