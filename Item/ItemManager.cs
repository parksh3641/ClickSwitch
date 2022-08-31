using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public ItemContent[] itemContents;

    public LocalizationContent selectItemText;


    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    void OnEnable()
    {
        StateManager.eChangeNumber += Initialize;
    }

    void OnDisable()
    {
        StateManager.eChangeNumber -= Initialize;
    }

    public void Initialize()
    {
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
                if (GameStateManager.instance.Clock)
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
            case ItemType.Combo:
                if (GameStateManager.instance.Combo)
                {
                    GameStateManager.instance.Combo = false;
                }
                else
                {
                    GameStateManager.instance.Combo = true;
                }

                break;
            case ItemType.Exp:
                if (GameStateManager.instance.Exp)
                {
                    GameStateManager.instance.Exp = false;
                }
                else
                {
                    GameStateManager.instance.Exp = true;
                }

                break;
            case ItemType.Slow:
                if (GameStateManager.instance.Slow)
                {
                    GameStateManager.instance.Slow = false;
                }
                else
                {
                    GameStateManager.instance.Slow = true;
                }

                break;
        }

        selectItemText.name = "Information_" + type.ToString();
        selectItemText.ReLoad();
    }
}
