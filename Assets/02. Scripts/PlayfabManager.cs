using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.ProfilesModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using EntityKey = PlayFab.ProfilesModels.EntityKey;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;

    public UnityEvent loginSuccessEvent;
    public UnityEvent logoutEvent;

    [ShowInInspector]
    string customId = "";

    public bool isActive = false;

#if UNITY_IOS
    private string AppleUserIdKey = "";

#endif

    public PlayerDataBase playerDataBase;
    public ShopDataBase shopDataBase;

    [Header("Entity")]
    private string entityId;
    private string entityType;
    private readonly Dictionary<string, string> entityFileJson = new Dictionary<string, string>();

    private List<ItemInstance> inventoryList = new List<ItemInstance>();


    private void Awake()
    {
        instance = this;

#if UNITY_ANDROID
        GoogleActivate();
#elif UNITY_IOS
        IOSActivate();
#endif

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
    }

    private void Start()
    {
        if(GameStateManager.instance.AutoLogin)
        {
            switch (GameStateManager.instance.Login)
            {
                case LoginType.None:
                    break;
                case LoginType.Guest:
                    OnClickGuestLogin();
                    break;
                case LoginType.Google:
                    OnClickGoogleLogin();
                    break;
                case LoginType.Facebook:
                    break;
            }
        }
    }

    #region Initialize

#if UNITY_ANDROID
    private void GoogleActivate()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .AddOauthScope("profile")
        .RequestServerAuthCode(false)
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
#endif
#if UNITY_IOS
    private void IOSActivate()
    {
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            _appleAuthManager = new AppleAuthManager(deserializer);
        }
        if (_appleAuthManager == null)
        {
            // SetupLoginMenuForUnsupportedPlatform();
            return;
        }
    }
#endif
    #endregion

    void LogOut()
    {
        Debug.Log("·Î±×¾Æ¿ô µÇ¾ú½À´Ï´Ù.");

        logoutEvent.Invoke();

        GameStateManager.instance.PlayfabId = "";
        GameStateManager.instance.CustomId = "";
        GameStateManager.instance.AutoLogin = false;
        GameStateManager.instance.Login = LoginType.None;
    }


    #region Message
    private void SetEditorOnlyMessage(string message, bool error = false)
    {
#if UNITY_EDITOR
        if (error) Debug.LogError("<color=red>" + message + "</color>");
        else Debug.Log(message);
#endif
    }
    private void DisplayPlayfabError(PlayFabError error) => SetEditorOnlyMessage("error : " + error.GenerateErrorReport(), true);

    #endregion
    #region GuestLogin
    public void OnClickGuestLogin()
    {
        customId = GameStateManager.instance.CustomId;

        if (string.IsNullOrEmpty(customId))
            CreateGuestId();
        else
            LoginGuestId();
    }

    private void CreateGuestId()
    {
        Debug.Log("New PlayfabId");

        customId = GetRandomPassword(16);

        GameStateManager.instance.CustomId = customId;

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = customId,
            CreateAccount = true
        }, result =>
        {
            GameStateManager.instance.AutoLogin = true;
            GameStateManager.instance.Login = LoginType.Guest;
            OnLoginSuccess(result);
        }, error =>
        {
            Debug.LogError("Login Fail - Guest");
        });
    }

    private string GetRandomPassword(int _totLen)
    {
        string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var chars = Enumerable.Range(0, _totLen)
            .Select(x => input[UnityEngine.Random.Range(0, input.Length)]);
        return new string(chars.ToArray());
    }
    private void LoginGuestId()
    {
        Debug.Log("Guest Login");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = customId,
            CreateAccount = false
        }, result =>
        {
            OnLoginSuccess(result);
        }, error =>
        {
            Debug.LogError("Login Fail - Guest");
        });
    }

    public void OnClickGuestLogout()
    {
        Debug.LogError("Guest Logout");

        LogOut();
        PlayFabClientAPI.ForgetAllCredentials();
    }

    #endregion
    #region Google Login
    public void OnClickGoogleLogin()
    {
#if UNITY_ANDROID
        LoginGoogleAuthenticate();
#else
        SetEditorOnlyMessage("Only Android Platform");
#endif
    }

    private void LoginGoogleAuthenticate()
    {
#if UNITY_ANDROID

        Debug.Log("±¸±Û ·Î±×ÀÎ ½ÃµµÁß");

        //if (Social.localUser.authenticated)
        //{
        //    Debug.Log("ÀÌ¹Ì ±¸±Û ·Î±×ÀÎ µÇ¾îÀÖ´Â »óÅÂÀÔ´Ï´Ù.");
        //    return;
        //}
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                Debug.Log("±¸±Û »ç¿ëÀÚ ÀÎÁõ ½ÇÆÐ!");
                return;
            }

            var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                ServerAuthCode = serverAuthCode,
                CreateAccount = true,
            },
            result =>
            {
                Debug.Log("ÇÃ·¹ÀÌÆÕ ±¸±Û ·Î±×ÀÎ ¼º°ø!");

                GameStateManager.instance.AutoLogin = true;
                GameStateManager.instance.Login = LoginType.Google;

                OnLoginSuccess(result);
            },
            error =>
            {
                Debug.Log("ÇÃ·¹ÀÌÆÕ ±¸±Û ·Î±×ÀÎ ½ÇÆÐ!");

                DisplayPlayfabError(error);
            });
        });

