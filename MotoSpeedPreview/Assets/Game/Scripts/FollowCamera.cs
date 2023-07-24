using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class FollowCamera : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _zAxisOffset = - 10f;
    void Update()
    {
        _mainCamera.transform.position = this.gameObject.transform.position + new Vector3(0f,0f,_zAxisOffset);
    }
}
