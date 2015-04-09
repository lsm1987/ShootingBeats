using UnityEngine;
using System.Collections;

// 노래 하나의 정보
public class BeatInfo : ScriptableObject
{
    public const string _resourcePath = "BeatInfos"; // 리소스 폴더 내에서 어떤 경로에 위치할 것인가?
    public enum Difficulty
    {
        Normal, Hard, Extreme,
    }

    public int _listPriority = 0;  // 리스트 목록 우선순위. 낮을수록 앞에 위치
    public string _title; // 노래 제목
    public Difficulty _difficulty;  // 난이도
    public string _songFile; // 음악 파일명
    public int _length; // 노래 길이. 단위 초
    public string _namespace;   // 코드 namespace

    /// <summary>
    /// 목록 우선순위 비교
    /// </summary>
    /// <param name="info1"></param>
    /// <param name="info2"></param>
    /// <returns></returns>
    public static int CompareByListPriority(BeatInfo info1, BeatInfo info2)
    {
        return info1._listPriority.CompareTo(info2._listPriority);
    }
}