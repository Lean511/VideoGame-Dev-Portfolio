using Unity.VisualScripting;
using UnityEngine;

public class AbilityList : MonoBehaviour
{
    public Ability ability;

    public Ability GetAbility()
    {
        ability.id = 0;
        ability.type = AbilityType.Type.SpecialSynergy;
        ability.name = "Castle";
        ability.description = "";
        ability.unlockLevel = 0;
        ability.PieceTypeSpecific = PieceType.Type.Rook;

        return ability;
    }
}
