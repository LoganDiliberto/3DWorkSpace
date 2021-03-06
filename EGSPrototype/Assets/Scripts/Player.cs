using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // declare reference variables
    CharacterController characterController;
    Animator animator;
    PlayerInput playerInput; // NOTE: PlayerInput class must be generated from New Input System in Inspector -- see 10:30 in this video
    public Transform meleeAttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public HealthBar healthbar;
    public int maxHealth = 12;
    public int currentHealth;
    public GameObject boomer;
    public int healthRegenerateThreshhold = 4;
    public float timeToRegenHealth;
    GameObject hat;//Reference To The Main Character's Weapon


    public float invincibilityLength;//Seconds for invincible length after being hit
    private float invincibilityCounter;
    public float invincibilityRollLength;//Seconds for invincible length after rolling
    private float invincibilityRollCounter;
    public float rollCooldownLength;//Seconds before you can roll again
    private float rollCounter;
    public float meleeAttackLength;//Seconds for time between attacks
    private float meleeAttackCounter;
    public float rangedAttackLength;//Seconds for time between ranged attacks
    private float rangedAttackCounter;
    // variables to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;
    int isMeleeAttackingHash;
    int isRangedAttackingHash;
    int isRollHash;

    // variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isMeleeAttackingPressed;
    bool isRangedAttackingPressed;
    bool isRollPressed;

    // constants
    float rotationFactorPerFrame = 15.0f;
    public float walkMultiplier = 2.0f;
    public float runMultiplier = 3.0f;
    int zero = 0;
    float gravity = -9.8f;
    float groundedGravity = -.05f;
    int meleeDamage = 10;
    int rangedDamage = 15;

    //true constants (dont change with code - for resetting other variables to their original value)
    int maxHealthDONOTCHANGE = 5;
    float walkMultiplierDONOTCHANGE = 2.0f;
    float runMultiplierDONOTCHANGE = 3.0f;
    int meleeDamageDONOTCHANGE = 10;
    int rangedDamageDONOTCHANGE = 5;

    //buff variables
    public bool isBuffed = false;
    private int whichBuff = 0;

    float totalBuffTime = 10f;
    int buffMultiplier = 2;
    bool immortal = false;
    bool alreadyBuffed = false;

    //status flag
    bool healthRegenerating = false;

    void Start(){
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void takeDamage(int damage){
        if(invincibilityCounter <= 0 && invincibilityRollCounter <=0){
            // Debug.Log("Took Damage");
            currentHealth -= damage;
            healthbar.SetHealth(currentHealth);
            invincibilityCounter = invincibilityLength;
        }
        // Debug.Log("Didn't Take Damage From Invincibliity!");
        
        if((currentHealth < healthRegenerateThreshhold) && !healthRegenerating){
            healthRegenerating = true;
            StartCoroutine(regenerateHealth());
        }
    }

    IEnumerator regenerateHealth(){
        while (currentHealth < healthRegenerateThreshhold){
            yield return new WaitForSeconds(timeToRegenHealth);
            currentHealth += 1;
            healthbar.SetHealth(currentHealth);
        }
        healthRegenerating = false;
        
    }

    // Awake is called earlier than Start in Unity's event life cycle
    void Awake() 
    {
        // initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // set the parameter hash references
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isMeleeAttackingHash = Animator.StringToHash("meleeAttack");
        isRangedAttackingHash = Animator.StringToHash("rangedAttack");
        isRollHash = Animator.StringToHash("isRolling");

        // set the player input callbacks
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.MeleeAttack.started += onMeleeAttack;
        playerInput.CharacterControls.MeleeAttack.canceled += onMeleeAttack;
        playerInput.CharacterControls.RangedAttack.started += onRangedAttack;
        playerInput.CharacterControls.RangedAttack.canceled += onRangedAttack;
        playerInput.CharacterControls.Roll.started += onRoll;
        playerInput.CharacterControls.Roll.canceled += onRoll;
    }

    void handleAttack(){

        if(isMeleeAttackingPressed){
            if(meleeAttackCounter <= 0){
                meleeAttackCounter = meleeAttackLength;
                //Play an animation
                //animator.SetTrigger("attack");
                //Debug.Log("I'm Swinging");
                //Detect the enemies in our hit range
                Collider [] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, attackRange, enemyLayers);
                
                //Damage them / Print there names
                foreach (Collider enemy in hitEnemies)
                {
                    // Debug.Log("We Hit " + enemy.name + " For " + meleeDamage + " Damage!");
                    enemy.GetComponent<Enemy>().TakeDamage(meleeDamage);
                }
            }
        }
        hat = GameObject.Find("Boomer");//The Weapon The Character Is Holding In The Scene
        if(hat != null){
            Collider [] hitEnemies = Physics.OverlapSphere(hat.transform.position, 3, enemyLayers);
            Debug.Log("The hat is flying!");
            //Damage them / Print there names
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("We Hit " + enemy.name + " For " + rangedDamage + " Damage!");
                enemy.GetComponent<Enemy>().TakeDamage(rangedDamage);
            }
        }
        
        if(isRangedAttackingPressed){
            if(rangedAttackCounter <= 0){
                rangedAttackCounter = rangedAttackLength;
                GameObject clone;
                clone = Instantiate(boomer, new Vector3(transform.position.x, transform.position.y+1,transform.position.z), transform.rotation) as GameObject;
            }
        }
    }

    void handleRoll(){
        //Needs a cooldown timer to be added so you cant roll again before animation ends
        bool isRolling = animator.GetBool(isRollHash);
        if(isRollPressed && !isRolling){
            if(rollCounter <= 0){
                animator.SetBool(isRollHash, true);
                rollCounter = rollCooldownLength;
                invincibilityRollCounter = invincibilityRollLength;
            }
            else{
                // Debug.Log("Can't roll again till timer is done!");
            }
        }
        else if(!isRollPressed && isRolling){
            animator.SetBool(isRollHash, false);
        }
    }

    //Sets a variable to true when the player has pressed the attack button
    void onMeleeAttack(InputAction.CallbackContext context){
        isMeleeAttackingPressed = context.ReadValueAsButton();
    }
    //For later
    void onRangedAttack(InputAction.CallbackContext context){
        isRangedAttackingPressed = context.ReadValueAsButton();
    }

    void OnDrawGizmosSelected(){
        if(meleeAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, attackRange);
    }

    void onRun (InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onRoll (InputAction.CallbackContext context)
    {
        isRollPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = currentMovement.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed) {
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            // rotate the character to face the positionToLookAt            
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // handler function to set the player input values
    void onMovementInput (InputAction.CallbackContext context)
    {
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x * walkMultiplier;
            currentMovement.z = currentMovementInput.y * walkMultiplier;
            currentRunMovement.x = currentMovementInput.x * runMultiplier;
            currentRunMovement.z = currentMovementInput.y * runMultiplier;
            isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }
    
    void handleMovementAnimation()
    {
        // get parameter values from animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        // start walking if movement pressed is true and not already walking
        if (isMovementPressed && !isWalking) {
            animator.SetBool(isWalkingHash, true);
        }
        // stop walking if isMovementPressed is false and not already walking
        else if (!isMovementPressed && isWalking) {
            animator.SetBool(isWalkingHash, false);
        }
        // run if movement and run pressed are true and not currently running
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        // stop running if movement or run pressed are false and currently running
        else if ((!isMovementPressed || !isRunPressed) && isRunning) {
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleGravity()
    {
        // apply proper gravity if the player is grounded or not
        if (characterController.isGrounded) {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        } else {
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibilityCounter > 0){
            invincibilityCounter -= Time.deltaTime;
        }
        if(invincibilityRollCounter > 0){
            invincibilityRollCounter -= Time.deltaTime;
        }
        if(rollCounter > 0){
            rollCounter -= Time.deltaTime;
        }
        if(meleeAttackCounter > 0){
            meleeAttackCounter -= Time.deltaTime;
        }
        if(rangedAttackCounter > 0){
            rangedAttackCounter -= Time.deltaTime;
        }
        handleGravity();
        handleRotation();
        handleRoll();
        handleMovementAnimation();
        handleAttack();

        if (isRunPressed) {
            characterController.Move(currentRunMovement * Time.deltaTime);
        } else {
            characterController.Move(currentMovement * Time.deltaTime);
        }

        if (isBuffed == true)
        {
            handleBuffs();
        }

    }

    void handleBuffs()
    {
        if (!alreadyBuffed)
        {
            //set random buff to be active
            whichBuff = Random.Range(1, 4);

            if (whichBuff == 1)
            {
                Debug.Log("speed buff");
                //Debug.Log(walkMultiplier);
                //Debug.Log(runMultiplier);
                walkMultiplier *= buffMultiplier;
                runMultiplier *= buffMultiplier;
                //Debug.Log(walkMultiplier);
                //Debug.Log(runMultiplier);
            }
            else if (whichBuff == 2)
            {
                Debug.Log("damage buff");
                meleeDamage *= buffMultiplier;
                rangedDamage *= buffMultiplier;
            }
            else if (whichBuff == 3)
            {
                Debug.Log("immortal buff");
                immortal = true;
            }
            else if (whichBuff == 4)
            {
                Debug.Log("health up buff");
                currentHealth = 5;
                healthbar.SetHealth(currentHealth);
            }

            alreadyBuffed = true;
        }
        else if (alreadyBuffed && totalBuffTime > 0)
        {
            //Debug.Log(totalBuffTime);
            totalBuffTime -= Time.deltaTime;
        }

        if (totalBuffTime <= 0)
        {
            if (whichBuff == 1)
            {
                //Debug.Log(walkMultiplier);
                //Debug.Log(runMultiplier);
                walkMultiplier = walkMultiplierDONOTCHANGE;
                runMultiplier = runMultiplierDONOTCHANGE;
                //Debug.Log(walkMultiplier);
                //Debug.Log(runMultiplier);
            }
            else if (whichBuff == 2)
            {
                meleeDamage = meleeDamageDONOTCHANGE;
                rangedDamage = rangedDamageDONOTCHANGE;
            }
            else if (whichBuff == 3)
            {
                immortal = false;
            }

            isBuffed = false;
            whichBuff = 0;
            alreadyBuffed = false;
            Debug.Log(alreadyBuffed + "buff off");
            totalBuffTime = 10f;
        }


    }

    void OnEnable()
    {
        // enable the character controls action map
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        playerInput.CharacterControls.Disable();
    }
}