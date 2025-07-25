using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private float shakeDuration = 0f;
    private float shakeStrength = 0f;
    private Transform shakeCenter;

    private Vector3 initialOffset;
    private Vector3 smoothVelocity;

    private float elapsedTime = 0f;
    private bool isShaking = false;
    private bool isReturning = false;

    [SerializeField] private float noiseSpeed = 15f;
    [SerializeField] private float dampingSpeed = 5f;

    public void StartShake(float strength, float duration, Transform center)
    {
        shakeStrength = strength;
        shakeDuration = duration;
        shakeCenter = center;

        if (shakeCenter == null)
        {
            Debug.LogError("CameraShaker: 揺れの中心が指定されていません");
            return;
        }

        initialOffset = transform.position - shakeCenter.position;
        elapsedTime = 0f;
        isShaking = true;
        isReturning = false;
    }

    void Update()
    {
        if (isShaking && shakeCenter != null)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= shakeDuration)
            {
                isShaking = false;
                isReturning = true;
                return;
            }

            float decay = 1f - Mathf.Pow(elapsedTime / shakeDuration, 2);

            float noiseX = Mathf.PerlinNoise(0, Time.time * noiseSpeed) * 2 - 1;
            float noiseY = Mathf.PerlinNoise(1, Time.time * noiseSpeed) * 2 - 1;
            float noiseZ = Mathf.PerlinNoise(2, Time.time * noiseSpeed) * 2 - 1;

            Vector3 shakeOffset = new Vector3(noiseX, noiseY, noiseZ) * shakeStrength * decay;
            Vector3 basePosition = shakeCenter.position + initialOffset;

            Vector3 targetPosition = basePosition + shakeOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 1f / dampingSpeed);

            Debug.Log("シェイク");
        }

        else if (isReturning && shakeCenter != null)
        {
            Vector3 targetPosition = shakeCenter.position + initialOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.2f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isReturning = false;
            }
        }
    }
}
