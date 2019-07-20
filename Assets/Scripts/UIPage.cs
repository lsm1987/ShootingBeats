using UnityEngine;

// 화면 전체를 차지하는 윈도우
public class UIPage : UIWindow
{
    [SerializeField]
    private RectTransform _safeRect = null;

    [SerializeField]
    private RectTransform _topRect = null;

    [SerializeField]
    private RectTransform _bottomRect = null;

    [SerializeField]
    private bool _useTopSafeArea = false;
    [SerializeField]
    private bool _useBottomSafeArea = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        InitializeRects();
    }

    private void InitializeRects()
    {
        float safeAreaAnchorMinY = _useBottomSafeArea ? (UIUtil.SafeArea.y / Screen.height) : 0.0f;
        float safeAreaAnchorMaxY = _useTopSafeArea ? ((UIUtil.SafeArea.y + UIUtil.SafeArea.height) / Screen.height) : 1.0f;

        if (_safeRect)
        {
            _safeRect.anchorMin = new Vector2(0.0f, safeAreaAnchorMinY);
            _safeRect.anchorMax = new Vector2(1.0f, safeAreaAnchorMaxY);
        }

        if (_topRect)
        {
            _topRect.anchorMin = new Vector2(0.0f, safeAreaAnchorMaxY);
            _topRect.anchorMax = new Vector2(1.0f, 1.0f);
        }

        if (_bottomRect)
        {
            _bottomRect.anchorMin = new Vector2(0.0f, 0.0f);
            _bottomRect.anchorMax = new Vector2(1.0f, safeAreaAnchorMinY);
        }
    }

    protected void AddHeaderPanel(string title, UnityEngine.Events.UnityAction onBackClicked)
    {
        if (!_safeRect)
        {
            Debug.LogWarning("Cannot add header. Safe Rect is not exist. title: " + title);
            return;
        }

        Object prefab = Resources.Load(Define._uiHeaderPanel);
        GameObject obj = Instantiate(prefab) as GameObject;
        UIHeaderPanel header = obj.GetComponent<UIHeaderPanel>();
        header._Trans.SetParent(_safeRect, false);
        header._RectTrans.localScale = Vector3.one;
        header.Initialize(title, onBackClicked);
    }
}