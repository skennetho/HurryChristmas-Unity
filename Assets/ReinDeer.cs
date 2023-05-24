using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ReinDeer : MonoBehaviour
{
    public UnityEvent<bool> OnCollidedSled;

    [SerializeField] private GameObject _prefab;
    private Collider _collider;
    private Rigidbody _rigidbody;

    public bool IsDestroying { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f);
    }

    public void Initialize(GameObject reindeerPrefab)
    {
        IsDestroying = false;

        _prefab = Instantiate(reindeerPrefab, transform.position, transform.rotation, transform);
        _collider = _prefab.GetComponentInChildren<Collider>();

        _rigidbody = _collider.gameObject.AddComponent<Rigidbody>();
        var colliderEnter = _collider.gameObject.AddComponent<ColliderEnter>();
        colliderEnter.OnCollisionEnterEvent.AddListener(OnCollided);
    }

    private void OnCollided(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameoverGround") && !IsDestroying)
        {
            _rigidbody.AddForce(Vector3.up * 100.0f);
            if (!IsDestroying)
            {
                StartCoroutine(BeginAndSetDestroyingCo());
                GameManager.Instance.SnowSled.OnLost();
            }
            return;
        }

        if (collision.gameObject.CompareTag("SnowSled") && !IsDestroying)
        {
            GameManager.Instance.SnowSled.OnRetrieve();
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator BeginAndSetDestroyingCo()
    {
        IsDestroying = true;
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    } 
}
