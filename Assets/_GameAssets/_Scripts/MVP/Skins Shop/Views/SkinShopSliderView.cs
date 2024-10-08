﻿using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TapAndRun.MVP.Skins_Shop.Views
{
    public class SkinShopSliderView : MonoBehaviour
    {
        public int CurrentSkinIndex { get; set; }
        public int SkinsCount { get; set; }

        public List<GameObject> SkinsList { get; set; }

        [field:SerializeField] public Transform SliderContent { get; private set; }

        [SerializeField] private float _xPosOffsetStep = 1.5f;
        [SerializeField] private float _scaleOffset = 0.5f;

        [SerializeField] private float _scrollDuration = 0.5f;

        public void Place(Vector3 cameraPosition, float cameraRotation)
        {
            transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0f);
            transform.eulerAngles = new Vector3(0f, 0f, cameraRotation);
        }

        public void Align()
        {
            for (var i = 0; i < SkinsCount; i++)
            {
                var skin = SkinsList[i].transform;
                var alignedPosition = new Vector3(i * _xPosOffsetStep, 0f, 0f);

                skin.transform.localPosition = alignedPosition;
                RescaleSkin(i, 0 ,destroyCancellationToken).Forget();
            }
        }

        public async UniTask ScrollContentAsync(CancellationToken token)
        {
            var sliderOffset = CurrentSkinIndex * _xPosOffsetStep * -1;

            for (var i = 0; i < SkinsCount; i++)
            {
                RescaleSkin(i, CurrentSkinIndex ,token).Forget();
            }

            await SliderContent.DOLocalMoveX(sliderOffset, _scrollDuration)
                .SetEase(Ease.InOutCubic)
                .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);
        }

        public async UniTaskVoid RescaleSkin(int skinIndex, int selectedIndex, CancellationToken token)
        {
            var distanceFromCenter = Mathf.Abs(selectedIndex - skinIndex);
            var scale =  Mathf.Clamp01(1 - (distanceFromCenter * _scaleOffset));
            var finalScale = new Vector3(scale,scale, 1f);
            var skin = SkinsList[skinIndex].transform;

            if (scale == 0)
            {
                if (skin.localScale.x != 0)
                {
                    await skin.DOScale(finalScale, _scrollDuration)
                        .SetEase(Ease.InOutCubic)
                        .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token);

                    skin.gameObject.SetActive(false);
                }
            }
            else
            {
                if (skin.localScale.x == 0)
                {
                    skin.gameObject.SetActive(true);
                }

                skin.DOScale(finalScale, _scrollDuration)
                    .SetEase(Ease.InOutCubic)
                    .AwaitForComplete(TweenCancelBehaviour.CompleteAndCancelAwait, token).Forget();
            }
        }
    }
}