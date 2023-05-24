using UnityEngine;

public class GameArea : MonoBehaviour
{ 
    private float _decreasingSpeed = 0;
    private Vector3 _originScale;

    private void Start()
    {
        GameManager.Instance.OnStartGame.AddListener(ResetScale);
        _originScale = transform.localScale;
    }

    void Update()
    {
        if(_decreasingSpeed == 0 || transform.localScale.x < 5 || transform.localScale.z < 5)
        {
            return;
        }

        var scale = transform.localScale;
        scale.x -= _decreasingSpeed * Time.deltaTime;
        scale.z -= _decreasingSpeed * Time.deltaTime;
        transform.localScale = scale;
    }

    private void ResetScale(int level)
    {
        transform.localScale = _originScale;
        _decreasingSpeed = level < 2 ? 0 : 0.1f;

        Debug.Log("Reset Scale" + _decreasingSpeed);
    }
}
