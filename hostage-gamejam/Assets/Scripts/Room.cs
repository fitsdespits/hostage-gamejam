using UnityEngine;
using System.Collections.Generic;

public abstract class Room : MonoBehaviour
{
    public string roomName;
    public string animationTrigger;
    public List<Pawn> pawnsInRoom = new List<Pawn>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Pawn>(out var pawn))
        {
            PawnEnter(pawn);     
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Pawn>(out var pawn))
        {
           PawnExit(pawn);        
        }
    }

    public void PawnEnter(Pawn pawn)
    {
        if (!pawnsInRoom.Contains(pawn))
        {
            if (pawn.currentRoom != null)
            {
                pawn.currentRoom.PawnExit(pawn);
            }
            pawn.currentRoom = this;
            pawnsInRoom.Add(pawn);
            
            OnPawnEnter(pawn);

            
        }
    }
    
    public void PawnExit(Pawn pawn)
    {
        if (pawnsInRoom.Contains(pawn))
        {
            pawnsInRoom.Remove(pawn);
            OnPawnExit(pawn);
        }
    }
    // Called when a pawn enters the room
    public virtual void OnPawnEnter(Pawn pawn)
    {

    }

    // Called when a pawn exits the room
    public virtual void OnPawnExit(Pawn pawn)
    {
 
    }

}
