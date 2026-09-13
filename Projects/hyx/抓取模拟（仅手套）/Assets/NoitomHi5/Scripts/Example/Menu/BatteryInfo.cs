using UnityEngine;

namespace HI5.VRCalibration
{
    public class BatteryInfo : MonoBehaviour
    {
        [SerializeField] private Hand m_Type;
        [SerializeField] private SpriteRenderer m_SpritRenderer;
        [SerializeField] private Sprite m_Full;
        [SerializeField] private Sprite m_Good;
        [SerializeField] private Sprite m_Normal;
        [SerializeField] private Sprite m_Low;
        [SerializeField] private Sprite m_None;
        private PowerLevel m_CurrentLevel = PowerLevel.None;
        private HI5_GloveStatus m_HI5Status;

        private void OnEnable()
        {
            if(HI5_Manager_Thread.Instance() != null)
                m_HI5Status = HI5_Manager_Thread.Instance().GetGloveStatus();
        }

        private void Start()
        {
            SetPowerLevelSpriteRenderer(m_CurrentLevel);
        }

        private void Update()
        {
            //m_CurrentLevel = m_HI5Status.GetPowerLevel(m_Type);

            //if (m_HI5Status.IsGloveAvailable(m_Type))
            //    SetPowerLevelSpriteRenderer(m_CurrentLevel);
            //else
            //    SetPowerLevelSpriteRenderer(PowerLevel.Unknown);
        }

        public void Fresh(int percent)
        {
            int temp = percent / 25;
            if (temp <= 4)
            {
                if (temp == 0)
                {
                    if (percent == 0)
                    {
                        SetPowerLevelSpriteRenderer(PowerLevel.None);
                    }
                    else
                    {
                        SetPowerLevelSpriteRenderer(PowerLevel.Low);
                    }
                }
                else if(temp == 2)
                {
                    SetPowerLevelSpriteRenderer(PowerLevel.Normal);
                }
                else if (temp == 3)
                {
                    SetPowerLevelSpriteRenderer(PowerLevel.Good);
                }
                else if (temp == 4)
                {
                    SetPowerLevelSpriteRenderer(PowerLevel.Full);
                }
            }
        }



        private void SetPowerLevelSpriteRenderer(PowerLevel level)
        {
            switch (level)
            {
                case PowerLevel.None:
                    m_SpritRenderer.sprite = m_None;
                    break;
                case PowerLevel.Full:
                    m_SpritRenderer.sprite = m_Full;
                    break;
                case PowerLevel.Good:
                    m_SpritRenderer.sprite = m_Good;
                    break;
                case PowerLevel.Normal:
                    m_SpritRenderer.sprite = m_Normal;
                    break;
                case PowerLevel.Low:
                    m_SpritRenderer.sprite = m_Low;
                    break;
            }
        }
    }
}

