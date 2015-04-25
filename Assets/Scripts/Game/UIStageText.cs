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
    /// 앵커 위치 지정
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetAnchorPoistion(float x, float y)
    {
        _rectTrans.anchorMin = new Vector2(x, y);
        _rectTrans.anchorMax = new Vector2(x, y);
    }

    /// <summary>
    /// 게임 좌표계로 위치 지정
    /// <para>SetAlign() 이후에 수행할 것</para>
    /// </summary>
    /// <param name="x">x 위치. 게임 좌표계</param>
    /// <param name="x">y 위치. 게임 좌표계</param>
    public void SetGamePosition(float x, float y)
    {
        Camera worldCamera = _canvas.worldCamera;
        Vector2 screenPos = worldCamera.WorldToScreenPoint(new Vector3(x, y, 0.0f));
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTrans, screenPos, worldCamera, out localPos);
        _rectTrans.localPosition = localPos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width">너비. UI 픽셀 좌표계</param>
    /// <param name="height">높이. UI 픽셀 좌표계</param>
    public void SetSize(float width, float height)
    {
        Vector2 newSize = new Vector2(width, height);
        Vector2 oldSize = _rectTrans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        _rectTrans.offsetMin = _rectTrans.offsetMin - new Vector2(deltaSize.x * _rectTrans.pivot.x, deltaSize.y * _rectTrans.pivot.y);
        _rectTrans.offsetMax = _rectTrans.offsetMax + new Vector2(deltaSize.x * (1f - _rectTrans.pivot.x), deltaSize.y * (1f - _rectTrans.pivot.y));
    }

    public void SetAlign(TextAnchor textAnchor)
    {
        _text.alignment = textAnchor;

        switch (textAnchor)
        {
            case TextAnchor.LowerCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.UpperCenter:
                {
                    _rectTrans.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
            case TextAnchor.LowerLeft:
            case TextAnchor.MiddleLeft:
            case TextAnchor.UpperLeft:
                {
                    _rectTrans.pivot = new Vector2(0.0f, 0.5f);
                    break;
                }
            case TextAnchor.LowerRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.UpperRight:
                {
                    _rectTrans.pivot = new Vector2(1.0f, 0.5f);
                    break;
                }
        }
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