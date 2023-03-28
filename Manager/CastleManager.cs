using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    public GameObject castleView;


    private void Awake()
    {
        castleView.SetActive(false);
    }

    public void OpenCastleView()
    {
        if(!castleView.activeInHierarchy)
        {
            castleView.SetActive(true);
        }
        else
        {
            castleView.SetActive(false);
        }
    }
}
