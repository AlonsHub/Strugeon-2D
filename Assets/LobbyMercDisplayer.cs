using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMercDisplayer : BasicDisplayer
{
    [SerializeField]
    ExpBarDisplayer expBarDisplayer;
    MercPoolDisplayer mercPoolDisplayer;
    MercSheet sheet;
    public bool SetMeFull(MercSheet mercSheet, MercPoolDisplayer poolDisplayer)
    {
        mercPoolDisplayer = poolDisplayer;
        sheet = mercSheet;

        Pawn p = mercSheet.MyPawnPrefabRef<Pawn>();

        int maxHpBenefit = 0;
        int damageBenefit = 0;

        foreach (var benefit in mercSheet.gear.GetAllBenefits())
        {
            switch ((benefit as StatBenefit).statToBenefit)
            {
                case StatToBenefit.MaxHP:
                    maxHpBenefit += benefit.Value();
                    break;
                case StatToBenefit.FlatDamage:
                    damageBenefit += benefit.Value();
                    break;
                default:
                    break;
            }
        }

        List<string> textsPerTextBox = new List<string> 
        { mercSheet.characterName.ToString(),
            mercSheet.currentAssignment.ToString(), 
            $"{mercSheet._maxHp}(<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}>+{maxHpBenefit}</color>)", 
            $"{mercSheet._minDamage} - {mercSheet._maxDamage}(<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}>+{damageBenefit}</color>)" };

        List<Sprite> spritesPerImage = new List<Sprite> {p.PortraitSprite, p.SASprite};


        expBarDisplayer.SetBar(mercSheet);

        return base.SetMe(textsPerTextBox, spritesPerImage);
    }

    public void OnMyClick()
    {
        //display merc to the MercGearDisplayer
        mercPoolDisplayer.ShowMercOnGearDisplayer(sheet);
    }
}
