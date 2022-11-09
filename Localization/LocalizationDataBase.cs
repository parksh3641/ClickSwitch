using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationDataBase", menuName = "ScriptableObjects/LocalizationDataBase")]
public class LocalizationDataBase : ScriptableObject
{
    public List<LocalizationData> localizationDatas = new List<LocalizationData>();


    public void Initialize()
    {
        localizationDatas.Clear();
    }

    public void SetLocalization(LocalizationData data)
    {
        localizationDatas.Add(data);
    }
    
}

[System.Serializable]
public class LocalizationData
{
    public string key = "";
    public string korean = "";
    public string english = "";
    public string japanese = "";
    public string chinese = "";
    public string indian = "";
    public string portuguese = "";
    public string russian = "";
    public string german = "";
    public string spanish = "";
    public string arabic = "";
    public string bengali = "";
    public string indonesian = "";
    public string italian = "";
    public string dutch = "";
}
