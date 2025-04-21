using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.MainScreen
{
    public class MainScreenWidgetBlock : WidgetBlock
    {
        public MainScreenWidgetBlockType widgetBlockType;

        private List<Sequence> _blowTweens = new ();

        public void StartBlowing(bool isOnce)
        {
            _objects.ForEach(x =>
            {
                var countLoops = isOnce ? 1 : 2;
                var sequence = DOTween.Sequence();
                sequence.Append(x.DOFade(1, 1));
                sequence.Append(x.DOFade(0.15f, 1));
                sequence.SetLoops(countLoops).SetEase(Ease.Linear);
                sequence.OnKill(() =>
                {
                    x.alpha = 1;
                    x.blocksRaycasts = true;
                    StopBlowing();
                });
                _blowTweens.Add(sequence);
            });
            
        }

        private void StopBlowing()
        {
            _blowTweens.ForEach(x => x.Kill());
            _blowTweens.Clear();
        }

        public void Hide()
        {
            _objects.ForEach(x =>
            {
                x.alpha = 0;
            });
        }
    }
}