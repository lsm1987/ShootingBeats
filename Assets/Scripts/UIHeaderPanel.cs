using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class UIHeaderPanel : MonoBehaviour
{
    [SerializeField]
    private LocalizeStringEvent _strEvtTitle = null;
    [SerializeField]
    private Button _backButton = null;

    private Transform _trans;
    public Transform _Trans { get { if (_trans == null) { _trans = transform; } return _trans; } }
    private RectTransform _rectTrans;
    public RectTransform _RectTrans { get { if (_rectTrans == null) { _rectTrans = GetComponent<RectTransform>(); } return _rectTrans; } }

    public void Initialize(TableEntryReference strKeyTitle, UnityEngine.Events.UnityAction onBackClicked)
    {
        _strEvtTitle.StringReference.SetReference(StringTableName._ui, strKeyTitle);

        if (onBackClicked != null)
        {
            _backButton.onClick.AddListener(onBackClicked);
        }
    }
}
