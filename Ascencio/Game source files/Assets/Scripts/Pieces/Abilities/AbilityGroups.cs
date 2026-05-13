using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGroups : MonoBehaviour
{
    public int AbilitiesGroupId;
    public List<Ability> abilityList;
    public AbilityFunctions abilityFunctions;

    //Crea un grupo de abilidades.
    public void SetAbilityGroup(int id)
    {
        var ability = new Ability();
        abilityList = new List<Ability>();

        switch (id)
        {
            case 0:
                this.AbilitiesGroupId = 0;
                ability.SetAbility(0, 0);
                abilityList.Add(ability);
                break;
        }
    }

    //Verifica si alguna habilidad del grupo puede ser ejecutada.
    public bool CheckAbilities(ChessPiece piece, Vector2Int destination)
    {
        ChessPiece selectedPiece = piece.GetGridSystem().GetPieceOnGridPosition(destination);
        abilityFunctions = gameObject.AddComponent<AbilityFunctions>();

        foreach (Ability ability in abilityList)
        {
            if (ability.targetType == selectedPiece.pieceType || ability.targetType == null)
            {
                if ((ability.targetTeam == 0 && selectedPiece.team == piece.team) ||
                    (ability.targetTeam == 1 && selectedPiece.team != piece.team) ||
                    (ability.targetTeam == 2))
                {
                    //Debug.Log("Ability ID: " + ability.id + ", Function ID: " + ability.functionsId);
                    return abilityFunctions.CheckAbilityFunction(ability.functionsId, piece, destination);
                }
            }
        }

        return false; //Returns false by default if the ability did not execute successfully.
    }

    //Devuelve el id de la habilidad que puede ser ejecutada.
    public int GetActivableAbilityId(ChessPiece piece, Vector2Int destination)
    {
        ChessPiece selectedPiece = piece.GetGridSystem().GetPieceOnGridPosition(destination);
        abilityFunctions = gameObject.AddComponent<AbilityFunctions>();

        foreach (Ability ability in abilityList)
        {
            if (ability.targetType == selectedPiece.pieceType || ability.targetType == null)
            {
                if ((ability.targetTeam == 0 && selectedPiece.team == piece.team) ||
                    (ability.targetTeam == 1 && selectedPiece.team != piece.team) ||
                    (ability.targetTeam == 2))
                {
                    //Debug.Log("Ability ID: " + ability.id + ", Function ID: " + ability.functionsId);

                    if (abilityFunctions.CheckAbilityFunction(ability.functionsId, piece, destination))
                    {
                        return ability.id;
                    }
                }
            }
        }

        return 0; //Returns 0 by default if the ability did not execute successfully.
    }

    //Devuelve la habilidad que puede ser ejecutada.
    public Ability GetActivableAbility(ChessPiece piece, Vector2Int destination)
    {
        ChessPiece selectedPiece = piece.GetGridSystem().GetPieceOnGridPosition(destination);
        abilityFunctions = gameObject.AddComponent<AbilityFunctions>();

        foreach (Ability ability in abilityList)
        {
            if (ability.targetType == selectedPiece.pieceType || ability.targetType == null)
            {
                if ((ability.targetTeam == 0 && selectedPiece.team == piece.team) ||
                    (ability.targetTeam == 1 && selectedPiece.team != piece.team) ||
                    (ability.targetTeam == 2))
                {
                    //Debug.Log("Ability ID: " + ability.id + ", Function ID: " + ability.functionsId);

                    if (abilityFunctions.CheckAbilityFunction(ability.functionsId, piece, destination))
                    {
                        return ability;
                    }
                }
            }
        }

        return null; //Returns null by default if the ability did not execute successfully.
    }

    //Por el momento no se utiliza esta función.
    public bool DoesAnyAbilityTargetsSameTeam(ChessPiece piece)
    {
        foreach (Ability ability in abilityList)
        {
            if (ability.targetTeam == 0)
            {
                return true;
            }
        }

        return false; //Returns default value.
    }

    //Por el momento no se utiliza esta función.
    public AbilityType.Type CheckForAbilityType()
    {
        foreach (Ability ability in abilityList)
        {
            return ability.type;
        }

        return AbilityType.Type.Passive; //Returns default value.
    }
}
