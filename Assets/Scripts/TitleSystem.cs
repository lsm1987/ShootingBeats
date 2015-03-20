using UnityEngine;

public class TitleSystem : MonoBehaviour
{
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        // 스테이지 시작
        bool start = false;
        if (Input.GetButtonDown("Start"))
        {
            start = true;
        }
        if (!start)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    start = true;
                }
            }
        }
        if (start)
        {
            Application.LoadLevel("Stage");
        }

        // 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
