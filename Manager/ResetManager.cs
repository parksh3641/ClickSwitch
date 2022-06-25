using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public bool OnCheckAttendanceDay()
    {
        bool check = false;

        if (playerDataBase.AttendanceDay == "")
        {
            playerDataBase.AttendanceDay = System.DateTime.Now.ToString("yyyyMMdd");
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(System.DateTime.Now.ToString("yyyyMMdd")));
        }

        if (ComparisonDate(playerDataBase.AttendanceDay))
        {
            Debug.Log("날짜 초기화");

            check = true;
        }
        else
        {
            Debug.Log("아직 하루가 안 지났습니다.");
        }

        return check;
    }
    public bool ComparisonDate(string target)
    {
        System.DateTime server = GetServerTime();
        System.DateTime system = System.DateTime.ParseExact(target, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        bool c = false;

        if (server.Year > system.Year)
        {
            c = true;
        }
        else
        {
            if (server.Year == system.Year)
            {
                if (server.Month > system.Month)
                {
                    c = true;
                }
                else
                {
                    if (server.Month == system.Month)
                    {
                        if (server.Day >= system.Day)
                        {
                            c = true;
                        }
                        else
                        {
                            c = false;
                        }
                    }
                    else
                    {
                        c = false;
                    }
                }
            }
            else
            {
                c = false;
            }
        }

        return c;
    }

    public System.DateTime GetServerTime()
    {
        System.DateTime _time = System.DateTime.Now;
        PlayFabClientAPI.GetTime(new GetTimeRequest(),
            result =>
            {
                _time = result.Time;
            }, error =>
            {

            });
        return _time;
    }
}
