using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour {

    static RoundController _instance;
    public static RoundController Instance {
        get {
            return _instance;
        }
    }

    int _playerCount;


    Dictionary<PlayerController, PlayerInfo> _players;

	void Start () {
        if (!_instance) {
            _instance = this;
        }
        else {
            Debug.LogError("There should only be one RoundController!");
        }


        _players = new Dictionary<PlayerController, PlayerInfo>();

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        _playerCount = players.Length;

        for (int i = 0; i < players.Length;) {
            _players.Add(players[i], new PlayerInfo(i++, 0, true));
        }
    }

    void Update() {

    }

    public void PlayerDeathCallback(PlayerController player) {
        PlayerInfo info = _players[player];
        info.IsActive = false;

        _players[player] = info;


        if (_playerCount > 1) {
            _playerCount--;
        }
        else {
            NewRound();
        }
    }

    void NewRound() {
        _playerCount = _players.Count;

        PlayerInfo info;

        foreach (var item in _players) {
            info = item.Value;
            if (info.IsActive) {
                info.Score++;
            }
            info.IsActive = true;
            _players[item.Key] = info;
            //TODO: Respawn characters?.
        }
    }

    struct PlayerInfo {
        int _id;
        public int ID {
            get {
                return _id;
            }
            set {
                _id = ID;
            }
        }
        int _score;
        public int Score {
            get {
                return _score;
            }
            set {
                _score = Score;
            }
        }
        bool _isActive;
        public bool IsActive {
            get {
                return _isActive;
            }
            set {
                _isActive = IsActive;
            }
        }


        public PlayerInfo(int id, int score, bool active) {
            _id = id;
            _score = score;
            _isActive = active;
        }
    }
}
