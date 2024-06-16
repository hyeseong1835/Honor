using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

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

        if(gizmos.Count > showColliderCount) gizmos.RemoveAt(0);
    }
    void IsCollide(SwordCollider other)
    {
        Line curLine = LineSet(this);
        Line otherCurLine = LineSet(other);

        prevLine = curLine;
        gizmos.Add(curLine);
    }
    bool ThirdRoot(float a, float b, float c, float d, out float[] r)
    {
        const float oneThird = 1 / 3f;
        const float root3 = 1.73205080757f;

        /// 2b^3 + 9abc + 27a^2d
        float A_Pow3_Plus_B_Pow3 = 2 * b * b * b - 9 * a * b * c + 27 * a * a * d;
        
        float D = b * b - 3 * a * c;

        if (D <= 0) return Result_X1(out r);

        float Fa_k = A_Pow3_Plus_B_Pow3 - 2 * Mathf.Pow(D, 1.5f);
        float Fb_k = Fa_k + 3 * A_Pow3_Plus_B_Pow3 - 54 * a * a * d;

        if (Fa_k == 0 || Fb_k == 0) // F(a) * F(b) : 0
        {
            return Result_X1_X4(out r);
        }
        else if (Fa_k > 0 && Fb_k > 0 || Fa_k < 0 && Fb_k < 0) //F(a) * F(b) : +
        {
            return Result_X1(out r);
        }
        else //F(a) * F(b) : -
        {
            return Result_X1_X2_X3(out r);
        }

        bool Result_X1(out float[] r)
        {
            float A, B;
            GetRealNumber_A_B(out A, out B);

            return Result(out r, X1());

            float X1() => (A + B + b) / (-3 * a);
        }

        bool Result_X1_X4(out float[] r)
        {
            float A, B;
            GetRealNumber_A_B(out A, out B);

            return Result(out r, X1(), X4());

            float X1() => (A + B + b) / -3 * a;
            float X4() => (Mathf.Pow(-8 * A_Pow3_Plus_B_Pow3 + 12 * (A + B), oneThird) - b) / (6 * a);
        }
        bool Result_X1_X2_X3(out float[] r)
        {
            float Q1 = default;//AB(A + B + root3(A - B)i)
            float Q2 = default;//AB(A + B - root3(A - B)i)

            return Result(out r, X1(), X2(), X3());

            float X1() => default;
            float X2() => (Mathf.Pow(-8 * A_Pow3_Plus_B_Pow3 + 12 * Q1, oneThird) - b) / (6 * a);
            float X3() => (Mathf.Pow(-8 * A_Pow3_Plus_B_Pow3 + 12 * Q2, oneThird) - b) / (6 * a);
        }
        void GetRealNumber_A_B(out float A, out float B)
        {
            float Q1 = Mathf.Pow(0.5f * A_Pow3_Plus_B_Pow3, oneThird);
            float Q2 = A_Pow3_Plus_B_Pow3 * A_Pow3_Plus_B_Pow3 - 4 * D * D * D;

            //A = 3Root[1/2(Q1 + 2Root[Q2])]
            A = Mathf.Pow(0.5f * Q1 + 2 * Mathf.Sqrt(Q2), oneThird);

            //B = 3Root[1/2(Q1 - 2Root[Q2])]
            B = Mathf.Pow(0.5f * Q1 - 2 * Mathf.Sqrt(Q2), oneThird);
        }
        bool Result(out float[] r, params float[] values)
        {
            List<float> result = new List<float>();

            foreach (float v in values)
            {
                if (0 <= v && v <= 1) result.Add(v);
            }
            r = result.ToArray();
            return r.Length > 0;
        }
    }
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
