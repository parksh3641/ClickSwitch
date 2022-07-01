using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetDownloader : MonoBehaviour
{
    const string LocalizationURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=0";
    const string ValueURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=1957583039";
    const string BadWordURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=582114712";

    public bool isActive = false;

    public Text messageText;

    LocalizationDataBase localizationDataBase;
    ValueDataBase valueDataBase;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Time.timeScale = 1;

        if(localizationDataBase == null) localizationDataBase = Resources.Load("LocalizationDataBase") as LocalizationDataBase;
        if (valueDataBase == null) valueDataBase = Resources.Load("ValueDataBase") as ValueDataBase;


        if (!Directory.Exists(SystemPath.GetPath()))
        {
            Directory.CreateDirectory(SystemPath.GetPath());
        }

        StartCoroutine(LoadingCoroution());

        SyncFile();
    }

    [Button]
    void OnResetVersion()
    {
        PlayerPrefs.SetInt("Version", 0);
    }

    IEnumerator LoadingCoroution()
    {
        messageText.text = "Loading";
        yield return new WaitForSeconds(0.5f);
        messageText.text = "Loading.";
        yield return new WaitForSeconds(0.5f);
        messageText.text = "Loading..";
        yield return new WaitForSeconds(0.5f);
        messageText.text = "Loading...";
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LoadingCoroution());
    }

    IEnumerator DownloadFile()
    {
        int saveVersion = PlayerPrefs.GetInt("Version");
        int version = int.Parse(Application.version.Replace(".", "").ToString());

        if (saveVersion < version)
        {
            Debug.Log("Localization File Downloading...");

            UnityWebRequest www = UnityWebRequest.Get(LocalizationURL);
            yield return www.SendWebRequest();
            SetLocalization(www.downloadHandler.text);

            PlayerPrefs.SetInt("Version", version);
        }

        if (!File.Exists(SystemPath.GetPath() + "Localization.txt"))
        {
            Debug.Log("Localization File Downloading...");

            UnityWebRequest www = UnityWebRequest.Get(LocalizationURL);
            yield return www.SendWebRequest();
            SetLocalization(www.downloadHandler.text);

            PlayerPrefs.SetInt("Version", version);
        }
        else
        {
            StreamReader reader = new StreamReader(SystemPath.GetPath() + "Localization.txt");
            string value = reader.ReadToEnd();
            reader.Close();

            SetLocalization(value);

            Debug.Log("Localization File is exists");
        }

        Debug.Log("Value File Downloading...");

        UnityWebRequest www2 = UnityWebRequest.Get(ValueURL);
        yield return www2.SendWebRequest();
        SetValue(www2.downloadHandler.text);

        if (!File.Exists(SystemPath.GetPath() + "BadWord.txt"))
        {
            Debug.Log("BadWord File Downloading...");

            UnityWebRequest www3 = UnityWebRequest.Get(BadWordURL);
            yield return www3.SendWebRequest();
            File.WriteAllText(SystemPath.GetPath() + "BadWord.txt", www3.downloadHandler.text);

            Debug.Log("BadWord File Download Complete!");
        }
        else
        {
            Debug.Log("BadWord File is exists");
        }

        isActive = true;
    }

    void SetLocalization(string tsv)
    {
        File.WriteAllText(SystemPath.GetPath() + "Localization.txt", tsv);

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        //int columnSize = row[0].Split('\t').Length;

        for (int i = 1; i < rowSize; i ++)
        {
            string[] column = row[i].Split('\t');
            LocalizationData content = new LocalizationData();

            content.key = column[0];
            content.korean = column[1].Replace('$','\n');
            content.english = column[2].Replace('$', '\n');
            content.japanese = column[3].Replace('$', '\n');
            content.chinese = column[4].Replace('$', '\n');
            content.indian = column[5].Replace('$', '\n');
            content.portuguese = column[6].Replace('$', '\n');
            content.russian = column[7].Replace('$', '\n');
            content.german = column[8].Replace('$', '\n');
            content.spanish = column[9].Replace('$', '\n');
            content.arabic = column[10].Replace('$', '\n');
            content.bengali = column[11].Replace('$', '\n');

            localizationDataBase.SetLocalization(content);
        }

        Debug.Log("Localization File Download Complete!");
    }

    void SetValue(string tsv)
    {
        File.WriteAllText(SystemPath.GetPath() + "Value.txt", tsv);

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;

        for (int i = 1; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            float value = float.Parse(column[1]);

            switch (column[0])
            {
                case "AdCoolTime":
                    valueDataBase.AdCoolTime = value;
                    break;
                case "ReadyTime":
                    valueDataBase.ReadyTime = value;
                    break;
                case "GamePlayTime":
                    valueDataBase.GamePlayTime = value;
                    break;
                case "ComboTime":
                    valueDataBase.ComboTime = value;
                    break;
                case "MoleNextTime":
                    valueDataBase.MoleNextTime = value;
                    break;
                case "MoleCatchTime":
                    valueDataBase.MoleCatchTime = value;
                    break;
                case "FilpCardRememberTime":
                    valueDataBase.FilpCardRememberTime = value;
                    break;
                case "ClockAddTime":
                    valueDataBase.ClockAddTime = value;
                    break;
                case "ComboAddTime":
                    valueDataBase.ComboAddTime = value;
                    break;
                case "DefaultExp":
                    valueDataBase.DefaultExp = value;
                    break;
                case "AddExp":
                    valueDataBase.AddExp = value;
                    break;
                case "GameChoice1Perfect":
                    valueDataBase.GameChoice1Perfect = value;
                    break;
                case "GameChoice2Perfect":
                    valueDataBase.GameChoice2Perfect = value;
                    break;
                case "GameChoice3Perfect":
                    valueDataBase.GameChoice3Perfect = value;
                    break;
                case "GameChoice4Perfect":
                    valueDataBase.GameChoice4Perfect = value;
                    break;
                case "GameChoice5Perfect":
                    valueDataBase.GameChoice5Perfect = value;
                    break;
                case "GameChoice6Perfect":
                    valueDataBase.GameChoice6Perfect = value;
                    break;

            }
        }

        Debug.Log("Value File Download Complete!");
    }

    [Button]
    public void SyncFile()
    {
        if(NetworkConnect.instance.CheckConnectInternet())
        {
            isActive = false;

            localizationDataBase.Initialize();
            valueDataBase.Initialize();

            StartCoroutine(DownloadFile());
        }
        else
        {
            StopAllCoroutines();
            messageText.text = "Reconnect internet...";
            StartCoroutine(DelayCorution());
        }
    }

    IEnumerator DelayCorution()
    {
        yield return new WaitForSeconds(3f);
        SyncFile();
    }
}
