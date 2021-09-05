using UnityEngine;

namespace ClassicTetris.GameplayVisual
{
    public class CellBlock : MonoBehaviour, IPoolable
    {
        public SpriteRenderer SpriteRenderer;
        private void OnDisable()
        {
            PoolManager.Instance.PoolObject("cellBlocks", this);
        }
    }
}