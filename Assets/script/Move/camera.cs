using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target;   // 따라갈 캐릭터
    public float smoothSpeed = 5f;
    public Vector3 offset;     // 카메라 위치 보정

    void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치
        Vector3 desiredPosition = new Vector3(
    target.position.x,
    target.position.y,
    -10f
);

        // 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

    }
}