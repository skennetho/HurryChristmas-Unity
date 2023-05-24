using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderEnter : MonoBehaviour
{
    public UnityEvent<Collision> OnCollisionEnterEvent = new();

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent.Invoke(collision);
    }
}
