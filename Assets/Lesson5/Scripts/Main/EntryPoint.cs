using UI;
using UnityEngine;
using UnityEngine.Networking;

namespace Main
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UINamePanelView _panelView;
        [SerializeField] private NetworkManagerHUD _managerHUD;
        [SerializeField] private SolarSystemNetworkManager _networkManager;

        private UINamePanelController _panelController;

        private void Start()
        {
            _panelController = new UINamePanelController(_panelView, _managerHUD, _networkManager);
        }
        private void OnDestroy()
        {
            _panelController.Dispose();
        }
    }
}