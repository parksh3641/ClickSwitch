using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public Text numberText;

    private int number = 0;

    private void Awake()
    {
        numberText.text = "0";
    }

    public void OnClick()
    {
        number += 1;

        numberText.text = number.ToString();
    }
}
