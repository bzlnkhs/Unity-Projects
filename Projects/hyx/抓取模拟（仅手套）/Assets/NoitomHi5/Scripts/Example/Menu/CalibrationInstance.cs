using System.Collections;
using UnityEngine;

namespace HI5.VRCalibration
{
    public class CalibrationInstance : MonoBehaviour
    {
        [SerializeField] private HI5_Pose m_Pose;
        [SerializeField] private CountDownUI m_CountDownUI;
        [SerializeField] private GameObject m_Progress;
        [SerializeField] private CalibrationStateMachine m_CalibrationSM;
        private int percentCalibration = 0;
        private void OnEnable()
        {
            m_CountDownUI.OnCountDownComplete += HandleCountDownComplete;
            HI5_Calibration.OnCalibrationComplete += HandleCalibrationComplete;
            ResetProgress();
        }

        private void OnDisable()
        {
            m_CountDownUI.OnCountDownComplete -= HandleCountDownComplete;
            HI5_Calibration.OnCalibrationComplete -= HandleCalibrationComplete;
            ResetProgress();
        }
        bool isRequestVpos = false;
        float Cd = 0.3f;
        private void HandleCountDownComplete()
        {
            //             if (m_Pose == HI5_Pose.BPose)
            //             {
            //                 HI5_Calibration.ResetCalibration();
            //                 HI5_Manager.GetGloveStatus().StartCalibrationBpos();
            //                 
            //                 //删除文件
            //                 // System.IO.File.Delete(HI5_Calibration.DefaultPathAndName);
            //             }
            //Debug.Log("Bpos");
            if (m_Pose == HI5_Pose.VPose)
            {
                bool IsAndroid = false;
#if UNITY_ANDROID
    IsAndroid = true;
#endif
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
                IsAndroid = false;
#endif
                if (IsAndroid)
                {
#if UNITY_ANDROID
        HI5_Hand_Bind.GetInstance().BindTrackedObjectInfo();
#endif
                }
                else
                {
#if (UNITY_STANDALONE_WIN)
                    if (HI5_Manager_Thread.Instance() != null)
                    {
                        HI5_Manager_Thread.Instance().m_leftTrackedDevice = null;
                        HI5_Manager_Thread.Instance().m_rightTrackedDevice = null;
                    }
                    // Inertia 路线不使用 HI5_Calculate_Hand_Type
#elif (UNITY_ANDROID)
    //#if (UNITY_EDITOR)        
    //                //if (HI5_Manager_Thread.Instance() != null)
    //                //{
    //                //     HI5_Manager_Thread.Instance().m_leftTrackedDevice = null;
    //                //     HI5_Manager_Thread.Instance().m_rightTrackedDevice = null;
    //                //}
    //                //if (HI5_Calculate_Hand_Type.GetInstance() != null)
    //                //     HI5_Calculate_Hand_Type.GetInstance().CalibrationRequesetBindTracked();    
    //#endif
#endif
                }
                isRequestVpos = true;
            }
            else
            {
                HI5_Calibration.StartCalibration(m_Pose);
                StartCoroutine(UpdateCalibrationProgress());
                StartCoroutine("UpdateCd");
            }
        }
        public void Update()
        {
            if (isRequestVpos)
            {
                Cd -= Time.deltaTime;
                if (Cd < 0.0f)
                {
                    isRequestVpos = false;
                    percentCalibration = 0;
                    HI5_Calibration.StartCalibration(m_Pose);
                    StartCoroutine(UpdateCalibrationProgress());
                    StartCoroutine("UpdateCd");
                }
            }
        }
        IEnumerator UpdateCd()
        {
            yield return new WaitForSeconds(10);
            if(percentCalibration == 0)
            {
                if(gameObject.GetComponent<StartButton>() != null && gameObject.GetComponent<StartButton>()._HomeButton != null) 
                {
                    gameObject.GetComponent<StartButton>()._HomeButton.EnableButton(true);
                }
            }
            StopCoroutine("UpdateCd");
        }

        IEnumerator UpdateCalibrationProgress()
        {
            //gameObject.GetComponent<SwitchCalibrationButtonState>().EnableButton(true);
            int calibrationProgress = 0;
            while (calibrationProgress < 100)
            {				
                calibrationProgress = HI5_Calibration.GetCalibrationProgress(m_Pose);
                percentCalibration = calibrationProgress;
                float percent = (float)calibrationProgress / 100;                
                Vector3 scale = m_Progress.transform.localScale;
                m_Progress.transform.localScale = new Vector3(percent, scale.y, scale.z);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            if (m_Pose == HI5_Pose.VPose)
            {
                
            }
            else if (m_Pose == HI5_Pose.BPose)
            {
               
            }
            else if (m_Pose == HI5_Pose.PPose)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    HI5_Manager_Thread.Instance().SaveCalibrationData();
                }               
            }
            if (HI5_Calibration.OnCalibrationComplete != null)
                HI5_Calibration.OnCalibrationComplete(m_Pose);

        }

        private void ResetProgress()
        {
            Vector3 scale = m_Progress.transform.localScale;
            m_Progress.transform.localScale = new Vector3(0f, scale.y, scale.z);
        }
        private void HandleCalibrationComplete(HI5_Pose pose)
        {
            if (pose == HI5_Pose.VPose)
            {
                m_CalibrationSM.State = CalibrationState.BPose;
            }
            else if (pose == HI5_Pose.BPose)
            {
                //HI5_GloveStatus.
                m_CalibrationSM.State = CalibrationState.PPose;
            }
            else if (pose == HI5_Pose.PPose)
            {
                m_CalibrationSM.State = CalibrationState.Finish;
            }

        }
        public bool GetIsVPose()
        {
            return m_Pose == HI5_Pose.VPose;
        }
    }
}

