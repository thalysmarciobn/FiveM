using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client.Helper
{
    public static class CameraHelper
    {
        public static void SetCamera(Vector3 coord, Vector3 point)
        {
            var cam = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
            SetCamCoord(cam, coord.X, coord.Y, coord.Z);
            PointCamAtCoord(cam, point.X, point.Y, point.Z);
            SetCamActive(cam, true);
        }
    }
}