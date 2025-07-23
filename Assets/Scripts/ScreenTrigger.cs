using UnityEngine;

/// <summary>
/// Trigger component for screen transitions.
/// Place at screen edges to detect when player should move to adjacent screens.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ScreenTrigger : MonoBehaviour
{
    [Header("Screen Transition")]
    public int targetScreenIndex = 0;
    
    [Header("Debug")]
    public bool showDebugMessages = false;
    
    void Start()
    {
        // Ensure the collider is set as trigger
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning($"ScreenTrigger on {gameObject.name}: Collider was not set as trigger. Fixed automatically.");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            if (showDebugMessages)
            {
                Debug.Log($"ScreenTrigger: Player entered trigger. Switching to screen {targetScreenIndex}");
            }
            
            // Switch to target screen via GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SwitchToScreen(targetScreenIndex);
            }
            else
            {
                Debug.LogError("ScreenTrigger: GameManager instance not found!");
            }
        }
    }
    
    void OnDrawGizmos()
    {
        // Visualize trigger area in editor
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider2D boxCol)
            {
                Gizmos.DrawCube(boxCol.offset, boxCol.size);
            }
            else if (col is CircleCollider2D circleCol)
            {
                Gizmos.DrawSphere(circleCol.offset, circleCol.radius);
            }
        }
    }
}