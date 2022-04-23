using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModeContent : MonoBehaviour
{
    public GamePlayType gamePlayType;

    public UnityEvent clickEvent;

    public LocalizationContent localizationContent;
    public Text nextEventText;
    public Image iconImg;

    private void Awake()
    {
        localizationContent.name = gamePlayType.ToString();

        clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().OnSetGameType((int)gamePlayType); });
    }

    public void OnClick()
    {
        clickEvent.Invoke();
    }

}
