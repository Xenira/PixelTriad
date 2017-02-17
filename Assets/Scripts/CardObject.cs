using UnityEngine;

public class CardObject : MonoBehaviour {

    //public GameObject cardFront;
    public int dmgUp = 1;
    public int dmgDown = 1;
    public int dmgLeft = 1;
    public int dmgRight = 1;

    private GameObject card;

    private void Start()
    {
        SetDamageNumbers();
        //card = Instantiate(cardFront, gameObject.transform.position, Quaternion.identity, gameObject.transform) as GameObject;
    }

    private void SetDamageNumbers()
    {
        var numbers = gameObject.transform.FindChild("DamageNumbers");
        numbers.FindChild("Top").gameObject.GetComponent<DamageNumber>().SetNumber(dmgUp);
        numbers.FindChild("Bottom").gameObject.GetComponent<DamageNumber>().SetNumber(dmgDown);
        numbers.FindChild("Left").gameObject.GetComponent<DamageNumber>().SetNumber(dmgLeft);
        numbers.FindChild("Right").gameObject.GetComponent<DamageNumber>().SetNumber(dmgRight);
    }

    private void OnDrawGizmos()
    {
        var origin = gameObject.transform.position + new Vector3(14.5f, 19f);
        var circleCenter = gameObject.transform.position + new Vector3(14.5f, 8.5f);
        Gizmos.DrawWireCube(origin, new Vector3(29f, 38f));
        Gizmos.DrawWireSphere(circleCenter, 8);
    }

    #region Selection Events
    private void OnMouseDown()
    {
        PlayerHand hand = transform.parent.GetComponent<PlayerHand>();
        if (hand)
        {
            hand.SelectCard(this);
        }
    }

    private void OnMouseEnter()
    {
        PlayerHand hand = transform.parent.GetComponent<PlayerHand>();
        if (hand)
        {
            hand.HoverCard(this, true);
        }
    }

    private void OnMouseExit()
    {
        PlayerHand hand = transform.parent.GetComponent<PlayerHand>();
        if (hand)
        {
            hand.HoverCard(this, false);
        }
    }
    #endregion
}
