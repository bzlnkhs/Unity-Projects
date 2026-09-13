using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5.VRCalibration;
using UnityEngine.UI;
public class RumblerVButton : Button_Base
{
    public VibratePanelScript mVibratePanelScript;
    public int RumblerIndex = 1;
    public Image imageRumbler1;
    public Sprite RumblerSprite1_1;
    public Sprite RumblerSprite1_2;
    override protected void OnEnable()
    {
        m_CoolDownf = 1f;
        m_IsCollDown = false;
        m_IsEnter = false;
        imageRumbler1.sprite = RumblerSprite1_1;
    }
    protected override void OnTriggerEnter(Collider col)
    {
        //if(RumblerIndex == 1 && !mVibratePanelScript.isEnableClickRumbler1Button())
        //{
        //    return;
        //}
        //if (RumblerIndex == 2 && !mVibratePanelScript.isEnableClickRumbler2Button())
        //{
        //    return;
        //}
        if (col.gameObject.GetComponent<HandInteractiveItem>())
        {
            if (!m_IsCollDown && !m_IsEnter)
            {
                m_IsEnter = true;
                StartCoroutine(CoolDown());
                m_IsCollDown = true;
                HandleTriggerEnter();
            }
        }
    }
}
