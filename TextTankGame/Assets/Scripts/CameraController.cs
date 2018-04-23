using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

    [SerializeField] Transform m_target = null;
    [SerializeField] [Range(1.0f, 10.0f)] float m_response = 1.0f;
    [SerializeField] [Range(0.1f, 10.0f)] float m_shakeAmplitude = 1.0f;
    [SerializeField] [Range(0.1f, 50.0f)] float m_shakeRate = 1.0f;

    Vector3 m_position = Vector3.zero;
    Vector3 m_shake = Vector3.zero;
    float m_shakeAmount = 0.0f;

    private void Start()
    {
        m_position = transform.position;
    }

    private void Update()
    {
        m_shakeAmount = m_shakeAmount - Time.deltaTime;
        m_shakeAmount = Mathf.Clamp01(m_shakeAmount);

        float time = Time.time * m_shakeRate;
        m_shake.x = m_shakeAmount * m_shakeAmplitude * ((Mathf.PerlinNoise(time, 0.0f) * 2.0f) - 1);
        m_shake.y = m_shakeAmount * m_shakeAmplitude * ((Mathf.PerlinNoise(0.0f, time) * 2.0f) - 1);
    }

    void LateUpdate()
    {
        Vector3 targetPosition = m_target.position;
        targetPosition.z = transform.position.z;
        m_position = Vector3.Lerp(transform.position, targetPosition, m_response * Time.deltaTime);

        transform.position = m_position + m_shake;
    }

    public void Shake(float amount)
    {
        m_shakeAmount = m_shakeAmount + amount;
    }
}
