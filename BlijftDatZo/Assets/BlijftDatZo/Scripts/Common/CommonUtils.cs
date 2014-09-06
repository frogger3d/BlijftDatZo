using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class CommonUtils
{
    public static Vector2 RotateVector2(Vector2 vec, float radians)
    {
        float cosRadians = (float)Mathf.Cos(radians);
        float sinRadians = (float)Mathf.Sin(radians);

        return new Vector2(
            vec.x * cosRadians - vec.y * sinRadians,
            vec.x * sinRadians + vec.y * cosRadians);
    }

    /// <summary>
    /// Rotates a Vector3 like it was a Vector2 (ignores z)
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static Vector3 RotateVector3(Vector3 vec, float radians)
    {
        float cosRadians = (float)Mathf.Cos(radians);
        float sinRadians = (float)Mathf.Sin(radians);

        return new Vector3(
            vec.x * cosRadians - vec.y * sinRadians,
            vec.x * sinRadians + vec.y * cosRadians,
            vec.z);
    }

    public static Vector3 RotatePointAroundPivot(Vector2 point, Vector2 pivot, Quaternion rotation)
    {
        Vector2 dir = point - pivot;
        dir = rotation * dir; 
        point = dir + pivot;
        return point;
    }

    public static Vector2 GetRandomPositionInLevel(float levelWidth, float levelHeight)
    {
        return new Vector2(Random.Range(levelWidth * -0.5f, levelWidth * 0.5f),
            Random.Range(levelHeight * -0.5f, levelHeight * 0.5f));
    }

    public static float GetNormalDistributedRandom(float min, float max)
    {
        float range = (max - min) / 3f;
        return min +
            Random.value * range +
            Random.value * range +
            Random.value * range;
    }

    public static string ConvertTimeToString(float time, bool showCentiSeconds)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Mathf.Floor(time / 60).ToString("00"));
        builder.Append(":");
        if (!showCentiSeconds)
            //builder.Append(Math.Ceiling(dTime % 60).ToString("00"));
            builder.Append(Mathf.Floor(time % 60).ToString("00"));
        else
        {
            builder.Append(Mathf.Floor(time % 60).ToString("00"));
            builder.Append(".");
            builder.Append(Mathf.Floor((time * 100) % 100).ToString("00"));
        }
        return builder.ToString();
    }
}