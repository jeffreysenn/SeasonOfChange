using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour {

    static RoundController _instance;
    public static RoundController Instance {
        get {
            if (!_instance) {
                GameObject gbo = Instantiate(new GameObject("Round Controller"), new Vector3(0, 0, 0), Quaternion.identity);
                _instance = gbo.AddComponent<RoundController>();
            }
            return _instance;
        }
    }

    public GameObject winScreen;

    int _playerCount;
    public int _pointsToWin = 3;

    SeasonController _seasonController;
    PlayerInfo[] _players;

	void Start () {
        if (!_instance || _instance == this) {
            _instance = this;
        }
        else {
            Debug.LogError("There should only be one RoundController!");
            Destroy(this);
        }

        winScreen.SetActive(false);

        _seasonController = FindObjectOfType<SeasonController>();

        PlayerControllersInit();
    }

    void PlayerControllersInit() {
        PlayerController[] temp = FindObjectsOfType<PlayerController>();
        _playerCount = temp.Length;
        _players = new PlayerInfo[_playerCount];

        for (int i = 0; i < _playerCount; i++) {
            SetPlayerInfo(temp[i].playerIndex, new PlayerInfo(0, true, temp[i]));
        }
    }

    static public PlayerInfo GetPlayerInfo(int playerIndex) {
        if(playerIndex <= Instance._playerCount) {
            return Instance._players[playerIndex-1];
        }
        else {
            Debug.LogError("GetPlayerInfo: playerIndex out of range.");
            return Instance._players[0];
        }
    }

    void SetPlayerInfo(int playerIndex, PlayerInfo info) {
        if (playerIndex <= _playerCount) {
            _players[playerIndex-1] = info;
        }
        else {
            Debug.LogError("SetPlayerInfo: playerIndex out of range.");
        }
    }

    static public void PlayerDeathCallback(int playerIndex) {
        if(Instance.GetSeason() == SEASON.NoSeason) {
            GetPlayerInfo(playerIndex).Controller.ResetCharacter();
            return;
        }
        PlayerInfo info = GetPlayerInfo(playerIndex);
        info.IsActive = false;

        Instance.SetPlayerInfo(playerIndex, info);


        if (Instance._playerCount > 1) {
            Instance._playerCount--;
        }
        else {
            Instance.NewRound();
        }
    }

    void NewRound() {
        _playerCount = _players.Length;

        PlayerInfo info;

        for (int i = 1; i <= _playerCount; i++) {
            info = GetPlayerInfo(i);
            if (info.IsActive) {
                info.Score++;
                if(info.Score >= _pointsToWin) {
                    Victory(info.Controller);
                    return;
                }
            }
            info.IsActive = true;
            SetPlayerInfo(i, info);
            info.Controller.ResetCharacter();
        }
    }

    void Victory(PlayerController controller) {
        winScreen.SetActive(true);
        for (int i = 1; i <= _playerCount; i++) {
            GetPlayerInfo(i).Controller.gameObject.SetActive(false);
        }
    }

    SEASON GetSeason() {
        if (!_seasonController) {
            _seasonController = FindObjectOfType<SeasonController>();
        }
        return _seasonController.GetCurrentSeason();
    }
}

public struct PlayerInfo
{
    int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = Score;
        }
    }
    bool _isActive;
    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = IsActive;
        }
    }
    PlayerController _controller;
    public PlayerController Controller
    {
        get
        {
            return _controller;
        }
    }

    public PlayerInfo(int score, bool active, PlayerController controller)
    {
        _score = score;
        _isActive = active;
        _controller = controller;
    }
}
