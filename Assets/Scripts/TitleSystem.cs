using UnityEngine;

public class TitleSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            Application.LoadLevel("Stage");
        }
    }
}
