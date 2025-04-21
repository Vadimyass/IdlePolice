using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Scripts.Tools
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CustomLineRenderer : MonoBehaviour
    {
        [SerializeField] private float lineWidth;
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh lineMesh;
        private List<Vector3> pathPoints = new List<Vector3>();
        [SerializeField] private GameObject _circle;

        private void Awake()
        {
            ClearMesh();
            HideCircle();
        }

        public void SetPoints(List<Vector3> points)
        {
            pathPoints = points;
            UpdateMesh(pathPoints);
        }

        private void UpdateMesh(List<Vector3> points)
        {
            lineMesh.Clear();

            if (points == null || points.Count < 2)
            {
                HideCircle();
                Debug.LogWarning("Недостаточно точек для создания линии.");
                return;
            }

            int vertexCount = points.Count * 2;
            Vector3[] vertices = new Vector3[vertexCount];
            List<int> triangles = new List<int>();
            Vector2[] uv = new Vector2[vertexCount];

            for (int i = 0; i < points.Count; i++)
            {
                Vector3 forward;

                // For the first point
                if (i == 0)
                {
                    forward = points[i + 1] - points[i];
                }
                // For the last point
                else if (i == points.Count - 1)
                {
                    forward = points[i] - points[i - 1];
                }
                // For all intermediate points
                else
                {
                    forward = (points[i + 1] - points[i - 1]).normalized; // Average direction
                }

                Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

                vertices[i * 2] = points[i] - right * lineWidth / 2;
                vertices[i * 2 + 1] = points[i] + right * lineWidth / 2;

                uv[i * 2] = new Vector2(0, i / (float)points.Count);
                uv[i * 2 + 1] = new Vector2(1, i / (float)points.Count);

                // Create triangles for connecting points
                if (i < points.Count - 1)
                {
                    int vert = i * 2;

                    triangles.Add(vert);
                    triangles.Add(vert + 2);
                    triangles.Add(vert + 1);

                    triangles.Add(vert + 1);
                    triangles.Add(vert + 2);
                    triangles.Add(vert + 3);
                }
            }

            lineMesh.vertices = vertices;
            lineMesh.triangles = triangles.ToArray();
            lineMesh.uv = uv;
            lineMesh.RecalculateNormals();

            DrawEndCircle(pathPoints[^1], pathPoints[^2]);
        }

        private void HideCircle()
        {
            _circle.SetActive(false);
        }

        public void ClearMesh()
        {
            lineMesh = new Mesh();
            _meshFilter.mesh = lineMesh;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void DrawEndCircle(Vector3 position, Vector3 prevPosition)
        {
            if (_circle == null)
            {
                Debug.LogError("Префаб круга не назначен.");
                return;
            }

            var direction = (position - prevPosition).normalized;

            // Calculate the center of the circle slightly beyond the end of the line
            var circleCenter = position + direction * (lineWidth / 2);

            _circle.SetActive(true);
            _circle.transform.position = new Vector3(circleCenter.x,_circle.transform.position.y,circleCenter.z);
            //_circle.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            //_circle.transform.localScale = new Vector3(lineWidth, lineWidth, 1); // Match circle size to line width
        }
    }
}