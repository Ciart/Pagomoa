using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Ciart.Pagomoa
{
    public class EscOptionUI : MonoBehaviour
    {
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private Button _quitButton;

        void Start()
        {
            _goToMenuButton.onClick.AddListener(GoToMenu);
            _quitButton.onClick.AddListener(Quit);
        }

        public void GoToMenu()
        {
            SaveSystem.Instance.Save();
            SceneManager.LoadScene("Scenes/NewTitleScene");
        }

        public void Quit()
        {
            SaveSystem.Instance.Save();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
