using UnityEngine;

public class PlayerDisplayer : MonoBehaviour
{ 
    public static void SetCameraBlackJack(int playerIndex)
    {
        GameObject instantiatedPlayer = Instantiate(Resources.Load<GameObject>("Players/PlayerWithCamera"));
        instantiatedPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
        instantiatedPlayer.transform.rotation = Quaternion.identity;
        if (playerIndex == 1)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(0, 0, 0);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Debug.LogWarning("Displaying 1st player's camera");
        }
        else if (playerIndex == 2)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(27f, 0, 22.31f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -69.77f, 0f);
            Debug.LogWarning("Displaying 2nd player's camera");

        }
        else if (playerIndex == 3)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(29.2486f, 0, 40.15456f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -103.669f, 0f);
        }
        else if (playerIndex == 4)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(18.38f, 0, 57.11f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -144.912f, 0f);
        }
        else if (playerIndex == 5)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(-18.76f, 0, 52.46f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, 140.332f, 0f);
        }
        if (instantiatedPlayer == null)
        {
            Debug.LogWarning("No player object found in Resources");
            return;
        }
    }

    public static void SetCameraPoker(int playerIndex)
    {
        GameObject instantiatedPlayer = Instantiate(Resources.Load<GameObject>("Players/PlayerWithCamera"));
        instantiatedPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
        instantiatedPlayer.transform.rotation = Quaternion.identity;
        if (playerIndex == 1)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(-1.11f, 0f, -0.09f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Debug.LogWarning("Displaying 1st player's camera");
        }
        else if (playerIndex == 2)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(26.77f, 0f, 24.1f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -74.1f, 0f);
            Debug.LogWarning("Displaying 2nd player's camera");

        }
        else if (playerIndex == 3)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(28.49612f, 0f, 41.40894f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -106.946f, 0f);
        }
        else if (playerIndex == 4)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(26.65f, 0f, 50.72f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -127.374f, 0f);
        }
        else if (playerIndex == 5)
        {
            instantiatedPlayer.transform.localPosition = new Vector3(-14.44f, 0f, 55.26f);
            instantiatedPlayer.transform.rotation = Quaternion.Euler(0f, -211.864f, 0f);
        }
        if (instantiatedPlayer == null)
        {
            Debug.LogWarning("No player object found in Resources");
            return;
        }
    }
    // private bool isPlayerDisplayed = false;
    //private static int playerIndex;
    // Start is called before the first frame update
    /*void Start()
    {
        GameServerManager.OnInitialized += OnGameServerSttarted;
        if (GameServerManager.IsInitialized())
        {
            OnGameServerSttarted();
        }
    }

    public void OnGameServerSttarted()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        playerIndex = GameServerManager.GetIndexForCamera(); //Starting from 1
       // if(base.Owner.IsLocalClient)
        {
            Debug.LogWarning("spawn camera position for index "+playerIndex);
            SpawnPlayerToScene();
        }
     //   playerIndex = GameServerManager.GetPlayerIndex(base.Owner);
     //   DisplayPlayer2();
    }*/


    /*
    public void DisplayPlayer(ClientIndexSetBroadcast msg)
    {
        if (msg.StartDisplay && msg.client == base.Owner)
        {
            if (!InstanceFinder.IsServer && base.Owner.IsLocalClient)
            {
                handleClientDisplay(msg.playerIndex);
                StartCoroutine(UpdatePlayersInDelay());
            }
        }
    }
    */

    /*    public void DisplayPlayer2()
        {
            if (!InstanceFinder.IsServer && base.Owner.IsLocalClient)
            {
                handleClientDisplay(playerIndex);
                StartCoroutine(UpdatePlayersInDelay());
            }
        }

        private void handleClientDisplay(int playerIndexSetFromServer)
        {
            Debug.LogWarning($"player index = {playerIndexSetFromServer}");
            playerIndex = playerIndexSetFromServer;
            Debug.LogWarning($"player index after init = {playerIndex}");
            updatePlayerDisplayForClient();
        }

        private void updatePlayerDisplayForClient()
        {
            if (base.Owner.IsValid && GameServerManager.IsInitialized())
            {
                if (base.Owner.IsLocalClient && !isPlayerDisplayed)
                {
                    //int playerIndex = GameServerManager.GetPlayerIndex(base.Owner);
                    //SpawnPlayerToScene(playerIndex);
                    SpawnPlayerToScene();
                    isPlayerDisplayed = true;

                }
            }
        }*/

    //void SpawnPlayerToScene(int playerIndex)

    /*private IEnumerator UpdatePlayersInDelay()
    {
        yield return new WaitForSeconds(0.2f);
        updatePlayerDisplayForClient();
        yield return new WaitForSeconds(1f);
        updatePlayerDisplayForClient();

        Debug.LogWarning("Updating players in delay");
    }*/
}