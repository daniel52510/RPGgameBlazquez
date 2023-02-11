using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
//Daniel Blazquez
public enum gameStates { START,PLAYERTURN,ENEMYTURN,WIN,LOSE }
public class gameController : MonoBehaviour
{
    //These are the variables that are needed in initializing the game...
    Unit playerUnit;
    Unit enemyUnit;
    public int onHitRounds;
    //Gems are reset in the startBattle function...
    public static int gems = 0;
    public Button ultButton;
    public Button secondButton;
    public Button potionButton;
    public AudioSource source;
    public AudioSource enemySound;
    public Image background;
    //background images change dependent on the stage of the game...
    public Sprite backgroundImage1;
    public Sprite backgroundImage2;
    public Text dialogueText;
    public Text gemText;
    public Text potionText;
    //below are the GameObjects that are created and Instantiated depending on the knight chosen...
    public GameObject mage;
    public GameObject paladin;
    public GameObject berserker;
    private GameObject enemyObject;
    private GameObject playerChosen;
    public GameObject slime;
    public GameObject snake;
    public GameObject bossBat;
    public GameObject enemyFighting;
    public Transform playerPlacement;
    public Transform enemyPlacement;
    //HUD creation is here to substantiate it...
    public unitHUD playerHUD;
    public unitHUD enemyHUD;
    public gameStates state;
    // Start is called before the first frame update
    
