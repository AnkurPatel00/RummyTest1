using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackInitiator : MonoBehaviour
{

    public enum CardPattern { color, order };

    public Vector3 cardPosition;
    public Vector3 cardRotation;
    public Vector3 cardIncrement;

    public GameObject cardsPositionPointsObject;
    public Transform deck;

    public CardPattern cardPattern;

    public Texture2D cardBack;

    public List<CardsPositionPoints> cardsPositionPoints = new List<CardsPositionPoints>();

    private string[] cardname = new string[] { "club", "diamond", "heart", "spade" };
    private int[] cardnumber = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

    private int cardsPositionPointsID = 0;

    public bool isAiInitiator;

    public void ArrangePack()
    {
        //ArrangeCards();
        d
    }

    public void InitiatePack(List<GameObject> cards)
    {
        StartCoroutine(PlaceCards(cards));
    }

    public void PlaceDeckCardOnPlayerHands(Cards cards)
    {

        PlaceDeckCardOnPlayerIenum(cards);
        //StartCoroutine(PlaceDeckCardOnPlayerIenum(cards));
    }

    IEnumerator PlaceCards(List<GameObject> cards)
    {

        int count = 0;

        yield return new WaitForSeconds(0.0f);

        while (count < cards.Count)
        {
            GameObject cardObj = Instantiate(cards[count], deck.position, Quaternion.identity) as GameObject;
            cardObj.transform.localScale = new Vector3(0.5f, -0.0003914368f, 0.8f);
            cardObj.name = cards[count].name;

            GameObject cardPosPointObj = Instantiate(cardsPositionPointsObject, cardPosition, Quaternion.identity) as GameObject;
            cardPosPointObj.transform.parent = transform;
            cardPosPointObj.GetComponent<CardsPositionPoints>().cards = cardObj.GetComponent<Cards>() as Cards;
            cardPosPointObj.GetComponent<CardsPositionPoints>().id = cardsPositionPointsID;
            cardsPositionPointsID++;
            cardsPositionPoints.Add(cardPosPointObj.GetComponent<CardsPositionPoints>());

            cardObj.transform.parent = cardPosPointObj.transform;
            cardObj.GetComponent<Cards>().cardsPositionPoints = cardPosPointObj.GetComponent<CardsPositionPoints>() as CardsPositionPoints;
            cardObj.GetComponent<Cards>().packInitiator = this;
            yield return new WaitForSeconds(0.0f);
            cardObj.GetComponent<Cards>().StartDeckToHandAnimation(cardPosPointObj.transform);
            cardPosition += cardIncrement;
            count++;
            yield return new WaitForSeconds(0.0f);
            cardObj.transform.rotation = Quaternion.Euler(cardRotation.x, cardRotation.y, cardRotation.z);

        }
    }



    void PlaceDeckCardOnPlayerIenum(Cards cards)
    {

        GameObject cardPosPointObj = Instantiate(cardsPositionPointsObject, cardPosition, Quaternion.identity) as GameObject;
        cardPosPointObj.transform.parent = transform;
        cardPosPointObj.GetComponent<CardsPositionPoints>().cards = cards;
        cardPosPointObj.GetComponent<CardsPositionPoints>().id = cardsPositionPointsID;
        cardsPositionPointsID++;
        cardsPositionPoints.Add(cardPosPointObj.GetComponent<CardsPositionPoints>());

        cards.transform.parent = cardPosPointObj.transform;
        cards.cardsPositionPoints = cardPosPointObj.GetComponent<CardsPositionPoints>() as CardsPositionPoints;
        cards.packInitiator = this;
        //yield return new WaitForSeconds(0.0f);
        cards.StartDeckToHandAnimation(cardPosPointObj.transform);
        cardPosition += cardIncrement;

        //yield return new WaitForSeconds(0.0f);
        cards.transform.rotation = Quaternion.Euler(cardRotation.x, cardRotation.y, cardRotation.z);

    }


    public void DestroyCardPosition(CardsPositionPoints point)
    {

        int index = 0;
        bool flag = false;

        for (int i = 0; i < cardsPositionPoints.Count; i++)
        {
            if (cardsPositionPoints[i].id == point.id)
            {
                flag = true;
                index = i;
            }
        }

        Vector3 tempPosition = cardsPositionPoints[index].transform.position;

        if (flag)
        {

            Destroy(cardsPositionPoints[index].gameObject);
            cardsPositionPoints.RemoveAt(index);
            cardPosition -= cardIncrement;
        }
        else
        {
            print("not found");
        }

        for (int i = index; i < cardsPositionPoints.Count; i++)
        {
            Vector3 tempPosition2 = cardsPositionPoints[i].transform.position;
            cardsPositionPoints[i].transform.position = tempPosition;
            tempPosition = tempPosition2;
        }
    }


    public void ArrangeByColor()
    {

        int indexCount = 0;

        for (int a = 0; a < cardname.Length; a++)
        {

            string cardName = cardname[a];

            for (int i = indexCount; i < cardsPositionPoints.Count; i++)
            {

                if (cardsPositionPoints[i].cards.cardName == cardName)
                {
                    indexCount++;
                }
                else
                {
                    for (int j = i + 1; j < cardsPositionPoints.Count; j++)
                    {
                        if (cardsPositionPoints[i].cards.cardName != cardName)
                        {
                            if (cardsPositionPoints[j].cards.cardName == cardName)
                            {
                                //spawing position
                                Vector3 tempPosition = cardsPositionPoints[j].cards.gameObject.transform.position;
                                cardsPositionPoints[j].cards.gameObject.transform.position = cardsPositionPoints[i].cards.gameObject.transform.position;
                                cardsPositionPoints[i].cards.gameObject.transform.position = tempPosition;
                                //spawing parents
                                Transform tempParent = cardsPositionPoints[j].cards.gameObject.transform.parent;
                                cardsPositionPoints[j].cards.gameObject.transform.parent = cardsPositionPoints[i].cards.gameObject.transform.parent;
                                cardsPositionPoints[i].cards.gameObject.transform.parent = tempParent;
                                //spawing CardsPositionPoints
                                CardsPositionPoints tempcardsPositionPoint = cardsPositionPoints[j].cards.cardsPositionPoints;
                                cardsPositionPoints[j].cards.cardsPositionPoints = cardsPositionPoints[i].cards.cardsPositionPoints;
                                cardsPositionPoints[i].cards.cardsPositionPoints = tempcardsPositionPoint;
                                //spawing cards
                                Cards tempCards = cardsPositionPoints[i].cards;
                                cardsPositionPoints[i].cards = cardsPositionPoints[j].cards;
                                cardsPositionPoints[j].cards = tempCards;
                                indexCount++;
                            }
                        }
                    }
                }
            }
        }



        // now arranging cards by order
        for (int a = 0; a < cardname.Length; a++)
        {
            int startIndex = 0;
            int endIndex = 0;
            bool checkStartIndex = true;
            bool checkEndIndex = false;
            for (int i = 0; i < cardsPositionPoints.Count; i++)
            {
                if (checkStartIndex)
                {
                    if (cardsPositionPoints[i].cards.cardName == cardname[a])
                    {
                        startIndex = i;
                        checkStartIndex = false;
                        checkEndIndex = true;
                    }
                }
                else if (checkEndIndex)
                {
                    if (cardsPositionPoints[i].cards.cardName != cardname[a])
                    {
                        endIndex = i - 1;
                        checkEndIndex = false;
                    }
                }
                // we never get endindex bec there is no other name card left in the array to get endindex for spade
                if (i == (cardsPositionPoints.Count - 1))
                {
                    if (checkEndIndex)
                    {//&& (cardname[a] == "club" || cardname[a] == "diamond" || cardname[a] == "heart" || cardname[a] == "spade")
                        endIndex = cardsPositionPoints.Count - 1;
                    }
                }
            }

            indexCount = startIndex;

            for (int b = 0; b < cardnumber.Length; b++)
            {

                int cardNumber = cardnumber[b];

                for (int i = indexCount; i <= endIndex; i++)
                {

                    if (cardsPositionPoints[i].cards.number == cardNumber)
                    {
                        indexCount++;
                    }
                    else
                    {
                        for (int j = i + 1; j <= endIndex; j++)
                        {
                            if (cardsPositionPoints[i].cards.number != cardNumber)
                            {
                                if (cardsPositionPoints[j].cards.number == cardNumber)
                                {
                                    //spawing position
                                    Vector3 tempPosition = cardsPositionPoints[j].cards.gameObject.transform.position;
                                    cardsPositionPoints[j].cards.gameObject.transform.position = cardsPositionPoints[i].cards.gameObject.transform.position;
                                    cardsPositionPoints[i].cards.gameObject.transform.position = tempPosition;
                                    //spawing parents
                                    Transform tempParent = cardsPositionPoints[j].cards.gameObject.transform.parent;
                                    cardsPositionPoints[j].cards.gameObject.transform.parent = cardsPositionPoints[i].cards.gameObject.transform.parent;
                                    cardsPositionPoints[i].cards.gameObject.transform.parent = tempParent;
                                    //spawing CardsPositionPoints
                                    CardsPositionPoints tempcardsPositionPoint = cardsPositionPoints[j].cards.cardsPositionPoints;
                                    cardsPositionPoints[j].cards.cardsPositionPoints = cardsPositionPoints[i].cards.cardsPositionPoints;
                                    cardsPositionPoints[i].cards.cardsPositionPoints = tempcardsPositionPoint;
                                    //spawing cards
                                    Cards tempCards = cardsPositionPoints[i].cards;
                                    cardsPositionPoints[i].cards = cardsPositionPoints[j].cards;
                                    cardsPositionPoints[j].cards = tempCards;

                                    indexCount++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    public void ArrangeByOrder()
    {

        int indexCount = 0;

        for (int a = 0; a < cardnumber.Length; a++)
        {

            int cardNumber = cardnumber[a];

            for (int i = indexCount; i < cardsPositionPoints.Count; i++)
            {

                if (cardsPositionPoints[i].cards.number == cardNumber)
                {
                    indexCount++;
                }
                else
                {
                    for (int j = i + 1; j < cardsPositionPoints.Count; j++)
                    {
                        if (cardsPositionPoints[i].cards.number != cardNumber)
                        {
                            if (cardsPositionPoints[j].cards.number == cardNumber)
                            {
                                //spawing position
                                Vector3 tempPosition = cardsPositionPoints[j].cards.gameObject.transform.position;
                                cardsPositionPoints[j].cards.gameObject.transform.position = cardsPositionPoints[i].cards.gameObject.transform.position;
                                cardsPositionPoints[i].cards.gameObject.transform.position = tempPosition;
                                //spawing parents
                                Transform tempParent = cardsPositionPoints[j].cards.gameObject.transform.parent;
                                cardsPositionPoints[j].cards.gameObject.transform.parent = cardsPositionPoints[i].cards.gameObject.transform.parent;
                                cardsPositionPoints[i].cards.gameObject.transform.parent = tempParent;
                                //spawing CardsPositionPoints
                                CardsPositionPoints tempcardsPositionPoint = cardsPositionPoints[j].cards.cardsPositionPoints;
                                cardsPositionPoints[j].cards.cardsPositionPoints = cardsPositionPoints[i].cards.cardsPositionPoints;
                                cardsPositionPoints[i].cards.cardsPositionPoints = tempcardsPositionPoint;
                                //spawing cards
                                Cards tempCards = cardsPositionPoints[i].cards;
                                cardsPositionPoints[i].cards = cardsPositionPoints[j].cards;
                                cardsPositionPoints[j].cards = tempCards;

                                indexCount++;
                            }
                        }
                    }
                }
            }
        }
    }
}
