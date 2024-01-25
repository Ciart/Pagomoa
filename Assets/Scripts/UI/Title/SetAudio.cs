using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Title
{
    public class SetAudio : MonoBehaviour
    {
        public static SetAudio instance = null; 

        [SerializeField] public AudioMixer audioMixer;
        [SerializeField] public Slider audioSlider;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(this.gameObject);
            LoadAudioOption();
        }
        public void AudioControl()
        {
            float sound = audioSlider.value;

            if (sound == -40f)
                audioMixer.SetFloat("BGM", -80);
            else
                audioMixer.SetFloat("BGM", sound);
        }
        public void LoadAudioOption()
        {
            if (!OptionDB.instance) return;
            float sound = OptionDB.instance.audioValue;
            if (sound == -40f)
            {
                audioMixer.SetFloat("BGM", -80);
                audioSlider.value = sound;
            }
            else
            {
                audioMixer.SetFloat("BGM", sound);
                audioSlider.value = sound;
            }
        }
    }
}
