using System;
using UnityEngine;

public static class CameraUtils
{
    public static Vector2 CalculateFrustumSize(float fieldOfView, float aspect, float distance)
    {
        var frustumHeight =
            2.0f * Mathf.Abs(distance) * (float)Math.Tan(fieldOfView * 0.5 * Math.PI / 180.0);
        var frustumWidth = frustumHeight * aspect;
        return new Vector2(frustumWidth, frustumHeight);
    }

    public static Vector2 CalculateOrthographicSize(float orthographicSize, float aspect) =>
        new(orthographicSize * 2.0f * aspect, orthographicSize * 2.0f);

    public static Vector2 CalculateFrustumSize(this Camera camera, float distance) =>
        CalculateFrustumSize(camera, distance);

    public static Vector2 CalculateOrthographicSize(this Camera camera) =>
        CalculateOrthographicSize(camera.orthographicSize, camera.aspect);

    public static Vector2 CalculateSize(this Camera camera, float distance) =>
        camera.orthographic
            ? CalculateOrthographicSize(camera)
            : CalculateFrustumSize(camera, distance);
}
