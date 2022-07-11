using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public GameObject[] modeArray;

    private void Awake()
    {
        for(int i = 0; i < modeArray.Length; i ++)
        {
            if(modeArray[i] != null)
            {
                modeArray[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnMode()
    {
        for (int i = 0; i < modeArray.Length; i++)
        {
            if (modeArray[i] != null)
            {
                modeArray[i].gameObject.SetActive(false);
            }
        }

        modeArray[(int)GameStateManager.instance.GameModeType].SetActive(true);
    }

    public void OffMode()
    {
        for (int i = 0; i < modeArray.Length; i++)
        {
            if (modeArray[i] != null)
            {
                modeArray[i].gameObject.SetActive(false);
            }
        }
    }
}
