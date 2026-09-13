using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace HI5
{
    public class Hi5_Read_Hand_Thickness : MonoBehaviour
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private static string ReaderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/HI5" + "/Thickness.txt";
#elif UNITY_ANDROID
        private static string ReaderPath = "file://" + "/storage/emulated/0/Download/Thickness.txt";
#endif
        public float Thickness = 0.2f;
        bool isRead = false;
        void Awake()
        {
            isRead = false;
            LoadByWWW();
        }

        public void LoadByWWW()
        {
            StartCoroutine(doLoadByWWW());

        }

        IEnumerator doLoadByWWW()
        {
            string url = ReaderPath;
            Debug.Log("doLoadByWWW == url ==================   " + url);

            WWW w = new WWW(url);

            yield return w;

            if (w.isDone)
            {
                Debug.Log(w.text);
                string constent = w.text;

                if (float.TryParse(constent, out Thickness))
                {
                    isRead = true;
                    Debug.Log("file complete = " + Thickness);
                }
                else
                {
                    Debug.Log("float.TryParse  err ");
                }
                //text1.gameObject.SetActive(true);
                //text1.text = w.text;
            }
            else
            {
                Debug.Log("file err");
            }

        }



        // Update is called once per frame
        void Update()
        {
            if (HI5_Manager_Thread.Instance() != null && HI5_Manager_Thread.Instance().IsConected && HI5_Manager_Thread.Instance().IsStartDongle)
            {
                if(isRead)
                {
                    HI5_Manager_Thread.Instance().SetHandThickness(Thickness);
                    isRead = false;
                }
            }
                
        }
    }

}
