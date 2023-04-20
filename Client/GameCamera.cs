﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client
{

    public static class GameCamera
    {
        public enum CameraType
        {
            Entity,
            Face,
            Hair,
            Shoes
        }

        private class CameraData
        {
            public Vector3 Coords { get; set; }
            public Vector3 Points { get; set; }
        }

        private static readonly Dictionary<CameraType, CameraData> Cameras = new Dictionary<CameraType, CameraData>
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

        private static Camera Camera { get; set; }

        public static void DeleteCamera()
        {
            Camera?.Delete();
        }

        public static async void SetCamera(CameraType cameraType, float fov, bool reverseCamera = false)
        {
            if (Camera != null && Camera.IsInterpolating) return;

            var data = Cameras[cameraType];
            var coords = data.Coords;
            var points = data.Points;

            var camCoords = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, coords.X, coords.Y, coords.Z);
            var camPoints = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, points.X, points.Y, points.Z);

            var tmpCamera = World.CreateCamera(camCoords, GameplayCamera.Rotation, fov);
            PointCamAtCoord(tmpCamera.Handle, camPoints.X, camPoints.Y, camPoints.Z);

            tmpCamera.IsActive = true;

            if (Camera != null)
            {
                await Task.Factory.StartNew(async () =>
                {
                    SetCamActiveWithInterp(tmpCamera.Handle, Camera.Handle, 1000, 1, 1);
                    await BaseScript.Delay(10);
                }).ContinueWith((_) =>
                {
                    if (!Camera.IsInterpolating && tmpCamera.IsActive)
                    {
                        Camera.Delete();
                        Camera = tmpCamera;
                    }
                });
            }
            else
            {
                Camera = tmpCamera;
            }

            PlaySoundFrontend(-1, "Zoom_Out", "DLC_HEIST_PLANNING_BOARD_SOUNDS", true);
        }
    }
}