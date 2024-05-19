using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DrawingPanel : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float minDistanceToAddPoint = 10f;
    [SerializeField] UnityEvent<List<Vector3>> onFinishDrawing;
    [Header("Components")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform canvasRectTransform;

    private Camera _camera;
    private Vector3 lastPosition;
    private bool isDrawing;
    private float cachedWidthMultiplier;

    List<Vector3> worldPositions = new();

    private void Awake()
    {
        _camera = Camera.main;
        cachedWidthMultiplier = lineRenderer.widthMultiplier;
    }

    private void Update()
    {
        if (isDrawing)
        {
            lineRenderer.widthMultiplier += Time.deltaTime / 60f;
        }
        else if (lineRenderer.widthMultiplier > 0f)
        {
            lineRenderer.widthMultiplier -= Time.deltaTime / 2f;

            if (lineRenderer.widthMultiplier < 0)
                lineRenderer.widthMultiplier = 0f;
        }
    }

    private void StartDrawing(Vector3 position)
    {
        lastPosition = position;

        lineRenderer.widthMultiplier = cachedWidthMultiplier;
        lineRenderer.positionCount = 0;
        lineRenderer.gameObject.SetActive(true);

        worldPositions = new();

        isDrawing = true;
    }

    private void EndDrawing()
    {
        isDrawing = false;
        onFinishDrawing?.Invoke(worldPositions);
        // lineRenderer.gameObject.SetActive(false);
    }

    private void AddPoint(Vector3 currentPosition)
    {
        lineRenderer.positionCount++;

        int newPositionIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(newPositionIndex, currentPosition);

        lastPosition = currentPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 screenPosition = eventData.pointerCurrentRaycast.screenPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, _camera, out Vector2 localPoint);
       
        StartDrawing(screenPosition);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!isDrawing)
            return;

        Vector3 currentPosition = eventData.pointerCurrentRaycast.screenPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, currentPosition, _camera, out Vector2 localPoint);

        float distance = Vector3.Distance(lastPosition, currentPosition);

        Vector2 squadPosition = localPoint / rectTransform.sizeDelta;

        squadPosition.x /= rectTransform.sizeDelta.y / 100;
        squadPosition.y /= rectTransform.sizeDelta.x / 100;

        if (distance > minDistanceToAddPoint
            &&
            RectTransformUtility.RectangleContainsScreenPoint(rectTransform, currentPosition, _camera)
        )
        {
            AddPoint(localPoint);

            worldPositions.Add(squadPosition);

            lastPosition = currentPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EndDrawing();
    }
}
