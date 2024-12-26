using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public bool _scaleVerticaly => true;

    RectTransform Panel;
    Rect LastSafeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        Panel = GetComponent<RectTransform>();
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        Rect safeArea = GetSafeArea();

        if (safeArea != LastSafeArea)
            ApplySafeArea(safeArea);
    }

    Rect GetSafeArea()
    {
        return Screen.safeArea;
    }

    void ApplySafeArea(Rect r)
    {
        LastSafeArea = r;

        var anchorMin = r.position;
        var anchorMax = r.position + r.size;
        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;

        if (_scaleVerticaly)
        {
            anchorMin.y /= Screen.height;
            anchorMax.y /= Screen.height;
        }
        else
        {
            anchorMin.y = 0;
            anchorMax.y = 1;
        }

        Panel.anchorMin = anchorMin;
        Panel.anchorMax = anchorMax;

        Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
            name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
    }
}
