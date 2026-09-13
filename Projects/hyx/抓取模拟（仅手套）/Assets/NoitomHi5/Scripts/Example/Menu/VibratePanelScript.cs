using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HI5;
public class VibratePanelScript : MonoBehaviour
{
    public Image imageRumbler1;
    public Sprite RumblerSprite1_1;
    public Sprite RumblerSprite1_2;
    public Image imageRumbler2;
    public Sprite RumblerSprite2_1;
    public Sprite RumblerSprite2_2;
    public VibrateButton mWeakButton;
    public VibrateButton mStrongButton;
    bool IsRumbler1;
    bool IsRumbler2;
    public HI5GloveState mstate;
    // Start is called before the first frame update
    void Start()
    {
        IsRumbler1 = false;
        IsRumbler2 = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(mWeakButton != null)
        {
            mWeakButton.VisibleSprite();
        }
        if (mStrongButton != null)
        {
            mStrongButton.VisibleSprite();
        }
    }
    public void ClickRumbler1()
    {
        //if (!isEnableClickRumbler1Button())
        //    return;
        if(IsRumbler1)
        {
            imageRumbler1.sprite = RumblerSprite1_1;
        }
        else
        {
            imageRumbler1.sprite = RumblerSprite1_2;
        }
        IsRumbler1 = !IsRumbler1;
    }
    public void ClickRumbler2()
    {
        //if (!isEnableClickRumbler2Button())
        //    return;
        if (IsRumbler2)
        {
            imageRumbler2.sprite = RumblerSprite2_1;
        }
        else
        {
            imageRumbler2.sprite = RumblerSprite2_2;
        }
        IsRumbler2 = !IsRumbler2;
    }
    public void VibratorStrong(bool isAppend)
    {
        if(IsRumbler1 && IsRumbler2 )
        {
            if (isAppend)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    if (isEnableClickRumbler1Button() && isEnableClickRumbler2Button())
                        HI5_Manager_Thread.Instance().Vibrate(0, 3, 500, 1);
                    else if(isEnableClickRumbler1Button())
                    {
                        HI5_Manager_Thread.Instance().Vibrate(1, 3, 500, 1);
                    }
                    else if (isEnableClickRumbler2Button())
                    {
                        HI5_Manager_Thread.Instance().Vibrate(2, 3, 500, 1);
                    }
                }
                    
            }
            else
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    //Debug.LogWarning("VibratorStrong 1000");
                   // HI5_Manager_Thread.Instance().Vibrate(0, 3, 1000, 1);
                    if (isEnableClickRumbler1Button() && isEnableClickRumbler2Button())
                        HI5_Manager_Thread.Instance().Vibrate(0, 3, 1000, 1);
                    else if (isEnableClickRumbler1Button())
                    {
                        HI5_Manager_Thread.Instance().Vibrate(1, 3, 1000, 1);
                    }
                    else if (isEnableClickRumbler2Button())
                    {
                        HI5_Manager_Thread.Instance().Vibrate(2, 3, 1000, 1);
                    }
                }
                    
            }
        }
        else if (IsRumbler1)
        {
            if (isAppend)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    if (isEnableClickRumbler1Button())
                        HI5_Manager_Thread.Instance().Vibrate(1, 3, 500, 1);
                }
                    
            }
            else
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    if (isEnableClickRumbler1Button())
                        HI5_Manager_Thread.Instance().Vibrate(1, 3, 1000, 1);
                }
                   
            }
        }
        else if (IsRumbler2)
        {
            if (isAppend)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    if (isEnableClickRumbler2Button())
                        HI5_Manager_Thread.Instance().Vibrate(2, 3, 500, 1);
                }
                    
            }
            else
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    if (isEnableClickRumbler2Button())
                        HI5_Manager_Thread.Instance().Vibrate(2, 3, 1000, 1);
                }
                   
            }
        }
    }
    public bool isEnableClickRumbler1Button()
    {
        // return true;
        if (mstate.IsRembler1Can())
            return true;
        else
            return false;
    }

    public bool isEnableClickRumbler2Button()
    {
        //return true;
        if (mstate.IsRembler2Can())
            return true;
        else
            return false;
    }

    protected void OnEnable()
    {
        if(imageRumbler1)
        {
            imageRumbler1.sprite = RumblerSprite1_1;
        }
        if (imageRumbler2)
        {
            imageRumbler2.sprite = RumblerSprite2_1;
        }
        IsRumbler1 = false;
        IsRumbler2 = false;
    }

    public bool isEnableVibrator()
    {
         bool isHaveClick = false;
        if(IsRumbler1)
        {
            if(mstate.IsRembler1Can())
                isHaveClick = true;
        }
        else if (IsRumbler2)
        {
            if (mstate.IsRembler2Can())
                isHaveClick = true;
        }
        //bool isHaveSingal = false;
        //{
        //    if(mstate.IsRembler1Can() || mstate.IsRembler2Can())
        //    {
        //        isHaveSingal = true;
        //    }
        //}
        if (isHaveClick)
            return true;
        else
            return false;
    }

    public void VibratorWeak(bool isAppend)
    {
        if (IsRumbler1 && IsRumbler2)
        {
            if (isAppend)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    //Debug.LogWarning("VibratorWeak 500");
                    HI5_Manager_Thread.Instance().Vibrate(0, 1, 500, 1);
                }
                    
            }
            else
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    //Debug.LogWarning("VibratorWeak 1000");
                    HI5_Manager_Thread.Instance().Vibrate(0, 1, 1000, 1);
                }
                    
            }
        }
        else if (IsRumbler1)
        {
            if (isAppend)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    //Debug.LogWarning("VibratorWeak 500");
                    HI5_Manager_Thread.Instance().Vibrate(1, 1, 500, 1);
                }
                    
            }
            else
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                   // Debug.LogWarning("VibratorWeak 1000");
                    HI5_Manager_Thread.Instance().Vibrate(1, 1, 1000, 1);
                }
                    
            }
        }
        else if (IsRumbler2)
        {
            if (isAppend)
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    //Debug.LogWarning("VibratorWeak 500");
                    HI5_Manager_Thread.Instance().Vibrate(2, 1, 500, 1);
                }
                    
            }
            else
            {
                if (HI5_Manager_Thread.Instance() != null)
                {
                    //Debug.LogWarning("VibratorWeak 1000");
                    HI5_Manager_Thread.Instance().Vibrate(2, 1, 1000, 1);
                }
                    
            }
        }
    }
}
