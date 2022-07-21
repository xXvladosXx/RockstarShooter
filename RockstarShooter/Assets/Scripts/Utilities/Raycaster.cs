using UnityEngine;

namespace Utilities
{
    public static class Raycaster
    {
        private static readonly Vector3 ScreenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        
        public static RaycastHit? GetRaycastOfMiddleScreen(Camera from, LayerMask layerMask)
        {
            var ray = from.ScreenPointToRay(ScreenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity,
                    layerMask))
            {
                return raycastHit;
            }

            return null;
        }
    }
}