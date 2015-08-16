using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public TurnType turnType;

    public bool isMyTurn = false;
    public bool canTouch = true;

    public PackInitiator packInitiator;

    public CardMovemenManager cardMovementManager;

    public ArrangeDeckAction arrangcards;

    public List<DashBoardCardHolder> dashBoardCardHolder = new List<DashBoardCardHolder>();

    public Cards pickedDeckCards;
    public Cards placedDeckCards;

    public GUIStyle dialogStyle;
    public GUIStyle dialogButtonStyle;
    public GUIStyle orderbyArrangeStyle;
    public GUIStyle colorbyArrangeStyle;
    public GUIStyle changeTurnStyle;
    public GUIStyle revertStyle;
    public GUIStyle uiBgStyle;
    public GUIStyle rummyIconStyle;
    public GUIStyle settingButtonStyle;
    public GUIStyle tutorButtonStyle;
    public GUIStyle leaderBoardButtonStyle;
    public GUIStyle trophyButtonStyle;


    public bool firstCheckDone;

    public bool showFalseCombinationMenu;
    public bool showDialogBox;
    public string dialog = "";
    public bool showChangeTurn;

    void Start()
    {
        //CheckDashboard();
    }

    public void InitiatePack(List<GameObject> cards)
    {

        if (isMyTurn)
        {
            if (turnType == TurnType.none || turnType == TurnType.pick)
            {
                if (cards.Count > 0)
                {
                    packInitiator.InitiatePack(cards);
                    //					if(!firstCheckDone)
                    //						turnType = TurnType.through;
                    //					else{
                    //						turnType = TurnType.throughtorshow;
                    //					}
                    turnType = TurnType.throughtorshow;
                }
            }
            else
            {
                if (!showChangeTurn)
                {

                    dialog = "Cant pick card,\n" + turnType.ToString() + " one card to pick";
                }
                else
                    dialog = "Cant Pick card,\n" + " Press Change Turn Button";
                UIEventHandler.Ins.OpenMenu("PickCardNotification");
                UIEventHandler.Ins.pickNotificationText.text = dialog;
                //showDialogBox = true;
            }
        }
    }

    int PlayerCardsCount()
    {
        int count = packInitiator.cardsPositionPointsList.Count;
        for (int i = 0; i < dashBoardCardHolder.Count; i++)
        {
            count += dashBoardCardHolder[i].cardsPositionPoints.Count;
        }
        return count;
    }

    public void ChangeTurn()
    {
        if (showChangeTurn)
        {
            if (showFalseCombinationMenu)
            {
                print("jagoasdfa");
                UIEventHandler.Ins.changeTurnButton.animation.Stop();
                RevertFromWrongOrder();
            }
            else
            {
                if (packInitiator.cardsPositionPointsList.Count >= 15)
                {
                    dialog = "Your firs move must \n be a discard from your hand"; //"Cant Change Turn,\n" + "Through or Show card";
                    UIEventHandler.Ins.OpenMenu("PickCardNotification");
                    UIEventHandler.Ins.pickNotificationText.text = dialog;
                }
                else
                {

                    UIEventHandler.Ins.changeTurnButton.animation.Stop();
                    if (placedDeckCards && turnType == TurnType.show && placedDeckCards.gameObject != pickedDeckCards.gameObject)
                    {
                        if (dashBoardCardHolder.Count > 0)
                        {
                            placedDeckCards.cardType = CardType.hand;
                            packInitiator.PlaceDeckCardOnPlayerHands(placedDeckCards);
                            ThrownCardsDeck.Ins.RemoveCardFromThrownCards();
                            placedDeckCards = null;
                            GameManager.Ins.ChangeTurn();
                            showChangeTurn = false;
                        }
                        else
                        {
                            CheckDashboard();
                            showChangeTurn = false;
                        }
                    }
                    else
                    {
                        GameManager.Ins.ChangeTurn();
                        showChangeTurn = false;
                    }

                }
            }
        }
        else
        {
            if (!showChangeTurn)
            {
                if (turnType == TurnType.throughtorshow)
                {
                    dialog = "Cant Change Turn,\n" + "Through or Show card to pick";
                }
                else if (turnType == TurnType.pick)
                {
                    dialog = "Cant Change Turn,\n" + turnType.ToString() + " one card ";
                }
                else if (turnType == TurnType.show)
                {
                    dialog = "Cant Change Turn,\n" + "Through or Show one card ";
                }
            }
            else
                dialog = "Cant Pick card,\n" + " Press Change Turn Button";
            UIEventHandler.Ins.OpenMenu("PickCardNotification");
            UIEventHandler.Ins.pickNotificationText.text = dialog;
        }
    }

    public void RevertTurn()
    {
        if (turnType == TurnType.throughtorshow)
        {// && turnType != TurnType.show){
            if (showChangeTurn)
            {
                UIEventHandler.Ins.changeTurnButton.animation.Stop();
                showChangeTurn = false;
                if (placedDeckCards != null)
                {
                    placedDeckCards.cardType = CardType.hand;
                    packInitiator.PlaceDeckCardOnPlayerHands(placedDeckCards);
                    ThrownCardsDeck.Ins.RemoveCardFromThrownCards();
                    placedDeckCards = null;
                }
                if (dashBoardCardHolder.Count > 0)
                    RevertFromWrongOrder();
            }
        }
        else if (turnType == TurnType.show)
        {
            if (showChangeTurn)
            {
                UIEventHandler.Ins.changeTurnButton.animation.Stop();
                showChangeTurn = false;
                if (placedDeckCards)
                {
                    placedDeckCards.cardType = CardType.hand;
                    packInitiator.PlaceDeckCardOnPlayerHands(placedDeckCards);
                    ThrownCardsDeck.Ins.RemoveCardFromThrownCards();
                    placedDeckCards = null;
                }
                StartCoroutine(RevertCardsOnDashboard());
            }
        }
    }

    public void RevertFromWrongOrder()
    {
        showFalseCombinationMenu = false;
        StartCoroutine(RevertCardsOnDashboard());
        UIEventHandler.Ins.CloseMenu("WronOrderNotification");
    }

    public void DestroyDashboard(DashBoardCardHolder dbh)
    {

        int index = 0;
        bool flag = false;

        for (int i = 0; i < dashBoardCardHolder.Count; i++)
        {

            if (dashBoardCardHolder[i].gameObject == dbh.gameObject)
            {
                flag = true;
                index = i;
            }
        }

        if (flag)
        {

            Destroy(dashBoardCardHolder[index].gameObject);
            dashBoardCardHolder.RemoveAt(index);

        }
        else
        {
            print("dashboard not found ");
        }

    }

    IEnumerator RevertCardsOnDashboard()
    {

        if (placedDeckCards)
        {
            placedDeckCards.cardType = CardType.hand;
            packInitiator.PlaceDeckCardOnPlayerHands(placedDeckCards);
            ThrownCardsDeck.Ins.RemoveCardFromThrownCards();
            placedDeckCards = null;
        }

        for (int i = 0; i < dashBoardCardHolder.Count; i++)
        {
            // if this dashboard has unmatched cards than revert
            if (!dashBoardCardHolder[i].doNotCheckforsequence)
            {
                for (int j = 0; j < dashBoardCardHolder[i].cardsPositionPoints.Count; j++)
                {
                    if (dashBoardCardHolder[i].cardsPositionPoints[j].transform.childCount >= 1)
                    {
                        dashBoardCardHolder[i].cardsPositionPoints[j].cards.cardType = CardType.hand;
                        packInitiator.PlaceDeckCardOnPlayerHands(dashBoardCardHolder[i].cardsPositionPoints[j].cards);
                    }
                }
            }
        }
        for (int i = dashBoardCardHolder.Count - 1; i > -1; i--)
        {
            if (!dashBoardCardHolder[i].doNotCheckforsequence)
            {
                Destroy(dashBoardCardHolder[i].gameObject);
                dashBoardCardHolder.RemoveAt(i);
            }
        }
        yield return new WaitForSeconds(2.5f);
        if (cardMovementManager.tempCardHolder != null)
            if (pickedDeckCards != null)
                pickedDeckCards.StartAnimation(cardMovementManager.tempCardHolder);
            else
                print("tempCardHolder is null");
        yield return new WaitForSeconds(1f);
        canTouch = true;
        if (pickedDeckCards != null)
        {
            pickedDeckCards.cardType = CardType.deck;
            pickedDeckCards = null;
        }
        if (packInitiator.cardsPositionPointsList.Count >= 15)
            turnType = TurnType.throughtorshow;
        else if (packInitiator.cardsPositionPointsList.Count < 15)
            turnType = TurnType.pick;

    }

    public void CheckDashboard()
    {
        print("checking dashboard");
        int points = 0;
        bool isSequenceCorrect = true;
        bool lessThanThreePairCards = false;
        int lessThanThreePairCardsCount = 0;

        if (!firstCheckDone)
        {
            print("frist check");
            for (int i = 0; i < dashBoardCardHolder.Count; i++)
            {
                dashBoardCardHolder[i].doNotCheckforsequence = true;
                if (dashBoardCardHolder[i].cardsPositionPoints.Count >= 3)
                {
                    bool numberCheck = true;
                    bool sequenceCheck = true;

                    int number = dashBoardCardHolder[i].cardsPositionPoints[0].cards.number;
                    List<string> cardNames = new List<string>();
                    List<string> tempCardNames = new List<string>();
                    int tempPoints = number;

                    cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[0].cards.cardName);
                    for (int j = 1; j < dashBoardCardHolder[i].cardsPositionPoints.Count; j++)
                    {
                        if (dashBoardCardHolder[i].cardsPositionPoints[j].cards.number == number)
                        {
                            tempPoints += number;
                            cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[j].cards.cardName);
                        }
                        else
                        {
                            print("no number match");
                            numberCheck = false;
                            isSequenceCorrect = false;
                            dashBoardCardHolder[i].doNotCheckforsequence = false;
                            break;
                        }
                    }

                    for (int a = 0; a < cardNames.Count; a++)
                    {
                        if (!ExistInNameList(tempCardNames, cardNames[a]))
                        {
                            tempCardNames.Add(cardNames[a]);
                        }
                        else
                        {
                            print("same color card twice");
                            numberCheck = false;
                            isSequenceCorrect = false;
                            dashBoardCardHolder[i].doNotCheckforsequence = false;
                            break;
                        }
                    }

                    if (numberCheck)
                    {
                        points += tempPoints;
                        print("number match found points = " + points);
                    }
                    else
                    {

                        isSequenceCorrect = true;
                        dashBoardCardHolder[i].doNotCheckforsequence = true;
                        ArrangeCardsByNumber();

                        number = dashBoardCardHolder[i].cardsPositionPoints[0].cards.number;
                        cardNames.Clear();
                        cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[0].cards.cardName);
                        for (int j = 1; j < dashBoardCardHolder[i].cardsPositionPoints.Count; j++)
                        {
                            if ((dashBoardCardHolder[i].cardsPositionPoints[j].cards.number - number) == 1)
                            {
                                number = dashBoardCardHolder[i].cardsPositionPoints[j].cards.number;
                                tempPoints += number;
                                cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[j].cards.cardName);
                            }
                            else
                            {
                                print("no color match");
                                sequenceCheck = false;
                                isSequenceCorrect = false;
                                dashBoardCardHolder[i].doNotCheckforsequence = false;
                                break;
                            }
                        }

                        if (!AreAllElementsSame(cardNames, cardNames[0]))
                        {
                            print("different color card found , no sequence match");
                            sequenceCheck = false;
                            isSequenceCorrect = false;
                            dashBoardCardHolder[i].doNotCheckforsequence = false;
                        }

                        if (sequenceCheck)
                        {
                            points += tempPoints;
                            print("sequence match found points = " + points);
                        }
                    }
                }
                else if (dashBoardCardHolder[i].cardsPositionPoints.Count < 3)
                {
                    print("count less than 3 in i = " + i);
                    lessThanThreePairCards = true;
                    lessThanThreePairCardsCount++;
                    //if(lessThanThreePairCardsCount > 2 || packInitiator.cardsPositionPoints.Count > 0)
                    //dashBoardCardHolder[i].doNotCheckforsequence = false;
                }
            }

            if (points >= 51 && isSequenceCorrect)
            {
                if (lessThanThreePairCards && lessThanThreePairCardsCount <= 2)
                {

                    if (packInitiator.cardsPositionPointsList.Count <= 2)
                    {
                        print("count less than 3 packInitiator.cardsPositionPoints.Count <= 0");
                        turnType = TurnType.none;
                        UIEventHandler.Ins.OpenMenu("GameOverNotification");
                        GameManager.Ins.showGameFinish = true;
                    }
                    else
                    {
                        print("count less than 3 packInitiator.cardsPositionPoints.Count > 0");
                        turnType = TurnType.none;
                        GameManager.Ins.ChangeTurn();
                        firstCheckDone = true;
                    }
                }
                else
                {

                    if (lessThanThreePairCardsCount > 2)
                    {
                        print("lessThanThreePairCardsCount > 2");
                        //dashBoardCardHolder[i].doNotCheckforsequence = false;
                        canTouch = false;
                        showFalseCombinationMenu = true;
                        UIEventHandler.Ins.OpenMenu("WronOrderNotification");
                        UIEventHandler.Ins.arrangementIssueText.text = "You should arrange cards properly, and atleast have 51 points";
                    }
                    else
                    {
                        if (packInitiator.cardsPositionPointsList.Count <= 2)
                        {
                            print("gameover");
                            UIEventHandler.Ins.OpenMenu("GameOverNotification");
                            //dashBoardCardHolder[i].doNotCheckforsequence = true;
                            turnType = TurnType.none;
                            GameManager.Ins.ChangeTurn();
                            firstCheckDone = true;
                        }
                        else
                        {
                            print("count less than 3 packInitiator.cardsPositionPoints.Count > 0");
                            turnType = TurnType.none;
                            GameManager.Ins.ChangeTurn();
                            firstCheckDone = true;
                        }

                    }
                }
            }
            else
            {
                //dashBoardCardHolder[i].doNotCheckforsequence = false;
                for (int i = 0; i < dashBoardCardHolder.Count; i++)
                    dashBoardCardHolder[i].doNotCheckforsequence = false;
                canTouch = false;
                showFalseCombinationMenu = true;
                UIEventHandler.Ins.OpenMenu("WronOrderNotification");
                UIEventHandler.Ins.arrangementIssueText.text = "You should arrange cards properly, and atleast have 51 points";
            }
        }
        else
        {

            for (int i = 0; i < dashBoardCardHolder.Count; i++)
            {
                dashBoardCardHolder[i].doNotCheckforsequence = true;
                if (dashBoardCardHolder[i].cardsPositionPoints.Count >= 3)
                {
                    bool numberCheck = true;
                    bool sequenceCheck = true;

                    int number = dashBoardCardHolder[i].cardsPositionPoints[0].cards.number;
                    List<string> cardNames = new List<string>();
                    List<string> tempCardNames = new List<string>();


                    cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[0].cards.cardName);
                    for (int j = 1; j < dashBoardCardHolder[i].cardsPositionPoints.Count; j++)
                    {
                        if (dashBoardCardHolder[i].cardsPositionPoints[j].cards.number == number)
                        {

                            cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[j].cards.cardName);
                        }
                        else
                        {
                            //print ("no number match");
                            numberCheck = false;
                            isSequenceCorrect = false;
                            dashBoardCardHolder[i].doNotCheckforsequence = false;
                            break;
                        }
                    }

                    for (int a = 0; a < cardNames.Count; a++)
                    {
                        if (!ExistInNameList(tempCardNames, cardNames[a]))
                        {
                            tempCardNames.Add(cardNames[a]);
                        }
                        else
                        {
                            //print("same color card twice");
                            numberCheck = false;
                            isSequenceCorrect = false;
                            dashBoardCardHolder[i].doNotCheckforsequence = false;
                            break;
                        }
                    }

                    if (numberCheck)
                    {

                        //print ("number match found points = "+points);
                    }
                    else
                    {

                        isSequenceCorrect = true;
                        dashBoardCardHolder[i].doNotCheckforsequence = true;
                        ArrangeCardsByNumber();

                        number = dashBoardCardHolder[i].cardsPositionPoints[0].cards.number;
                        cardNames.Clear();
                        cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[0].cards.cardName);
                        for (int j = 1; j < dashBoardCardHolder[i].cardsPositionPoints.Count; j++)
                        {
                            if ((dashBoardCardHolder[i].cardsPositionPoints[j].cards.number - number) == 1)
                            {
                                number = dashBoardCardHolder[i].cardsPositionPoints[j].cards.number;
                                cardNames.Add(dashBoardCardHolder[i].cardsPositionPoints[j].cards.cardName);
                            }
                            else
                            {
                                //print ("no color match");
                                sequenceCheck = false;
                                isSequenceCorrect = false;
                                dashBoardCardHolder[i].doNotCheckforsequence = false;
                                break;
                            }
                        }

                        if (!AreAllElementsSame(cardNames, cardNames[0]))
                        {
                            //print("different color card found , no sequence match");
                            sequenceCheck = false;
                            isSequenceCorrect = false;
                            dashBoardCardHolder[i].doNotCheckforsequence = false;
                        }

                        if (sequenceCheck)
                        {

                            //print ("sequence match found points = "+points);
                        }
                    }
                }
                else if (dashBoardCardHolder[i].cardsPositionPoints.Count < 3)
                {
                    print("count less than 3 in i = " + i);
                    //showFalseCombinationMenu = true;
                    lessThanThreePairCards = true;
                    lessThanThreePairCardsCount++;
                    if (lessThanThreePairCardsCount > 2)
                        dashBoardCardHolder[i].doNotCheckforsequence = false;
                }
            }

            if (isSequenceCorrect)
            {
                //print ("seq correct ");
                if (lessThanThreePairCards && lessThanThreePairCardsCount <= 2)
                {
                    //dashBoardCardHolder[i].doNotCheckforsequence = true;
                    if (packInitiator.cardsPositionPointsList.Count <= 0)
                    {
                        //print ("count less than 3 packInitiator.cardsPositionPoints.Count <= 0");
                        turnType = TurnType.none;
                        //GameManager.Ins.showGameFinish = true;
                        UIEventHandler.Ins.OpenMenu("GameOverNotification");
                        GameManager.Ins.ShowGameOverGUI();
                    }
                    else
                    {
                        //print ("count less than 3 packInitiator.cardsPositionPoints.Count >= 0");
                        if (packInitiator.cardsPositionPointsList.Count <= 2)
                        {
                            UIEventHandler.Ins.OpenMenu("GameOverNotification");
                            turnType = TurnType.none;
                            GameManager.Ins.ChangeTurn();
                        }
                        else
                        {
                            print("count less than 3 packInitiator.cardsPositionPoints.Count > 0");
                            turnType = TurnType.none;
                            GameManager.Ins.ChangeTurn();
                            firstCheckDone = true;
                        }
                    }
                }
                else
                {

                    if (lessThanThreePairCardsCount > 2)
                    {
                        print("greaterThanThreePairCardsCount > 2");
                        //dashBoardCardHolder[i].doNotCheckforsequence = false;
                        canTouch = false;
                        showFalseCombinationMenu = true;
                        UIEventHandler.Ins.OpenMenu("WronOrderNotification");
                        UIEventHandler.Ins.arrangementIssueText.text = "You should arrange cards properly";
                    }
                    else
                    {
                        if (packInitiator.cardsPositionPointsList.Count <= 2)
                        {
                            print("gameover");
                            UIEventHandler.Ins.OpenMenu("GameOverNotification");
                            //dashBoardCardHolder[i].doNotCheckforsequence = true;
                            turnType = TurnType.none;
                            GameManager.Ins.ChangeTurn();
                            firstCheckDone = true;
                        }
                        else
                        {
                            print("count less than 3 packInitiator.cardsPositionPoints.Count > 0");
                            turnType = TurnType.none;
                            GameManager.Ins.ChangeTurn();
                            firstCheckDone = true;
                        }
                    }
                }
            }
            else
            {
                //dashBoardCardHolder[i].doNotCheckforsequence = false;
                canTouch = false;
                showFalseCombinationMenu = true;
                UIEventHandler.Ins.OpenMenu("WronOrderNotification");
                UIEventHandler.Ins.arrangementIssueText.text = "You should arrange cards properly";
            }
        }
    }

    void ArrangeCardsByNumber()
    {
        int indexCount = 0;
        int[] cardnumber = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        for (int i = 0; i < dashBoardCardHolder.Count; i++)
        {

            for (int a = 0; a < cardnumber.Length; a++)
            {

                int cardNumber = cardnumber[a];

                for (int x = indexCount; x < dashBoardCardHolder[i].cardsPositionPoints.Count; x++)
                {

                    if (dashBoardCardHolder[i].cardsPositionPoints[x].cards.number == cardNumber)
                    {
                        indexCount++;
                    }
                    else
                    {
                        for (int y = x + 1; y < dashBoardCardHolder[i].cardsPositionPoints.Count; y++)
                        {
                            if (dashBoardCardHolder[i].cardsPositionPoints[y].cards.number == cardNumber)
                            {
                                //spawing position
                                Vector3 tempPosition = dashBoardCardHolder[i].cardsPositionPoints[y].cards.gameObject.transform.position;
                                dashBoardCardHolder[i].cardsPositionPoints[y].cards.gameObject.transform.position = dashBoardCardHolder[i].cardsPositionPoints[x].cards.gameObject.transform.position;
                                dashBoardCardHolder[i].cardsPositionPoints[x].cards.gameObject.transform.position = tempPosition;
                                //spawing parents
                                Transform tempParent = dashBoardCardHolder[i].cardsPositionPoints[y].cards.gameObject.transform.parent;
                                dashBoardCardHolder[i].cardsPositionPoints[y].cards.gameObject.transform.parent = dashBoardCardHolder[i].cardsPositionPoints[x].cards.gameObject.transform.parent;
                                dashBoardCardHolder[i].cardsPositionPoints[x].cards.gameObject.transform.parent = tempParent;
                                //spawing CardsPositionPoints
                                CardsPositionPoints tempcardsPositionPoint = dashBoardCardHolder[i].cardsPositionPoints[y].cards.cardsPositionPoints;
                                dashBoardCardHolder[i].cardsPositionPoints[y].cards.cardsPositionPoints = dashBoardCardHolder[i].cardsPositionPoints[x].cards.cardsPositionPoints;
                                dashBoardCardHolder[i].cardsPositionPoints[x].cards.cardsPositionPoints = tempcardsPositionPoint;
                                //spawing cards
                                Cards tempCards = dashBoardCardHolder[i].cardsPositionPoints[x].cards;
                                dashBoardCardHolder[i].cardsPositionPoints[x].cards = dashBoardCardHolder[i].cardsPositionPoints[y].cards;
                                dashBoardCardHolder[i].cardsPositionPoints[y].cards = tempCards;

                                indexCount++;
                            }
                        }
                    }
                }
            }
        }
    }

    private bool ExistInNameList(List<string> list, string cardName)
    {
        bool flag = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (cardName == list[i])
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    private bool AreAllElementsSame(List<string> list, string cardName)
    {
        bool flag = true;
        for (int i = 0; i < list.Count; i++)
        {
            if (cardName != list[i])
            {
                flag = false;
                break;
            }
        }
        return flag;
    }
}
