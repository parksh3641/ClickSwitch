using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public GameObject itemView;

    public ItemContent[] itemContents;

    public LocalizationContent selectItemText;

    public GameManager gameManager;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        itemView.SetActive(false);
    }

    void OnEnable()
    {
        StateManager.eChangeNumber += Initialize;
    }

    void OnDisable()
    {
        StateManager.eChangeNumber -= Initialize;
    }

    public void OpenItem()
    {
        if(GameStateManager.instance.GamePlayType == GamePlayType.GameChoice8)
        {
            gameManager.OnGameStartButton();
        }
        else
        {
            if (!itemView.activeSelf)
            {
                itemView.SetActive(true);

                Initialize();

            }
            else
            {
                itemView.SetActive(false);
            }
        }
    }

    public void Initialize()
    {
        for(int i = 0; i < itemContents.Length; i ++)
        {
            ItemType itemtype = ItemType.Clock;

            itemContents[i].Initialize(this, itemtype + i, playerDataBase.GetItemCount(itemtype + i));
            itemContents[i].gameObject.SetActive(true);
        }

        if (GameStateManager.instance.GameModeType == GameModeType.Perfect)
        {
            itemContents[0].gameObject.SetActive(false);
            itemContents[4].gameObject.SetActive(false);
        }

        selectItemText.localizationName = "SelectItem";
        selectItemText.ReLoad();


        if(GameStateManager.instance.WatchAdItem)
        {
            selectItemText.localizationName = "BoastItemUse";
            selectItemText.ReLoad();

            for (int i = 0; i < itemContents.Length; i++)
            {
                itemContents[i].UseItem();
            }

            GameStateManager.instance.Clock = true;
            GameStateManager.instance.Shield = true;
            GameStateManager.instance.Combo = true;
            GameStateManager.instance.Exp = true;
            GameStateManager.instance.Slow = true;
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

        selectItemText.localizationName = "Information_" + type.ToString();
        selectItemText.ReLoad();
    }

    public void SuccessWatchAd()
    {
        GameStateManager.instance.WatchAdItem = true;

        GameStateManager.instance.Clock = true;
        GameStateManager.instance.Shield = true;
        GameStateManager.instance.Combo = true;
        GameStateManager.instance.Exp = true;
        GameStateManager.instance.Slow = true;

        for (int i = 0; i < itemContents.Length; i ++)
        {
            itemContents[i].UseItem();
        }

        selectItemText.localizationName = "BoastItemUse";
        selectItemText.ReLoad();
    }
}
