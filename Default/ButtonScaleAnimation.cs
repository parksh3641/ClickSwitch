using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaleAnimation : MonoBehaviour
{
    private float minScale = 0.9f;
    private float maxScale = 1.15f;

    private float speed = 0.015f;
    public float delay = 5f;

    float scale = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ButtonAnimation());
    }

    public void StopAnim()
    {
        StopAllCoroutines();

        transform.localScale = Vector3.one;
    }

    IEnumerator ButtonAnimation()
    {
        scale = 1;

        while (transform.localScale.x > minScale)
        {
            scale -= speed;

            transform.localScale = Vector3.one * scale;

            yield return waitForSeconds;
        }

        while(transform.localScale.x < maxScale)
        {
            scale += speed;

            transform.localScale = Vector3.one * scale;

            yield return waitForSeconds;
        }

        while (transform.localScale.x > 1)
        {
            scale -= speed;

            transform.localScale = Vector3.one * scale;

            yield return waitForSeconds;
        }

        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(Random.Range(delay * 0.8f, delay * 1.2f));

        StartCoroutine(ButtonAnimation());
    }

}
