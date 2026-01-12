using UnityEngine;
using UnityEngine.UI;

namespace RAIL_SHOOTER.MENU
{
    public class MainMenu : MenuState
    {
        public override void EnterState(MenuManager menuManager)
        {
            base.EnterState(menuManager);
            _menuManager = menuManager;
        }

        public override void ExitState()
        {
            base.ExitState();
        }
        public override void UpdateState()
        {

        }

    }
}