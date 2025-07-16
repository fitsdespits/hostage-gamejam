using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConferenceRoom : Room
{
    public TextMeshProUGUI wantText;
    public TextMeshProUGUI giveText;

    private void Update()
    {
        UpdateSentences();
    }

    private void UpdateSentences()
    {
        if (pawnsInRoom == null || pawnsInRoom.Count == 0)
        {
            wantText.text = "I want ...";
            giveText.text = "I give ...";
            return;
        }

        List<Pawn> pawnsOnLeft = new List<Pawn>();
        List<Pawn> pawnsOnRight = new List<Pawn>();

        foreach (var pawn in pawnsInRoom)
        {
            Vector3 localPos = transform.InverseTransformPoint(pawn.transform.position);
            if (localPos.x < 0)
                pawnsOnLeft.Add(pawn);
            else
                pawnsOnRight.Add(pawn);
        }

        wantText.text = GenerateSentencePart(pawnsOnLeft, "I want");
        giveText.text = GenerateSentencePart(pawnsOnRight, "I give");
    }

    private string GenerateSentencePart(List<Pawn> pawns, string prefix)
    {
        if (pawns.Count == 0)
            return $"{prefix} ...";

        List<string> items = new List<string>();
        foreach (var pawn in pawns)
        {
            if (pawn.pawnType == PawnType.Robber)
            {
                switch (pawn.robberType)
                {
                    case RobberType.GasRobber:
                        items.Add("gas");
                        break;
     
                    default:
                        items.Add("money");
                        break;
                }
            }
            else if (pawn.pawnType == PawnType.Hostage)
            {
                items.Add("hostage");
            }
            else
            {
                items.Add("something");
            }
        }

        return $"{prefix} {string.Join(", ", items)}";
    }
}