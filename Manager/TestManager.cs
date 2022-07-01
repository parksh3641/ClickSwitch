using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestManager : MonoBehaviour
{
    string customId = "";

    public int player = 10;

    public int number = 0;

    private void Start()
    {
        CreateGuestId();
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

    private void CreateGuestId()
    {
        Debug.LogError("New PlayfabId : " + number);

        customId = GetRandomPassword(16);

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = customId,
            CreateAccount = true
        }, result =>
        {
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

    public void OnLoginSuccess(LoginResult result)
    {
        UpdateStatistics();
    }

    void UpdateStatistics()
    {
        int score = 0;
        int combo = 0;

        int scoreR = 0;
        int comboR = 0;

        scoreR = Random.Range(10, 800);
        scoreR = (scoreR / 10) * 10;
        score += scoreR;
        UpdatePlayerStatisticsInsert("SpeedTouchScore", scoreR);

        scoreR = Random.Range(10, 400);
        scoreR = (scoreR / 10) * 10;
        score += scoreR;
        UpdatePlayerStatisticsInsert("MoleCatchScore", scoreR);

        scoreR = Random.Range(10, 600);
        scoreR = (scoreR / 10) * 10;
        score += scoreR;
        UpdatePlayerStatisticsInsert("FilpCardScore", scoreR);

        scoreR = Random.Range(10, 600);
        scoreR = (scoreR / 10) * 10;
        score += scoreR;
        UpdatePlayerStatisticsInsert("ButtonActionScore", scoreR);

        scoreR = Random.Range(10, 400);
        scoreR = (scoreR / 10) * 10;
        score += scoreR;
        UpdatePlayerStatisticsInsert("TimingActionScore", scoreR);

        scoreR = Random.Range(10, 400);
        scoreR = (scoreR / 10) * 10;
        score += scoreR;
        UpdatePlayerStatisticsInsert("DragActionScore", scoreR);

        UpdatePlayerStatisticsInsert("TotalScore", score);



        comboR = Random.Range(1, 80);
        combo += comboR;
        UpdatePlayerStatisticsInsert("SpeedTouchCombo", comboR);

        comboR = Random.Range(1, 40);
        combo += comboR;
        UpdatePlayerStatisticsInsert("MoleCatchCombo", comboR);

        comboR = Random.Range(1, 10);
        combo += comboR;
        UpdatePlayerStatisticsInsert("FilpCardCombo", comboR);

        comboR = Random.Range(1, 60);
        combo += comboR;
        UpdatePlayerStatisticsInsert("ButtonActionCombo", comboR);

        comboR = Random.Range(1, 40);
        combo += comboR;
        UpdatePlayerStatisticsInsert("TimingActionCombo", comboR);

        comboR = Random.Range(1, 40);
        combo += comboR;
        UpdatePlayerStatisticsInsert("DragActionCombo", comboR);


        UpdatePlayerStatisticsInsert("TotalCombo", combo);

        number++;

        if (number < player)
        {
            CreateGuestId();
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
        }, OnCloudUpdateStats, DisplayPlayfabError);
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
}
