using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using HI5.VRCalibration;
public class Button_Base : MonoBehaviour
{
    //public SpriteRenderer enableSprit;
    //public SpriteRenderer unEnableSprit;
    protected float m_CoolDownf = 1f;
    protected bool m_IsCollDown = false;
    protected bool m_IsEnter = false;
    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("ButtonBaseonClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    virtual protected void OnEnable()
    {
        m_CoolDownf = 1f;
        m_IsCollDown = false;
        m_IsEnter = false;
    }

    protected void OnDisable()
    {
        m_CoolDownf = 1f;
        m_IsCollDown = false;
        m_IsEnter = false;
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
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

    protected virtual void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<HandInteractiveItem>())
        {
            m_IsEnter = false;          
        }
    }
    protected IEnumerator CoolDown()
    {
        float timer = m_CoolDownf;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        CoolDownComplete();
        m_IsCollDown = false;
    }
    protected virtual void HandleTriggerEnter()
    {
        UISystemProfilerApi.AddMarker("ButtonBase.onClick", this);
        m_OnClick.Invoke();
    }

    protected virtual void CoolDownComplete()
    {

    }

    protected  void SetCoolDownf(float coolDown)
    {
        m_CoolDownf = coolDown;
    }
    protected float GetCoolDownf()
    {
       return  m_CoolDownf;
    }
}
