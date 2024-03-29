﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaveVariable : MonoBehaviour
{
    [SerializeField, Tooltip("Action Item to be effected")]
    protected ActionItem affectedBehaviour;
    [SerializeField]
    protected int modifier;

    private void Start()
    {
        affectedBehaviour.AddBehaveVariable(this);
    }

    public virtual void ConditionallyApplyMod()
    {

    }
}
