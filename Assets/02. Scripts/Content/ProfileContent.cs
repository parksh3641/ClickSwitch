using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileContent : MonoBehaviour
{
    public Text title;
    public Text bestScoreText;
    public Text bestComboText;

    public void InitState(string txt, int score, int combo)
    {
        title.text = txt;
        bestScoreText.text = score.ToString();
        bestComboText.text = combo.ToString();
    }
}
