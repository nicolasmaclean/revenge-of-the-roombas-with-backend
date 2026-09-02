using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public void PlayClipData(AudioClipData audioClipData)
        {
            audioClipData.Play();
        }
    }
}