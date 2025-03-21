using UnityEngine;

public class GizmoPoint : MonoBehaviour
{
    public Vector3 pointPosition; // 点的位置
    public float pointSize = 0.1f; // 点的大小
    public Color color = Color.red;
    private float timer;

    void OnDrawGizmos()
    {
        if (timer > 0.0f)
        {
            Gizmos.color = Color.red; // 设置点的颜色
            Gizmos.DrawSphere(pointPosition, pointSize); // 绘制点
        }
    }

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void DrawPoint(Vector3 pointPosition, float duration)
    {
        this.pointPosition = pointPosition;
        this.color = color;
        timer = duration;
    }
}