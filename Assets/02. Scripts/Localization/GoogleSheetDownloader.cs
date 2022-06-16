using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetDownloader : MonoBehaviour
{
    const string LocalizationURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=0";
    const string ValueURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=1957583039";

    public bool isActive = false;

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

        //if(localizationDataBase.localizationDatas.Count <= 0)
        //{
        //    SyncFile();
        //}

        SyncFile();
    }

    IEnumerator DownloadFile()
    {
        UnityWebRequest www = UnityWebRequest.Get(LocalizationURL);
        yield return www.SendWebRequest();
        SetLocalization(www.downloadHandler.text);

        UnityWebRequest www2 = UnityWebRequest.Get(ValueURL);
        yield return www2.SendWebRequest();
        SetValue(www2.downloadHandler.text);
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
            }
        }

        isActive = true;

        Debug.Log("Value File Download Complete!");
    }

    [Button]
    public void SyncFile()
    {
        Debug.Log("Localization File Downloading...");

        isActive = false;

        localizationDataBase.Initialize();
        valueDataBase.Initialize();

        StartCoroutine(DownloadFile());
    }
}
