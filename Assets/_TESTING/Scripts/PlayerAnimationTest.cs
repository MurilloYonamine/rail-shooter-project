using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER.TESTING
{
    public class PlayerAnimationTest : MonoBehaviour
    {
        private const string ANIMATOR_PARAM_IS_FIRING = "isFiring";
        private const string ANIMATOR_PARAM_IS_RELOADING = "isReloading";
        private const string ANIMATOR_PARAM_IS_AIMING = "isAiming";
        private const string ANIMATOR_PARAM_IS_FIRE_AIMING = "isFireAiming";
        [SerializeField] private Animator _armAnimator;
        [SerializeField] private Animator _gunAnimator;

        [SerializeField] private KeyCode _fireKey = KeyCode.Mouse0;
        [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
        [SerializeField] private KeyCode _reloadKey = KeyCode.R;


        private void Update()
        {
            if (Input.GetKey(_aimKey))
            {
                Aim(true);
            }
            else
            {
                Aim(false);
            }

            if (Input.GetKeyDown(_fireKey))
            {
                StartCoroutine(Shoot());
            }

            if (Input.GetKeyDown(_reloadKey))
            {
                StartCoroutine(Reload());
            }
        }
        private void Aim(bool isAiming = true)
        {
            _armAnimator.SetBool(ANIMATOR_PARAM_IS_AIMING, isAiming);
        }
        private IEnumerator Shoot()
        {
            float animationDuration = 14f / 60f;

            _armAnimator.SetBool(ANIMATOR_PARAM_IS_FIRING, true);
            _gunAnimator.SetBool(ANIMATOR_PARAM_IS_FIRING, true);
            yield return new WaitForSeconds(animationDuration);
            _armAnimator.SetBool(ANIMATOR_PARAM_IS_FIRING, false);
            _gunAnimator.SetBool(ANIMATOR_PARAM_IS_FIRING, false);

        }
        private IEnumerator Reload()
        {
            float animationDuration = 72f / 60f;

            _armAnimator.SetBool(ANIMATOR_PARAM_IS_RELOADING, true);
            _gunAnimator.SetBool(ANIMATOR_PARAM_IS_RELOADING, true);
            yield return new WaitForSeconds(animationDuration);
            _armAnimator.SetBool(ANIMATOR_PARAM_IS_RELOADING, false);
            _gunAnimator.SetBool(ANIMATOR_PARAM_IS_RELOADING, false);

        }
    }
}
