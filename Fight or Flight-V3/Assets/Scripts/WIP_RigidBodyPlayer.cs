using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIP_RigidBodyPlayer : MonoBehaviour
{
    [Header("--- Character Components ---")]
    public Transform playerCamera;
    public Transform playerOrientation;
    [SerializeField] AudioSource sounds;

    // Other
    private Rigidbody playersRigidBody;
    private float playersDesiredXPos;

    [Header("--- Camera Components ---")]
    //Floats for the players rotation and camera sensitivity
    private float cameraXRotation;
    [SerializeField] float cameraSensitivity; //found 25 to 50 to be a good range
    [SerializeField] float sensitivityMultiplier; // found 1-3 to be a good range

    [Header("--- Movement Components ---")]
    //Variables for the players movement
    [SerializeField] float playerMovementSpeed; // found 2000 - 4500 to be a good range
    [SerializeField] float playersMaxSpeed; // found 10 - 20 to be a good range
    [SerializeField] float counterMovement; // found 0.1 to 0.2 to be a good range
    [SerializeField] float movementThreshold; // found 0.01 to 0.03 to be a good range
    [SerializeField] float maxAngle; // found 35 - 45 to be a good range

    public bool isGrounded;
    private bool isGround;
    public LayerMask whatIsGround;

    [Header("--- Jump Comonents ---")]
    //Variables for the players Jump
    [SerializeField] float timeBeforeNextJump; //found 0.1 to 0.25 to be a good range
    [SerializeField] float jumpHeight; //found 350 - 550 to be a good range
    [SerializeField] int jumpAmount; // need to figur out hot make a double jump
    private bool isJumpReady = true;

    [Header("--- Wall Running Components ---")]
    //WallRunning Code
    public LayerMask whatIsAWall;
    public float wallRunForce;
    public float playersMaxWallRunTime;
    public float playersMaxWallSpeed;
    bool isWallRightOfPlayer;
    bool isWallLeftOfPlayer;
    bool isPlayerWallRunning;

    [Header("---- Gun Stats ----")]
    [SerializeField] List<gunStats> gunObjects = new List<gunStats>();
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(15, 35)] [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [Range(1, 9)] [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;

    //WallRunning Camera variables
    public float maxCameraTilt;
    public float wallRunCameraTilt;

    //player inputs, did not want to create a new script just for a rigid body player just in case this script causes to many issues for the current iteration of our game
    float xInput;
    float yInput;

    //other variables
    bool isJumping;
    bool isSprinting;
    private Vector3 plainVector = Vector3.up;
    bool isShooting;
    int selectedGun;

    private void Awake()
    {
        playersRigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    // Update is called once per frame
    void Update()
    {
        MyPlayersInput();
        PlayersLookDirection();
        CheckForWhichWallSidePlayerIsOn();
        InputsToWallRun();
        if (gunObjects.Count > 0 && !isShooting && Input.GetButton("Shoot"))
        {
            StartCoroutine(Shoot());
        }
        SelectGun();
    }

    private void MyPlayersInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        isJumping = Input.GetButton("Jump");
    }

    private void PlayerMovement()
    {
        playersRigidBody.AddForce(Vector3.down * Time.deltaTime * 10);

        Vector2 playersMagnitude = VelocityRelativeToWherePLayerIsLooking();
        float playersXMagnitude = playersMagnitude.x;
        float playersYMagnitude = playersMagnitude.y;

        PlayersCounterMovement(xInput, yInput, playersMagnitude);

        if (isJumpReady && isJumping)
        {
            Jump();
        }

        float maxPlayerSpeed = this.playersMaxSpeed;

        if (xInput > 0 && playersXMagnitude > maxPlayerSpeed)
        {
            xInput = 0;
        }
        if (xInput < 0 && playersXMagnitude < -maxPlayerSpeed)
        {
            xInput = 0;
        }
        if (yInput > 0 && playersYMagnitude > maxPlayerSpeed)
        {
            yInput = 0;
        }
        if (yInput < 0 && playersYMagnitude < -maxPlayerSpeed)
        {
            yInput = 0;
        }

        float multiplier = 1f;
        float vectorMultiplier = 1f;

        if (!isGrounded)
        {
            multiplier = 0.5f;
            vectorMultiplier = 0.5f;
        }

        playersRigidBody.AddForce(playerOrientation.transform.forward * yInput * playerMovementSpeed * Time.deltaTime * multiplier * vectorMultiplier);
        playersRigidBody.AddForce(playerOrientation.transform.right * xInput * playerMovementSpeed * Time.deltaTime * multiplier);

    }

    private void Jump()
    {
        if (isGrounded && isJumpReady)
        {
            isJumpReady = false;

            playersRigidBody.AddForce(Vector2.up * jumpHeight * 1.5f);

            Vector3 playerVelocity = playersRigidBody.velocity;

            if (playersRigidBody.velocity.y < 0.5f)
            {
                playersRigidBody.velocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
            }
            else if (playersRigidBody.velocity.y > 0)
            {
                playersRigidBody.velocity = new Vector3(playerVelocity.x, playerVelocity.y / 2, playerVelocity.z);
            }

            Invoke(nameof(ResetPlayerJump), timeBeforeNextJump);
        }

        if (isPlayerWallRunning)
        {
            //still figuring out how to implement a function where the player is on a certain wall and isn't holding down the opposite walls input and then jumping
            //if (isWallLeftOfPlayer && !Input.GetKey(KeyCode.D) || isWallRightOfPlayer && !Input.GetKey(KeyCode.A))
            //{
            //    playersRigidBody.AddForce(Vector2.up * jumpHeight * 1.5f);
            //    playersRigidBody.AddForce(plainVector * jumpHeight * 0.5f);
            //}

            //if (isWallRightOfPlayer || isWallLeftOfPlayer && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            //{
            //    playersRigidBody.AddForce(-playerOrientation.up * jumpHeight * 1f);
            //}
            if (isWallRightOfPlayer && Input.GetKey(KeyCode.A))
            {
                playersRigidBody.AddForce(playerOrientation.up * jumpHeight * 0.55f);
                playersRigidBody.AddForce(playerOrientation.forward * jumpHeight * 0.15f);
                playersRigidBody.AddForce(-playerOrientation.right * jumpHeight * 0.55f);
                
            }
            if (isWallLeftOfPlayer && Input.GetKey(KeyCode.D))
            {
                playersRigidBody.AddForce(playerOrientation.up * jumpHeight * 0.55f);
                playersRigidBody.AddForce(playerOrientation.forward * jumpHeight * 0.15f);
                playersRigidBody.AddForce(playerOrientation.right * jumpHeight * 0.55f);
            }

            //playersRigidBody.AddForce(playerOrientation.forward * jumpHeight * 1f);
        }

    }

    private void ResetPlayerJump()
    {
        isJumpReady = true;
    }

    private void PlayersLookDirection()
    {
        float xPosOfMouse = Input.GetAxis("Mouse X") * cameraSensitivity * Time.fixedDeltaTime * sensitivityMultiplier;
        float yPosOfMouse = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.fixedDeltaTime * sensitivityMultiplier;

        Vector3 playerRotation = playerCamera.transform.localRotation.eulerAngles;
        playersDesiredXPos = playerRotation.y + xPosOfMouse;

        cameraXRotation -= yPosOfMouse;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraXRotation, playersDesiredXPos, wallRunCameraTilt);
        playerOrientation.transform.localRotation = Quaternion.Euler(0, playersDesiredXPos, 0);

        //tilt the camera left or right based on if the wall is to the left or right of the player
        if (Mathf.Abs(wallRunCameraTilt) < maxCameraTilt && isPlayerWallRunning && isWallRightOfPlayer)
        {
            wallRunCameraTilt += Time.deltaTime * maxCameraTilt * 2;
        }
        if (Mathf.Abs(wallRunCameraTilt) < maxCameraTilt && isPlayerWallRunning && isWallLeftOfPlayer)
        {
            wallRunCameraTilt -= Time.deltaTime * maxCameraTilt * 2;
        }

        //tilt the camera back to normal
        if (wallRunCameraTilt > 0 && !isWallRightOfPlayer && !isWallLeftOfPlayer)
        {
            wallRunCameraTilt -= Time.deltaTime * maxCameraTilt * 2;
        }
        if (wallRunCameraTilt < 0 && !isWallRightOfPlayer && !isWallLeftOfPlayer)
        {
            wallRunCameraTilt += Time.deltaTime * maxCameraTilt * 2;
        }
    }

    //helps limit players mobility when preforming certain movment functions
    private void PlayersCounterMovement(float x, float y, Vector2 magnitude)
    {
        if (!isGrounded || isJumping)
        {
            return;
        }

        if (Mathf.Abs(magnitude.x) > movementThreshold && Mathf.Abs(x) < 0.05f || (magnitude.x < -movementThreshold && x > 0) || (magnitude.x > movementThreshold && x < 0))
        {
            playersRigidBody.AddForce(playerMovementSpeed * playerOrientation.transform.right * Time.deltaTime * -magnitude.x * counterMovement);
        }
        if (Mathf.Abs(magnitude.y) > movementThreshold && Mathf.Abs(y) < 0.05f || (magnitude.y < -movementThreshold && y > 0) || (magnitude.y > movementThreshold && y < 0))
        {
            playersRigidBody.AddForce(playerMovementSpeed * playerOrientation.transform.forward * Time.deltaTime * -magnitude.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(playersRigidBody.velocity.x, 2) + Mathf.Pow(playersRigidBody.velocity.z, 2))) > playersMaxSpeed)
        {
            float playerFallSpeed = playersRigidBody.velocity.y;
            Vector3 newPos = playersRigidBody.velocity.normalized * playersMaxSpeed;
            playersRigidBody.velocity = new Vector3(newPos.x, playerFallSpeed, newPos.z);
        }
    }

    public Vector2 VelocityRelativeToWherePLayerIsLooking()
    {
        float playersLookAngle = playerOrientation.transform.eulerAngles.y;
        float playersMovingAngle = Mathf.Atan2(playersRigidBody.velocity.x, playersRigidBody.velocity.z) * Mathf.Rad2Deg;

        float currentAngle = Mathf.DeltaAngle(playersLookAngle, playersMovingAngle);
        float currentVector = 90 - currentAngle;

        float playersMagnitude = playersRigidBody.velocity.magnitude;
        float playersXMagnitude = playersMagnitude * Mathf.Cos(currentVector * Mathf.Deg2Rad);
        float playersYMagnitude = playersMagnitude * Mathf.Cos(currentAngle * Mathf.Deg2Rad);

        return new Vector2(playersXMagnitude, playersYMagnitude);

    }

    private bool IsPlayerTouchingFloor(Vector3 v)
    {
        float floorAngle = Vector3.Angle(Vector3.up, v);

        return floorAngle < maxAngle;
    }

    private void PlayerIsNotGrounded()
    {
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        int gameLayer = collision.gameObject.layer;

        if (whatIsGround != (whatIsGround | (1 << gameLayer)))
        {
            return;
        }

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.contacts[i].normal;

            if (IsPlayerTouchingFloor(normal))
            {
                isGrounded = true;
                isGround = false;
                plainVector = normal;
                CancelInvoke(nameof(PlayerIsNotGrounded));
            }
        }

        float playerDelay = 3f;
        if (!isGround)
        {
            isGround = true;
            Invoke(nameof(PlayerIsNotGrounded), Time.deltaTime * playerDelay);
        }
    }

    private void InputsToWallRun()
    {
        if (Input.GetKey(KeyCode.D) && isWallRightOfPlayer)
        {
            StartingTheWallRun();
        }
        if (Input.GetKey(KeyCode.A) && isWallLeftOfPlayer)
        {
            StartingTheWallRun();
        }
    }

    private void StartingTheWallRun()
    {
        playersRigidBody.useGravity = false;
        isPlayerWallRunning = true;

        if (playersRigidBody.velocity.magnitude <= playersMaxWallSpeed)
        {
            playersRigidBody.AddForce(playerOrientation.forward * wallRunForce * Time.deltaTime);

            if (isWallRightOfPlayer)
            {
                playersRigidBody.AddForce(playerOrientation.right * wallRunForce / 5 * Time.deltaTime);
            }
            else
            {
                playersRigidBody.AddForce(-playerOrientation.right * wallRunForce / 5 * Time.deltaTime);
            }
        }
    }

    private void StoppingTheWallRun()
    {
        playersRigidBody.useGravity = true;
        isPlayerWallRunning = false;
    }

    private void CheckForWhichWallSidePlayerIsOn()
    {
        isWallRightOfPlayer = Physics.Raycast(transform.position, playerOrientation.right, 1f, whatIsAWall);
        isWallLeftOfPlayer = Physics.Raycast(transform.position, -playerOrientation.right, 1f, whatIsAWall);

        //where we leave the wall run
        if (!isWallLeftOfPlayer && !isWallRightOfPlayer)
        {
            StoppingTheWallRun();
        }
        //if statement for double jump, still need to figure out how to implment 
        if (isWallLeftOfPlayer || isWallRightOfPlayer)
        {
            ResetPlayerJump();
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        //weapon.GetComponent<NewGuns>().Shoot();
        sounds.PlayOneShot(gunObjects[selectedGun].gunShots, gunObjects[selectedGun].gunShotsVolume);

        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = playerCamera.transform.forward * bulletSpeed;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);

            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void GunPickUp(gunStats gunObj)
    {
        if (gunModel.CompareTag("Untagged"))
        {
            gunObjects.Add(gunObj);

            shootRate = gunObj.Rate;
            shootDist = gunObj.Range;
            shootDamage = gunObj.Damage;
            gunObj.gunBulletPos = shootPos;

            gunModel.GetComponent<MeshFilter>().sharedMesh = gunObj.weaponModel.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObj.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
            gunModel.transform.localScale = gunObj.scale;

            selectedGun = gunObjects.Count - 1;
        }
    }

    void ChangeGun()
    {
        shootRate = gunObjects[selectedGun].Rate;
        shootDist = gunObjects[selectedGun].Range;
        shootDamage = gunObjects[selectedGun].Damage;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunObjects[selectedGun].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObjects[selectedGun].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunModel.transform.localScale = gunObjects[selectedGun].scale;
    }

    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunObjects.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }
}
