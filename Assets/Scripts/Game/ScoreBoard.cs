using UnityEngine;
using System.Collections.Generic;

// 게임 내 점수판
namespace Game
{
    public class ScoreBoard : MonoBehaviour
    {
        public GameObject _prefabDigit = null;
        public Sprite[] _sprites = new Sprite[10]; // 0 ~ 9 까지의 숫자 스프라이트

        // 자릿수별 스프라이트 렌더러
        // [0]: 1의 자리
        private List<SpriteRenderer> _digits = new List<SpriteRenderer>();

        public void SetScore(int score)
        {
            int digitCount = 1; // 자릿수
            int cur10 = 1;
            int next10 = 10;
            do
            {
                // 이번 자릿수의 숫자
                int num = (score % next10) / cur10;
                
                if (_digits.Count < digitCount)
                {
                    // 생성된 오브젝트 수가 이번 자릿수보다 적다면 새로 생성하여 끝에 추가
                    _digits.Add(CreateDigit());
                }

                // 숫자 할당
                _digits[digitCount - 1].sprite = _sprites[num];

                // 처리된 자리 비움
                score -= num * cur10;

                if (score > 0)
                {
                    // 다음 처리용 변수 갱신
                    ++digitCount;
                    cur10 *= 10;
                    next10 *= 10;
                }
                else
                {
                    // 더이상 처리할 자리가 없으면 루프 종료
                    break;
                }
            } while (true);

            // 오브젝트 정렬
            const float spriteWidth = 0.1f;
            float totalWidth = spriteWidth * digitCount;    // 모든 자리 너비 합
            float rightMostX = totalWidth / 2.0f - spriteWidth / 2.0f;  // 스프라이트 중앙이 기준점
            for (int i = 0; i < _digits.Count; ++i)
            {
                if (i < digitCount)
                {
                    // 유효한 자리
                    _digits[i].gameObject.SetActive(true);
                    _digits[i].transform.localPosition = new Vector3(rightMostX - spriteWidth * i, 0.0f, 0.0f); // 오른쪽에서 왼쪽으로 채워나감
                }
                else
                {
                    // 무효한 자리
                    _digits[i].gameObject.SetActive(false);
                }
            }
        }

        // 자식 오브젝트로 자릿수 오브젝트 생성
        private SpriteRenderer CreateDigit()
        {
            GameObject obj = GameObject.Instantiate(_prefabDigit) as GameObject;
            obj.transform.parent = transform;
            return obj.GetComponent<SpriteRenderer>();
        }
    }
}