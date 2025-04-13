using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [Header("Position Parallax")]
    public float positionParallax = 1.0f;
    public Vector2 positionParallaxScale = Vector2.one;

    [Space]

    public float positionLerpSpeed = 10.0f;

    [Header("Rotation Parallax")]
    [Range(0.0f, 180.0f)] public float rotationParallax = 45.0f;
    public Vector2 rotationParallaxScale = Vector2.one;

    [Space]

    public float rotationLerpSpeed = 10.0f;

    void Start()
    {

    }

    void Update()
    {
        // Remap to [0.0, 1.0].
        // Mouse position across screen.

        Vector2 normalizedMousePosition = new(

            Mathf.Clamp01(Input.mousePosition.x / Screen.width),
            Mathf.Clamp01(Input.mousePosition.y / Screen.height));

        // Remap to [-1.0, 1.0].

        normalizedMousePosition -= Vector2.one * 0.5f;
        normalizedMousePosition *= 2.0f;

        // Position.

        Vector2 targetPosition = -normalizedMousePosition * (positionParallaxScale * positionParallax);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, positionLerpSpeed * Time.deltaTime);

        // Rotation.

        Quaternion targetRotation = Quaternion.Euler(

            -normalizedMousePosition.y * (rotationParallaxScale.y * rotationParallax),
            +normalizedMousePosition.x * (rotationParallaxScale.x * rotationParallax),

            0.0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
    }
}
