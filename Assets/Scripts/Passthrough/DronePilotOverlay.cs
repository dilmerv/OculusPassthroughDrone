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
            Logger.Instance.LogWarning("Drone Pilot is missing serialize fields");
            invalid = true;
        }

        if (!invalid)
        {
            ovrPassthroughLayer.AddSurfaceGeometry(gameObject, true);;
        }

    }

    private void OnDestroy()
    {
        if(ovrPassthroughLayer != null)
            ovrPassthroughLayer.RemoveSurfaceGeometry(gameObject);
    }
}