using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HI5;
public class VibrateButton : MonoBehaviour
{
    public bool isStrong = false;
    public Image imageButton;
    public Sprite imageCommon;
    public Sprite clickCommon;
    public Sprite NovisibleCommon;

    public VibratePanelScript vibratepanel;
    private float VisibleCd = 1.0f;
    private bool isVisibleClick = false;
    // public bool isEnableUse = true;
    public VibratePanelScript panelScript;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        isVisibleClick = false;
        VisibleSprite();
    }

    private void OnDisable()
    {
        StopCoroutine("RumblerDown");
        isVisibleClick = false;
        VisibleSprite();
    }

    public void ClickButton()
    {
        if (!panelScript.isEnableVibrator())
            return;
        if (isVisibleClick)
        {
            if (vibratepanel)
            {
                if (isStrong)
                    vibratepanel.VibratorStrong(true);
                else
                    vibratepanel.VibratorWeak(true);
            }
            VisibleCd = 0.5f;
        }
        else
        {
            if (vibratepanel)
            {
                if (isStrong)
                    vibratepanel.VibratorStrong(false);
                else
                    vibratepanel.VibratorWeak(false);
            }
            VisibleCd = 1.0f;
            isVisibleClick = true;
            VisibleSprite();
            StartCoroutine("RumblerDown");
        }
    }

    IEnumerator RumblerDown()
    {
        while (VisibleCd > 0)
        {
            VisibleCd -= Time.deltaTime;
            isVisibleClick = true;
            yield return null;
        }
        isVisibleClick = false;
        VisibleSprite();
    }

    public void VisibleSprite()
    {
        if (isStrong)
        {
            if (isVisibleClick)
            {
                imageButton.sprite = clickCommon;
            }
            else
            {
                imageButton.sprite = imageCommon;
            }
        }
        else
        {
            if (isVisibleClick)
            {
                imageButton.sprite = clickCommon;
            }
            else
            {
                imageButton.sprite = imageCommon;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //VisibleSprite();
    }


}
