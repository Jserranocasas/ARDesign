namespace UnityEngine
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Representation of a plane in 3D space to Virtual Room.
    /// </summary>
    public class Plane3D
    {
        /// <summary>
        /// A points any of the plane
        /// </summary>
        Vector3 p;

        /// <summary>
        /// Plane normal
        /// </summary>
        Vector3 normal;

        /// <summary>
        /// Manages detected planes in the scene.
        /// </summary>
        /// <param name="p">A points any of the plane.</param>
        /// <param name="normal">Plane normal.</param>
        public Plane3D(Vector3 _p, Vector3 _n)
        {
            p = _p;
            normal = _n;
        }

        /// <summary>
        ///  Return value of A in AX+BY+CZ+D = 0.
        /// </summary>
        public float GetA()
        {
            return normal.x;
        }

        /// <summary>
        ///  Return value of B in AX+BY+CZ+D = 0.
        /// </summary>
        public float GetB()
        {
            return normal.y;
        }

        /// <summary>
        ///  Return value of C in AX+BY+CZ+D = 0.
        /// </summary>
        public float GetC()
        {
            return normal.z;
        }

        /// <summary>
        ///  Return value of D in AX+BY+CZ+D = 0.
        /// </summary>
        public float GetD()
        {
            return (-1) * (GetA() * p.x + GetB() * p.y + GetC() * p.z);
        }

        /// <summary>
        ///  Return normal formed by (A, B, C) of the equation Ax+By+Cz+D = 0
        /// </summary>
        public Vector3 GetNormal()
        {
            return normal;
        }

        /// <summary>
        ///  Calculate the distance between plane and the given point
        /// </summary>
        /// <param name="p">Point to calculate distance</param>
        /// <returns>Distance between plane and the given point.</returns>
        public float Distance(Vector3 p)
        {
            float denominator = Mathf.Abs(GetA() * p.x + GetB() * p.y + GetC() * p.z + GetD());
            float numerator = Mathf.Sqrt(GetA() * GetA() + GetB() * GetB() + GetC() * GetC());
            return denominator / numerator;
        }

        /// <summary>
        ///  Check if a point belongs to a plane
        /// </summary>
        /// <param name="p">Point to check</param>
        /// <returns><c>true</c>, if point belongs to a plane, <c>false</c> otherwise.</returns>
        public bool Coplanar(Vector3 p)
        {
            return (Mathf.Abs(Distance(p)) < 0.00001);
        }

        /// <summary>
        ///  Calculate determinant of a matrix2x2.
        /// </summary>
        /// <param name="a">Parameter 1x1</param>
        /// <param name="b">Parameter 1x2</param>
        /// <param name="c">Parameter 2x1</param>
        /// <param name="d">Parameter 2x2</param>
        /// <returns>Determinant of a matrix2x2.</returns>
        private float Determinant2x2(float a, float b, float c, float d)
        {
            return (a * c - b * d);
        }

        /// <summary>
        ///  Calculate determinant of a matrix3x3.
        /// </summary>
        /// <param name="a">Parameter 1x1</param>
        /// <param name="b">Parameter 1x2</param>
        /// <param name="c">Parameter 1x3</param>
        /// <param name="d">Parameter 2x1</param>
        /// <param name="e">Parameter 2x2</param>
        /// <param name="f">Parameter 2x3</param>
        /// <param name="g">Parameter 3x1</param>
        /// <param name="h">Parameter 3x2</param>
        /// <param name="i">Parameter 3x3</param>
        /// <returns>Determinant of a matrix3x3.</returns>
        public float Determinant3x3(float a, float b, float c,
                                    float d, float e, float f,
                                    float g, float h, float i)
        {
            return (a * e * i + g * b * f + c * d * i - c * e * g - i * d * b - a * h * f);
        }


        /// <summary>
        ///  Check if there is a intersection between this plane and p. l contains the result
        /// </summary>
        /// <param name="pl2">Plane to check</param>
        /// <param name="pl3">Intersection line</param>
        /// <returns><c>true</c>, if there is intersection, <c>false</c> otherwise.</returns>
        public static bool IntersectPlanes(Plane3D pl1, Plane3D pl2, Plane3D pl3, ref Vector3 intersectionPoint)
        {
            float d1 = pl1.GetD(); float d2 = pl2.GetD(); float d3 = pl3.GetD();

            Vector3 p0 = -d1 * (Vector3.Cross(pl2.GetNormal(), pl3.GetNormal()))
                        -d2 * (Vector3.Cross(pl3.GetNormal(), pl1.GetNormal()))
                        -d3 * (Vector3.Cross(pl1.GetNormal(), pl2.GetNormal()));

            float denominator = Vector3.Dot(pl1.GetNormal(), (Vector3.Cross(pl2.GetNormal(), pl3.GetNormal())));

            intersectionPoint = p0 / denominator;
            
            return true;
        }
    }
}