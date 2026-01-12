using UnityEngine;
using UnityEngine.UI;

namespace RAIL_SHOOTER.MENU
{
    public abstract class MenuState : MonoBehaviour
    {
        protected MenuManager _menuManager;
        
        public virtual void EnterState(MenuManager menuManager)
        {
            _menuManager = menuManager;
            gameObject.SetActive(true);
            Debug.Log("[MenuState] Entered " + this.GetType().Name + " state");
        }
        public virtual void ExitState()
        {
            Debug.Log("[MenuState] Exited " + this.GetType().Name + " state");
            gameObject.SetActive(false);
        }
        public virtual void UpdateState()
        {
            
        }
    }
}