using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceType.Type pieceType;
    public int id;
    public string pieceName;
    public ItemClass.ClassName rarity;
    public bool canFly;
    public int minHorizontalMoves;
    public int maxHorizontalMoves;
    public int minVerticalMoves;
    public int maxVerticalMoves;
    public int minDiagonalMoves;
    public int maxDiagonalMoves;
    public bool hasSpecialMovement;
    public int abilityGroup = -1;
    [SerializeField] private AbilityGroups abilitiesAssignment;

    public void SetupAbilities() {
        //Si la pieza tiene un grupo de habilidades, se las asigna.
        if (abilityGroup >= 0)
        {
            abilitiesAssignment = gameObject.AddComponent<AbilityGroups>();
            abilitiesAssignment.SetAbilityGroup(this.abilityGroup);
            //Debug.Log("abilitiesAssignment.abilities: " + abilitiesAssignment.abilities);
        }
    }

    public AbilityGroups GetAbilityGroup()
    {
        return this.abilitiesAssignment;
    }
}
