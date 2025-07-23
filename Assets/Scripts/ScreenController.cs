using UnityEngine;
using System.Collections.Generic;

public class ScreenController : MonoBehaviour
{
    public static ScreenController Instance { get; private set; }

    [Header("Camera Settings")]
    public Camera mainCamera;
    public float cameraMoveSpeed = 2f;

    [Header("Player Settings")]
    public Transform player;

    [Header("Screen Boundaries")]
    public float rightBoundary = 8f;
    public float leftBoundary = -8f;

    [Header("Screen Management")]
    public List<Transform> screenTransforms = new List<Transform>();

    private int currentScreenIndex = 0;
    private Vector3 targetCameraPosition;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScreenController();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTransitioning)
        {
            MoveCameraToTarget();
        }
        CheckPlayerBoundaries();
    }

    public void MoveToScreen(int screenIndex)
    {
        if (screenIndex < 0 || screenIndex >= screenTransforms.Count)
        {
            Debug.LogWarning($"ScreenController: Invalid screen index {screenIndex}.");
            return;
        }
        if (screenTransforms[screenIndex] == null)
        {
            Debug.LogWarning($"ScreenController: Screen transform at index {screenIndex} is null");
            return;
        }
        currentScreenIndex = screenIndex;
        Vector3 screenCenter = screenTransforms[screenIndex].position;
        targetCameraPosition = new Vector3(screenCenter.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        isTransitioning = true;
    }

    private void InitializeScreenController()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (screenTransforms.Count > 0 && screenTransforms[0] != null)
        {
            targetCameraPosition = new Vector3(screenTransforms[0].position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        else
        {
            targetCameraPosition = mainCamera.transform.position;
        }
    }

    private void MoveCameraToTarget()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, cameraMoveSpeed * Time.deltaTime);
        if (Vector3.Distance(mainCamera.transform.position, targetCameraPosition) < 0.1f)
        {
            mainCamera.transform.position = targetCameraPosition;
            isTransitioning = false;
        }
    }

    private void CheckPlayerBoundaries()
    {
        if (player == null || isTransitioning) return;
        Vector3 currentScreenCenter = screenTransforms[currentScreenIndex].position;
        float relativePlayerX = player.position.x - currentScreenCenter.x;
        if (relativePlayerX > rightBoundary)
        {
            int nextScreenIndex = currentScreenIndex + 1;
            if (nextScreenIndex < screenTransforms.Count) MoveToScreen(nextScreenIndex);
        }
        else if (relativePlayerX < leftBoundary)
        {
            int previousScreenIndex = currentScreenIndex - 1;
            if (previousScreenIndex >= 0) MoveToScreen(previousScreenIndex);
        }
    }
}