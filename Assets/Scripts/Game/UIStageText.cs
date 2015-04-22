using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 영역에 맞춰 생성되는 텍스트 UI
/// </summary>
public class UIStageText : MonoBehaviour
{
    [SerializeField]
    private RectTransform _rectTrans;
    [SerializeField]
    private Text _text;

    private GameObject _go;
    protected GameObject _Go { get { if (_go == null) { _go = gameObject; } return _go; } }
    private Transform _trans;
    public Transform _Trans { get { if (_trans == null) { _trans = transform; } return _trans; } }
    private Canvas _canvas;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x">x 위치. 게임 좌표계</param>
    /// <param name="x">y 위치. 게임 좌표계</param>
    /// <param name="width">너비. UI 픽셀 좌표계</param>
    /// <param name="height">높이. UI 픽셀 좌표계</param>
    /// <param name="text"></param>
    public void Set(float x, float y, float width, float height, string text)
    {
        SetPosition(x, y);
        SetSize(width, height);
        SetText(text);
    }

    public void SetPosition(float x, float y)
    {
        Camera worldCamera = _canvas.worldCamera;
        Vector2 screenPos = worldCamera.WorldToScreenPoint(new Vector3(x, y, 0.0f));
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTrans, screenPos, worldCamera, out localPos);
        _rectTrans.localPosition = localPos;
    }

    public void SetSize(float width, float height)
    {
        Vector2 newSize = new Vector2(width, height);
        Vector2 oldSize = _rectTrans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        _rectTrans.offsetMin = _rectTrans.offsetMin - new Vector2(deltaSize.x * _rectTrans.pivot.x, deltaSize.y * _rectTrans.pivot.y);
        _rectTrans.offsetMax = _rectTrans.offsetMax + new Vector2(deltaSize.x * (1f - _rectTrans.pivot.x), deltaSize.y * (1f - _rectTrans.pivot.y));
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetActive(bool active)
    {
        _Go.SetActive(active);
    }

    public void Initialize(Transform parent)
    {
        _Trans.SetParent(parent, false);
        _rectTrans.localScale = Vector3.one;
        _canvas = GetComponentInParent<Canvas>();
    }
}