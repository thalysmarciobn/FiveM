using CitizenFX.Core;
using CitizenFX.Core.Native;
using Mono.CSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public enum CameraType
    {
        Entity,
        Face,
        Features
    }
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
            },
            {
                CameraType.Features, new CameraData
                {
                    Coords = new Vector3(0, 1.6f, 0.45f),
                    Points = new Vector3(0, 0, 0.25f)
                }
            }
        };

        public static void DeleteCamera()
        {
            if (Camera != null)
                Camera.Delete();
        }

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

                var tmpCamera = World.CreateCamera(camCoords, Camera.Rotation, fov);

                PointCamAtCoord(tmpCamera.Handle, camPoints.X, camPoints.Y, camPoints.Z);

                SetCamActiveWithInterp(tmpCamera.Handle, Camera.Handle, 1000, 1, 1);

                Task.Factory.StartNew(async () =>
                {
                    await BaseScript.Delay(1000);
                    if (!Camera.IsInterpolating && tmpCamera.IsActive)
                    {
                        Camera.Delete();

                        Camera = tmpCamera;
                    }
                });
            }
            else
            {
                var camCoords = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, coords.X, coords.Y, coords.Z);

                var camPoints = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, points.X, points.Y, points.Z);

                Camera = World.CreateCamera(camCoords, GameplayCamera.Rotation, fov);

                PointCamAtCoord(Camera.Handle, camPoints.X, camPoints.Y, camPoints.Z);

                Camera.IsActive = true;

            }
            PlaySoundFrontend(-1, "Zoom_Out", "DLC_HEIST_PLANNING_BOARD_SOUNDS", true);
        }
    }
}
