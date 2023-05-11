using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _buttonStartServer;
    [SerializeField] private Button _buttonShutDownServer;
    [SerializeField] private Button _buttonConnectClient;
    [SerializeField] private Button _buttonDisconnectClient;
    [SerializeField] private Button _buttonSendMessage;
    [SerializeField] private Button _buttonExit;
    [SerializeField] private InputField _inputField;
    [SerializeField] private MyTextField _textField;
    [SerializeField] private Server _server;
    [SerializeField] private Client _client;

    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private InputField _loginInputField;
    [SerializeField] private Button _buttonLogin;

    private void Start()
    {
        _buttonStartServer.onClick.AddListener(() => StartServer());
        _buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
        _buttonConnectClient.onClick.AddListener(() => Connect());
        _buttonDisconnectClient.onClick.AddListener(() => Disconnect());
        _buttonSendMessage.onClick.AddListener(() => SendMessage());
        _buttonExit.onClick.AddListener(() => Exit());
        _client.onMessageReceive += ReceiveMessage;

        _buttonLogin.onClick.AddListener(() => LoginClient());
    }
    private void StartServer()
    {
        _server.StartServer();
    }
    private void ShutDownServer()
    {
        _server.ShutDownServer();
    }
    private void Connect()
    {
        _loginPanel.SetActive(true);
    }
    private void LoginClient() 
    {
        _loginPanel.SetActive(false);
        _client.GetLogin(_loginInputField.text);
        _client.Connect();
    }
    private void Disconnect()
    {
        _client.Disconnect();
    }
    private void SendMessage()
    {
        _client.SendMessage(_inputField.text);
        _inputField.text = "";
    }
    private void Exit()
    {
        Application.Quit();
    }
    public void ReceiveMessage(object message)
    {
        _textField.ReceiveMessage(message);
    }
}