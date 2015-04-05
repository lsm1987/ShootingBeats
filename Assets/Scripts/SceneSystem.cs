using UnityEngine;
using System.Collections;

/// <summary>
/// 하나의 씬 전반을 관리
/// </summary>
public class SceneSystem : MonoBehaviour
{
    protected UISystem _UISystem { get; private set; } // 이 씬에 배치된 UI 시스템
    public bool _HasKeyInputFocus { get; private set; } // 씬이 키입력 포커스를 갖고 있는가?

    private void Awake()
    {
        _UISystem = FindObjectOfType<UISystem>();
        OnAwake();
    }

    /// <summary>
    /// 자식 클래스용 Awake
    /// </summary>
    protected virtual void OnAwake()
    {
    }

    /// <summary>
    /// 엔진에 의한 업데이트
    /// </summary>
    private void Update()
    {
        _HasKeyInputFocus = true;
        if (_UISystem != null)
        {
            // 이미 키입력처리를 가로챈 UI가 있다면 씬이 포커스를 갖지 않음
            if (_UISystem.OnKeyInput())
            {
                _HasKeyInputFocus = false;
            }
        }
        OnUpdate();
    }

    /// <summary>
    /// 자식클래스용 Update
    /// </summary>
    protected virtual void OnUpdate()
    {
    }
}
