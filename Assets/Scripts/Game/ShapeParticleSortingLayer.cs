using UnityEngine;

// 파티클에 스프라이트 소팅 지정
public class ShapeParticleSortingLayer : MonoBehaviour
{
    [SerializeField]
    private string _sortingLayerName;
    [SerializeField]
    private int _sortingOrder;

    private void Start()
    {
        if (particleSystem != null)
        {
            particleSystem.renderer.sortingLayerName = _sortingLayerName;
            particleSystem.renderer.sortingOrder = _sortingOrder;
        }
    }
}
