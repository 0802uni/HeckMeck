using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPanel : MonoBehaviour
{
    bool isDropped;
    [SerializeField]
    int distance;
    [SerializeField]
    float duration;

    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void DropDown()
    {
        Vector3 pos = rect.localPosition;

        if (!isDropped)
        {
            pos.y -= distance;
            isDropped = true;
        }
        else
        {
            pos.y += distance;
            isDropped = false;
        }

        rect.DOLocalMove(pos, duration);
    }
}
