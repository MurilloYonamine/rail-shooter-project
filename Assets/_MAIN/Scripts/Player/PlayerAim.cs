using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerAim : MonoBehaviour // TODO: remove monobehaviour
    {
        private InputReader m_inputReader;

        [SerializeField] private bool m_hideCursor = true;
        [SerializeField] private float m_sensitivity = 2.0f;

        [SerializeField] private RectTransform m_crosshair;

        private void Awake()
        {
            m_inputReader = new InputReader();
        }
        private void Start()
        {
            if (m_hideCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        private void OnEnable()
        {
            m_inputReader.OnEnable();
            InputReader.OnLookInput += HandleLookInput;
        }
        private void OnDisable()
        {
            m_inputReader.OnDisable();
            InputReader.OnLookInput -= HandleLookInput;
        }
        private void HandleLookInput(Vector2 lookInput)
        {
            float mouseX = lookInput.x * m_sensitivity;
            float mouseY = lookInput.y * m_sensitivity;

            (Vector2 minBounds, Vector2 maxBounds) = GetScreenBounds();

            Vector3 currentPosition = m_crosshair.anchoredPosition;
            currentPosition += new Vector3(mouseX, mouseY, 0);

            currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
            currentPosition.y = Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y);

            m_crosshair.anchoredPosition = currentPosition;
        }
        private static (Vector2, Vector2) GetScreenBounds()
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            Vector2 minBounds = new Vector2(-screenWidth / 2f, -screenHeight / 2f);
            Vector2 maxBounds = new Vector2(screenWidth / 2f, screenHeight / 2f);

            return (minBounds, maxBounds);
        }
    }
}