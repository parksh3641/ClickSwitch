using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionContent : MonoBehaviour
{
    public OptionType optionType;

    public UnityEvent unityEvent;

    public Image iconImg;
    public Sprite[] iconList;

    public Text iconText;

    public Text buttonText;

    private void Start()
    {
        
    }

    public void OnClick()
    {
        switch(optionType)
        {

        }
    }

}
