using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour {
    // baseball's rigidbody
    public Rigidbody2D ballRigidbody;
    
    // force of ball launching off of bat
    public float hitForce = 27f;


    // audio clips of bat crack, balloon pop, and crowd cheering sound effects
    // based on different events in game
    public AudioClip batHitSound;
    public AudioClip balloonPopSound;
    public AudioClip crowdCheerSound;
    private AudioSource audioSource;

    // boolean to give sense of when the current round is over
    private bool roundEnded = false;
    
    // boolean that detects if a homerun has been counted
    // this prevents multiple homeruns per round
    private bool homerunCounted = false;


    // get audio source to play sound clips
    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // collision detection for bat and homeruns
    void OnCollisionEnter2D(Collision2D collision) {
        // bat hits pitched ball
        if (collision.gameObject.CompareTag("Bat")) {
            // Debug.Log("Collision with bat detected");

            // play bat crack sound
            audioSource.PlayOneShot(batHitSound);

            // with the velocity detected from collision as well as direction,
            // add calculated force to the ball to launch is appropriate with speed and direction
            Vector2 batVelocity = collision.relativeVelocity;
            Vector2 hitDirection = new Vector2(batVelocity.x, Mathf.Abs(batVelocity.y) + 1).normalized;
            ballRigidbody.AddForce(hitDirection * hitForce, ForceMode2D.Impulse);
        }

        // if the no homerun has been counted, and the ball collides with
        // either the stadium or objects with "OutOfPark" tag, homerun becomes true,
        // points are added to score based on homerun score or out of park score,
        // and crowd cheering is played
        else if (!homerunCounted && (collision.gameObject.CompareTag("Stadium") || collision.gameObject.CompareTag("OutOfPark"))) {
            homerunCounted = true;
            GameManager.instance.IncrementHomerunCount();

            int points = collision.gameObject.CompareTag("Stadium") ? 500 : 750;
            GameManager.instance.AddScore(points);
            audioSource.PlayOneShot(crowdCheerSound);
        }

        // end the round
        else if (!roundEnded) {
            roundEnded = true;
            Invoke("EndRound", 5f);
        }
    }

    // collision detection for balloons
    void OnTriggerEnter2D(Collider2D other) {
        // red balloons are 50 pts, gold balloons are 200pts
        // if a collision is made with a balloon, it plays a pop sound
        // and disappears
        if (other.CompareTag("Balloon")) {
            GameManager.instance.AddScore(50);
            audioSource.PlayOneShot(balloonPopSound);
            // Debug.Log("balloon hit");
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("GoldBalloon")) {
            GameManager.instance.AddScore(200);
            // Debug.Log("gold balloon hit");
            audioSource.PlayOneShot(balloonPopSound);
            other.gameObject.SetActive(false);
        }

    }

    // end round function
    void EndRound() {
        // reset roundEnded and homrunCounted booleans
        roundEnded = false;
        homerunCounted = false;
        
        // reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}