using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notion : MonoBehaviour
{
    private float first_speed = 0.5f;
    private float first_cooltime = 0.05f;
    private float second_speed = 0.06f;
    private float second_cooltime = 1.6f;
    private float second_speed_down;
    private float plus_scale = 0.9f;
    private float alpha_time = 1.5f;
    private float scale;

    private Transform trans;
    private Vector3 which;
    public Text txt;

    Tween tween;

    private int value = 0;

    void Awake()
    {
        trans = GetComponent<Transform>();

        txt.text = "";
    }

    void OnEnable()
    {
        tween.Kill();
        second_speed_down = second_speed;
        which = trans.localPosition;
        scale = plus_scale;
        value = 0;

        StartCoroutine(FirstWait());
    }
    void OnDisable()
    {
        StopAllCoroutines();
        second_speed = second_speed_down;
        trans.localPosition = which;
        transform.localScale = new Vector3(plus_scale, plus_scale, 0);

        DOTween.ToAlpha(() => txt.color, color => txt.color = color, 1, 0);
    }

    void Update()
    {
        transform.localScale = new Vector3(scale, scale, 0);

        if (value == 0)
        {
            transform.Translate(0, first_speed * Time.deltaTime, 0);
        }
        else if (value == 1)
        {
            transform.Translate(0, second_speed * Time.deltaTime, 0);
        }
    }

    IEnumerator FirstWait()
    {
        StartCoroutine(FirstUp());
        yield return new WaitForSeconds(first_cooltime);
        value = 1;
        StartCoroutine(SecondWait());
    }
    IEnumerator FirstUp()
    {
        if (scale < 1)
        {
            scale += 0.02f;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(FirstUp());
        }
        else
        {
            StopCoroutine(FirstUp());
        }
    }
    IEnumerator SecondWait()
    {
        StartCoroutine(SecondDown());
        yield return new WaitForSeconds(second_cooltime);
        value = 2;

        tween = DOTween.ToAlpha(() => txt.color, color => txt.color = color, 0, alpha_time);
        yield return tween.WaitForCompletion();
        gameObject.SetActive(false);

    }
    IEnumerator SecondDown()
    {
        if (second_speed > 0)
        {
            second_speed -= second_speed * 0.015f;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(SecondDown());
        }
    }
}
