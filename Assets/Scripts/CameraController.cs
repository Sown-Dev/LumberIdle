using System.Collections;
using UnityEngine;

/// <summary>
/// Manages camera transitions between screens.
/// Camera remains static per screen and smoothly transitions when switching screens.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Transition Settings")]
    public float transitionDuration = 1f;
    
    private Camera cam;
    private bool isTransitioning = false;
    
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    
    /// <summary>
    /// Smoothly moves the camera to a new screen position.
    /// </summary>
    /// <param name="screenCenterPosition">The target screen center position</param>
    public void MoveToScreen(Vector2 screenCenterPosition)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScreen(screenCenterPosition));
        }
    }
    
    private IEnumerator TransitionToScreen(Vector2 targetPosition)
    {
        isTransitioning = true;
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetPosition.x, targetPosition.y, startPosition.z);
        
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            
            // Smooth transition using Lerp
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            
            yield return null;
        }
        
        // Ensure final position is exact
        transform.position = endPosition;
        isTransitioning = false;
    }
    
    /// <summary>
    /// Instantly sets the camera position without transition.
    /// </summary>
    /// <param name="screenCenterPosition">The target screen center position</param>
    public void SetScreenPosition(Vector2 screenCenterPosition)
    {
        if (isTransitioning)
        {            StopAllCoroutines();
            isTransitioning = false;
        }
        
        transform.position = new Vector3(screenCenterPosition.x, screenCenterPosition.y, transform.position.z);
    }
}