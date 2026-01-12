using UnityEngine;

namespace RAIL_SHOOTER.MENU
{
    public class Controls : MenuState
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