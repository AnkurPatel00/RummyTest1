using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    private List<CardsPositionPoints> m_cardsPositionPointsList;
    public List<CardsPositionPoints> cardsPositionPointsList
    {
        get
        {
            return m_cardsPositionPointsList;
        }
        set
        {
            m_cardsPositionPointsList = value;
            SetPositionOfCards();
        }
    }

    private string[] cardname = new string[] { "club", "diamond", "heart", "spade" };
    private int[] cardnumber = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

    private int cardsPositionPointsID = 0;

    public bool isAiInitiator;

    public Vector3[] PlayerCardsPosArray;

    void Start()
    {
        m_cardsPositionPointsList = new List<CardsPositionPoints>();
        cardsPositionPointsList = new List<CardsPositionPoints>();
        PlayerCardsPosArray = new Vector3[15];
        SetArrayForDefautCardPosition();
    }

    public void ArrangePack()
    {
        //ArrangeCards();        
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
            cardPosPointObj.transform.localScale = Vector3.one * 1.25f;
            cardPosPointObj.GetComponent<CardsPositionPoints>().cards = cardObj.GetComponent<Cards>() as Cards;
            cardPosPointObj.GetComponent<CardsPositionPoints>().id = cardsPositionPointsID;
            cardsPositionPointsID++;
            cardsPositionPointsList.Add(cardPosPointObj.GetComponent<CardsPositionPoints>());

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
        cardsPositionPointsList.Add(cardPosPointObj.GetComponent<CardsPositionPoints>());

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

        for (int i = 0; i < cardsPositionPointsList.Count; i++)
        {
            if (cardsPositionPointsList[i].id == point.id)
            {
                flag = true;
                index = i;
            }
        }

        Vector3 tempPosition = cardsPositionPointsList[index].transform.position;

        if (flag)
        {

            Destroy(cardsPositionPointsList[index].gameObject);
            cardsPositionPointsList.RemoveAt(index);
            cardPosition -= cardIncrement;
        }
        else
        {
            print("not found");
        }

        for (int i = index; i < cardsPositionPointsList.Count; i++)
        {
            Vector3 tempPosition2 = cardsPositionPointsList[i].transform.position;
            cardsPositionPointsList[i].transform.position = tempPosition;
            tempPosition = tempPosition2;
        }
    }

    public void ArrangeByColor()
    {
        ArrangeByColorOld();
        //cardsPositionPointsList.fin
    }

    public void ArrangeByOrder()
    {
        //ArrangeByOrderOld();           
        cardsPositionPointsList = cardsPositionPointsList.OrderBy(x => x.cards.number).ToList();
    }

    void ArrangeByOrderOld()
    {
        Debug.Log("Called");
        int indexCount = 0;

        for (int a = 0; a < cardnumber.Length; a++)
        {
            int cardNumber = cardnumber[a];

            for (int i = indexCount; i < cardsPositionPointsList.Count; i++)
            {
                if (cardsPositionPointsList[i].cards.number == cardNumber)
                {
                    indexCount++;
                }
                else
                {
                    for (int j = i + 1; j < cardsPositionPointsList.Count; j++)
                    {
                        if (cardsPositionPointsList[i].cards.number != cardNumber)
                        {
                            if (cardsPositionPointsList[j].cards.number == cardNumber)
                            {
                                //spawing position
                                Vector3 tempPosition = cardsPositionPointsList[j].cards.gameObject.transform.position;
                                cardsPositionPointsList[j].cards.gameObject.transform.position = cardsPositionPointsList[i].cards.gameObject.transform.position;
                                cardsPositionPointsList[i].cards.gameObject.transform.position = tempPosition;
                                //spawing parents
                                Transform tempParent = cardsPositionPointsList[j].cards.gameObject.transform.parent;
                                cardsPositionPointsList[j].cards.gameObject.transform.parent = cardsPositionPointsList[i].cards.gameObject.transform.parent;
                                cardsPositionPointsList[i].cards.gameObject.transform.parent = tempParent;
                                //spawing CardsPositionPoints
                                CardsPositionPoints tempcardsPositionPoint = cardsPositionPointsList[j].cards.cardsPositionPoints;
                                cardsPositionPointsList[j].cards.cardsPositionPoints = cardsPositionPointsList[i].cards.cardsPositionPoints;
                                cardsPositionPointsList[i].cards.cardsPositionPoints = tempcardsPositionPoint;
                                //spawing cards
                                Cards tempCards = cardsPositionPointsList[i].cards;
                                cardsPositionPointsList[i].cards = cardsPositionPointsList[j].cards;
                                cardsPositionPointsList[j].cards = tempCards;

                                indexCount++;
                            }
                        }
                    }
                }
            }
        }
    }

    void ArrangeByColorOld()
    {

        int indexCount = 0;

        for (int a = 0; a < cardname.Length; a++)
        {

            string cardName = cardname[a];

            for (int i = indexCount; i < cardsPositionPointsList.Count; i++)
            {

                if (cardsPositionPointsList[i].cards.cardName == cardName)
                {
                    indexCount++;
                }
                else
                {
                    for (int j = i + 1; j < cardsPositionPointsList.Count; j++)
                    {
                        if (cardsPositionPointsList[i].cards.cardName != cardName)
                        {
                            if (cardsPositionPointsList[j].cards.cardName == cardName)
                            {
                                //spawing position
                                Vector3 tempPosition = cardsPositionPointsList[j].cards.gameObject.transform.position;
                                cardsPositionPointsList[j].cards.gameObject.transform.position = cardsPositionPointsList[i].cards.gameObject.transform.position;
                                cardsPositionPointsList[i].cards.gameObject.transform.position = tempPosition;
                                //spawing parents
                                Transform tempParent = cardsPositionPointsList[j].cards.gameObject.transform.parent;
                                cardsPositionPointsList[j].cards.gameObject.transform.parent = cardsPositionPointsList[i].cards.gameObject.transform.parent;
                                cardsPositionPointsList[i].cards.gameObject.transform.parent = tempParent;
                                //spawing CardsPositionPoints
                                CardsPositionPoints tempcardsPositionPoint = cardsPositionPointsList[j].cards.cardsPositionPoints;
                                cardsPositionPointsList[j].cards.cardsPositionPoints = cardsPositionPointsList[i].cards.cardsPositionPoints;
                                cardsPositionPointsList[i].cards.cardsPositionPoints = tempcardsPositionPoint;
                                //spawing cards
                                Cards tempCards = cardsPositionPointsList[i].cards;
                                cardsPositionPointsList[i].cards = cardsPositionPointsList[j].cards;
                                cardsPositionPointsList[j].cards = tempCards;
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
            for (int i = 0; i < cardsPositionPointsList.Count; i++)
            {
                if (checkStartIndex)
                {
                    if (cardsPositionPointsList[i].cards.cardName == cardname[a])
                    {
                        startIndex = i;
                        checkStartIndex = false;
                        checkEndIndex = true;
                    }
                }
                else if (checkEndIndex)
                {
                    if (cardsPositionPointsList[i].cards.cardName != cardname[a])
                    {
                        endIndex = i - 1;
                        checkEndIndex = false;
                    }
                }
                // we never get endindex bec there is no other name card left in the array to get endindex for spade
                if (i == (cardsPositionPointsList.Count - 1))
                {
                    if (checkEndIndex)
                    {//&& (cardname[a] == "club" || cardname[a] == "diamond" || cardname[a] == "heart" || cardname[a] == "spade")
                        endIndex = cardsPositionPointsList.Count - 1;
                    }
                }
            }

            indexCount = startIndex;

            for (int b = 0; b < cardnumber.Length; b++)
            {

                int cardNumber = cardnumber[b];

                for (int i = indexCount; i <= endIndex; i++)
                {

                    if (cardsPositionPointsList[i].cards.number == cardNumber)
                    {
                        indexCount++;
                    }
                    else
                    {
                        for (int j = i + 1; j <= endIndex; j++)
                        {
                            if (cardsPositionPointsList[i].cards.number != cardNumber)
                            {
                                if (cardsPositionPointsList[j].cards.number == cardNumber)
                                {
                                    //spawing position
                                    Vector3 tempPosition = cardsPositionPointsList[j].cards.gameObject.transform.position;
                                    cardsPositionPointsList[j].cards.gameObject.transform.position = cardsPositionPointsList[i].cards.gameObject.transform.position;
                                    cardsPositionPointsList[i].cards.gameObject.transform.position = tempPosition;
                                    //spawing parents
                                    Transform tempParent = cardsPositionPointsList[j].cards.gameObject.transform.parent;
                                    cardsPositionPointsList[j].cards.gameObject.transform.parent = cardsPositionPointsList[i].cards.gameObject.transform.parent;
                                    cardsPositionPointsList[i].cards.gameObject.transform.parent = tempParent;
                                    //spawing CardsPositionPoints
                                    CardsPositionPoints tempcardsPositionPoint = cardsPositionPointsList[j].cards.cardsPositionPoints;
                                    cardsPositionPointsList[j].cards.cardsPositionPoints = cardsPositionPointsList[i].cards.cardsPositionPoints;
                                    cardsPositionPointsList[i].cards.cardsPositionPoints = tempcardsPositionPoint;
                                    //spawing cards
                                    Cards tempCards = cardsPositionPointsList[i].cards;
                                    cardsPositionPointsList[i].cards = cardsPositionPointsList[j].cards;
                                    cardsPositionPointsList[j].cards = tempCards;

                                    indexCount++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void SetArrayForDefautCardPosition()
    {
        if (!isAiInitiator)
        {
            Vector3 m_cardPos = cardPosition;
            for (int i = 0; i < 15; i++)
            {
                PlayerCardsPosArray[i] = m_cardPos;
                m_cardPos += cardIncrement;              
            }
        }
    }

    void SetPositionOfCards()
    {
        for (int i = 0; i < cardsPositionPointsList.Count; i++)
        {            
            cardsPositionPointsList[i].transform.position = PlayerCardsPosArray[i];
        }
    }
}
