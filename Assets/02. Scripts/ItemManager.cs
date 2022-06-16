using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemContent[] itemContents;


    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize()
    {
        Debug.Log("ItemManager Initialize");

        for(int i = 0; i < itemContents.Length; i ++)
        {
            ItemType itemtype = ItemType.Clock;

            itemContents[i].Initialize(this, itemtype + i, playerDataBase.GetItemCount(itemtype + i));
        }
    }

    public void UseItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Clock:
                if(GameStateManager.instance.Clock)
                {
                    GameStateManager.instance.Clock = false;
                }
                else
                {
                    GameStateManager.instance.Clock = true;
                }
                break;
            case ItemType.Shield:
                if (GameStateManager.instance.Shield)
                {
                    GameStateManager.instance.Shield = false;
                }
                else
                {
                    GameStateManager.instance.Shield = true;
                }
                break;
        }
    }
}
