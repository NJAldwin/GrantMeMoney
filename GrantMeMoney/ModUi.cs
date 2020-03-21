#region

using ColossalFramework.UI;

#endregion

namespace GrantMeMoney
{
    public class ModUi : UICustomControl
    {
        public static ModUi Instance;
        private readonly UiGrantMoneyPanel _grantMoneyPanel;

        private readonly UiMainButton _mainButton;

        public ModUi()
        {
            Dbg.Log("UI Init");
            var uiView = UIView.GetAView();
            _mainButton = (UiMainButton) uiView.AddUIComponent(typeof(UiMainButton));
            _grantMoneyPanel = (UiGrantMoneyPanel) uiView.AddUIComponent(typeof(UiGrantMoneyPanel));
            Hide();
        }

        private bool Visible => _grantMoneyPanel.isVisible;

        ~ModUi()
        {
            Dbg.Log("UI Destroy");
            Destroy(_mainButton);
            Destroy(_grantMoneyPanel);
        }

        public void Toggle()
        {
            if (Visible)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            Dbg.Log("UI Show");
            _grantMoneyPanel.ReInitUi();
            _grantMoneyPanel.Show();
            _grantMoneyPanel.Focus();
        }

        public void Hide()
        {
            Dbg.Log("UI Hide");
            _grantMoneyPanel.Hide();
            _grantMoneyPanel.Unfocus();
        }
    }
}
