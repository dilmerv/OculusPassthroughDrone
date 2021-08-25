using UnityEngine;
public class DronePilotOverlay : MonoBehaviour
{
    [SerializeField]
    private OVRPassthroughLayer ovrPassthroughLayer;

    private bool invalid = false;

    private void Awake()
    {
        if (ovrPassthroughLayer == null)
        {
            Logger.Instance.LogError("Drone Pilot is missing serialize fields");
            invalid = true;
        }

        if (invalid) return;

        ovrPassthroughLayer.AddSurfaceGeometry(gameObject, true);
    }

    private void OnDestroy()
    {
        ovrPassthroughLayer.RemoveSurfaceGeometry(gameObject);
    }
}