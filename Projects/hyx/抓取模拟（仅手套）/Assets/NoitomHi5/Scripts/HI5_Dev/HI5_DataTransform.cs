using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HI5
{
    /// <summary>
    /// Transform data among Unity, Hi5 and HTC VIVE.
    /// </summary>
    public static class HI5_DataTransform
    {
        /*
        public static Vector3 Vector3ToUnityPosition(Vector3 pos)
        {
            return new Vector3(pos.x, pos.y, - pos.z);
        }

        public static Vector3 EulerAnglesToUnityEulerAngles(Vector3 euler)
        {
            return new Vector3(- euler.x, euler.y, -euler.z);
        }

        public static Vector3 TransformToUnityPosition(float[] pos)
        {
            float vx = pos[0];
            float vy = pos[1];
            float vz = -pos[2];
            Vector3 tv = new Vector3(vx, vy, vz);
            return tv;
        }

        public static Vector3 ToUnityEuler(Vector3 euler)
        {
            Quaternion rot = Quaternion.Euler(euler);
            float[] rot_ = new float [] { rot.w, rot.x, rot.y, rot.z };

            Quaternion tranformedRot = TransformToUnityRotation(rot_);
            Vector3 eulerangles = tranformedRot.eulerAngles;
            return eulerangles;
        }

        public static Quaternion TransformToUnityRotation(float[] rot)
        {
            float qw = -rot[0];
            float qx = rot[1];
            float qy = rot[2];
            float qz = -rot[3];
            Quaternion tq = new Quaternion(qx, qy, qz, qw);
            return tq;
        }
        */

        /// <summary>
        /// Transform received HI5 position data to Unity position data.
        /// </summary>
        /// <param name="pos">
        /// Received Hi5 position data by <see cref="UnityEngine.Vector3"/>.
        /// </param>
        /// <returns>
        /// Position data in Vector3.
        /// </returns>
        public static Vector3 ToUnityPosition(Vector3 pos)
        {
            float x = pos.x * 0.01f;
            float y = pos.y * 0.01f;
            float z = -pos.z * 0.01f;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Transform received HI5 rotation data in euler angles to Unity euler angles.
        /// </summary>
        /// <param name="eulerAngles">
        /// Received Hi5 euler angles by <see cref="UnityEngine.Vector3"/>.
        /// </param>
        /// <returns>
        /// Euler angles in Vector3.
        /// </returns>
        public static Vector3 ToUnityEulerAngles(Vector3 eulerAngles)
        {
            float x = eulerAngles.x;
            float y = eulerAngles.y + 180;
            if (y > 360) { y = y - 360; }
            float z = eulerAngles.z;
            return new Vector3(x, y, z);
        }

        /*
        public static void Push(bool isLeft, Vector3 pos, Quaternion rot)
        {
            float[] fpos = new float[3];
            fpos = ToVIVEPosition(pos);
            float[] frot = new float[4];
            frot = ToVIVERotation(rot);

            HI5_Device.PushTrackerData(isLeft, fpos, frot);
        }
        */

        /// <summary>
        /// Push received optical devices data into Hi5 data stream.
        /// </summary>
        /// <param name="serialNumber">
        /// The serial number of the device. Input by <see cref="System.String"/>.
        /// </param>
        /// <param name="deviceType">
        /// The type of <see cref="HI5.OPTDeviceType"/>.
        /// </param>
        /// <param name="pos">
        /// The position data of the device by <see cref="UnityEngine.Vector3"/>.
        /// </param>
        /// <param name="rot">
        /// The rotation data of the device by <see cref="UnityEngine.Quaternion"/>.
        /// </param>
        /// 
        //傳輸光學數據
        public static void PushOpticalData(string serialNumber, OPTDeviceType deviceType, Vector3 pos, Quaternion rot)
        {
            float[] fpos = new float[3];
            float[] frot = new float[4];
            //四元数传

            OpticalSensorType optType = OpticalSensorType.OST_Unknown;
            if (deviceType == OPTDeviceType.HTC_VIVE_Controller)
            {
                optType = OpticalSensorType.OST_HTC_VIVE_Controller;
                fpos = ToVIVEPosition(pos);
                frot = ToVIVERotation(rot);
            }
            else if (deviceType == OPTDeviceType.HTC_VIVE_Tracker)
            {
                optType = OpticalSensorType.OST_HTC_VIVE_Tracker;
                fpos = ToVIVEPosition(pos);
                frot = ToVIVERotation(rot);
            }
            else if (deviceType == OPTDeviceType.Pico_Neo3_Controller)
            {
                optType = OpticalSensorType.OST_Alice_Rigid_Body;
                fpos = ToPicoNeo3Position(pos);
                frot = ToPicoNeo3Rotation(rot);
            }
            if(HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddOpticData(serialNumber, optType, fpos, frot);      
        }
        private static float[] ToVIVEPosition(Vector3 pos)
        {
            float[] fpos = new float[3];
            fpos[0] = pos.x;
            fpos[1] = pos.y;
            fpos[2] = -pos.z;

            return fpos;
        }

        private static float[] ToVIVERotation(Quaternion rot)
        {
            float[] frot = new float[4];
            frot[0] = -rot.w;
            frot[1] = rot.x;
            frot[2] = rot.y;
            frot[3] = -rot.z;

            return frot;
        }

        private static float[] ToPicoNeo3Position(Vector3 pos)
        {
            float[] fpos = new float[3];
            fpos[0] = pos.x;
            fpos[1] = pos.y;
            fpos[2] = -pos.z;

            return fpos;
        }

        private static float[] ToPicoNeo3Rotation(Quaternion rot)
        {
            float[] frot = new float[4];
            frot[0] = -rot.w;
            frot[1] = rot.x;
            frot[2] = rot.y;
            frot[3] = -rot.z;

            return frot;
        }
    }
}