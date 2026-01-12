using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using RAIL_SHOOTER.AUDIO;

namespace RAIL_SHOOTER.MENU
{
    public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Image _hoverImage;

        [Header("Colors")]
        [SerializeField] private Color _normalTextColor = Color.white;
        [SerializeField] private Color _hoverTextColor = Color.yellow;
        [SerializeField] private Color _normalImageColor = Color.white;
        [SerializeField] private Color _hoverImageColor = Color.yellow;

        [Header("Audio")]
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private float _hoverVolume = 0.5f;
        [SerializeField] private float _clickVolume = 0.7f;

        private void Start()
        {
            if (_buttonText == null)
                _buttonText = GetComponentInChildren<TextMeshProUGUI>();

            if (_hoverImage == null)
            {
                Transform child = transform.Find("HoverImage");
                _hoverImage = child.GetComponent<Image>();
            }

            SetNormalState();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetHoverState();
            PlayHoverSound();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetNormalState();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayClickSound();
        }

        private void SetHoverState()
        {
            if (_buttonText != null)
                _buttonText.color = _hoverTextColor;

            if (_hoverImage != null)
            {
                _hoverImage.enabled = true;
                _hoverImage.color = _hoverImageColor;
            }
        }

        private void SetNormalState()
        {
            if (_buttonText != null)
                _buttonText.color = _normalTextColor;

            if (_hoverImage != null)
            {
                _hoverImage.enabled = false;
                _hoverImage.color = _normalImageColor;
            }
        }

        public void ResetHover()
        {
            SetNormalState();
        }

        public void SetAudioClips(AudioClip hoverSound, AudioClip clickSound, float hoverVolume = 0.5f, float clickVolume = 0.7f)
        {
            _hoverSound = hoverSound;
            _clickSound = clickSound;
            _hoverVolume = hoverVolume;
            _clickVolume = clickVolume;
        }

        private void PlayHoverSound()
        {
            AudioManager.Instance.PlaySFX(_hoverSound, _hoverVolume);
        }

        private void PlayClickSound()
        {
            AudioManager.Instance.PlaySFX(_clickSound, _clickVolume);
        }
    }
}