using System;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.UI.Title;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private OptionMenu _option;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;
        
        private void Start()
        {
            _optionButton.onClick.AddListener(_option.UIToggle);
            _exitButton.onClick.AddListener(ExitGame);
            
            _option.cancleButton.onClick.AddListener(_option.UIToggle);
            _option.confirmButton.onClick.AddListener(_option.UIToggle);
        }
        
        public void ExitGame() { Application.Quit(); }

        private void OnEnable()
        {
            _restartButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            
            var title = TitleController.Instance;
            _restartButton.onClick.AddListener(title.StartGame);
            _continueButton.onClick.AddListener(title.ReStart);
        }
    }
}