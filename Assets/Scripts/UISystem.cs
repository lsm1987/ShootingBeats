using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 씬의 UI들을 관리
/// </summary>
public class UISystem : MonoBehaviour
{
    // 활성화된 윈도우 목록
    // 상위의 윈도우일수록 앞으로 정렬
    private List<UIWindow> _windows = new List<UIWindow>();

    /// <summary>
    /// 새 윈도우가 활성화 되었을 때
    /// </summary>
    /// <param name="window"></param>
    public void OnWindowEnabled(UIWindow window)
    {
        if (window != null)
        {
            // 새 윈도우 생성으로 SiblingIndex가 바뀌었을 수 있으므로 전체 재정렬
            _windows.Add(window);
            _windows.Sort(UIWindow.CompareBySiblingIndexReverse);
        }
    }

    public void OnWindowDisabled(UIWindow window)
    {
        if (window != null)
        {
            _windows.Remove(window);
            _windows.Sort(UIWindow.CompareBySiblingIndexReverse);
        }
    }

    /// <summary>
    /// 지정한 윈도우 열기
    /// </summary>
    /// <param name="prefabPath">윈도우 프리팹 리소스 경로</param>
    /// <returns>생성된 윈도우</returns>
    public UIWindow OpenWindow(string prefabPath)
    {
        Object prefabWindow = Resources.Load(prefabPath);
        GameObject objWindow = Instantiate(prefabWindow) as GameObject;
        objWindow.name = prefabWindow.name; // 오브젝트명의 (Clone) 삭제
        UIWindow window = objWindow.GetComponent<UIWindow>();
        window._trans.SetParent(transform, false);
        window._rectTrans.localScale = Vector3.one;
        return window;
    }

    private UIWindow GetTopWindow()
    {
        return (_windows.Count != 0) ? _windows[0] : null;
    }

    /// <summary>
    /// UI들에게서 키입력 처리를 시도한다.
    /// </summary>
    /// <returns>true: UI 내에서 키입력 처리가 완료되어 추가 처리하지 않음</returns>
    public bool OnKeyInput()
    {
        foreach (UIWindow window in _windows)
        {
            if (window.OnKeyInput())
            {
                return true; // 키입력 전달하지 않는 UI 발생
            }
        }
        return false;
    }
}