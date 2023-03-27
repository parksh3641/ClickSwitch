using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceContent : MonoBehaviour
{
    public int index = 0;

    public LocalizationContent titleText;

    public ReceiveContent receiveContent;

    public GameObject lockObj;
    public GameObject clearObj;


    public AttendanceManager attendanceManager;

    private void Awake()
    {


    }

    public void Initialize(int number, bool check, AttendanceManager manager)
    {
        attendanceManager = manager;

        titleText.localizationName = LocalizationManager.instance.GetString((index + 1) + "Day");

        clearObj.SetActive(false);

        if (index == number)
        {
            if(!check)
            {
                lockObj.SetActive(false);
            }
            else
            {
                lockObj.SetActive(true);
            }
        }
        else
        {
            if(index > number)
            {
                lockObj.SetActive(true);
            }
            else
            {
                clearObj.SetActive(true);
            }
        }
    }

    public void ReceiveButton()
    {
        attendanceManager.ReceiveButton(index, SuccessReceive);
    }

    public void SuccessReceive()
    {
        clearObj.SetActive(true);
    }
}
