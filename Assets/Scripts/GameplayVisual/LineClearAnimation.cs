using UnityEngine;

public class LineClearAnimation : MonoBehaviour, IPoolable
{
    public Animator Animator;
    private float _timer;


    private void Update()
    {
        if (_timer > 0.5f)
        {
            _timer = 0;
            PoolManager.Instance.PoolObject("lineClearAnim", this);
            this.gameObject.SetActive(false);
        }
        _timer += Time.deltaTime;
    }
}
