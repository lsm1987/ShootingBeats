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
        if (GetComponent<ParticleSystem>() != null)
        {
            GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = _sortingLayerName;
            GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = _sortingOrder;
        }
    }
}
