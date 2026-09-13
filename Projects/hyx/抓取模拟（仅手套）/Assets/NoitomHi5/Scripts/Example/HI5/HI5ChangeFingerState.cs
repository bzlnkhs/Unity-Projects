using HI5;
using UnityEngine;
using HI5.VRCalibration;
using System.Collections;

public class HI5ChangeFingerState : MonoBehaviour
{
    private bool isCanChangeFingerState = false;
    private bool isEnter = false;
   // private bool isCanEnter = true;
    private Hi5_Thread_MonoBehaviour hi5_Thread_MonoBehaviour;
   // private SpriteRenderer spriteRenderer;
    private BoxCollider boxCollider;
    [SerializeField]
    private GameObject OnState, OffState;
    [SerializeField]
    private MenuStateMachine m_MenuSM;

    private void Awake()
    {
       // spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        hi5_Thread_MonoBehaviour=FindObjectOfType<Hi5_Thread_MonoBehaviour>();
        if (hi5_Thread_MonoBehaviour==null)
        {
            Debug.LogError("Hi5_Thread_MonoBehaviour is null , can not change finger state");
            this.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            isCanChangeFingerState=true;
            //start state 
            RefreshSprite();
            Interactive(true);
        }
    }

    private void OnEnable()
    {
        if (m_MenuSM != null)
            m_MenuSM.OnStateEnter += HandleStateEnter;
    }

    private void OnDisable()
    {
        if (m_MenuSM != null)
            m_MenuSM.OnStateEnter -= HandleStateEnter;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (isCanChangeFingerState&&!isEnter&&col.GetComponent<HandInteractiveItem>())
        {
           // Debug.Log("HI5ChangeFingerState HandleTriggerEnter1");
            isEnter = true;           
            ChangeFingerState();
            StartCoroutine(C_DelayTrigger());
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (isCanChangeFingerState&&isEnter && col.GetComponent<HandInteractiveItem>())
        {
            isEnter = false;
        }
    }

    private void ChangeFingerState()
    {
        hi5_Thread_MonoBehaviour.isEnableFingerFixed = !hi5_Thread_MonoBehaviour.isEnableFingerFixed;
        HI5_Device.EnableFingerAdbFixed(hi5_Thread_MonoBehaviour.isEnableFingerFixed);
        RefreshSprite();
    }

   
     private void RefreshSprite()
    {
        if (hi5_Thread_MonoBehaviour.isEnableFingerFixed)//固定 未开分指
        {
            OnState.SetActive(false);
            OffState.SetActive(true);
        }
        else//开分指
        {
            OnState.SetActive(true);
            OffState.SetActive(false);
            //spriteRenderer.sprite = OnState;
        }
    }
    private IEnumerator C_DelayVisible()
    {
        yield return new WaitForSeconds(5f);
        Interactive(true);
    }
    private IEnumerator C_DelayTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("HI5ChangeFingerState C_DelayTrigger");
        isEnter = true;
    
    }

    private void HandleStateEnter(MenuState state)
    {
        if (state == MenuState.Exit)
        {

            Interactive(true);
        }
        else
        {
          //  Interactive(false);
        }
    }

    private void Interactive(bool isCan)
    {
        boxCollider.enabled = isCan;
        //spriteRenderer.enabled = isCan;
    }
}
