using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SEASON
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER,
    COMMUNISM
}

public class SeasonController : MonoBehaviour {

    SEASON _season;

    public float seasonDuration_ = 15;

    public delegate void SeasonChange();
    public static SeasonChange OnSeasonChange;


    private float seasonTimer = 0;
    private SEASON originSeason;
    private int seasonAmount = 0;

	void Start ()
    {
        _season = (SEASON)Random.Range(0, 4);
        originSeason = _season;
        if (OnSeasonChange != null) { OnSeasonChange(); }
	}

    //private void Update()
    //{
    //    seasonTimer += Time.deltaTime;
    //    if(seasonTimer > seasonDuration_ && _season != SEASON.COMMUNISM)
    //    {
            
    //        if((SEASON)(((int)(_season + 1)) % 4) == originSeason) { _season = SEASON.COMMUNISM; }
    //        else { _season = (SEASON)(((int)(_season + 1)) % 4); }
    //        seasonTimer = 0;
    //        if(OnSeasonChange != null) { OnSeasonChange(); }
    //    }
    //}

    public SEASON GetCurrentSeason() { return _season; }

    public void IncreaseSeasonAmount()
    {
        seasonAmount++;
        gameObject.GetComponent<Animator>().SetInteger("seasonAmount", seasonAmount);
    }

}
