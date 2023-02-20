using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnStartStatusEffect : StatusEffect
{
    public TurnStartStatusEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        //Dont apply effect on all - just in case you want a clean one
    }

    public override void ApplyEffect()
    {

    }

    public override void EndEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void Perform()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
