using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GameOverTrigger : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(GameManager.PlayeLayer))
        {
            GameManager.Instance.EndGameWithCompletionCheck(false);
        }
    }
}
