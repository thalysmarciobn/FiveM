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
        Hair,
        Shoes
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
                CameraType.Hair, new CameraData
                {
                    Coords = new Vector3(0, 1.6f, 0.45f),
                    Points = new Vector3(0, 0, 0.25f)
                }
            },
            {
                CameraType.Shoes, new CameraData
                {
                    Coords = new Vector3(0, 1.0f, -0.5f),
                    Points = new Vector3(0, 0, -0.8f)
                }
            }
        };

        public static void DeleteCamera()
        {
            Camera?.Delete();
        }

        public static void SetCamera(CameraType cameraType, float fov, bool reverseCamera = false)
        {
            if (Camera != null && Camera.IsInterpolating) return;

            var data = Cameras[cameraType];
            var coords = data.Coords;
            var points = data.Points;

            var camCoords = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, coords.X, coords.Y, coords.Z);
            var camPoints = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, points.X, points.Y, points.Z);

            var tmpCamera = World.CreateCamera(camCoords, GameplayCamera.Rotation, fov);
            PointCamAtCoord(tmpCamera.Handle, camPoints.X, camPoints.Y, camPoints.Z);

            if (Camera != null)
            {
                SetCamActiveWithInterp(tmpCamera.Handle, Camera.Handle, 1000, 1, 1);

                Task.Factory.StartNew(async () =>
                {
                    await BaseScript.Delay(1500);
                    if (!Camera.IsInterpolating && tmpCamera.IsActive)
                    {
                        Camera.Delete();
                        Camera = tmpCamera;
                    }
                });
            }
            else
            {
                tmpCamera.IsActive = true;
                Camera = tmpCamera;
            }

            PlaySoundFrontend(-1, "Zoom_Out", "DLC_HEIST_PLANNING_BOARD_SOUNDS", true);
        }
    }
}
