//using Facebook.Unity;
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
#if UNITY_IOS
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Extensions;
#endif

using EntityKey = PlayFab.ProfilesModels.EntityKey;
using System.Text;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;

    public UIManager uiManager;
    public SoundManager soundManager;
    public OptionContent optionContent;

    [ShowInInspector]
    string customId = "";

    public bool isActive = false;

#if UNITY_IOS
    private string AppleUserIdKey = "";
    private IAppleAuthManager _appleAuthManager;

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
                    //OnClickFacebookLogin();
                    break;
                case LoginType.Apple:
                    OnClickAppleLogin();
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
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            _appleAuthManager = new AppleAuthManager(deserializer);
        }
        StartCoroutine(AppleAuthUpdate());
    }

    IEnumerator AppleAuthUpdate()
    {
        while (true)
        {
            _appleAuthManager?.Update();
            yield return null;
        }
    }
#endif
    #endregion

    public void LogOut()
    {
#if UNITY_EDITOR
        OnClickGuestLogout();
#elif UNITY_ANDROID
        OnClickGoogleLogout();
#endif

        uiManager.OnLogout();

        GameStateManager.instance.PlayfabId = "";
        GameStateManager.instance.CustomId = "";
        GameStateManager.instance.AutoLogin = false;
        GameStateManager.instance.Login = LoginType.None;

        Debug.Log("Logout");

        PlayerPrefs.SetInt("Version", 0);

        SceneManager.LoadScene("LoginScene");
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

        Debug.Log("???????????? ?????????????? ??????????????");

        //if (Social.localUser.authenticated)
        //{
        //    Debug.Log("???????? ???????????? ?????????????? ???????????????? ??????????????????????.");
        //    return;
        //}
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
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
                Debug.Log("???????????????????? ???????????? ?????????????? ????????????!");

                GameStateManager.instance.AutoLogin = true;
                GameStateManager.instance.Login = LoginType.Google;

                OnLoginSuccess(result);
            },
            error =>
            {
                Debug.Log("???????????????????? ???????????? ?????????????? ????????????!");

                DisplayPlayfabError(error);
            });
        });

#endif
    }

    public void OnClickGoogleLogout()
    {
#if UNITY_ANDROID
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

                        GameStateManager.instance.AutoLogin = true;
                        GameStateManager.instance.Login = LoginType.Google;
                        optionContent.SuccessLink(LoginType.Google);
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
#endif
    }
#endregion
#region Apple Login

    public void OnClickAppleLogin()
    {
#if UNITY_IOS
        Debug.Log("Try Apple Login");
        StartCoroutine(AppleLoginCor());
#endif
    }

#if UNITY_IOS
    void SignInWithApple()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        _appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    OnClickAppleLogin(appleIdCredential.IdentityToken);
                }
            }, error =>
            {
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
            });
    }

    IEnumerator AppleLoginCor()
    {
        IOSActivate();

        var _newAppleUser = false;

        while (_appleAuthManager == null) yield return null;

        if (!_newAppleUser)
        {
            var quickLoginArgs = new AppleAuthQuickLoginArgs();

            _appleAuthManager.QuickLogin(
                quickLoginArgs,
                credential =>
                {
                    var appleIdCredential = credential as IAppleIDCredential;
                    if (appleIdCredential != null)
                    {
                        OnClickAppleLogin(appleIdCredential.IdentityToken);
                    }
                },
                error =>
                {
                    _newAppleUser = true;
                    SignInWithApple();
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                });
        }
        else
        {
            SignInWithApple();
        }
        yield return null;
    }

    public void OnClickAppleLogin(byte[] identityToken)
    {
        PlayFabClientAPI.LoginWithApple(new LoginWithAppleRequest
        {
            CreateAccount = true,
            IdentityToken = Encoding.UTF8.GetString(identityToken),
            TitleId = PlayFabSettings.TitleId
        }
        , result =>
        {
            Debug.Log("Apple Login Success");

            GameStateManager.instance.AutoLogin = true;
            GameStateManager.instance.Login = LoginType.Apple;

            OnLoginSuccess(result);
        }
        , DisplayPlayfabError);
    }

    public void OnClickAppleLink(bool forceLink = false)
    {
        var quickLoginArgs = new AppleAuthQuickLoginArgs();

        _appleAuthManager.QuickLogin(quickLoginArgs, credential =>
        {
            var appleIdCredential = credential as IAppleIDCredential;
            if (appleIdCredential != null)
            {
                TryLinkAppleAccount(appleIdCredential.IdentityToken, forceLink);
            }
        }, error =>
        {
            var authorizationErrorCode = error.GetAuthorizationErrorCode();
        });
    }

    public void TryLinkAppleAccount(byte[] identityToken, bool forceLink)
    {
        PlayFabClientAPI.LinkApple(new LinkAppleRequest
        {
            ForceLink = forceLink,
            IdentityToken = Encoding.UTF8.GetString(identityToken)
        }
        , result =>
        {
            Debug.Log("Link Apple Success!!");

            GameStateManager.instance.AutoLogin = true;
            GameStateManager.instance.Login = LoginType.Apple;
            optionContent.SuccessLink(LoginType.Apple);
        }
        , DisplayPlayfabError);
    }
