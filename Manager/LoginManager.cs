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
        numberText.text = "0 / 100";

        buttonObj.SetActive(true);
    }

    public void OnClick()
    {
        if (!buttonObj.activeInHierarchy) return;

        number += 1;

        numberText.text = number.ToString() + " / 100";
    }

    public void NowLoaded()
    {
        buttonObj.SetActive(false);

        numberText.text = "";
    }
}
