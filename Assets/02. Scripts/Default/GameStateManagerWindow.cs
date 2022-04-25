#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.Serialization;
using UnityEditor;
using Sirenix.Utilities;

public class GameStateManagerWindow : OdinMenuEditorWindow
{
    public GameStateManager.GameSettings gameSettings;

    [MenuItem("Manager/GameStateManager")]
    private static void OpenWindow()
    {
        var window = GetWindow<GameStateManagerWindow>();

        // Nifty little trick to quickly position the window in the middle of the editor.
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        OnClose += SaveData;

        LoadData();
        tree.Add("GameStateManager", gameSettings);

        return tree;
    }

    private void LoadData()
    {
        Debug.Log("Load");
        try
        {
            string stjs = FileIO.LoadData(GameStateManager.DEVICESETTINGFILENAME, true); // 세팅 파일 호출

            if (!string.IsNullOrEmpty(stjs))
            {
                gameSettings = JsonUtility.FromJson<GameStateManager.GameSettings>(stjs);
            }
            else//없을 경우, 처음시작하는 경우
            {
                gameSettings = new GameStateManager.GameSettings();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Load Error\n" + e.Message);
        }
    }

    void SaveData()
    {
        Debug.Log("Save");
        try
        {
            string stjs = JsonUtility.ToJson(gameSettings);
            FileIO.SaveData(GameStateManager.DEVICESETTINGFILENAME, stjs, true);

        }
        catch (System.Exception e)
        {
            Debug.LogError("Save Error\n" + e.Message);
        }
    }
}

#endif