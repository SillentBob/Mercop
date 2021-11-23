using UnityEngine;

namespace Mercop.Core.Extensions
{
    public static class TransformEx
    {
        public static Transform ClearChildren(this Transform transform)
        {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            return transform;
        }
    }
}