#endif
    }

    public void OnClickGoogleLogout()
    {
#if UNITY_ANDROID
        LogOut();
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
    }

    public void OnClickGoogleLink()
    {
#if UNITY_ANDROID
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                    LinkGoogleAccountRequest request = new LinkGoogleAccountRequest()
                    {
                        ForceLink = true,
                        ServerAuthCode = serverAuthCode
                    };

                    PlayFabClientAPI.LinkGoogleAccount(request, result =>
                    {
                        Debug.Log("Link Google Account Success");
                    }, error =>
                    {
                        Debug.Log(error.GenerateErrorReport());
                    });
                }
                else
                {
                    Debug.Log("Link Google Account Fail");
                }
            });
        }
        else
        {
            Debug.Log("Link Google Account Fail");
        }
    }

#endif
    #endregion

    public void OnLoginSuccess(LoginResult result)
    {
        SetEditorOnlyMessage("Playfab Login Success");

        customId = result.PlayFabId;
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;

        GameStateManager.instance.PlayfabId = result.PlayFabId;

        StartCoroutine(LoadDataCoroutine());
    }

    public void SetProfileLanguage(LanguageType type)
    {
        EntityKey entity = new EntityKey();
        entity.Id = entityId;
        entity.Type = entityType;

        var request = new SetProfileLanguageRequest
        {
            Language = type.ToString(),
            ExpectedVersion = 0,
            Entity = entity
        };
        PlayFabProfilesAPI.SetProfileLanguage(request, res =>
        {
            Debug.Log("The language on the entity's profile has been updated.");
        }, FailureCallback);
    }

    public void GetPlayerNickName()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = customId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        (result) =>
        {
            GameStateManager.instance.NickName = result.PlayerProfile.DisplayName;

            if (GameStateManager.instance.NickName == null)
            {
                UpdateDisplayName(GameStateManager.instance.PlayfabId);
            }
            // GameStateManager.Instance.SavePlayerData();
        },
        DisplayPlayfabError);
    }

    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    private void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        SetEditorOnlyMessage(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        foreach (var json in jsonResult)
        {
            SetEditorOnlyMessage(json.Key + " / " + json.Value);
        }
        object messageValue;
        jsonResult.TryGetValue("OnCloudUpdateStats() messageValue", out messageValue);
        SetEditorOnlyMessage((string)messageValue);

        //GetUserInventory();
    }


    IEnumerator LoadDataCoroutine()
    {
        Debug.Log("Load Data...");

        playerDataBase.Initialize();
        shopDataBase.Initialize();

        GetPlayerNickName();

        yield return new WaitForSeconds(0.5f);

        yield return GetUserInventory();

        yield return GetCatalog();

        yield return GetStatistics();

        yield return GetPlayerData();

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Load Data Complete");

        isActive = true;

        loginSuccessEvent.Invoke();
    }

    public bool GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            var Inventory = result.Inventory;
            int gold = result.VirtualCurrency["GO"]; //Get Money
            int crystal = result.VirtualCurrency["ST"]; //Get Money

            playerDataBase.Gold = gold;
            playerDataBase.Crystal = crystal;

            if (Inventory != null)
            {
                for (int i = 0; i < Inventory.Count; i++)
                {
                    inventoryList.Add(Inventory[i]);
                }

                foreach (ItemInstance list in inventoryList)
                {
                    if(list.ItemId.Equals("RemoveAds"))
                    {
                        playerDataBase.removeAd = true;
                    }
                }
            }
            else
            {
                return;
            }

        }, DisplayPlayfabError);

        return true;
    }

    public bool GetCatalog()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = "Shop" }, shop =>
        {
            for (int i = 0; i < shop.Catalog.Count; i++)
            {
                var catalog = shop.Catalog[i];

                ShopClass shopClass = new ShopClass();

                shopClass.catalogVersion = catalog.CatalogVersion;
                shopClass.itemClass = catalog.ItemClass;
                shopClass.itemId = catalog.ItemId;

                foreach(string item in catalog.VirtualCurrencyPrices.Keys)
                {
                    shopClass.virtualCurrency = item;
                }

                foreach (uint item in catalog.VirtualCurrencyPrices.Values)
                {
                    shopClass.price = item;
                }

                if (catalog.ItemId.Equals("RemoveAds"))
                {
                    shopDataBase.RemoveAds = shopClass;
                }

            }
        }, (error) =>
        {

        });

        return true;
    }

    public bool GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           (Action<GetPlayerStatisticsResult>)((result) =>
           {
               foreach (var statistics in result.Statistics)
               {
                   switch (statistics.StatisticName)
                   {
                       //case "":
                       //    string text = statistics.Value.ToString();
                       //    break;
                       case "TotalScore":
                           playerDataBase.TotalScore = statistics.Value;
                           break;
                       case "SpeedTouchScore":
                           playerDataBase.BestSpeedTouchScore = statistics.Value;
                           break;
                       case "SpeedTouchCombo":
                           playerDataBase.BestSpeedTouchCombo = statistics.Value;
                           break;
                       case "MoleCatchScore":
                           playerDataBase.BestMoleCatchScore = statistics.Value;
                           break;
                       case "MoleCatchCombo":
                           playerDataBase.BestMoleCatchCombo = statistics.Value;
                           break;
                       case "FilpCardScore":
                           playerDataBase.BestFilpCardScore = statistics.Value;
                           break;
                       case "FilpCardCombo":
                           playerDataBase.BestFilpCardCombo = statistics.Value;
                           break;
                       case "ButtonActionScore":
                           playerDataBase.BestButtonActionScore = statistics.Value;
                           break;
                       case "ButtonActionCombo":
                           playerDataBase.BestButtonActionCombo = statistics.Value;
                           break;
                   }
               }
           })
           , (error) =>
           {

           });

        return true;
    }

    public void SetPlayerData(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Public };
        try
        {
            PlayFabClientAPI.UpdateUserData(request, (result) =>
            {
                Debug.Log("Update Player Data!");

            }, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public bool GetPlayerData()
    {
        var request = new GetUserDataRequest() { PlayFabId = GameStateManager.instance.PlayfabId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            AchievementData content = new AchievementData();

            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                content = JsonUtility.FromJson<AchievementData>(eachData.Value.Value);
                playerDataBase.OnSetAchievementContent(content);
            }

        }, DisplayPlayfabError);

        return true;
    }

    public void UpdatePlayerStatistics(List<StatisticUpdate> data)
    {
        if (NetworkConnect.instance.CheckConnectInternet())
        {
            try
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "UpdatePlayerStatistics",
                    FunctionParameter = new
                    {
                        Statistics = data
                    },
                    GeneratePlayStreamEvent = true,
                }, OnCloudUpdateStats
                , DisplayPlayfabError);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }
    }

    public void UpdatePlayerStatisticsInsert(string name, int value)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStatistics",
            FunctionParameter = new
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = name, Value = value}
                }
            },
            GeneratePlayStreamEvent = true,
        }, OnCloudUpdateStats
