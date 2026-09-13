using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HI5.VRCalibration
{
    public class MagneticInfo : MonoBehaviour
    {
        [SerializeField]
        private Hand m_Type;
        [SerializeField]
        private SpriteRenderer m_SpritRenderer;
        [SerializeField]
        private Sprite m_Bad;
        [SerializeField]
        private Sprite m_Fair;
        [SerializeField]
        private Sprite m_Good;
        [SerializeField]
        private Sprite m_None;

        private MagneticStatus currentState = MagneticStatus.None;
        private HI5_GloveStatus m_HI5Status;

        private void OnEnable()
        {
            if(HI5_Manager_Thread.Instance() != null)
                m_HI5Status = HI5_Manager_Thread.Instance().GetGloveStatus();
        }

        private void OnDisable()
        {
        }

        private void Start()
        {
            SetMagneticStateSpriteRenderer(currentState);
        }

        private void Update()
        {

        }

        public void Fresh(int Value)
        {
           if (Value >= 75)
           {
                SetMagneticStateSpriteRenderer(MagneticStatus.Good);
            }
           else if (Value >= 50)
           {
                SetMagneticStateSpriteRenderer(MagneticStatus.Fair);
            }
            else if (Value >= 25)
            {
                SetMagneticStateSpriteRenderer(MagneticStatus.Bad);
            }
             else
             {
                SetMagneticStateSpriteRenderer(MagneticStatus.None);
             }
        }

        private void SetMagneticStateSpriteRenderer(MagneticStatus state)
        {
            switch (state)
            {
                case MagneticStatus.Unknown:
                    m_SpritRenderer.sprite = null;
                    break;
                case MagneticStatus.Good:
                    m_SpritRenderer.sprite = m_Good;
                    break;
                case MagneticStatus.Fair:
                    m_SpritRenderer.sprite = m_Fair;
                    break;
                case MagneticStatus.Bad:
                    m_SpritRenderer.sprite = m_Bad;
                    break;
                case MagneticStatus.None:
                    m_SpritRenderer.sprite = m_None;
                    break;
            }
        }
    }
}

