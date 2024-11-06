using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glove : MonoBehaviour {
    
    // assign baseball prefab
    public GameObject baseballPrefab;
    
    // asign bat
    public Transform batTransform;
    
    // minimum & maximum ball pitch force
    public float pitchForceMin = 200f;
    public float pitchForceMax = 400f;
    
    // minimum & maximum angles chosen at random
    public float randomAngleMin = 30f;
    public float randomAngleMax = 65f;

    // reference spawned in baseball
    private Rigidbody2D currentBaseball;

    // reference camera follow script
    public CameraFollow cameraFollow;

    // sound clip for beginning of round
    public AudioClip songSound;
    private AudioSource audioSource;

    // if game started boolean from GameManager script is true, start round
    void Start() {
        if (GameManager.instance.GameStarted) {
            StartRoundProtocol();
        }
    }

    // for each round
    public void StartRoundProtocol() {

        audioSource = gameObject.AddComponent<AudioSource>();

        // play start round song
        audioSource.PlayOneShot(songSound);
        
        // update pitch num for decremented pitch
        GameManager.instance.UpdatePitchCounterText();
        
        // spawn baseball for pitch after 2 second delay
        Invoke("SpawnBaseball", 2f);
    }

    public void SpawnBaseball() {

        // if baseball is destroyed, and player has more pitches,
        // spawn in baseball for pitch
        if (currentBaseball == null && GameManager.instance.totalPitches > 0) {
            GameObject baseballObject = Instantiate(baseballPrefab, transform.position, Quaternion.identity);
            currentBaseball = baseballObject.GetComponent<Rigidbody2D>();
            
            // set baseball to kinematic so it doesn't fall
            currentBaseball.bodyType = RigidbodyType2D.Kinematic;
            currentBaseball.velocity = Vector2.zero;

            // pitch baseball
            StartCoroutine(PitchBaseball());
        }
    }

    // once baseball is spawned in, wait 3 seconds, then call actual pitch function
    private IEnumerator PitchBaseball() {
        yield return new WaitForSeconds(3f);
        PitchBaseballTowardsBat();
    }

    // function to pitch ball towards bat
    void PitchBaseballTowardsBat() {
        
        // make sure spawned baseball exists
        if (currentBaseball != null) {

            // decrement pitch counter
            GameManager.instance.UsePitch();
            
            // change baseball back to dynamic, reset velocity
            currentBaseball.bodyType = RigidbodyType2D.Dynamic;
            currentBaseball.velocity = Vector2.zero;

            // calculate direction for ball to pitch towards bat
            Vector2 direction = (batTransform.position - currentBaseball.transform.position).normalized;

            // create a random angle to pitch at
            float randomAngle = Random.Range(randomAngleMin, randomAngleMax);
            Quaternion angleRotation = Quaternion.Euler(0, 0, randomAngle);
            Vector2 angledDirection = angleRotation * direction;

            // create a random pitch force, and apply angle to pitch at
            float randomPitchForce = Random.Range(pitchForceMin, pitchForceMax);
            currentBaseball.AddForce(angledDirection.normalized * randomPitchForce);


            // start process for camera to follow ball
            StartCoroutine(SetCameraFollow(currentBaseball.transform));
        }
    }

    // camera follow process
    private IEnumerator SetCameraFollow(Transform baseballTransform) {
        
        // wait 1 second before setting camera to follow baseball
        yield return new WaitForSeconds(1f);
        if (cameraFollow != null) {
            cameraFollow.SetTarget(baseballTransform);
        }
    }

    private void Update() {
        // check if game should end, if so, go to game over screen
        if (GameManager.instance.totalPitches == 0 && currentBaseball == null) {
            GameManager.instance.ShowGameOverScreen();
        }

    }
}