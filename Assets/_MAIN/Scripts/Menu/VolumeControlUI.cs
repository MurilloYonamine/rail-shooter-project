using UnityEngine;
using UnityEngine.UI;

namespace RAIL_SHOOTER.MENU
{
    [System.Serializable]
    public class VolumeControlUI
    {
        [Header("Volume Control")]
        public string volumeTypeName;
        public Button decreaseButton;
        public Button increaseButton;
        public Image[] volumeIndicators = new Image[7];

        public void Initialize()
        {
            if (volumeIndicators == null || volumeIndicators.Length != 7)
            {
                volumeIndicators = new Image[7];
            }
        }

        public void UpdateDisplay(int volumeLevel, Color activeColor, Color inactiveColor)
        {
            for (int i = 0; i < volumeIndicators.Length; i++)
            {
                if (volumeIndicators[i] != null)
                {
                    volumeIndicators[i].color = i < volumeLevel ? activeColor : inactiveColor;
                }
            }

            decreaseButton.interactable = volumeLevel > 0;
            increaseButton.interactable = volumeLevel < 6;
        }
    }
}