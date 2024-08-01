using FishNet;
using FishNet.Broadcast;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Observing;
using IO.Swagger.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;
using UnityEngine.UI;
using static GameServerManager;


public class BlackJackDisplayer : NetworkBehaviour
{
    [SerializeField]
    public Transform[] _playerSpots = new Transform[5];///
    public TMP_Text cardsText;
    public Button hitButton;
    public Button checkButton;
    public TMP_Text winText;
    public Button newRoundButton;
    public Transform cardParent;

    private bool dealerChecked = false;
    private List<GameObject> spawnedCards = new();
    private List<GameObject> spawnedPlayers = new();
    private bool dealerRevealAllCards = false;

    //////////////////////////////////
    private void Start()
    {
        GameServerManager.OnInitialized += OnBlackJackServerStarted;
        if (GameServerManager.IsInitialized())
        {
            OnBlackJackServerStarted();
        }
    }

    private void OnBlackJackServerStarted()
    {
        DisplayPlayer();
    }

    public void DisplayPlayer()
    {
        if (base.Owner.IsValid && GameServerManager.IsInitialized())
        {
            if (InstanceFinder.IsServer)
            {
                int amountOfActivePlayers = GameServerManager.GetAmoutOfActivePlayers();
                DisplayPlayerCharacterForServer(amountOfActivePlayers);
                //List<string> cards = GetAllPlayerHands(base.Owner);
                //cardsText.text = "Players cards:\n" + string.Join("\n", cards);
                //DisplayCardsServer(cards);
            }
            else if (base.Owner.IsLocalClient)
            {
                int playerIndex = GameServerManager.GetPlayerIndex(base.Owner);
                DisplayPlayerCharacterForClient(playerIndex);
                //string cards = GameServerManager.GetPlayerHand(base.Owner);
                //   int playerIndex = GameServerManager.GetPlayerIndex(base.Owner);
                //DisplayCardsOnBoard(cards);
            }
        }
    }
    /// <summary>
    /// This function need to be fixed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>
    /// <param name="amountOfActivePlayers"></param>
    void DisplayPlayerCharacterForServer(int amountOfActivePlayers)
    {
        string playerDir = "Player/PlayerInstruments";

        for(int i = 0; i < amountOfActivePlayers; i++)
        {
            GameObject instantiatedPlayer = Instantiate(Resources.Load<GameObject>(playerDir), _playerSpots[i]);
            if (instantiatedPlayer == null)
            {
                Debug.LogWarning("No player object found in Resources");
                return;
            }
            //   ServerManager.Spawn(instantiatedCard);
            spawnedPlayers.Add(instantiatedPlayer);
            ServerManager.Spawn(instantiatedPlayer);

        }
        /*
        float cardSpacing = 300f;

        string[] dealerCardsNames = cards[0].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < dealerCardsNames.Length; i++)
        {
            string cardName = dealerCardsNames[i].Trim();

            string cardDir = "Cards/" + cardName;
            GameObject instantiatedCard = Instantiate(Resources.Load<GameObject>(cardDir), cardParent);
            instantiatedCard.transform.localScale = new Vector3(2000f, 1900f, 1f);
            instantiatedCard.transform.rotation = Quaternion.identity;
            instantiatedCard.transform.localPosition = new Vector3((i * cardSpacing) + 537, 288, 15);
            instantiatedCard.transform.rotation = Quaternion.Euler(0f, 181f, 0f);
            if (instantiatedCard == null)
            {
                Debug.LogWarning("No card object found in Resources");
                return;
            }
            //   ServerManager.Spawn(instantiatedCard);
            spawnedCards.Add(instantiatedCard);

            if (!dealerRevealAllCards)
                break;
        }
        */
    }

    void DisplayPlayerCharacterForClient(int playerIndex)
    {
        //string playerDir = "Player/PlayerInstruments";
        //string playerCameraDir = "PlayerPlayerViewCamera";
        string playerDir = "Player/PlayerWithCamera";


        //GameObject instantiatedPlayer = Instantiate(Resources.Load<GameObject>(playerDir), _playerSpots[playerIndex]);
        //GameObject instatiatedPlayerCamera = Instantiate(Resources.Load<GameObject>(playerCameraDir), _playerSpots[playerIndex]);
        GameObject instantiatedPlayer = Instantiate(Resources.Load<GameObject>(playerDir), _playerSpots[playerIndex]);

        /*
        if (instantiatedPlayer == null)
        {
            Debug.LogWarning("No player object found in Resources");
            return;
        }

        if (instatiatedPlayerCamera == null)
        {
            Debug.LogWarning("No player camera object found in Resources");
            return;
        }
        ServerManager.Spawn(instantiatedPlayer);
        ServerManager.Spawn(instatiatedPlayerCamera);
        spawnedPlayers.Add(instantiatedPlayer);
        spawnedPlayers.Add(instatiatedPlayerCamera);
        */
        if (instantiatedPlayer == null)
        {
            Debug.LogWarning("No player object found in Resources");
            return;
        }
        ServerManager.Spawn(instantiatedPlayer);
        spawnedPlayers.Add(instantiatedPlayer);

    }