#endif
#endregion

    public void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        SetEditorOnlyMessage("Playfab Login Success");

        customId = result.PlayFabId;
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;

        GameStateManager.instance.PlayfabId = result.PlayFabId;

#if UNITY_EDITOR
        StartCoroutine(LoadDataCoroutine());
#else
        GetTitleInternalData("CheckVersion", CheckVersion);
#endif

    }

    public void CheckVersion(bool check)
    {
        if(check)
        {
#if UNITY_EDITOR || UNITY_ANDROID
            GetTitleInternalData("AOSVersion", CheckUpdate);
#elif UNITY_IOS
            GetTitleInternalData("IOSVersion", CheckUpdate);
#endif
        }
        else
        {
            StartCoroutine(LoadDataCoroutine());
        }
    }

    public void CheckUpdate(bool check)
    {
        Debug.Log("Checking Version...");

        if (check)
        {
            StartCoroutine(LoadDataCoroutine());
        }
        else
        {
            uiManager.OnNeedUpdate();
        }
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

        yield return GetCatalog();

        yield return new WaitForSeconds(0.5f);

        yield return GetUserInventory();

        yield return GetStatistics();

        yield return GetPlayerData();

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Load Data Complete");

        isActive = true;

        uiManager.OnLoginSuccess();

        StateManager.instance.Initialize();
    }

    public bool GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            var Inventory = result.Inventory;
            int gold = result.VirtualCurrency["GO"]; //Get Money
            int crystal = result.VirtualCurrency["ST"]; //Get Money

            if(gold > 1000000)
            {
                gold = 1000000;
            }

            if(crystal > 100000)
            {
                crystal = 100000;
            }

            playerDataBase.Coin = gold;
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
                        playerDataBase.RemoveAd = true;
                    }

                    if(list.ItemId.Equals("Clock"))
                    {
                        playerDataBase.Clock = (int)list.RemainingUses;
                    }

                    if (list.ItemId.Equals("Shield"))
                    {
                        playerDataBase.Shield = (int)list.RemainingUses;
                    }

                    if (list.ItemId.Equals("Combo"))
                    {
                        playerDataBase.Combo = (int)list.RemainingUses;
                    }

                    if (list.ItemId.Equals("Exp"))
                    {
                        playerDataBase.Exp = (int)list.RemainingUses;
                    }

                    if (list.ItemId.Equals("Slow"))
                    {
                        playerDataBase.Slow = (int)list.RemainingUses;
                    }

                    if (list.ItemId.Contains("Icon_"))
                    {
                        IconType icon = (IconType)Enum.Parse(typeof(IconType), list.ItemId);
                        shopDataBase.SetIcon(icon, (int)list.RemainingUses);
                    }

                    shopDataBase.SetItemInstanceId(list.ItemId, list.ItemInstanceId);
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
                else if(catalog.ItemClass.Equals("Item"))
                {
                    shopDataBase.SetItem(shopClass);
                }
                else if(catalog.ItemId.Equals("IconBox"))
                {
                    shopDataBase.SetETC(shopClass);
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
                       case "Level":
                           playerDataBase.Level = statistics.Value;
                           break;
                       case "Exp":
                           playerDataBase.Experience = statistics.Value;
                           break;
                       case "Icon":
                           playerDataBase.Icon = statistics.Value;
                           break;
                       case "TotalScore":
                           playerDataBase.TotalScore = statistics.Value;
                           break;
                       case "TotalCombo":
                           playerDataBase.TotalCombo = statistics.Value;
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
                       case "TimingActionScore":
                           playerDataBase.BestTimingActionScore = statistics.Value;
                           break;
                       case "TimingActionCombo":
                           playerDataBase.BestTimingActionCombo = statistics.Value;
                           break;
                       case "DragActionScore":
                           playerDataBase.BestDragActionScore = statistics.Value;
                           break;
                       case "DragActionCombo":
                           playerDataBase.BestDragActionCombo = statistics.Value;
                           break;
                       case "AttendanceDay":
                           playerDataBase.AttendanceDay = statistics.Value.ToString();
                           break;
                       case "GameMode":
                           playerDataBase.GameMode = statistics.Value.ToString();
                           break;
                       case "NewsAlarm":
                           playerDataBase.NewsAlarm = statistics.Value;
                           break;
                       case "DailyMissionCount":
                           playerDataBase.DailyMissionCount = statistics.Value;
                           break;
                       case "DailyMissionClear":
                           if(statistics.Value == 0)
                           {
                               playerDataBase.DailyMissionClear = false;
                           }
                           else
                           {
                               playerDataBase.DailyMissionClear = true;
                           }
                           break;
                       case "StartTimeLevel":
                           playerDataBase.StartTimeLevel = statistics.Value;
                           break;
                       case "CriticalLevel":
                           playerDataBase.CriticalLevel = statistics.Value;
                           break;
                       case "BurningLevel":
                           playerDataBase.BurningLevel = statistics.Value;
                           break;
                       case "AddExpLevel":
                           playerDataBase.AddExpLevel = statistics.Value;
                           break;
                       case "IconBox":
                           playerDataBase.IconBox = statistics.Value;
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
            TrophyData trophyData = new TrophyData();
            DailyMission dailyMission = new DailyMission();

            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                if(key.Contains("GameChoice"))
                {
                    trophyData = JsonUtility.FromJson<TrophyData>(eachData.Value.Value);
                    playerDataBase.SetTrophyData(trophyData);
                }
                else if(key.Contains("DailyMission"))
                {
                    string[] number = key.Split('_');
                    dailyMission = JsonUtility.FromJson<DailyMission>(eachData.Value.Value);
                    playerDataBase.SetDailyMission(dailyMission, int.Parse(number[1]));
                }

            }

        }, DisplayPlayfabError);

        return true;
    }

    public void GetPlayerProfile(string playFabId, Action<string> action)
    {
        string countryCode = "";

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowLocations = true
            }
        }, result =>
        {
            countryCode = result.PlayerProfile.Locations[0].CountryCode.Value.ToString();
            action?.Invoke(countryCode);

        }, error =>
        {
            action?.Invoke("");
        });
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
        try
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
            },
            result =>
            {
                OnCloudUpdateStats(result);

                switch(name)
                {
                    case "IconBox":
                        playerDataBase.IconBox += value;
                        break;
                }
            }
            , DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void UpdateAddCurrency(MoneyType type ,int number)
    {
        string currentType = "";

        switch (type)
        {
            case MoneyType.Coin:
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


                switch (type)
                {
                    case MoneyType.Coin:
                        uiManager.goldAnimation.OnPlayCoinAnimation(MoneyType.Coin, playerDataBase.Coin, number);
                        playerDataBase.Coin += number;
                        break;
                    case MoneyType.Crystal:
                        uiManager.goldAnimation.OnPlayCoinAnimation(MoneyType.Crystal, playerDataBase.Crystal, number);
                        playerDataBase.Crystal += number;
                        break;
                }

                soundManager.PlaySFX(GameSfxType.GetMoney);
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
            case MoneyType.Coin:
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

            switch (type)
            {
                case MoneyType.Coin:
                    playerDataBase.Coin -= number;
                    break;
                case MoneyType.Crystal:
                    playerDataBase.Crystal -= number;
                    break;
            }

            uiManager.RenewalVC();
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }


        uiManager.RenewalVC();
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

    public void GetTitleInternalData(string name, Action<bool> action)
    {
        PlayFabServerAPI.GetTitleInternalData(new PlayFab.ServerModels.GetTitleDataRequest(),
            result =>
            {
                if (name.Contains("Version"))
                {
                    if (result.Data[name].Equals(Application.version))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else
                {
                    if (result.Data[name].Equals("ON"))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
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

    public void GetLeaderboardMyRank(string name, Action<GetLeaderboardAroundPlayerResult> successCalback)
    {
        var request = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = name,
            MaxResultsCount = 1,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, successCalback, DisplayPlayfabError);
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

    public void ReadTitleNews(Action<List<TitleNewsItem>> action)
    {
        List<TitleNewsItem> item = new List<TitleNewsItem>();

        PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result =>
        {
            foreach (var list in result.News)
            {
                item.Add(list);
            }
            action.Invoke(item);

        }, error => Debug.LogError(error.GenerateErrorReport()));
    }


#region PurchaseItem
    public void PurchaseRemoveAd()
    {
        PurchaseItemToRM(shopDataBase.RemoveAds);

        playerDataBase.RemoveAd = true;
    }

    public void PurchaseCoin(int number)
    {
        UpdateAddCurrency(MoneyType.Coin, number);
    }

    public void PurchaseCrystal(int number)
    {
        UpdateAddCurrency(MoneyType.Crystal, number);
    }

    public void PurchaseItemToRM(ShopClass shopClass)
    {
        //try
        //{
        //    PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        //    {
        //        FunctionName = "PurchaseItem",
        //        FunctionParameter = new {
        //            ItemId = shopClass.itemId,
        //            Price = (int)shopClass.price,
        //            VirtualCurrency = shopClass.virtualCurrency
        //        },
        //        GeneratePlayStreamEvent = true,
        //    }, OnCloudUpdateStats, DisplayPlayfabError);
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError(e.Message);
        //}

        var request = new PurchaseItemRequest()
        {
            CatalogVersion = shopClass.catalogVersion,
            ItemId = shopClass.itemId,
            VirtualCurrency = shopClass.virtualCurrency,
            Price = (int)shopClass.price
        };
        PlayFabClientAPI.PurchaseItem(request, (result) =>
        {
            Debug.Log(shopClass.itemId + " ???????????? ????????????!");
        }, error =>
        {
            Debug.Log(shopClass.itemId + " ???????????? ????????????!");
        });
    }

    public void PurchaseItem(ShopClass shopClass, Action<bool> action, int number)
    {
        bool failed = false;

        for (int i = 0; i < number; i++)
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
                switch (shopClass.itemId)
                {
                    case "Clock":
                        playerDataBase.Clock += 1;
                        break;
                    case "Shield":
                        playerDataBase.Shield += 1;
                        break;
                    case "Combo":
                        playerDataBase.Combo += 1;
                        break;
                    case "Exp":
                        playerDataBase.Exp += 1;
                        break;
                    case "Slow":
                        playerDataBase.Slow += 1;
                        break;
                }

                switch (shopClass.virtualCurrency)
                {
                    case "GO":
                        playerDataBase.Coin -= (int)shopClass.price;
                        break;
                    case "ST":
                        playerDataBase.Crystal -= (int)shopClass.price;
                        break;
                }
            }, error =>
            {
                failed = true;
            });

            if(failed)
            {
                action.Invoke(false);
                Debug.Log(shopClass.itemId + " Buy Failed!");
                break;
            }
        }

        uiManager.RenewalVC();
        StateManager.instance.ChangeNumber();
        soundManager.PlaySFX(GameSfxType.BuyItem);

        action.Invoke(true);
        Debug.Log(shopClass.itemId + " Buy Success!");
    }

    public void CheckConsumeItem()
    {
        StartCoroutine(ConsumeItemCorution());
    }

    IEnumerator ConsumeItemCorution()
    {
        if (GameStateManager.instance.Clock)
        {
            playerDataBase.Clock -= 1;

            ConsumeItem(shopDataBase.GetItemInstanceId("Clock"));
        }

        yield return new WaitForSeconds(0.5f);

        if (GameStateManager.instance.Shield)
        {
            playerDataBase.Shield -= 1;

            ConsumeItem(shopDataBase.GetItemInstanceId("Shield"));
        }

        yield return new WaitForSeconds(0.5f);

        if (GameStateManager.instance.Combo)
        {
            playerDataBase.Combo -= 1;

            ConsumeItem(shopDataBase.GetItemInstanceId("Combo"));
        }

        yield return new WaitForSeconds(0.5f);

        if (GameStateManager.instance.Exp)
        {
            playerDataBase.Exp -= 1;

            ConsumeItem(shopDataBase.GetItemInstanceId("Exp"));
        }

        yield return new WaitForSeconds(0.5f);

        if (GameStateManager.instance.Slow)
        {
            playerDataBase.Slow -= 1;

            ConsumeItem(shopDataBase.GetItemInstanceId("Slow"));
        }

        yield return new WaitForSeconds(0.5f);

        StateManager.instance.ChangeNumber();
    }

    public void ConsumeItem(string itemInstanceID)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "ConsumeItem",
                FunctionParameter = new { ConsumeCount = 1, ItemInstanceId = itemInstanceID },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

#endregion

    public void GrantItemsToUser(string itemIds, string catalogVersion)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "GrantItemsToUser",
                FunctionParameter = new { ItemIds = itemIds, CatalogVersion = catalogVersion },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
