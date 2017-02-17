using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DropPoint : MonoBehaviour
{
    private CardObject card;

    private void OnMouseDown()
    {
        //if (!isLocalPlayer) return;
        if (card) return;

        var player = FindObjectOfType<PlayerHand>();
        if (player.selectedCard)
        {
            SetCard(player.selectedCard);
            player.selectedCard = null;
        }
        else
        {
            print("No card selected");
        }
    }

    public void SetCard(CardObject card)
    {
        this.card = card;
        card.transform.parent = transform;

        var mover = card.GetComponent<ObjectMover>();
        mover.SetOrigin(transform.position + new Vector3(0, 0, 1));
        mover.ResetPosition();
    }

    private void OnDrawGizmos()
    {
        var origin = gameObject.transform.position + new Vector3(14.5f, 19f);
        Gizmos.DrawWireCube(origin, new Vector3(29f, 38f));
    }
}
