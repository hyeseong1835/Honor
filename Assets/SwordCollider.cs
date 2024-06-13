using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public struct Line
    {
        public Vector3 P;
        public Vector3 l;
    }

    [SerializeField] SwordCollider otherColl;

    [SerializeField]
    [Range(2, 10)] 
    int gizmosLineCount = 4;
    [SerializeField]
    int showColliderCount = 1;
    List<Line> gizmos = new List<Line>();

    public float length = 1;

    public Line prevLine;

    void FixedUpdate()
    {

        IsCollide(otherColl);

        prevLine = cur;
        gizmos.Add(cur);
        if(gizmos.Count > showColliderCount) gizmos.RemoveAt(0);
    }
    void IsCollide(SwordCollider other)
    {
        Line curLine = LineSet(this);
        Line otherCurLine = LineSet(other);
        
    }
    bool ThirdRoot(float a, float b, float c, float d, out float r)
    {
        float p1 = 2 * b * b * b - 9 * a * b * c + 27 * a * a * d;
        float p2 = b * b - 3 * a * c;
        float q = Mathf.Sqrt(p1 * p1 - 4 * p2 * p2 * p2);
        float A = Mathf.Pow(0.5f * (p1 + q), 0.33333333f);
        float B = Mathf.Pow(0.5f * (p1 - q), 0.33333333f);

        float r1 = b / (-3 * a) - A / (3 * a) - B / (3 * a);
        if (A == B)
        {
            float r2 = b / (-3 * a) - (A + B) / (6 * a);
            float r3 = b / (-3 * a) - (A + B) / (6 * a);
        }

    }
    /*
    bool IsTransformElementCollide(
        float cur, float curOffset, float last, float lastOffset,
        float _cur, float _curOffset, float _last, float _lastOffset,
        out float hitPos
        ) 
    {
        float tDenominator = (last + lastOffset) - (_last + _lastOffset);
        float tNumerator = ((cur - last) + (curOffset - lastOffset)) - ((_cur - _last) + (_curOffset - _lastOffset));
        
        if(tNumerator > 0)
        {
            return (0 <= tDenominator && tDenominator <= tNumerator);
        }
        else
        {
            return (0 >= tDenominator && tDenominator >= tNumerator);
        }

        //((last + lastOffset) - (_last + _lastOffset))
        //( ((cur - last) + (curOffset - lastOffset) ) - ( (_cur - _last) + (_curOffset - _lastOffset)) )
        
        //t ((cur - last) + (curOffset - lastOffset)) + ((last - lastOffset)
    }
    */
    Line LineSet(SwordCollider coll)
    {
        Line line = new Line();
        line.P = coll.transform.position;
        line.l = coll.transform.TransformVector(Vector3.up * coll.length);

        return line;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int collIndex = 1; collIndex < gizmos.Count; collIndex++)
        {
            Line cur = gizmos[collIndex - 1];
            Line last = gizmos[collIndex];

            Gizmos.DrawLine(cur.P, cur.P + cur.l);
            Gizmos.DrawLine(last.P, last.P + last.l);

            for (float fillIndex = 0; fillIndex < gizmosLineCount; fillIndex++)
            {
                Gizmos.DrawLine(
                    Vector3.Lerp(cur.P, cur.P + cur.l, fillIndex / (gizmosLineCount - 1)),
                    Vector3.Lerp(last.P, last.P + last.l, fillIndex / (gizmosLineCount - 1))
                );
            }
        }
    }
}
