using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralEnemy : MonoBehaviour
{
    [Header("Spiral Settings")]
    [SerializeField] private float rotationSpeed = 60f;       // 降低旋转速度
    [SerializeField] private float expansionSpeed = 0.3f;     // 降低扩张速度
    [SerializeField] private float maxRadius = 2.5f;
    [SerializeField] private float minRadius = 0.8f;
    [SerializeField] private Transform[] smallEyes;

    private float currentRadius;
    private bool expanding = true;

    void Start()
    {
        currentRadius = minRadius;
    }

    void Update()
    {
        // 更平滑的半径变化
        float speedMultiplier = expanding ? 1f : 1.5f; // 收回时稍快
        currentRadius += (expanding ? 1 : -1) * expansionSpeed * speedMultiplier * Time.deltaTime;

        if (currentRadius >= maxRadius) expanding = false;
        if (currentRadius <= minRadius) expanding = true;

        // 更新小眼球位置
        float angleStep = 360f / smallEyes.Length;
        float currentAngle = Time.time * rotationSpeed;

        for (int i = 0; i < smallEyes.Length; i++)
        {
            float angle = currentAngle + i * angleStep;
            Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * currentRadius;
            smallEyes[i].localPosition = Vector3.Lerp(
                smallEyes[i].localPosition,
                offset,
                5f * Time.deltaTime); // 使用Lerp使移动更平滑
        }
    }
}
