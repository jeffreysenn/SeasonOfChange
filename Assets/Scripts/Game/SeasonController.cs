using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonController : MonoBehaviour {

    SEASON _season;

    public float _timer;
    public float seasonDuration_;

	void Start () {
        _season = (SEASON)Random.Range(1, 6);
        OnSeasonEnter(_season);
	}

    void SetSeason(SEASON newSeason) {
        OnSeasonExit(_season);
        OnSeasonEnter(newSeason);
        _season = newSeason;
    }

    void OnSeasonEnter(SEASON newSeason) {
        switch (newSeason) {
            case SEASON.SPRING:
                break;
            case SEASON.SUMMER:
                break;
            case SEASON.AUTUMN:
                break;
            case SEASON.WINTER:
                break;
            case SEASON.COMMUNISM:
                break;
            default:
                break;
        }
    }

    void OnSeasonExit(SEASON newSeason) {
        switch (newSeason) {
            case SEASON.SPRING:
                break;
            case SEASON.SUMMER:
                break;
            case SEASON.AUTUMN:
                break;
            case SEASON.WINTER:
                break;
            case SEASON.COMMUNISM:
                break;
            default:
                break;
            }
        }

    enum SEASON {
        SPRING = 1,
        SUMMER = 2,
        AUTUMN = 3,
        WINTER = 4,
        COMMUNISM = 5
    }
}
