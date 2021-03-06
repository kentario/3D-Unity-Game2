using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float gunAmmunition = 30.0f;
    public float gunMagazines = 15.0f;
    public float Sensitivity = 70.0f;
    
    public bool isOnGround = true;
    public Camera normalCam;

    public ParticleSystem muzzleFlash;
    public ParticleSystem collisionParticle;

    private float health = 100.0f;
    private float maxAngle = 90.0f;
    private float sprintModifier = 1.2f;
    private float speed = 5.0f;
    private float jumpForce = 5.0f;
    private float horizontalInput;
    private float forwardInput;
    private float baseFOV;
    private float sprintFOVModifier = 1.2f;
    
    public Transform player;
    public Transform cams;

    private Rigidbody playerRb;
    private Quaternion camCenter;
    
    void Start() {
    	baseFOV = normalCam.fieldOfView;
	playerRb = GetComponent<Rigidbody>();
	camCenter = cams.localRotation;
	Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update() {
	horizontalInput = Input.GetAxisRaw("Horizontal");
	forwardInput = Input.GetAxisRaw("Vertical");
	
	bool sprint = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt);
	bool isSprinting = sprint && forwardInput > 0;
	
	float t_adjustedSpeed = speed;
	
	if (isSprinting) t_adjustedSpeed *= sprintModifier;
	
	transform.Translate(Vector3.forward * Time.deltaTime * t_adjustedSpeed * forwardInput);
	transform.Translate(Vector3.right * Time.deltaTime * t_adjustedSpeed * horizontalInput);
	
	if (Input.GetKey(KeyCode.Space) && isOnGround) {
	    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	    isOnGround = false;
	}

      	if (isSprinting) {normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);}
	else {normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);}
	
	SetY();
	SetX();
	
	Gun();

	Health();
    }
    
    private void OnCollisionEnter(Collision collision) {
	if (collision.gameObject.CompareTag("Ground")) {isOnGround = true;}
	collisionParticle.Play();
    }
    
    void SetY() {
	float t_input = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
	Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
	Quaternion t_delta = cams.localRotation * t_adj;
	
	if (Quaternion.Angle(camCenter, t_delta) < maxAngle) {
	    cams.localRotation = t_delta;
	}
    }
    
    void SetX() {
	float t_input = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
	Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
	Quaternion t_delta = player.localRotation * t_adj;
	player.localRotation = t_delta;
    }
 
    public void Gun() {
    	if (Input.GetKeyDown(KeyCode.Mouse0) && gunAmmunition > 0) {
    	    gunAmmunition -= 1;
	    muzzleFlash.Play();
    	}
	
	if (Input.GetKeyDown(KeyCode.R) && gunMagazines > 0) {
	    gunAmmunition = 30;
    	    gunMagazines -= 1;
    	}
    }

    public void Health() {
	
    }
}
