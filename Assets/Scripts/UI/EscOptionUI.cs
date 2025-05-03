using System;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Ciart.Pagomoa
{
    public class EscOptionUI : MonoBehaviour
    {
        [SerializeField] private OptionMenu _option;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Image _Logo;
        
        void Start()
        {
            _option.confirmButton.onClick.AddListener(OnToggleButton);
            _option.cancleButton.onClick.AddListener(OnToggleButton);
            _optionButton.onClick.AddListener(OnToggleButton);
            _goToMenuButton.onClick.AddListener(GoToMenu);
            _quitButton.onClick.AddListener(Quit);
        }

        public void OnToggleButton()
        {
            _option.UIToggle();
            _optionButton.gameObject.SetActive(!_optionButton.gameObject.activeSelf);
            _goToMenuButton.gameObject.SetActive(!_goToMenuButton.gameObject.activeSelf);
            _quitButton.gameObject.SetActive(!_quitButton.gameObject.activeSelf);
            _Logo.gameObject.SetActive(!_Logo.gameObject.activeSelf);
        }
        
        public void GoToMenu()
        {
            Game.Instance.Time.ResumeTime();
            SceneManager.LoadScene("Scenes/NewTitleScene");
        }

        public void Quit()
        {
#if UNITY_EDITOR
            Game.Instance.Time.ResumeTime();
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Game.Instance.Time.ResumeTime();
            Application.Quit();
#endif
        }

        private void OnDisable()
        {
            _option.CancelOption();
            _option.gameObject.SetActive(false);
            _optionButton.gameObject.SetActive(true);
            _goToMenuButton.gameObject.SetActive(true);
            _quitButton.gameObject.SetActive(true);
            _Logo.gameObject.SetActive(true);
        }
    }
}
