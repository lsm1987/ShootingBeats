using UnityEngine;

public class TitleSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiOverlay;
    [SerializeField]
    private GameObject _uiOptionWindow;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        // 스테이지 시작
        if (Input.GetButtonDown("Start"))
        {
            OnStartClicked();
        }

        // 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitClicked();
        }
    }

    public void OnStartClicked()
    {
        Application.LoadLevel("Stage");
    }

    public void OnOptionClicked()
    {
        _uiOverlay.SetActive(true);
        _uiOptionWindow.SetActive(true);
        /*
        Object prefabOptionWindow = Resources.Load("UI/OptionWindow");
        GameObject objOptionWindow = Instantiate(prefabOptionWindow) as GameObject;
        objOptionWindow.transform.SetParent(_uiOverlay.transform);
        objOptionWindow.GetComponent<RectTransform>().localScale = Vector3.one;
        */
    }

    public void OnOptionCloseClicked()
    {
        _uiOptionWindow.SetActive(false);
        _uiOverlay.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
