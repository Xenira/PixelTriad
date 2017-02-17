using UnityEngine;
using UnityEngine.Networking;

public class PlayerHand : MonoBehaviour
{
    public bool playerControllabel = true;
    public CardObject[] cards = new CardObject[5];

    public Vector3 cardOffset = new Vector3(0, 20f, -1);
    public Vector3 selectedCardLocation = Vector3.right;
    public Vector3 hoverOffset = Vector3.right * 2;
    internal CardObject selectedCard;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            if (card)
            {
                var position = transform.position + (i * cardOffset);
                Instantiate(card, position, Quaternion.identity, transform);
            }
        }

        //ServerConnection.connection.StartLongPolling(GamesCallback, "localhost:9000/games", ServerConnection.HTTP_METHOD.PUT);
    }

    private void GamesCallback(string error, string data, UnityWebRequest request)
    {
        if (error == null)
        {
            Debug.Log(data);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        var handSize = new Vector3(29, 38) + (4 * cardOffset);
        Gizmos.DrawWireCube(transform.position + selectedCardLocation + new Vector3(29, 38) / 2, new Vector3(29, 38));
        Gizmos.DrawWireCube(transform.position + handSize / 2, handSize);
    }

    internal void HoverCard(CardObject card, bool isHovered)
    {
        if (card == selectedCard) return;

        ObjectMover mover = card.GetComponent<ObjectMover>();
        if (!isHovered)
        {
            mover.ResetPosition();
        } else {
            mover.MoveToOffset(hoverOffset);
        }
    }

    internal void SelectCard(CardObject card)
    {
        if (selectedCard)
        {
            ObjectMover oldMover = selectedCard.GetComponent<ObjectMover>();
            oldMover.ResetPosition();
        }

        if (selectedCard != card)
        {
            ObjectMover mover = card.GetComponent<ObjectMover>();
            mover.MoveToLocation(transform.position + selectedCardLocation);
            selectedCard = card;
        }
        else
        {
            selectedCard = null;
        }

    }
}
