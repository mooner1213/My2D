using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace My2DGame
{
    /// <summary>
    /// 충돌체 감지 클래스
    /// </summary>
    public class DetectionZone : MonoBehaviour
    {
        [SerializeField]

        // 감지된 콜라이더 리스트
        public List<Collider2D> detectedColliders = new List<Collider2D>();

        public bool IsDectected => detectedColliders.Count > 0;

        public UnityAction noRemainColider; // 충돌체가 감지되었다가 모두 없어졌을때

        private void OnTriggerEnter2D(Collider2D collision)
        {
            detectedColliders.Add(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            detectedColliders.Remove(collision);

            if (detectedColliders.Count == 0)
            {
                noRemainColider?.Invoke();
            }
        }
    }
}