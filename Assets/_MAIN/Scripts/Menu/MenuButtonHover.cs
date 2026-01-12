using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace RAIL_SHOOTER.MENU
{
    public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Image _hoverImage;

        [Header("Colors")]
        [SerializeField] private Color _normalTextColor = Color.white;
        [SerializeField] private Color _hoverTextColor = Color.yellow;
        [SerializeField] private Color _normalImageColor = Color.white;
        [SerializeField] private Color _hoverImageColor = Color.yellow;

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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetNormalState();
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
    }
}