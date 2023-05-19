using System;
using UnityEngine.Networking;
using Main;

namespace UI
{
    public class UINamePanelController : IDisposable
    {
        private UINamePanelView _panelView;
        private NetworkManagerHUD _managerHUD;
        private SolarSystemNetworkManager _networkManager;

        public UINamePanelController(UINamePanelView panelView, NetworkManagerHUD managerHUD, SolarSystemNetworkManager networkManager) 
        {
            _panelView = panelView;
            _managerHUD = managerHUD;
            _networkManager = networkManager;

            _panelView.AcñeptButton.onClick.AddListener(Accept);
        }
        private void Accept()
        {
            _managerHUD.enabled = true;
            _panelView.gameObject.SetActive(false);
            _networkManager.PlayerName = _panelView.InputField.text;
        }

        public void Dispose()
        {
            _panelView.AcñeptButton.onClick.RemoveListener(Accept);
        }
    }
}