using UnityEngine;

namespace Utils
{
    public static class TransformUtils
    {
        public static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }
       
            obj.layer = newLayer;
       
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }

        public static Vector3 GetPointOnEdgeBounds(this Collider collider, Vector3 normalizeVector)
        {
            return collider.ClosestPointOnBounds(collider.transform.position + normalizeVector * 10) +
                   normalizeVector * 0.68f;
        }
    }
}