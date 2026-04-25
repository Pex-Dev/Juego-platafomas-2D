using UnityEngine;

public class FinalNivel : MonoBehaviour
{   
    [SerializeField] private HadaNivelCompletado hada;
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private UIController uIController;
    [SerializeField] private CameraControl cameraControl;
    public AudioClip musicLevelComplete;
    private GameObject player;
    private ArmAim playerAim;
    private PlayerMovement playerMovement;
    private Health playerHealth;
    private PlayerAttack playerAttack;

    private bool active = false;
    private bool stopCamera = false;

    void Start()
    {   
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerHealth = player.GetComponent<Health>();
            playerAttack = player.GetComponent<PlayerAttack>();
            playerAim = player.GetComponentInChildren<ArmAim>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(active && playerMovement != null)
        {
            playerMovement.ForceMove(1.0f,  3f);
            if (playerMovement.isGrounded && !stopCamera)
            {
                stopCamera = true;
                StopCamera();
            }
        }
    }

    void StopCamera()
    {  
        targetCamera.transform.position = player.transform.position;
        cameraControl.target =  targetCamera.transform;  
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Player"))
        {   
            ScenesManager.instance.PlayMusic(musicLevelComplete);
            active = true;
            playerMovement.canMove = false;
            playerAim.canAim = false;
            playerAttack.canAttack = false;
            playerHealth.invulnerability = true;
            uIController.ShowCompleteLevelScreen(true);
            hada.moveToPlayer = true;
        }
    }
}
