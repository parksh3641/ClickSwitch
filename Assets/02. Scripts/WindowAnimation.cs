using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WindowAnimation : MonoBehaviour
{

    float duration = 0.15f;

    Vector3 scaleTo = new Vector3(1f, 1f, 1f);


    void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(scaleTo, duration);
    }

    void OnDisable()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }

}