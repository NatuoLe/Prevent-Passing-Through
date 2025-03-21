using UnityEngine;


public class PersistentGizmoLine : MonoBehaviour
{
    public bool Draw = true;
    public Vector3 start;
    public Vector3 end;
    public Color color = Color.red;
    public float duration = 1.0f; // 线持续的时间

    private float timer;

    void OnDrawGizmos()
    {
        if (timer > 0.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(start, end);
        }
    }

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        this.start = start;
        this.end = end;
        this.color = color;
        this.duration = duration;
        timer = duration;
    }
}