using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContent : MonoBehaviour
{
    private float posX = 0;
    private float posY = 0;

    private Vector3 pos;
    private Vector3 vel = Vector3.zero;

    public void OnEnable()
    {
        StopAllCoroutines();

        posX = Random.Range(-300, 300);
        posY = Random.Range(-150, 150);

        pos = new Vector3(posX, posY, 1);

        StartCoroutine(RandomMoveCorution());
    }

    public void OnDisable()
    {
        gameObject.transform.localPosition = Vector3.zero;
        StopAllCoroutines();
    }

    IEnumerator RandomMoveCorution()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, pos, ref vel, 0.5f);

        yield return new WaitForSeconds(0.01f);

        StartCoroutine(RandomMoveCorution());
    }

    public void GoToTarget(Vector3 target)
    {
        StopAllCoroutines();

        StartCoroutine(GoToTargetCorution(target));
    }

    IEnumerator GoToTargetCorution(Vector3 target)
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, target, ref vel, 0.5f);

        if (transform.localPosition.y >= target.y - 2f)
        {
            gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(GoToTargetCorution(target));
    }
}
