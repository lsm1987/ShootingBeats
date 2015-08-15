using UnityEngine;
using UnityEngine.UI;
using System;

// 노래 목록 UI
public class UIBeatList : UIWindow
{
    [SerializeField]
    private RectTransform _transContents;
    [SerializeField]
    private UIBeatReady _uiBeatReady;

    private BeatInfo[] _beatInfos; // 정렬된 정보들
    public delegate void InfoSelectedHandler(int index); // 정보 선택되었을 때 호출될 함수 형식
    private const string _uiTitle = "Beat List";
    private const float _firstItemYOffset = -15.0f; // 첫 항목 상단 여백

    protected override void OnAwake()
    {
        base.OnAwake();
        AddHeaderPanel(_uiTitle, OnBackClicked);
        BuildList();
    }

    // 목록 구성하기
    private void BuildList()
    {
        // 모든 노래 정보 구하기
        _beatInfos = Resources.LoadAll<BeatInfo>(BeatInfo._resourcePath);
        if (_beatInfos != null)
        {
            // 정보 정렬
            Array.Sort<BeatInfo>(_beatInfos, BeatInfo.CompareByListPriority);

            // 정보별 항목 UI 만들기
            float y = _firstItemYOffset;
            UnityEngine.Object prefabItem = Resources.Load("UI/UIBeatListItem");
            for (int i = 0; i < _beatInfos.Length; ++i)
            {
                BeatInfo beatInfo = _beatInfos[i];
                GameObject objItem = Instantiate(prefabItem) as GameObject;
                objItem.name = prefabItem.name + "_" + beatInfo.name;
                objItem.transform.SetParent(_transContents.transform, false);
                
                // Y 위치 재지정
                // http://orbcreation.com/orbcreation/page.orb?1099 참고
                RectTransform trans = objItem.GetComponent<RectTransform>();
                trans.localScale = Vector3.one;
                trans.localPosition = new Vector3(
                    trans.localPosition.x, y - ((1.0f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
                y -= (trans.rect.height + 1);

                // 정보 지정
                UIBeatListItem item = objItem.GetComponent<UIBeatListItem>();
                item.SetBeatInfo(i, OnInfoSelected, beatInfo);
            }

            // 목록 전체 높이 재지정
            {
                Vector2 newSize = new Vector2(_transContents.rect.size.x, Mathf.Abs(y));
                Vector2 oldSize = _transContents.rect.size;
                Vector2 deltaSize = newSize - oldSize;
                _transContents.offsetMin = _transContents.offsetMin - new Vector2(deltaSize.x * _transContents.pivot.x, deltaSize.y * _transContents.pivot.y);
                _transContents.offsetMax = _transContents.offsetMax + new Vector2(deltaSize.x * (1.0f - _transContents.pivot.x), deltaSize.y * (1.0f - _transContents.pivot.y));
            }

        } // if (beatInfos != null)
    }

    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadTitleScene();
        }
        return true;
    }

    // Back 버튼이 클릭되었을 때
    public void OnBackClicked()
    {
        LoadTitleScene();
    }

    /// <summary>
    /// 타이틀 씬으로 돌아감
    /// </summary>
    private void LoadTitleScene()
    {
        Application.LoadLevel(SceneName._Title);
    }

    /// <summary>
    /// 정보가 선택되었을 때
    /// </summary>
    /// <param name="index">몇 번째 정보인가?</param>
    private void OnInfoSelected(int index)
    {
        //Debug.Log("[UIBeatList] [" + index.ToString() + "] " + _beatInfos[index]._title + " selcted");
        _uiBeatReady.Open(_beatInfos[index]);
    }
}
