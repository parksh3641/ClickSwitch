using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationContent : MonoBehaviour
{
    private Text localizationText;
    public string localizationName = "";

    bool setValue = false;
    int value = 0;

    private void Awake()
    {
        localizationText = GetComponent<Text>();
    }

    public void TextColor(Color color)
    {
        localizationText.color = color;
    }

    private void Start()
    {
        if (localizationName.Length > 0) localizationText.text = LocalizationManager.instance.GetString(localizationName);

        if(LocalizationManager.instance != null) LocalizationManager.instance.AddContent(this);
    }

    public void ReLoad()
    {
        if (localizationName.Length > 0) localizationText.text = LocalizationManager.instance.GetString(localizationName);

        if (setValue)
        {
            localizationText.text += " : \n" + value;
        }
    }

    public void SetNumber(int number)
    {
        if (localizationName.Length > 0) localizationText.text = LocalizationManager.instance.GetString(localizationName);

        value = number;

        Invoke("Delay", 0.3f);
    }

    void Delay()
    {
        localizationText.text += " : \n" + value;
    }
}
