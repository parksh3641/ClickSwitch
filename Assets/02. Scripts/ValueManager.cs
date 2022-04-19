using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public static ValueManager instance;

    [SerializeField]
    private float readyTimer = 4;

    [SerializeField]
    private float timer = 30;

    [SerializeField]
    private float comboTimer = 0.5f;

    [SerializeField]
    private float moleTimer = 3.0f;

    [SerializeField]
    private float cardTimer = 2.0f;


    private void Awake()
    {
        instance = this;
    }


    public float GetReadyTimer()
    {
        return readyTimer;
    }

    public float GetTimer()
    {
        return timer;
    }

    public float GetComboTimer()
    {
        return comboTimer;
    }

    public float GetMoleTimer()
    {
        return moleTimer;
    }

    public float GetCardTimer()
    {
        return cardTimer;
    }
}