, DisplayPlayfabError);
    }

    public void UpdateAddCurrency(MoneyType type ,int number)
    {
        string currentType = "";

        switch (type)
        {
            case MoneyType.Gold:
                currentType = "GO";
                break;
            case MoneyType.Crystal:
                currentType = "ST";
                break;
        }

        if (NetworkConnect.instance.CheckConnectInternet())
        {
            try
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "AddMoney",
                    FunctionParameter = new { currencyType = currentType, currencyAmount = number },
                    GeneratePlayStreamEvent = true,
                }, OnCloudUpdateStats, DisplayPlayfabError);

                playerDataBase.Gold += number;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }

    }

    public void UpdateSubtractCurrency(MoneyType type, int number)
    {
        string currentType = "";

        switch (type)
        {
            case MoneyType.Gold:
                currentType = "GO";
                break;
            case MoneyType.Crystal:
                currentType = "ST";
                break;
        }

        if (NetworkConnect.instance.CheckConnectInternet())
        {
            try
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "SubtractMoney",
                    FunctionParameter = new { currencyType = currentType, currencyAmount = number },
                    GeneratePlayStreamEvent = true,
                }, OnCloudUpdateStats, DisplayPlayfabError);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }

    }

    public void UpdateDisplayName(string nickname, Action successAction, Action failAction)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nickname
        },
        result =>
        {
            Debug.Log("Update NickName : " + result.DisplayName);

            GameStateManager.instance.NickName = result.DisplayName;
            successAction?.Invoke();
        }
        , error =>
        {
            string report = error.GenerateErrorReport();
            if (report.Contains("Name not available"))
            {
                failAction?.Invoke();
            }
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void UpdateDisplayName(string nickname)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nickname
        },
        result =>
        {
            Debug.Log("Update First NickName : " + result.DisplayName);

            GameStateManager.instance.NickName = result.DisplayName;
        }
        , error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void GetTitleInternalData(string name, Action<bool> action) //true ï¿½Ï°ï¿½ï¿?ï¿½Ø´ï¿½ ï¿½ï¿½ï¿?ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½.
    {
        PlayFabServerAPI.GetTitleInternalData(new PlayFab.ServerModels.GetTitleDataRequest(),
            result =>
            {
                if (result.Data[name].Equals("ON"))
                {
                    action?.Invoke(true);
                }
                else
                {
                    action?.Invoke(false);
                }
            },
            error =>
            {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());

                action?.Invoke(false);
            }
        );
    }

    public void GetLeaderboarder(string name, Action<GetLeaderboardResult> successCalback)
    {
        var requestLeaderboard = new GetLeaderboardRequest
        {
            StartPosition = 0,
            StatisticName = name,
            MaxResultsCount = 100,

            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowLocations = true,
                ShowDisplayName = true,
                ShowStatistics = true
            }
        };

        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, successCalback, DisplayPlayfabError);
    }

    public void SetProfileLanguage(string language)
    {
        EntityKey entity = new EntityKey();
        entity.Id = entityId;
        entity.Type = entityType;

        var request = new SetProfileLanguageRequest
        {
            Language = language,
            ExpectedVersion = 0,
            Entity = entity
        };
        PlayFabProfilesAPI.SetProfileLanguage(request, res =>
        {
            Debug.Log("The language on the entity's profile has been updated.");
        }, FailureCallback);
    }

    public void GetServerTime(Action<DateTime> action)
    {
        if (NetworkConnect.instance.CheckConnectInternet())
        {
            try
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "GetServerTime",
                    GeneratePlayStreamEvent = true,
                }, result =>
                {
                    string date = PlayFabSimpleJson.SerializeObject(result.FunctionResult);

                    string year = date.Substring(1, 4);
                    string month = date.Substring(6, 2);
                    string day = date.Substring(9, 2);
                    string hour = date.Substring(12, 2);
                    string minute = date.Substring(15, 2);
                    string second = date.Substring(18, 2);

                    DateTime serverTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), 0, 0, 0);

                    serverTime = serverTime.AddDays(1);

                    DateTime time = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));

                    TimeSpan span = serverTime - time;

                    action?.Invoke(DateTime.Parse(span.ToString()));
                }, DisplayPlayfabError);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }
    }

    //"2022-04-24T22:17:04.548Z"


    #region PurchaseItem
    public void PurchaseRemoveAd()
    {
        Debug.Log("±¤°í Á¦°Å ±¸¸Å ¿Ï·á");

        playerDataBase.removeAd = true;

        PurchaseItem(shopDataBase.RemoveAds);
    }

    public void PurchaseItem(ShopClass shopClass)
    {
        var request = new PurchaseItemRequest()
        {
            CatalogVersion = shopClass.catalogVersion,
            ItemId = shopClass.itemId,
            VirtualCurrency = shopClass.virtualCurrency,
            Price = (int)shopClass.price
        };
        PlayFabClientAPI.PurchaseItem(request, (result) =>
        {
            Debug.Log(shopClass.itemId + " ±¸¸Å ¼º°ø!");
        }, error =>
        {
            Debug.Log(shopClass.itemId + " ±¸¸Å ½ÇÆÐ!");
        });
    }

    #endregion
}
