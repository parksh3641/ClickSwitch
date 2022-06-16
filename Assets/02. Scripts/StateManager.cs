using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public ItemManager itemManager;




    public void Initialize()
    {
        itemManager.Initialize();
    }

}
