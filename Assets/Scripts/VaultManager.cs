using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VaultManager : MonoBehaviour
{
    // Settings

    // Connections
    public GameObject stickman;
    public GameObject vaultDoor;
    StickmanManager stickmanManager;

    // State Variables
    bool vaultOpeningTweenActive;
    bool vaultClosingTweenActive;
    bool vaultDoorIsOpen;

    public DOTweenAnimation vaultOpeningAnimation;
    public DOTweenAnimation vaultClosingAnimation;

    public float punchScaleMagnitude;

    [HideInInspector] public int totalMoneyCount;


    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        EventManager.ScreenClicked += ScreenClicked;
    }
    void InitState()
    {
        vaultOpeningTweenActive = false;
        vaultClosingTweenActive = false;
        vaultDoorIsOpen = false;
    }

    void Update()
    {
        
    }

    public void SetClosingAnimation()
    {
        if (!vaultClosingTweenActive && vaultDoorIsOpen)
        {
            vaultClosingTweenActive = true;
            vaultDoorIsOpen = false;
            //vaultClosingAnimation.DORestart();
            vaultDoor.transform.DORotate(new Vector3(0, -180, 0), 1).OnComplete(() =>
            {
                vaultClosingTweenActive = false;
            });
        }
    }

    public void SetOpeningAnimation()
    {
        if (!vaultOpeningTweenActive && !vaultDoorIsOpen)
        {
            vaultOpeningTweenActive = true;
            vaultDoorIsOpen = true;
            //vaultOpeningAnimation.DORestart();
            vaultDoor.transform.DORotate(new Vector3(0, -70, 0), 1).OnComplete(() =>
            {
                vaultOpeningTweenActive = false;
            });
        }
    }

    public void OnTweenComplete()
    {
        if (vaultDoorIsOpen)
        {
            vaultOpeningTweenActive = false;
        }
        else
        {
            vaultClosingTweenActive = false;
        }
    }

    public void VaultPunchScaleAnimation(float duration)
    {
        //transform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, punchScaleMagnitude), duration);
    }

    void ScreenClicked()
    {
        
    }

    void OnDestroy()
    {
        EventManager.ScreenClicked -= ScreenClicked;
    }
}

