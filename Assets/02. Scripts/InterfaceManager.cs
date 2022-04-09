using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
}

public interface IContentEvent
{
    void Reset(int number);
    void First();
    void Choice();
    void ChoiceAction(bool check);
}

public interface IGameEvent
{

    void GameStart();

    void GamePause();

    void GameEnd();
}
