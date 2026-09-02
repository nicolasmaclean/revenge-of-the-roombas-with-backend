using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MovementAudio : MonoBehaviour
    {
        [SerializeField] Audio.AudioClipData clipData;

        bool _isMoving = false;
        AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
        }

        public void OnMove()
        {
            _isMoving = true;
        }

        void LateUpdate()
        {
            if (_isMoving)
            {
                if (!_audioSource.isPlaying)
                {
                    clipData.Play(_audioSource);
                }
                _isMoving = false;
            }
            else if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
        }
    }
}