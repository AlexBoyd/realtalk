using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fungus
{
    [ExecuteInEditMode]

    public class RealTalkDialog : Dialog
    {
                
        public Slider timeoutSlider;
//        public Slider valenceSlider;
//        public GameObject VAChart;
        public float valenceScale;
        bool isTimerRunning = false;


        public class Option
        {
            public string text;
            public UnityAction onSelect;
            public float valence;
        }
        
//              public List<UnityEngine.UI.Button> optionButtons = new List<UnityEngine.UI.Button> ();

        static public List<RealTalkDialog> activeDialogs = new List<RealTalkDialog> ();
                
        protected virtual void OnEnable ()
        {
            if (!activeDialogs.Contains (this))
            {
                activeDialogs.Add (this);
            }

        }
                
        protected virtual void Update ()
        {
            if (isTimerRunning)
            {
//                float horizontal = Input.GetAxis ("Horizontal");
//                valenceSlider.value += horizontal * valenceScale;
            }
        }

        protected virtual void OnDisable ()
        {
            activeDialogs.Remove (this);
        }
        
        public override void ShowDialog (bool visible)
        {
            base.ShowDialog (visible);
//            valenceSlider.gameObject.SetActive (false);
            timeoutSlider.gameObject.SetActive (false);
        }
        
        public virtual void RealTalk (string text, List<Option> options, float timeoutDuration, Action onTimeout)
        {
            Clear ();
            
            Action onWritingComplete = delegate {
//                foreach (Option option in options)
//                {
//                    AddRealTalkOption (option.valence, option.onSelect);
//                }

//                valenceSlider.gameObject.SetActive (true);

                if (timeoutDuration > 0)
                {
                    timeoutSlider.gameObject.SetActive (true);
                    StartCoroutine (WaitForTimeout (timeoutDuration, onTimeout, options));
                }
            };
            StartCoroutine (WriteText (text, onWritingComplete, onTimeout));
        }

        protected virtual IEnumerator WaitForTimeout (float timeoutDuration, Action onTimeout, List<Option> options)
        {
            float elapsedTime = 0;
            
            while (elapsedTime < timeoutDuration)
            {
                if (timeoutSlider != null)
                {
                    isTimerRunning = true;
                    float t = elapsedTime / timeoutDuration;
                    timeoutSlider.value = t;
                }
                
                elapsedTime += Time.deltaTime;
                
                yield return null;
            }

            isTimerRunning = false;
                        
            Clear ();                       
            
            if (onTimeout != null)
            {
//                float difference;
//                Option executableOption;
//
//                foreach (Option option in options)
//                {
//                    difference = Math.Abs(valenceSlider.value - option.valence);
//                    Debug.Log(difference);
//                    if (difference < 1.1)
//                    {
//                        executableOption = option;
//                        Debug.Log(executableOption.valence);
//                        //break from for loop
//                    }
//                }
                onTimeout ();
            }
        }

        protected override void Clear ()
        {
            base.Clear ();
//                      ClearOptions ();
        }

//              protected virtual void ClearOptions ()
//              {
//                      if (optionButtons == null) {
//                              return;
//                      }
//          
//                      foreach (UnityEngine.UI.Button button in optionButtons) {
//                              button.onClick.RemoveAllListeners ();
//                      }
//          
//                      foreach (UnityEngine.UI.Button button in optionButtons) {
//                              if (button != null) {
//                                      button.gameObject.SetActive (false);
//                              }
//                      }
//              }
        
//              protected virtual bool AddRealTalkOption (float number, UnityAction action)
//              {
//                      if (optionButtons == null) {
//                              return false;
//                      }
            
//                      bool addedOption = false;
//                      foreach (UnityEngine.UI.Button button in optionButtons) {
//                              if (!button.gameObject.activeSelf) {
//                                      button.gameObject.SetActive (true);
//                  
//                                      Text textComponent = button.GetComponentInChildren<Text> ();
//                                      if (textComponent != null) {
//                                              textComponent.text = text;
//                                      }
//                  
//                                      UnityAction buttonAction = action;
//                  
//                                      button.onClick.AddListener (delegate {
//                                              StopAllCoroutines (); // Stop timeout
//                                              Clear ();
//                                              if (buttonAction != null) {
//                                                      buttonAction ();
//                                              }
//                                      });
//                  
//                                      addedOption = true;
//                                      break;
//                              }
//                      }
//          
//                      return addedOption;
//              }           

    }
}