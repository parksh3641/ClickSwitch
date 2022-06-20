using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent clickSoundEvent;

    void Awake()
    {
        clickSoundEvent.AddListener(() => { GameObject.FindWithTag("ClickSound").GetComponent<AudioSource>().Play(); });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 0.95f;

        clickSoundEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}