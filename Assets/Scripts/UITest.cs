using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake " + name + " " + gameObject.transform.GetSiblingIndex().ToString());
    }
	
    private void Start()
    {
        Debug.Log("Start " + name);
	}

    private void OnEnable()
    {
        Debug.Log("OnEnable " + name);
    }
}
