using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5_Inertia_Hand_Type : MonoBehaviour
    {
        bool isInitSendBind = false;
        public static HI5_Inertia_Hand_Type GetInstance()
        {
            return _instance;
        }
        static HI5_Inertia_Hand_Type _instance = null;
        private void Awake()
        {
            _instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isInitSendBind && HI5_Manager_Thread.Instance() != null && HI5_Manager_Thread.Instance().IsConected)
            {
                initBindOptic();
                HI5_Manager_Thread.Instance().LoadCalibrationData();
                isInitSendBind = true;
            }
        }
        private void OnDisable()
        {

        }

        //初始化发送光学绑定信息
        void initBindOptic()
        {
            HI5_BindInfoManager.LoadItems(true);
            HI5_Manager_Thread.Instance().SetBindTrackedObjectInfo(false);
        }
    }
}
