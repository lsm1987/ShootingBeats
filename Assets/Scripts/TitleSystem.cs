using UnityEngine;

public class TitleSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiRoot; // UI 루트 오브젝트

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
        Application.LoadLevel("BeatList");
    }

    public void OnOptionClicked()
    {
        Object prefabUIOption = Resources.Load("UI/UIOption");
        GameObject objUIOption = Instantiate(prefabUIOption) as GameObject;
        objUIOption.transform.SetParent(_uiRoot.transform, false);
        objUIOption.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
