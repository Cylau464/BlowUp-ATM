using UnityEngine;

namespace physic
{
    public enum NormalAxis { Vertical, Horizontal }

    public class CrossRay
    {
        public bool isReached { get; private set; }
        public Vector3 point { get; private set; }

        public float offset { get; private set; }
        public NormalAxis normalAxis { get; private set; }

        private Camera _camera;
        private Plane _plane;

        public CrossRay(Camera camera, NormalAxis normalAxis = NormalAxis.Horizontal , float offset = 0.0f)
        {
            if (camera == null) throw new System.ArgumentNullException();

            _camera = camera;
            this.offset = offset;
            this.normalAxis = normalAxis;

            switch (this.normalAxis)
            {
                case NormalAxis.Vertical:
                    _plane = new Plane(Vector3.forward, -Vector3.forward * this.offset);
                    break;
                case NormalAxis.Horizontal:
                    _plane = new Plane(Vector3.up, Vector3.up * this.offset);
                    break;
            }
        }

        /// <summary>
        /// Get cross point on plane.
        /// </summary>
        /// <param name="camera"> The camera that will give ray. </param>
        /// <returns></returns>
        public void ThrowRaycast()
        {
            ThrowRaycast(Input.mousePosition);
        }

        public void ThrowRaycast(Vector3 castPosition)
        {
            isReached = false;

            Ray ray = _camera.ScreenPointToRay(castPosition);
            if (_plane.Raycast(ray, out float enter))
            {
                isReached = true;
                point = ray.GetPoint(enter);
            }
        }

        public void OnDrawGizmosSelected()
        {
            switch (normalAxis)
            {
                case NormalAxis.Vertical:
                    Gizmos.color = Color.yellow;
                    Vector3 center = -Vector3.forward * offset;
                    Gizmos.DrawLine(center + new Vector3(20, 20, 0), center + new Vector3(20, -20, 0));
                    Gizmos.DrawLine(center + new Vector3(20, -20, 0), center + new Vector3(-20, -20, 0));
                    Gizmos.DrawLine(center + new Vector3(-20, -20, 0), center + new Vector3(-20, 20, 0));
                    Gizmos.DrawLine(center + new Vector3(-20, 20, 0), center + new Vector3(20, 20, 0));
                    break;
                case NormalAxis.Horizontal:
                    Gizmos.color = Color.yellow;
                    center = Vector3.up * offset;
                    Gizmos.DrawLine(center + new Vector3(20, 0, 20), center + new Vector3(20, 0, -20));
                    Gizmos.DrawLine(center + new Vector3(20, 0, -20), center + new Vector3(-20, 0, -20));
                    Gizmos.DrawLine(center + new Vector3(-20, 0, -20), center + new Vector3(-20, 0, 20));
                    Gizmos.DrawLine(center + new Vector3(-20, 0, 20), center + new Vector3(20, 0, 20));
                    break;
            }
        }
    }
}
