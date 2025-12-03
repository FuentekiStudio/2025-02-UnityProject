using UnityEngine;
using UnityEngine.Pool;

public class ArrowPool : MonoBehaviour
{
    public ObjectPool<Arrow> arrowPool;
    [SerializeField] private Arrow _arrow;

    [SerializeField] private TrapArrow _trapArrowRef;
    [HideInInspector] public Vector2 spawnPos = Vector2.zero;

    private void Awake()
    {
        arrowPool = new ObjectPool<Arrow>(CreateBullet, OnGet, OnRelease, OnDestroyBullet, false, 10, 100);
    }

    private void Start()
    {
        spawnPos = _trapArrowRef.transform.position;
    }

    private void OnDestroyBullet(Arrow arrow)
    {
        Destroy(arrow);
    }

    private void OnRelease(Arrow arrow)
    {
        arrow.transform.localPosition = new Vector2(0.30f, 0f);
        arrow.gameObject.SetActive(false);
    }

    private void OnGet(Arrow bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private Arrow CreateBullet()
    {
        Arrow arrow = _arrow.GetComponent<Arrow>();
        arrow.gameObject.SetActive(false);
        arrow.poolRef = this;
        return Instantiate(arrow);
    }
}
