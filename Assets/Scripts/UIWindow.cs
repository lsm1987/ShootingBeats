using UnityEngine;

/// <summary>
/// 키입력 우선권을 갖는 윈도우
/// 캔버스 아래 첫 depth에 있다고 가정하고 siblilng index로 상위 UI 여부를 판단한다.
/// </summary>
public class UIWindow : MonoBehaviour
{
    private GameObject _go;
    protected GameObject _Go { get { if (_go == null) { _go = gameObject; } return _go; } }
    private Transform _trans;
    public Transform _Trans { get { if (_trans == null) { _trans = transform; } return _trans; } }
    private RectTransform _rectTrans;
    public RectTransform _RectTrans { get { if (_rectTrans == null) { _rectTrans = GetComponent<RectTransform>(); } return _rectTrans; } }
    private UISystem _uiSystem;
    protected UISystem _UISystem { get { if (_uiSystem == null) { _uiSystem = FindObjectOfType<UISystem>(); } return _uiSystem; } }

    private void Awake()
    {
        OnAwake();
    }

    /// <summary>
    /// 자식클래스용 Awake
    /// </summary>
    protected virtual void OnAwake()
    {
    }

    private void OnEnable()
    {
        if (_UISystem != null)
        {
            _UISystem.OnWindowEnabled(this);
        }
    }

    private void OnDisable()
    {
        if (_UISystem != null)
        {
            _UISystem.OnWindowDisabled(this);
        }
    }

    public int GetSiblingIndex()
    {
        return _Trans.GetSiblingIndex();
    }

    /// <summary>
    /// 키입력 처리. 상위 윈도우부터 호출함
    /// </summary>
    /// <returns>true: 하위 윈도우까지 키입력 처리를 전달하지 않는다.</returns>
    public virtual bool OnKeyInput()
    {
        return false;
    }

    /// <summary>
    /// Sibling Index 로 윈도우간 비교
    /// <para>Index 클수록 큼</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int CompareBySiblingIndex(UIWindow a, UIWindow b)
    {
        if (a == null)
        {
            if (b == null)
            {
                return 0;
            }
            else
            {
                return -1; // a < b
            }
        }
        else
        {
            if (b == null)
            {
                return 1; // a > b
            }
            else
            {
                return a.GetSiblingIndex().CompareTo(b.GetSiblingIndex());
            }
        }
    }

    /// <summary>
    /// CompareBySiblingIndex() 의 반대
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static int CompareBySiblingIndexReverse(UIWindow a, UIWindow b)
    {
        return CompareBySiblingIndex(a, b) * -1;
    }
}
