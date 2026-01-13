using UnityEngine;
using RAIL_SHOOTER.UI;

namespace RAIL_SHOOTER
{
    public class DamageOverlayManager : MonoBehaviour
    {
        [Header("Overlay Settings")]
        [SerializeField] private bool _autoCreateOverlay = true;
        [SerializeField] private Color _overlayColor = new Color(1f, 0f, 0f, 0.5f);
        [SerializeField] private float _maxAlpha = 0.8f;
        [SerializeField, Range(0f, 100f)] private float _deathThreshold = 25f;
        [SerializeField] private bool _enablePulseEffect = true;
        
        private void Awake()
        {
            if (_autoCreateOverlay && DamageScreenOverlay.Instance == null)
            {
                CreateDamageOverlay();
            }
        }
        
        private void CreateDamageOverlay()
        {
            GameObject overlayGO = new GameObject("DamageScreenOverlay");
            DontDestroyOnLoad(overlayGO);
            
            DamageScreenOverlay overlay = overlayGO.AddComponent<DamageScreenOverlay>();
            
        }
        
        /// </summary>
        [ContextMenu("Test Overlay Effect")]
        private void TestOverlayEffect()
        {
            if (DamageScreenOverlay.Instance != null)
            {
                StartCoroutine(TestSequence());
            }
        }
        
        private System.Collections.IEnumerator TestSequence()
        {
            for (float alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                yield return new UnityEngine.WaitForSeconds(0.5f);
            }
        }
    }
}