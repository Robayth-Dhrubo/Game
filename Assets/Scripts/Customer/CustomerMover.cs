﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMover : MonoBehaviour
{
    [SerializeField] private QueueController _queueController;

    public QueueSpot _queueSpot;

    //private CustomerController _customerController;

    public Action OnCustomerWaiting;

    //public static Action<QueueSpot> OnCustomerRelinquishQueueSpot;
    private Animator animator;

    private LTSeq _moveSeq;
    private void Awake()
    {
    }

    public void OnEnable()
    {
        QueueController.OnQueueSpotOpenedUp += OnQueueSpotOpenedUp;
    }

    public void OnDisable()
    {
        QueueController.OnQueueSpotOpenedUp -= OnQueueSpotOpenedUp;

    }

    void Start()
    {
        _queueController = FindObjectOfType<QueueController>();

        MoveCustomerToQueue();
    }

    private void MoveCustomerToQueue()
    {
        CheckIfAnimatorHasBeenAssigned();
        _queueSpot = _queueController.GetAvailableSpot();

        LeanTween.init(800);
        _moveSeq = LeanTween.sequence();
        _moveSeq.append(
            LeanTween.move(gameObject, _queueSpot.transform.position, 3.0f)
            .setOnStart(() => { animator.SetBool("Walking", true); })
            .setOnComplete(TriggerEvent));
    }

    private void CheckIfAnimatorHasBeenAssigned()
    {
        if(animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void TriggerEvent()
    {
        animator.SetBool("Walking", false);
        if (!_queueSpot.isQueueFront) { return; }
        AkSoundEngine.PostEvent("NPC_Bell", gameObject);
        OnCustomerWaiting?.Invoke();
    }

    private void OnQueueSpotOpenedUp(QueueSpot oldQueueSpot, QueueSpot newQueueSpot)
    {
        if (_queueSpot == oldQueueSpot)
        {
            CheckIfAnimatorHasBeenAssigned();
            _queueSpot = newQueueSpot;
            _queueSpot.IsEmpty = false;
            _moveSeq.append(
                LeanTween.move(gameObject, _queueSpot.transform.position, 0.5f)
                .setOnStart(() => { animator.SetBool("Walking", true); })
                .setOnComplete(TriggerEvent));
        }
    }
}


