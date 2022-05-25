using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageDataBase", menuName = "ScriptableObjects/ImageDataBase")]
public class ImageDataBase : ScriptableObject
{
    [Title("Image")]
    public Sprite[] iconArray;




    public Sprite[] GetIconArray()
    {
        return iconArray;
    }


}
