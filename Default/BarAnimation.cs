using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarAnimation : MonoBehaviour
{
    public Transform main;
    public Transform target;

    Vector3 saveMain;

    public float animationSpeed = 2.0f;



    private void Awake()
    {
        saveMain = main.transform.localPosition;
    }

    [Button]
    public void PlayAnimation()
    {
        StopAllCoroutines();
        main.transform.localPosition = main.localPosition;
        StartCoroutine(PlayAnimationCoroution());
    }

    IEnumerator PlayAnimationCoroution()
    {
        while(main.localPosition.x >= target.localPosition.x + 10f)
        {
            main.transform.localPosition = Vector3.Lerp(main.localPosition, target.localPosition, animationSpeed * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }

        while(main.localPosition.x != saveMain.x)
        {
            main.transform.localPosition = Vector3.MoveTowards(main.localPosition, saveMain, animationSpeed * 10f);

            yield return new WaitForSeconds(0.01f);
        }

    }
}
