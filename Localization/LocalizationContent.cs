using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationContent : MonoBehaviour
{
    private Text text;
    public string name = "";

    bool setValue = false;
    int value = 0;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void TextColor(Color color)
    {
        text.color = color;
    }

    private void Start()
    {
        if (name.Length > 0) text.text = LocalizationManager.instance.GetString(name);

        LocalizationManager.instance.AddContent(this);
    }

    public void ReLoad()
    {
        if (name.Length > 0) text.text = LocalizationManager.instance.GetString(name);

        if (setValue)
        {
            text.text += " : \n" + value;
        }
    }

    public void SetNumber(int number)
    {
        if(!setValue)
        {
            setValue = true;

            value = number;

            Invoke("Delay", 0.5f);
        }
        else
        {
            ReLoad();
        }
    }

    void Delay()
    {
        text.text += " : \n" + value;
    }

    public void OnReset()
    {
        setValue = false;

        if (name.Length > 0) text.text = LocalizationManager.instance.GetString(name);
    }
}
