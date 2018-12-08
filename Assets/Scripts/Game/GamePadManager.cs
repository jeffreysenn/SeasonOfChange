using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadManager : MonoBehaviour {

    static GamePadManager instance_;
    static GamePadManager Instance {
        get {
            return instance_;
        }
    }

    GamePadController.Controller[] gamepads_;

    void Start() {
        if (!instance_) {
            instance_ = this;
        }
        else {
            Debug.LogError("Should only be one GamePadManager!");
            Destroy(this);
        }
        gamepads_ = new GamePadController.Controller[4];

        gamepads_[1] = GamePadController.GamePadOne;
        gamepads_[2] = GamePadController.GamePadTwo;
        gamepads_[3] = GamePadController.GamePadThree;
        gamepads_[4] = GamePadController.GamePadFour;
    }

    GamePadController.Controller GetGamePad(int playerIndex) {
        return gamepads_[playerIndex];
    }

    public static GamePadController.Controller GamePad(int playerIndex) {

        return Instance.GetGamePad(playerIndex);
    }
}
