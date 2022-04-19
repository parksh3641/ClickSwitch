using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
}

public interface IContentEvent
{
    void Initialize(GamePlayType type);
    void OnReset();
    void NormalReset(int number);
    void FilpCardReset(int number);
    void First();
    void Choice();
    void ChoiceAction(bool check);

    int GetIndex();
}

public interface IGameEvent
{

    void GameStart();

    void GamePause();

    void GameEnd();
}
