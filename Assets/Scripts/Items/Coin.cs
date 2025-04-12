using System;
using DG.Tweening;
using Settings.Audio;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Items
{
    public class Coin : MonoBehaviour
    {
        private float _timer;

        private float _timeToDeath = 5f;

        [SerializeField] private GameObject fireEffect;

        public bool InFire;

        private int _rand;

        public bool IsDestroying;
        
        public YieldInstruction Pickup()
        {
            int rand = Random.Range(1, 2);

            if (rand == 1) AudioManager.instance.PlaySfx("CoinDrop1");
            else AudioManager.instance.PlaySfx("CoinDrop3");
            
            return DOTween.Sequence()
                .Append(transform.DOMove(transform.position + Vector3.up * 3, 0.25f))
                .Join(transform.DORotate(Vector3.up * 180, 0.25f))
                .Append(transform.DOScale(0, 0.25f).SetEase(Ease.InQuint))
                .Play()
                .WaitForCompletion();
        }

        private void Update()
        {
            if (IsDestroying)
            {
                return;
            }
            
            if (_timer <= _timeToDeath)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                DestroyCoin();
            }
        }

        public void DestroyCoin()
        {
            Destroy(gameObject);
        }

        public void ChooseTheCoinType()
        {
            _rand = Random.Range(0, 3);
            if (_rand == 0)
            {
                InFire = false;
            }
            else
            {
                InFire = true;
                fireEffect.SetActive(true);
            }
        }
    }
}
