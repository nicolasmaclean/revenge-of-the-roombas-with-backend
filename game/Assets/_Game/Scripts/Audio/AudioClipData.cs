using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    [CreateAssetMenu(menuName = "Audio/SFX Data")]
    public class AudioClipData : ScriptableObject
    {
        [SerializeField]
        AudioClip _audioClip = null;

        [SerializeField]
        [Range(0f, 1f)]
        float _volume = 1f;

        [SerializeField]
        [Min(0f)]
        float _delay = 0;

        public void Play(AudioSource audioSource)
        {
            if (_audioClip == null) return;

            audioSource.clip = _audioClip;
            audioSource.volume = _volume;
            audioSource.PlayDelayed(_delay);
        }

        /// <summary>
        /// Plays a one-shot sfx by creating an empty gameObject with an AudioSource.
        /// The gameObject will be destoryed after the clip is done being played.
        /// Does nothing if <see cref="_audioClip"/> is null.
        /// </summary>
        public void Play()
        {
            if (_audioClip == null) return;

            GameObject go = new GameObject($"{_audioClip}");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            Utility.Empty component = go.AddComponent<Utility.Empty>();

            Play(audioSource);

            float lifespan = _audioClip.length + _delay;
            component.StartCoroutine(Utility.Coroutines.WaitBeforeCallback(lifespan,
            () =>
            {
                Destroy(go);
            }));
        }
    }
}