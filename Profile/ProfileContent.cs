using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileContent : MonoBehaviour
{
    public Image icon;
    public LocalizationContent title;
    public Text bestScoreText;
    public Text bestComboText;

    public void InitState(string txt, int score, int combo, Sprite sp)
    {
        icon.sprite = sp;
        title.name = txt;
        title.ReLoad();
        bestScoreText.text = score.ToString();
        bestComboText.text = combo.ToString();
    }
}
