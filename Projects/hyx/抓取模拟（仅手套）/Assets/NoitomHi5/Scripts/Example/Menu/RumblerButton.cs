using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using HI5.VRCalibration;
using UnityEngine.UI;
public class RumblerButton :  Button_Base
{
    [Serializable]
    public class ButtonClickedAppendEvent : UnityEvent { }
    // Event delegates triggered on click.
    [FormerlySerializedAs("ButtonBaseonClick")]
    [SerializeField]
    private ButtonClickedAppendEvent m_OnAppendClick = new ButtonClickedAppendEvent();
    public ButtonClickedAppendEvent onOnAppendlick
    {
        get { return m_OnAppendClick; }
        set { m_OnAppendClick = value; }
    }

    public Image mImage;
    public Sprite ClickSprite;
    public Sprite NoClickSprite;
    public bool  isStrong;
    protected float rumblerCd;
    protected float clickTimer;
    bool isClick = true;
    public VibratePanelScript mVibratePanelScript;
    private void Awake()
    {
        SetCoolDownf(0.5f);
        isClick = true;
    }

    override protected void OnEnable()
    {
        m_CoolDownf = 1f;
        m_IsCollDown = false;
        m_IsEnter = false;
        SetCoolDownf(0.5f);
        mImage.sprite = NoClickSprite;
        m_IsCollDown = false;
        isClick = true;
    }

    protected override void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<HandInteractiveItem>())
        {
            m_IsEnter = false;
        }
    }
    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<HandInteractiveItem>())
        {
            if (!mVibratePanelScript.isEnableVibrator())
                return;

            if(!m_IsEnter)
            {
                if (!m_IsCollDown && isClick)
                {
                    HandleTriggerEnter();
                    clickTimer = GetCoolDownf();
                    rumblerCd = 1.0f;
                    m_IsCollDown = true;
                    isClick = false;
                    mImage.sprite = ClickSprite;
                    StartCoroutine("RumblerDown");

                }
                else if(m_IsCollDown && isClick)
                {
                    rumblerCd = 0.5f;
                    clickTimer = GetCoolDownf();
                    UISystemProfilerApi.AddMarker("ButtonBase.onClick", this);
                    m_OnAppendClick.Invoke();
                    isClick = false;
                }
                m_IsEnter = true;
            }
        }
    }

    protected override void CoolDownComplete()
    {
        StopCoroutine("RumblerDown");
        mImage.sprite = NoClickSprite;
        m_IsCollDown = false;
       isClick = true;
    }

    IEnumerator RumblerDown()
    {
        while (rumblerCd > 0)
        {
            rumblerCd -= Time.deltaTime;
            clickTimer -= Time.deltaTime;
            if (clickTimer <= 0.0f && !isClick)
                isClick = true;
            yield return null;
        }
        CoolDownComplete();
    }
};