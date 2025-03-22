using System;
using ThGold.Pool;
using UnityEngine;

public class GizmoPoint : MonoBehaviour, PoolObject
{
    public Vector3 pointPosition; // 点的位置
    public float pointSize = 0.1f; // 点的大小
    public Color color = Color.red;
    private float timer;

    private GameObjectPool pool;

    public bool Inited;

    
    void OnDrawGizmos()
    {
        if (timer > 0.0f)
        {
            Gizmos.color = color; // 设置点的颜色
            Gizmos.DrawSphere(pointPosition, pointSize); // 绘制点
        }
    }

    void Update()
    {
        if (!Inited)
        {
            return;
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            Inited = false;
            if (pool != null)
            {
                pool.Return(this.gameObject);
            }
        }
    }

    public void DrawPoint(Vector3 pointPosition, float duration)
    {
        this.pointPosition = pointPosition;
        this.color = color;
        timer = duration;
    }

    public void InitPoolObject(GameObjectPool pool)
    {
        this.pool = pool;
    }
}