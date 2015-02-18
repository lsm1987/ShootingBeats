using UnityEngine;

// 외양 장식용. 항상 돌기
public class ShapeRotate : MonoBehaviour
{
    public float rotSpeed = 0.0f; // 1회 업데이트에 회전할 각도. 단위 도
    
    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, rotSpeed, Space.Self);
	}
}
