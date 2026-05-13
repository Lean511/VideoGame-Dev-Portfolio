using UnityEngine;

public class Ability
{
    public int id;
    public AbilityType.Type type;
    public string name;
    public string description;
    public int unlockLevel;
    public PieceType.Type? PieceTypeSpecific;
    public int functionsId;
    public int targetTeam; //0 = Same team, 1 = Opponent team, 2 = Any team
    public PieceType.Type? targetType;

    public void SetAbility(int id, int unlockLevel)
    {
        switch (id)
        {
            case 0:
                this.id = 0;
                this.type = AbilityType.Type.SpecialSynergy;
                this.name = "Castle";
                this.description = "";
                this.PieceTypeSpecific = PieceType.Type.Rook;
                this.functionsId = 0;
                this.targetTeam = 0;
                this.targetType = PieceType.Type.King;
                break;
            case 1:
                this.id = 1;
                this.type = AbilityType.Type.Active;
                this.name = "Speed";
                this.description = "";
                this.PieceTypeSpecific = null;
                //this.functionsId = #;
                this.targetTeam = 1;
                this.targetType = null;
                break;
        }
        this.unlockLevel = unlockLevel;
    }
}
