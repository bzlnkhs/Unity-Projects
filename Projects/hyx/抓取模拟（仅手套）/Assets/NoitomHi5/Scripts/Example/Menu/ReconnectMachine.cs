using HI5;
using UnityEngine;
using HI5.VRCalibration;
using System.Collections;

public class ReconnectMachine : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer CountDownSpriteRender;
    [SerializeField]
    private Sprite[] CountDownSpites;
    private int currentCountDownIndex=0;
    private bool isStartLoading = false;
    [SerializeField]
    private GameObject LoadingBar;
    private float timer = 0f;
    private float loadingTime = 8f;


    [SerializeField]
    private MenuStateMachine m_MenuSM;

    private void OnEnable()
    {
        LoadingBar.transform.localScale = new Vector3(0f,1f,1f);
        StartCoroutine(C_CountDown());
    }

    private void Update()
    {
        if (isStartLoading)
        {
            timer+=Time.deltaTime;

            LoadingBar.transform.localScale = new Vector3(timer/loadingTime,1f,1f);

            if (timer>=loadingTime)
            {
                timer=0;
                isStartLoading=false;
                m_MenuSM.State = MenuState.Main;
            }
        }
    }

    private IEnumerator C_CountDown()
    {
        currentCountDownIndex = 0;
        SetCountDownSprite();//5
        yield return new WaitForSeconds(1f);
        currentCountDownIndex++;
        SetCountDownSprite();//4
        yield return new WaitForSeconds(1f);
        currentCountDownIndex++;
        SetCountDownSprite();//3
        yield return new WaitForSeconds(1f);
        currentCountDownIndex++;
        SetCountDownSprite();//2
        Reconnect();
        yield return new WaitForSeconds(1f);
        currentCountDownIndex++;
        SetCountDownSprite();//1
        yield return new WaitForSeconds(1f);
        currentCountDownIndex++;
        SetCountDownSprite();//null
        isStartLoading = true;
        StopCoroutine(C_CountDown());
    }

    private void Reconnect()
    {
        //Warning 接口处理
        HI5_Device.ReConnect();
    }

    private void SetCountDownSprite()
    {
        if (currentCountDownIndex<CountDownSpites.Length)
        {
            CountDownSpriteRender.sprite = CountDownSpites[currentCountDownIndex];
        }
        else
        {
            CountDownSpriteRender.sprite = null;
        }
    }
}
