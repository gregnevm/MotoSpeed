using System.Collections.Generic;
using UnityEngine;

public class LevelGameplayController : MonoBehaviour
{
    [SerializeField] private List<CollisionPair> _endGameCollisionPairs;
    [SerializeField] private SpriteRenderer _endGameSprite;
    [SerializeField] private AudioSource _endGameSound;
    [SerializeField] private float _endGameEffectDuration;
    [Range(0, 1f)]
    [SerializeField] private float _endGameSlowMotion = 0.3f;

    public static bool IsMovementBlocked { get; private set; }

    private float _originalTimeScale;
    private Color _originalSpriteColor;
    private bool _isGameEnded = false;

    [System.Serializable]
    public class CollisionPair
    {
        public Collider2D collider1;
        public Collider2D collider2;
    }

    private void Awake()
    {
        _originalTimeScale = Time.timeScale;
        _originalSpriteColor = _endGameSprite.color;
    }

    private void Update()
    {
        if (!_isGameEnded)
        {
            CheckCollisions();
        }
    }

    private void CheckCollisions()
    {
        foreach (var collisionPair in _endGameCollisionPairs)
        {
            if (AreColliding(collisionPair))
            {
                EndGame();
                break; 
            }
        }
    }

    private bool AreColliding(CollisionPair collisionPair)
    {
        return collisionPair.collider1.IsTouching(collisionPair.collider2);
    }

    private void EndGame()
    {
        _isGameEnded = true; 
        StartCoroutine(EndGameEffect());
    }

    private System.Collections.IEnumerator EndGameEffect()
    {
        IsMovementBlocked = true;
        _endGameSound.Play();
        Time.timeScale = _endGameSlowMotion;

        float elapsedTime = 0f;
        while (elapsedTime < (_endGameEffectDuration/3) * _endGameSlowMotion)
        {
            float normalizedTime = elapsedTime / _endGameEffectDuration;
            Color newColor = new Color(_originalSpriteColor.r, _originalSpriteColor.g, _originalSpriteColor.b, Mathf.Lerp(1f, 0f, normalizedTime));
            _endGameSprite.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }        

        yield return new WaitForSeconds(_endGameSound.clip.length/5);

        ResetEffects();

        RestartLevel();
    }

    private void ResetEffects()
    {
        Time.timeScale = _originalTimeScale;
        _endGameSprite.color = _originalSpriteColor;
        _isGameEnded = false; 
        IsMovementBlocked = false;
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
