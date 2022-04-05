using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
}

public interface IContentEvent
{
    void Reset(int number);
    void Choice();
}

public interface IGameEvent
{

    void GameStart();

    void GamePause();

    void GameEnd();
}
