using CitizenFX.Core;
using CitizenFX.Core.Native;
using Mono.CSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public static class GameCamera
    {
        private static Camera Camera { get; set; }
        private class CameraData
        {
            public Vector3 Coords { get; set; }
            public Vector3 Points { get; set; }
        }

        private static Dictionary<CameraType, CameraData> Cameras = new Dictionary<CameraType, CameraData>
        {
            {
                CameraType.Entity, new CameraData
                {
                    Coords = new Vector3(0, 2f, 0.2f),
                    Points = new Vector3(0, 0, 0.05f)
                }
            },
            {
                CameraType.Face, new CameraData
                {
                    Coords = new Vector3(0, 0.9f, 0.65f),
                    Points = new Vector3(0, 0, 0.6f)
                }
            }
        };

        public static void SetCamera(CameraType cameraType, float fov, bool reverseCamera = false)
        {
            if (Camera != null && Camera.IsInterpolating)
                return;

            var data = Cameras[cameraType];

            var coords = data.Coords;
            var points = data.Points;

            if (GlobalVariables.S_Debug)
                Debug.WriteLine($"[GameCamera] SetCamera: {cameraType}");

            if (Camera != null)
            {
                var camCoords = GetOffsetFromEntityInWorldCoords(
                    Game.PlayerPed.Handle,
                    coords.X,
                    coords.Y,
                    coords.Z
                );

                var camPoints = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, points.X, points.Y, points.Z);

                var tmpCamera = new Camera(CreateCamWithParams(
                    "DEFAULT_SCRIPTED_CAMERA",
                    camCoords.X,
                    camCoords.Y,
                    camCoords.Z,
                    Camera.Rotation.X,
                    Camera.Rotation.Y,
                    Camera.Rotation.Z,
                    fov,
                    false,
                    0
                ));
                
                PointCamAtCoord(tmpCamera.Handle, camPoints.X, camPoints.Y, camPoints.Z);

                Camera.InterpTo(tmpCamera, 1000, 1, 1);

                Camera.Delete();

                Camera = tmpCamera;
            }
            else
            {
                var camCoords = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, coords.X, coords.Y, coords.Z);

                var camPoints = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, points.X, points.Y, points.Z);

                var cameraHandler = CreateCamWithParams(
                    "DEFAULT_SCRIPTED_CAMERA",
                    camCoords.X,
                    camCoords.Y,
                    camCoords.Z,
                    0,
                    0,
                    0,
                    fov,
                    false,
                    0
                );

                Camera = new Camera(cameraHandler);

                PointCamAtCoord(Camera.Handle, camPoints.X, camPoints.Y, camPoints.Z);
                SetCamActive(Camera.Handle, true);
            }
        }
    }

    public enum CameraType
    {
        Entity,
        Face
    }
}
