using System;
using DG.Tweening;
using UnityEngine;

namespace Items
{
    public class Coin : MonoBehaviour
    {
        private float _timer;

        private float _timeToDeath = 5f;
        
        public YieldInstruction Pickup()
        {
            return DOTween.Sequence()
                .Append(transform.DOMove(transform.position + Vector3.up * 3, 0.5f))
                .Join(transform.DORotate(Vector3.up * 180, 0.5f))
                .Append(transform.DOScale(0, 0.25f).SetEase(Ease.InQuint))
                .Play()
                .WaitForCompletion();
        }

        private void Update()
        {
            if (_timer <= _timeToDeath)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
