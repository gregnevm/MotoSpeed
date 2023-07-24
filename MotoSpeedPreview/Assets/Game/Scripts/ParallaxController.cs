using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ParallaxController : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundSettings
    {
        public Transform backgroundTransform;
        public float speedMultiplier;
    }

    [SerializeField] private List<BackgroundSettings> _backgrounds;
    [SerializeField] private float _speedStepByBackgroundsRange;

    private Transform _mainCameraTransform;
    private Vector3 _startPositionOfMainCamera;

    private void Awake()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        _startPositionOfMainCamera = _mainCameraTransform.position;
        _backgrounds = _backgrounds.OrderByDescending(bg => bg.backgroundTransform.GetComponent<SpriteRenderer>().sortingOrder).ToList();

    }

    private void FixedUpdate()
    {
        ParallaxEffect();
    }

    private void ParallaxEffect()
    {
        Vector3 deltaPosition = _mainCameraTransform.position - _startPositionOfMainCamera;
        float deltaX = deltaPosition.x;

        for (int i = 0; i < _backgrounds.Count; i++)
        {
            float speedMultiplier =  _speedStepByBackgroundsRange/ (i + 2f);
            float offset = deltaX * speedMultiplier * Time.deltaTime;

            Transform backgroundTransform = _backgrounds[i].backgroundTransform;
            Vector3 backgroundPosition = backgroundTransform.position;
            backgroundPosition.x += offset;

            backgroundTransform.position = backgroundPosition;
        }

        _startPositionOfMainCamera = _mainCameraTransform.position;
    }
   
}
