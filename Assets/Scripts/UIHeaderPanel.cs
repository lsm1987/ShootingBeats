using UnityEngine;
using UnityEngine.UI;

public class UIHeaderPanel : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Button _backButton;

    private Transform _trans;
    public Transform _Trans { get { if (_trans == null) { _trans = transform; } return _trans; } }
    private RectTransform _rectTrans;
    public RectTransform _RectTrans { get { if (_rectTrans == null) { _rectTrans = GetComponent<RectTransform>(); } return _rectTrans; } }

    public void Initialize(string title, UnityEngine.Events.UnityAction onBackClicked)
    {
        _title.text = title;

        if (onBackClicked != null)
        {
            _backButton.onClick.AddListener(onBackClicked);
        }
    }
}
