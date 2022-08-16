using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public GameObject buttonObj;
    public Text numberText;

    private int number = 0;

    private void Awake()
    {
        numberText.text = "0";

        buttonObj.SetActive(true);
    }

    public void OnClick()
    {
        number += 1;

        numberText.text = number.ToString();
    }

    public void NowLoaded()
    {
        buttonObj.SetActive(false);
    }
}
