﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// http://orbcreation.com/orbcreation/page.orb?1099 참고
/// </summary>
public static class UIUtil
{
    /*
     * (0, 0)이 좌하단임에 주의
     * 
     * - 아이폰X
     * Screen w:1125, h:2436
     * SafeArea x:0, y:102, w:1125, h:2202
     * - 안드로이드, Dobule Cutout, Full
     * Screen w:1080, h:1920
     * SafeArea x:0, y:84, w:1080, h:1752
     * - 안드로이드, Corner Cutout, Tall Cutout, Full
     * ScreenSize w:1080, h:1920
     * SafeArea x:0, y:0, w:1080, h:1794
     */

    public static Rect SafeArea
    {
        //get { return Screen.safeArea; }

        // 테스트용
        get
        {
            if (Screen.width == 1125 && Screen.height == 2436)
            {
                return new Rect(0f, 102f, 1125f, 2202f);
            }
            else
            {
                return Screen.safeArea;
            }
        }
    }

    public static void SetDefaultScale(RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void SetPivotAndAnchors(RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(RectTransform trans)
    {
        return trans.rect.size;
    }
    public static float GetWidth(RectTransform trans)
    {
        return trans.rect.width;
    }
    public static float GetHeight(RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetPositionOfPivot(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetLeftTopPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightBottomPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightTopPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetLeft(RectTransform trans, float newPos)
    {
        trans.localPosition = new Vector3(newPos + (trans.pivot.x * trans.rect.width), trans.localPosition.y, trans.localPosition.z);
    }
    public static void SetRight(RectTransform trans, float newPos)
    {
        trans.localPosition = new Vector3(newPos - ((1f - trans.pivot.x) * trans.rect.width), trans.localPosition.y, trans.localPosition.z);
    }

    public static void SetSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }
}
