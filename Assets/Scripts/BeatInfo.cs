﻿using UnityEngine;
using UnityEngine.Localization;

// 노래 하나의 정보
public class BeatInfo : ScriptableObject
{
    public const string _resourcePath = "BeatInfos"; // 리소스 폴더 내에서 어떤 경로에 위치할 것인가?

    public string _namespace;   // 문자열 구분자. 코드 음악파일명, namespace 등으로 사용
    public int _listPriority = 0;  // 리스트 목록 우선순위. 낮을수록 앞에 위치

    /// <summary>
    /// 노래 제목
    /// </summary>
    public LocalizedString _titleString;

    /// <summary>
    /// 저자
    /// </summary>
    public LocalizedString _authorString;

    public Difficulty _difficulty;  // 난이도
    public int _length; // 노래 길이. 단위 초
    public string _clearAchievementKey; // 클리어시 달성할 업적 key

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