using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
    [CommandInfo("Dialog", 
             "SayVideo", 
             "PlayVideo")]
    public class SayVideo : Command
    {
        public MovieTexture movie;
        public new GUITexture guiTexture;
        public new AudioSource audio;
        private bool skipped = false;
        private bool playing = false;

        public override void OnEnter ()
        {
            Debug.Log (name);   
            movie.Stop ();
            skipped = false;
            playing = true;
            guiTexture.texture = movie;
            ((MovieTexture)guiTexture.texture).Play ();

            audio.clip = ((MovieTexture)guiTexture.texture).audioClip;
            audio.Play ();
            StartCoroutine (PlayMovieCoroutine ());
        }

        private IEnumerator PlayMovieCoroutine ()
        {
            yield return new WaitForSeconds (movie.duration - 0.1f);
            if (!skipped)
            {
                ((MovieTexture)guiTexture.texture).Pause ();
                playing = false;
                Continue ();
            }
        }
        
        private void Update ()
        {
            if (Input.GetKeyUp (KeyCode.Space) && playing)
            {
                skipped = true;
                playing = false;
                Continue ();
            }
        }
    }
}