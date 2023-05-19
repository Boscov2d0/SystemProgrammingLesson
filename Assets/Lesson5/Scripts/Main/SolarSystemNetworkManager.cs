using Characters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        [SerializeField] private InputField _inputField;

        Dictionary<int, ShipController> _players = new Dictionary<int, ShipController>();
        private string _playerName;

        public string PlayerName { get { return _playerName; }  set { _playerName = value; } }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            player.GetComponent<ShipController>().PlayerName = _playerName;
            _players.Add(conn.connectionId, player.GetComponent<ShipController>());
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnStartServer() 
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler(100, ReceiceName);
        }
        public class MessageName : MessageBase
        {
            public string Name;

            public override void Deserialize(NetworkReader reader)
            {
                Name = reader.ReadString();
            }
            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(Name);
            }
        }
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            MessageName _name = new MessageName();
            _name.Name = _inputField.text;
            conn.Send(100, _name);
        }
        public void ReceiceName(NetworkMessage networkMessage) 
        {
            _players[networkMessage.conn.connectionId].PlayerName = networkMessage.reader.ReadString();
            _players[networkMessage.conn.connectionId].gameObject.name = _players[networkMessage.conn.connectionId].PlayerName;
        }
    }
}