using UnityEngine;

public static class LayerExtensions
{
    public static bool ContainsLayer(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}
