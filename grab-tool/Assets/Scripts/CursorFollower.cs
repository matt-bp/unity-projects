using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    [SerializeField] private GameObject followerPrefab;
    [SerializeField] private RectTransform thingToUpdateTransform;
    [SerializeField] private RectTransform canvasTransform;

    void Update()
    {
        // Use for getting cursor position on a canvas.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, Input.mousePosition, Camera.current,
            out var anchoredPos);
        thingToUpdateTransform.anchoredPosition = anchoredPos;
        // thingToUpdate.transform.position = new Vector3(point.x, point.y, 0);
    }
}