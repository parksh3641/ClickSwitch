using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveByte : MonoBehaviour {

    public static void SaveByteArrayToPlayerPref(byte[] data)
    {
        string dataString = System.Convert.ToBase64String(data);

        PlayerPrefs.SetString("AppleLogin", dataString);

        PlayerPrefs.Save();

        Debug.Log("Apple Login Data Save");
    }

    public static byte[] LoadByteArrayToPlayerPrefs()
    {
        string dataString = PlayerPrefs.GetString("AppleLogin");

        if(!string.IsNullOrEmpty(dataString))
        {
            byte[] data = System.Convert.FromBase64String(dataString);

            Debug.Log("Apple Login Data Load");

            return data;
        }
        else
        {
            Debug.Log("Apple Login Data is Null");
            return null;
        }
    }
}
