using UnityEngine;

public abstract class Room : MonoBehaviour
{
    public string roomName;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Pawn>(out var pawn))
        {
            OnPawnEnter(pawn);
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Pawn>(out var pawn))
        {
            OnPawnExit(pawn);
           
        }
    }
   
    // Called when a robber enters
    public virtual void OnPawnEnter(Pawn pawn)
    {
        Debug.Log("in");
    }
    // Called when a robber leaves
    public virtual void OnPawnExit(Pawn pawn)
    {
        Debug.Log("out");
    }


}
