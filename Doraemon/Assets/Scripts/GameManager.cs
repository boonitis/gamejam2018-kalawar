using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Reference to all game objects
    public GameObject playButtonGO; //UI Playbutton
    public GameObject gameOverGO; //UI Game Over
    public GameObject scoreTextGO; //UI Text Score
    public GameObject playerCharacter;
    public GameObject enemySpawner;

    //Create state of Game Manager
    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameManagerState GMState;

    // Use this for initialization
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    // Update is called once per frame
    void UpdateGameManagerState()
    {

        switch (GMState)
        {
            case GameManagerState.Opening:

                //Hide game over
                gameOverGO.SetActive(false);

                //Set play button on game play sate
                playButtonGO.SetActive(true);


                break;
            case GameManagerState.Gameplay:

                //Reset the score
                scoreTextGO.GetComponent<GameScore>().Score = 0;

                //hide play button on game play state
                playButtonGO.SetActive(false);

                //set the player active and init the player Lives
                playerCharacter.GetComponent<PlayerControl>().Init();

                //Start every enemy spawner script in EnemySpawnerGO
                foreach(EnemySpawner script in enemySpawner.GetComponents<EnemySpawner>())
                {
                    script.ScheduleEnemySpawner();
                }


                break;
            case GameManagerState.GameOver:

                //Display game over
                gameOverGO.SetActive(true);

                //Stop every enemy spawner script in EnemySpawnerGO
                foreach (EnemySpawner script in enemySpawner.GetComponents<EnemySpawner>())
                {
                    script.UnscheduleEnemySpawner();
                }

                //Change to Opening State
                Invoke("ChangeToOpeningState", 8f);

                break;

        }

    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    public void ChangeToOpeningState()
    {
        //Change game manager to Opening state
        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
        //Destroy left over enemy in the scene
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyCharacterTag")) {
            enemy.GetComponent<EnemyControl>().PlayExplosion();
            Destroy(enemy);
        }
        //Destroy left over collectable in the scence
        foreach (GameObject collectable in GameObject.FindGameObjectsWithTag("CollectableTag"))
        {
            Destroy(collectable);
        }
    }
}
