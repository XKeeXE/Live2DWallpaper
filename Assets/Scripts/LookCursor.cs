using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Framework.LookAt;
using UnityEngine.InputSystem;
using UnityEngine;

public class LookCursor : MonoBehaviour, ICubismLookTarget
{
    public Vector3 GetPosition()
    {   
        var targetPosition = Input.mousePosition;

        targetPosition = (Camera.main.ScreenToViewportPoint(targetPosition) * 2) - Vector3.one;

        return targetPosition;
    }

    public bool IsActive()
    {
        return true;
    }
}
