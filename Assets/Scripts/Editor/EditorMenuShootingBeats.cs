using UnityEngine;
using UnityEditor;

// 에디터의 ShootingBeats 메뉴
public class EditorMenuShootingBeats
{
    // 노래 정보 생성
    [MenuItem("ShootingBeats/Create BeatInfo")]
    private static void CreateBeatInfo()
    {
        string path = "Assets/Resources/" + BeatInfo._resourcePath + "/NewBeatInfo.asset";
        BeatInfo info = ScriptableObject.CreateInstance<BeatInfo>();
        AssetDatabase.CreateAsset(info, path);
        Debug.Log("[BeatInfo] BeatInfo is created in " + path);
    }
}