using UnityEngine;

public class DroneController : MonoBehaviour
{
    private int forwardDirection = 0;
    private int backwardDirection = 0;
    private int leftDirection = 0;
    private int rightDirection = 0;
    private int upDirection = 0;
    private int downDirection = 0;
    private int rotateRightDirection = 0;
    private int rotateLeftDirection = 0;

    void Update()
    {
        #region Main Commands
        DroneActionMapping.Instance.CoreActionInputBindings[DroneAction.Connect]();
        DroneActionMapping.Instance.CoreActionInputBindings[DroneAction.InitializeSDK]();

        if (!DroneClient.Instance.SDKInitialized) return;

        DroneActionMapping.Instance.CoreActionInputBindings[DroneAction.TakeOff]();
        DroneActionMapping.Instance.CoreActionInputBindings[DroneAction.Landing]();
        DroneActionMapping.Instance.CoreActionInputBindings[DroneAction.Emergency]();
        #endregion

        #region Handle Movement
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.Forward](ref forwardDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.Backward](ref backwardDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.Left](ref leftDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.Right](ref rightDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.Up](ref upDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.Down](ref downDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.RotateRight](ref rotateRightDirection);
        DroneActionMapping.Instance.MovementInputBindings[DroneAction.RotateLeft](ref rotateLeftDirection);
        #endregion
    }
}
