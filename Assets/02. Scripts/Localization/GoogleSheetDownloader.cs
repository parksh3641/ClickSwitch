using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetDownloader : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv";

    public bool isActive = false;

    public LocalizationDataBase localizationDataBase;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        if(!Directory.Exists(SystemPath.GetPath()))
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
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        SetLocalization(www.downloadHandler.text);
    }

    void SetLocalization(string tsv)
    {
        File.WriteAllText(SystemPath.GetPath() + "Localization.txt", tsv);

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 1; i < rowSize; i ++)
        {
            string[] column = row[i].Split('\t');
            LocalizationData content = new LocalizationData();

            content.key = column[0];
            content.korean = column[1].Replace('$','\n');
            content.english = column[2];

            localizationDataBase.SetLocalization(content);
        }

        isActive = true;

        Debug.Log("Localization File Download Complete!");
    }

    [Button]
    public void SyncFile()
    {
        Debug.Log("Localization File Downloading...");

        isActive = false;

        localizationDataBase.OnReset();
        StartCoroutine(DownloadFile());
    }
}
