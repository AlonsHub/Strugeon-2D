using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class BaseStatSetter : ScriptableObject
{
    [SerializeField]
    List<Pawn> pawns;
    [SerializeField]
    BaseStatBlock rogueStatBlock;
    [SerializeField]
    BaseStatBlock priestStatBlock;
    [SerializeField]
    BaseStatBlock figherStatBlock;
    [SerializeField]
    BaseStatBlock mageStatBlock;

    [ContextMenu("Set Advancement for all mercs, by class")]
    public void SetMercsADVANCEMENTByClass()
    {
        //foreach pawns -> switch on class to decide which stat block to apply (or list, and enum as index)
        foreach (var item in pawns)
        {
            switch (item._mercSheet.mercClass)
            {
                case MercClass.Fighter:
                    item._mercSheet.statBlock.CopyAdvancementPercentages(figherStatBlock.block);
                    break;
                case MercClass.Rogue:
                    item._mercSheet.statBlock.CopyAdvancementPercentages(rogueStatBlock.block);
                    break;
                case MercClass.Mage:
                    item._mercSheet.statBlock.CopyAdvancementPercentages(mageStatBlock.block);
                    break;
                case MercClass.Priest:
                    item._mercSheet.statBlock.CopyAdvancementPercentages(priestStatBlock.block);
                    break;
                default:
                    break;
            }
        }
    }
}
