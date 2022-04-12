using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationContent : MonoBehaviour
{
    private Text text;
    public string name = "";

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        text.text = LocalizationManager.instance.GetString(name);

        LocalizationManager.instance.AddContent(this);
    }

    public void ReLoad()
    {
        text.text = LocalizationManager.instance.GetString(name);
    }
}
