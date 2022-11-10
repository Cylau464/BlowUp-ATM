using engine.senser;
using UnityEngine;
using UnityEngine.UI;

namespace main.ui
{
    public class SettingsView : MonoBehaviour, IPanel
    {
        #region variables
        [Header("Info")]
        [SerializeField] private SenserInfo _audioInfo;
        [SerializeField] private SenserInfo _vibrationInfo;

        [Header("Panels")]
        [SerializeField] private GameObject _window;

        [Header("Switch State")]
        [SerializeField] private bool _switchColor = true;
        [SerializeField] private bool _switchSprite = true;
        [NaughtyAttributes.ShowIf(nameof(_switchColor))]
        [SerializeField] private Color _turnOnColor;

        [NaughtyAttributes.ShowIf(nameof(_switchColor))]
        [SerializeField] private Color _turnOffColor;

        [NaughtyAttributes.ShowIf(nameof(_switchSprite))]
        [SerializeField] private Sprite _audioEnableImage;

        [NaughtyAttributes.ShowIf(nameof(_switchSprite))]
        [SerializeField] private Sprite _audioDisableImage;

        [NaughtyAttributes.ShowIf(nameof(_switchSprite))]
        [SerializeField] private Sprite _vibroEnableImage;

        [NaughtyAttributes.ShowIf(nameof(_switchSprite))]
        [SerializeField] private Sprite _vibroDisableImage;

        [Header("Open / Close")]
        [SerializeField] private Button _openBtn;
        [SerializeField] private Button[] _closeBtns;

        [Header("Audio")]
        [SerializeField] private Button _audioBtn;

        [Header("Vibration")]
        [SerializeField] private Button _vibrateBtn;

        public bool isShowed { get; private set; }
        #endregion

        protected void Start()
        {
            _window.SetActive(false);

            _openBtn.onClick.AddListener(Show);
            _audioBtn.onClick.AddListener(SwitchAudio);
            _vibrateBtn.onClick.AddListener(SwitchVibrate);

            foreach (Button btn in _closeBtns)
                btn.onClick.AddListener(Hide);
        }

        #region panel
        public void SwitchPanel()
        {
            if (!isShowed)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            isShowed = true;
            _window.SetActive(true);

            OnSwitchedAudio(_audioInfo.isEnable);
            OnSwitchedVibrate(_vibrationInfo.isEnable);
        }

        public void Hide()
        {
            isShowed = false;
            _window.SetActive(false);
        }
        #endregion

        #region switchs
        public void SwitchAudio()
        {
            _audioInfo.SwitchEnable();
            OnSwitchedAudio(_audioInfo.isEnable);
        }

        public void SwitchVibrate()
        {
            _vibrationInfo.SwitchEnable();
            OnSwitchedVibrate(_vibrationInfo.isEnable);
        }
        #endregion

        #region OnSwitched
        public void OnSwitchedAudio(bool enable)
        {
            if(_switchColor == true)
            {
                _audioBtn.targetGraphic.color = enable == true ?
                    _turnOnColor : _turnOffColor;
            }

            if(_switchSprite == true)
            {
                _audioBtn.image.sprite = enable == true ?
                    _audioEnableImage : _audioDisableImage;
            }
        }

        public void OnSwitchedVibrate(bool enable)
        {
            if (_switchColor == true)
            {
                _vibrateBtn.targetGraphic.color = enable == true ?
                    _turnOnColor : _turnOffColor;
            }

            if (_switchSprite == true)
            {
                _vibrateBtn.image.sprite = enable == true ?
                    _vibroEnableImage : _vibroDisableImage;
            }
        }
        #endregion
    }
}
