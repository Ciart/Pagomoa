using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Title
{
    public class AudioSlider : MonoBehaviour
    {
        public static AudioSlider instance = null;

        private void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
                Destroy(this.gameObject);
            gameObject.GetComponent<Slider>().value = OptionDB.instance.audioValue;
        }
        public void SaveAudioValue()
        {
            OptionDB.instance.audioValue = gameObject.GetComponent<Slider>().value;
        }
    
    }
}