    public void changeBackground()
    { 
        //change our background for the game...
        background.sprite = backgroundImage2;
    }
    public void checkUltAvail()
    {
        //random ult function so it gives a 20% chance for the user to have it...
        int i = Random.Range(1, 6);
        Debug.Log(i);
        if (i == 5)
        {
            ultButton.interactable = true;
        }
    }
    void Start()
    {
        //simple Start function...
        state = gameStates.START;
        StartCoroutine(startBattle());
    }
    public void chosenKnight()
    {
        //This is the chosenKnight function that decides which one you chosen in the scene previous...
        if (changeScene.knightChosen == 1)
        {
            playerChosen = paladin;
        }
        else if (changeScene.knightChosen == 2)
        {
            playerChosen = mage;
        }
        else if (changeScene.knightChosen == 3)
        {
            playerChosen = berserker;
        }
   
    }
    public void randomEnemy()
    {
        //This function decides the random enemy, as well as set the final boss at 100 gems...
        int f = Random.Range(1, 100);

        if (gems >= 100)
        {
            enemyFighting = bossBat;
        }
        else
        {
            if (f <= 50)
            {
                enemyFighting = slime;
            }
            else if (f >= 51)
            {
                enemyFighting = snake;
            }
        }

            //remember to set up boss in which you gather 100 gems to fight!
    }
    IEnumerator startBattle()
    {
        //IMPORTANT STARTBATTLE, all initialized and instantiated here...
        gems = 0;
        gemText.text = "Gems: " + gems.ToString();
        chosenKnight();
        randomEnemy();
        //enemy and players instantiated down here...
        GameObject playerObject = Instantiate(playerChosen, playerPlacement);
        playerUnit = playerObject.GetComponent<Unit>();
        if (playerUnit.unitName == "Paladin Knight")
        {
            secondButton.interactable = false;
        }
        enemyObject = Instantiate(enemyFighting, enemyPlacement);
        enemyUnit = enemyObject.GetComponent<Unit>();
        potionText.text = "Potions X " + playerUnit.numPotions.ToString();
        dialogueText.text = "A " + enemyUnit.unitName + " stands in your way!";
        ultButton.interactable = false;
        playerHUD.setHUD(playerUnit);
        enemyHUD.setHUD(enemyUnit);

        yield return new WaitForSeconds(1f);

        state = gameStates.PLAYERTURN;
        playerTurn();
    }
    IEnumerator nextBattle()
    {
        //This is the next battle function in which this will keep creating new fights as long as you do not have 100 gems and won against the boss...
        Debug.Log("This is the nextBattle Function");
        onHitRounds = 0;
        playerUnit.currentMP = playerUnit.maxMP;
        randomEnemy();
        //destroyed enemy object gets recreated...
        enemyObject = Instantiate(enemyFighting, enemyPlacement);
        enemyUnit = enemyObject.GetComponent<Unit>();
        dialogueText.text = "A " + enemyUnit.unitName + " stands in your way!";
        playerHUD.setHUD(playerUnit);
        enemyHUD.setHUD(enemyUnit);
        enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
        yield return new WaitForSeconds(1f);
        //switch to players turn...
        state = gameStates.PLAYERTURN;
        playerTurn();
    }
    void playerTurn()
    {
        checkHP();
        checkUltAvail();
        dialogueText.text = "It's Your Turn!";
    }
    void checkHP()
    {
        //checkHP function is used to make sure player is not overusing health potions....
        if (playerUnit.unitName == "Paladin Knight" && playerUnit.currentHP >= playerUnit.maxHP)
        {
            secondButton.interactable = false;
            potionButton.interactable = false;
        }
        if (playerUnit.currentHP >= playerUnit.maxHP)
        {
            potionButton.interactable = false;
        }
        else
        {
            secondButton.interactable = true;
            potionButton.interactable = true;
        }
    }
    public void checkOnHit()
    {
        //This function is crucial to onHit in which both the mage and berserker have...
        if(onHitRounds > 0)
        {
            if (playerUnit.unitName == "Mage Knight")
            {
                enemyUnit.takeDamage(3);
            }
            else if(playerUnit.unitName == "Berserker Knight")
            {
                enemyUnit.takeDamage(5);
            }
            onHitRounds--;
            enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
        }

    }
    IEnumerator playerAttack()
    {
        //first player attack for knights...
        bool dead = enemyUnit.takeDamage(playerUnit.primary);
        //bool needs to initialized like so, so I wanted to take mana away with the specific knight since they all have different needs...
        bool o = playerUnit.useMana(0);
        if (playerUnit.unitName == "Paladin Knight")
        {
            o = playerUnit.useMana(5);
        }
        else if (playerUnit.unitName == "Berserker Knight")
        {
            o = playerUnit.useMana(10);
        }
        else if(playerUnit.unitName == "Mage Knight")
        {
            o = playerUnit.useMana(10);
        }
        //update our HUDs
        playerHUD.updateHPMP(playerUnit.currentHP, playerUnit.currentMP);
        enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
        dialogueText.text = playerUnit.unitName + " uses his primary attack!";

        yield return new WaitForSeconds(1f);
        //if out of mana, you lose...
        if (o)
        {
            state = gameStates.LOSE;
            dialogueText.text = "You do not have enough mana to fight!";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            endBattle();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            //if the enemy is dead, you win and move on...
            if (dead)
            {
                state = gameStates.WIN;
                endBattle();
                yield return new WaitForSeconds(2f);
            }
            else
            {
                ///switch to enemy turn...
                checkOnHit();
                state = gameStates.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
    }
    IEnumerator secondaryPlayerAttack()
    {
        //very similar to the primary Attack function above...
        bool dead = enemyUnit.takeDamage(playerUnit.secondary);
        bool o = playerUnit.useMana(0);
        enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
        if (playerUnit.unitName == "Paladin Knight")
        {
            dialogueText.text = "Paladin Knight uses Heal!";
            if (playerUnit.currentHP < playerUnit.maxHP)
            {
                StartCoroutine(playerHeal(15));
                o = playerUnit.useMana(20);
                playerHUD.updateHPMP(playerUnit.currentHP, playerUnit.currentMP);
                checkHP();
            }
        }
        else if(playerUnit.unitName == "Mage Knight")
        {
            //this is where on hits occur since the mage and berserker have them...
            onHitRounds = 2;
            o = playerUnit.useMana(20);
            dialogueText.text = playerUnit.unitName + " uses Firestorm!";
        }
        else if (playerUnit.unitName == "Berserker Knight")
        {
            onHitRounds = 2;
            o = playerUnit.useMana(30);
            dialogueText.text = playerUnit.unitName + " uses Bleeding Blade!";
        }
        playerHUD.updateHPMP(playerUnit.currentHP, playerUnit.currentMP);
        enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);

            yield return new WaitForSeconds(1f);
        //same as original attack function...(for the most part)
        if (o)
        {
            state = gameStates.LOSE;
            dialogueText.text = "You do not have enough mana to fight!";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            endBattle();
            yield return new WaitForSeconds(1f);
        }

        if (dead)
        {
            state = gameStates.WIN;
            dialogueText.text = "Another Enemy Approaches!";
            endBattle();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            checkOnHit();
            state = gameStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator ultAttack()
    {
        //This is the ult function where more damage is done and the mage and berserker have on hit effects...
            bool dead = enemyUnit.takeDamage(playerUnit.special);
            bool o = playerUnit.useMana(0);
            enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
            if (playerUnit.unitName == "Paladin Knight")
            {
                o = playerUnit.useMana(40);
                dialogueText.text = playerUnit.unitName + " uses Calling of the Sun!";
                enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
            }
            else if (playerUnit.unitName == "Mage Knight")
            {
                o = playerUnit.useMana(40);
                onHitRounds = 2;
                dialogueText.text = playerUnit.unitName + " uses Call to the Wind!";
                enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
            }
            else if (playerUnit.unitName == "Berserker Knight")
            {
                o = playerUnit.useMana(40);
                onHitRounds = 1;
                dialogueText.text = playerUnit.unitName + " uses Guts and Glory!";
                enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
            }
            yield return new WaitForSeconds(1f);
        if (o)
        {
            state = gameStates.LOSE;
            dialogueText.text = "You do not have enough mana to fight!";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            endBattle();
            yield return new WaitForSeconds(1f);
        }
            if (dead)
            {
                state = gameStates.WIN;
                dialogueText.text = "Another Enemy Approaches!";
                yield return new WaitForSeconds(1f);
                endBattle();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(1f);
                checkOnHit();
                state = gameStates.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
    }
    IEnumerator playerHeal(int x)
    {
        //playerHeal function uses heal from Unit.cs...
        playerUnit.heal(x);
        playerHUD.updateHPMP(playerUnit.currentHP, playerUnit.currentMP);
        yield return new WaitForSeconds(2f);
        //change to Enemy Turn...
        state = gameStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {
        //This is the enemyTurn (pretty much the enemyController) where ther enemy chooses what to do...either a regular attack, strong attack, or heal...
        bool dead = playerUnit.takeDamage(0);
        int f = Random.Range(1, 11);
        if(f <= 5)
        {
            //sounds play for attacks, but not for heals...
            dead = playerUnit.takeDamage(enemyUnit.primary);
            dialogueText.text = enemyUnit.unitName + " uses a regular attack!";
            enemySound.Play();
        }
        else if (f > 5 && f < 8)
        {
            dead = playerUnit.takeDamage(enemyUnit.secondary);
            dialogueText.text = enemyUnit.unitName + " uses a strong attack!";
            enemySound.Play();
        }
        else if (f >= 8)
        {
            dialogueText.text = enemyUnit.unitName + " uses heal!";
            enemyUnit.heal(enemyUnit.special);
            enemyHUD.updateHPMP(enemyUnit.currentHP, enemyUnit.currentMP);
        }    
         
        yield return new WaitForSeconds(1f);
        
        playerHUD.updateHPMP(playerUnit.currentHP, playerUnit.currentMP);
        yield return new WaitForSeconds(1f);
        if (dead)
        {
            //as you can see, game states are super important...
            state = gameStates.LOSE;
            endBattle();
        }
        else
        {
            state = gameStates.PLAYERTURN;
            playerTurn();
        }
    }
    void endBattle()
    {
        //Important endBattle function where it will either give gems, or if you win or lose entirely, you will transfer to the appropriate scene...
        if (state == gameStates.WIN)
        {
            if (enemyUnit.unitName == "Slime")
            {
                gems += 10;
            }
            else if (enemyUnit.unitName == "Giant Snake")
            {
                gems += 20;
            }
            else if (enemyUnit.unitName == "Bat Boss")
            {
                gems += 500;
                state = gameStates.WIN;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            gemText.text = "Gems: " + gems.ToString();
            Destroy(enemyObject);
            dialogueText.text = "Another Enemy Approaches!";
            changeBackground();
            StartCoroutine(nextBattle());
        }
        else if (state == gameStates.LOSE)
        {
            dialogueText.text = "You Lost!";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
    //below are the functions for the buttons in which they will call the needed functions...
    public void onAttackButton()
    {
        if (state != gameStates.PLAYERTURN)
            return;
        else
        {
            StartCoroutine(playerAttack());
        }
    }
    public void onSecondaryButton()
    {
        if (state != gameStates.PLAYERTURN)
            return;
        else
            StartCoroutine(secondaryPlayerAttack());
    }
    public void onUltButton()
    {
        if (state != gameStates.PLAYERTURN)
            return;
        else
        {
            StartCoroutine(ultAttack());
            ultButton.interactable = false;
        }
    }
    public void usePotionButton()
    {
        //potion button is special where if the number of potions is 0, it will make the potions unusable,
        //otherwise you have the amount of potions that your character was given (3 in these cases but it can be changed of course)...
        if (state != gameStates.PLAYERTURN)
            return;
        else
        {
            if(playerUnit.numPotions <= 0)
            {
                potionButton.interactable = false;
                potionText.text = "Potions X " + playerUnit.numPotions;
                dialogueText.text = "You Have No More Potions! Please Pick Another Option!";
            }
            else
            {
                    playerUnit.numPotions--;
                    Debug.Log("Potions: " + playerUnit.numPotions);
                    potionButton.interactable = true;
                    potionText.text = "Potions X " + playerUnit.numPotions;
                    StartCoroutine(playerHeal(25));
            }
        }
    }
}