    //public void InitGame()
    //{
    //DisplayPlayer();
    //  StartCoroutine(StartRoundInDelay());
    /*
    InstanceFinder.ClientManager.RegisterBroadcast<TurnPassBroadcast>(OnTurnPassBroadcast);
    InstanceFinder.ClientManager.RegisterBroadcast<UpdateBroadcast>(OnUpdateFromServer);
    InstanceFinder.ClientManager.RegisterBroadcast<ClientMsgBroadcast>(OnClientMsgBroadcast);
    */
    //}

    //////////////////////////////////


    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<TurnPassBroadcast>(OnTurnPassBroadcast);
        InstanceFinder.ClientManager.RegisterBroadcast<UpdateBroadcast>(OnUpdateFromServer);
        InstanceFinder.ClientManager.RegisterBroadcast<ClientMsgBroadcast>(OnClientMsgBroadcast);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<TurnPassBroadcast>(OnTurnPassBroadcast);
        InstanceFinder.ClientManager.UnregisterBroadcast<UpdateBroadcast>(OnUpdateFromServer);
        InstanceFinder.ClientManager.UnregisterBroadcast<ClientMsgBroadcast>(OnClientMsgBroadcast);
    }

    private void OnClientMsgBroadcast(ClientMsgBroadcast msg)
    {
        if (msg.IsWinMessage)
        {
            if (!InstanceFinder.IsServer && base.Owner.IsLocalClient)
            {
                ShowWinMessage();
                handleClientTurn();
                StartCoroutine(FetchCoinsInDelay());
            }
        }
        if (msg.IsNewRoundMessage)
        {
            if (!InstanceFinder.IsServer)
            {
                winText.text = "";
                handleClientTurn();
                StartCoroutine(ClientTurnInDelay());
            }
            else
            {
                dealerRevealAllCards = false;
            }

            DespawnAllCards();
            UpdateCardsDisplay();
        }
    }

    private IEnumerator FetchCoinsInDelay()
    {
        yield return new WaitForSeconds(0.7f);
        LoggedUser.FetchCoins();
    }

    private void OnTurnPassBroadcast(TurnPassBroadcast msg)
    {
        if (!InstanceFinder.IsServer && base.Owner.IsLocalClient)
        {
            handleClientTurn();
            StartCoroutine(ClientTurnInDelay());
        }
        else if (msg.HostTurn && base.Owner.IsHost && InstanceFinder.IsServer)
        {
            Debug.LogWarning("Dealers turn");
            handleDealerTurn();
        }
    }

    private IEnumerator ClientTurnInDelay()
    {
        yield return new WaitForSeconds(0.8f);
        handleClientTurn();
        yield return new WaitForSeconds(1.8f);
        handleClientTurn();
    }

    private void OnUpdateFromServer(UpdateBroadcast msg)
    {
        if (msg.NewRound && InstanceFinder.IsServer)
        {
            handleDealerTurn();
            ClientMsgBroadcast msgForClients = new()
            {
                IsNewRoundMessage = true,
                IsWinMessage = false
            };
            InstanceFinder.ServerManager.Broadcast(msgForClients);
            UpdateCardsDisplay();
        }
        else if (msg.UpdateCards && !InstanceFinder.IsServer && base.Owner.IsLocalClient)
        {
            handleClientTurn();
            StartCoroutine(ClientTurnInDelay());
        }
        else if (msg.UpdateCards) // only server reaches this part
        {
            handleDealerTurn();
            UpdateCardsDisplay();
        }
    }

    public void NewRound_OnClick()
    {
        dealerChecked = false;
        NewRoundInit();
        newRoundButton.gameObject.SetActive(false);
    }

    public void Check_OnClick()
    {
        ClientCheck();
    }

    public void Hit_OnClick()
    {
        HitCard();
    }

    private async void ShowWinMessage()
    {
        if (InstanceFinder.IsServer || !base.Owner.IsLocalClient)
        {
            winText.text = "";
            return;
        }

        GameResult result = await DidIWin(base.Owner, LoggedUser.Username);

        switch (result)
        {
            case GameResult.Win:
                winText.text = "You win!";
                break;
            case GameResult.Lose:
                winText.text = "You lost...";
                break;
            case GameResult.Tie:
                winText.text = "Tie!";
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Just in case theres a non update bug
        {
            if (InstanceFinder.IsServer)
                handleDealerTurn();
            else if (base.Owner.IsLocalClient)
                handleClientTurn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ShowWinMessage();
        }
    }

    private void handleDealerTurn()
    {
        if (IsMyTurn(base.Owner) && InstanceFinder.IsServer)
        {
            if (!dealerChecked)
            {
                StartCoroutine(StartRoundInDelay());
                return;
            }
            int cardsValue = Deck.GetHandValue(GetAllPlayerHands(Owner)[0]);
            dealerRevealAllCards = true;
            if (cardsValue < 17)
            {
                HitCard();
            }
            else
            {
                Debug.LogWarning("dealers card value is " + cardsValue);
                ClientMsgBroadcast msg = new()
                {
                    IsNewRoundMessage = false,
                    IsWinMessage = true
                };
                InstanceFinder.ServerManager.Broadcast(msg);

                newRoundButton.gameObject.SetActive(true);
                Debug.Log("Round finished. showing results");
            }
        }
    }

    private IEnumerator StartRoundInDelay()
    {
        Debug.LogWarning("Server checked");
        dealerChecked = true;
        yield return new WaitForSeconds(0.2f); // Wait for client to get up before broadcasting it. 
        ClientCheck();
    }

    private void handleClientTurn()
    {
        UpdateCardsDisplay();
        if (IsMyTurn(base.Owner))
        {
            hitButton.gameObject.SetActive(true);
            checkButton.gameObject.SetActive(true);
        }
        else
        {
            hitButton.gameObject.SetActive(false);
            checkButton.gameObject.SetActive(false);
        }
    }

    private void UpdateCardsDisplay()
    {
        if (base.Owner.IsValid && GameServerManager.IsInitialized())
        {
            if (InstanceFinder.IsServer && !base.Owner.IsLocalClient)
            {
                bool isDealersTurn = IsMyTurn(base.Owner);

                string turnOf = isDealersTurn ? "Dealer's" : "Client's";
                List<string> cards = GetAllPlayerHands(base.Owner);
                cardsText.text = "Players cards:\n" + string.Join("\n", cards) +
                    "\nAnd it's " + turnOf + " turn";

                DisplayCardsServer(cards);
            }
            else if (base.Owner.IsLocalClient)
            {
                string cards = GameServerManager.GetPlayerHand(base.Owner);
                //   int playerIndex = GameServerManager.GetPlayerIndex(base.Owner);
                DisplayCardsOnBoard(cards);
                cardsText.text = "\nIt's " + (IsMyTurn(base.Owner) ? "Clinet's" : "Dealer's") + " turn";
            }
        }
        else
        {
            Debug.LogWarning("Cant update, no valid owner");
        }
    }

    void DisplayCardsServer(List<string> cards)
    {
        float cardSpacing = 300f;

        string[] dealerCardsNames = cards[0].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < dealerCardsNames.Length; i++)
        {
            string cardName = dealerCardsNames[i].Trim();

            string cardDir = "Cards/" + cardName;
            GameObject instantiatedCard = Instantiate(Resources.Load<GameObject>(cardDir), cardParent);
            instantiatedCard.transform.localScale = new Vector3(2000f, 1900f, 1f);
            instantiatedCard.transform.rotation = Quaternion.identity;
            instantiatedCard.transform.localPosition = new Vector3((i * cardSpacing) + 537, 288, 15);
            instantiatedCard.transform.rotation = Quaternion.Euler(0f, 181f, 0f);
            if (instantiatedCard == null)
            {
                Debug.LogWarning("No card object found in Resources");
                return;
            }
            //   ServerManager.Spawn(instantiatedCard);
            spawnedCards.Add(instantiatedCard);

            if (!dealerRevealAllCards)
                break;
        }
    }

    void DisplayCardsOnBoard(string cards)
    {
        float card2Spacing_X = 0.07f;
        float card2Spacing_Y = 0.03f;
        float card2Spacing_Z = -0.1f;
        float cardSpacing_X = -0.03f;
        float cardSpacing_Y = 0.02f;
        float cardSpacing_Z = -0.1f;


        string[] cardNames = cards.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < cardNames.Length; i++)
        {
            // Trim any extra whitespace from the card name
            string cardName = cardNames[i].Trim();

            // Load the card prefab from the Resources/Cards folder
            string cardDir = "Cards/" + cardName;
            GameObject instantiatedCard = Instantiate(Resources.Load<GameObject>(cardDir), cardParent);
            instantiatedCard.transform.localScale = new Vector3(2.2816f, 2.2816f, 2.2816f);
            instantiatedCard.transform.rotation = Quaternion.identity;
            if (i < 2)
            {
                instantiatedCard.transform.localPosition = new Vector3((i * card2Spacing_X) - 4.05f, (i * card2Spacing_Y) + 2.48f, (i * card2Spacing_Z) + 31.42f);
            }
            else
            {
                instantiatedCard.transform.localPosition = new Vector3(((i - 2) * cardSpacing_X) -3.47f, ((i - 2) * cardSpacing_Y) + 2.48f, ((i - 2) * cardSpacing_Z) + 31.45f);
            }
            instantiatedCard.transform.rotation = Quaternion.Euler(270f, 0f, -69.864f);
            if (instantiatedCard == null)
            {
                Debug.LogWarning("No card object found in Resources");
                return;
            }
            
            ServerManager.Spawn(instantiatedCard);
            spawnedCards.Add(instantiatedCard);
        }
    }

    private void DespawnAllCards()
    {
        foreach (GameObject cardObject in spawnedCards)
        {
            //   ServerManager.Despawn(cardObject);
            Destroy(cardObject);
        }
        Debug.Log("Despawning all cards");
        spawnedCards.Clear();
    }

    public struct ClientMsgBroadcast : IBroadcast
    {
        public bool IsWinMessage;
        public bool IsNewRoundMessage;
    }
    
}
