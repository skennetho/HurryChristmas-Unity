using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _slipperyness = 0.95f;
    [SerializeField] private GameObject _playerCharacter;

    private Rigidbody _rigidbody;
    private Vector3 _input = Vector3.zero;
    private Vector3 _velocity;
    private GameManager _gameManager;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_playerCharacter == null)
        {
            Debug.LogError("No playerCharacter");
            return;
        }
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (_gameManager.IsPause)
        {
            return;
        }
        UpdateInputMovement();
        UpdateCharacterRotation();
    }

    private void UpdateInputMovement()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.z = Input.GetAxisRaw("Vertical");
        _velocity = _rigidbody.velocity;
        if (_input.sqrMagnitude == 0)
        {
            _rigidbody.velocity = _velocity * _slipperyness;
            return;
        }
        _input = _input.normalized;
        _velocity.x = _input.x * _speed;
        _velocity.z = _input.z * _speed;
        _rigidbody.velocity = _velocity;
    }

    private void UpdateCharacterRotation()
    {
        if (_input.sqrMagnitude == 0)
        {
            return;
        }
        var forward = _playerCharacter.transform.forward;
        forward.z += 0.001f;
        forward.x += 0.001f;
        var normalizedInput = _input.normalized;
        
        forward = Vector3.Lerp(forward, normalizedInput, 30.0f * Time.deltaTime);
        forward.y = 0;
        _playerCharacter.transform.forward = forward;
        //_playerCharacter.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_input), 10.0f * Time.deltaTime);
    }
}
