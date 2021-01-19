using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceshipControls : MonoBehaviour
{

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public float thrust;
    public float turnThrust;
    private float thrustInput;
    private float turnInput;
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public float bulletForce;
    public float deathForce;
    private bool hyperspace;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject newHighScorePanel;
    public InputField highScoreInput;
    public Text highScoreListText;
    public GameManager gm;

    public Color inColor;
    public Color normalColor;

    public GameObject bullet;
    

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        hyperspace = false;

        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Fire1")){
            GameObject newBullet = Instantiate(bullet,transform.position,transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet,2.0f);
        }

        if(Input.GetButtonDown("Hyperspace") && !hyperspace)
        {   
            hyperspace = true;
            spriteRenderer.enabled = false;
            collider.enabled = false;
            Invoke("Hyperspace",1f);
        }

        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnThrust); 

        Vector2 newPos = transform.position;
        if(transform.position.y > screenTop){
            newPos.y = screenBottom;
        }
        if(transform.position.y < screenBottom){
            newPos.y = screenTop;
        }
        if(transform.position.x > screenRight){
            newPos.x = screenLeft;
        }
        if(transform.position.x < screenLeft){
            newPos.x = screenRight;
        }

        transform.position = newPos;

        
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustInput * 6);
        //rb.AddTorque(-turnInput * 3);
    }

    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score;
    }

    void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        
        spriteRenderer.enabled = true;
        spriteRenderer.color = inColor;
        Invoke("Invulnerable", 2f);

    }

    void Invulnerable()
    {
        collider.enabled = true;
        spriteRenderer.color = normalColor;
    }

    void Hyperspace()
    {
        Vector2 newPosition = new Vector2(Random.Range(-10f,10f),Random.Range(-7f,7f));
        transform.position = newPosition;
        spriteRenderer.enabled = true;
        collider.enabled = true;

        hyperspace = false;
        
    }





    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("hit");
        Debug.Log(col.relativeVelocity.magnitude);
        if (col.relativeVelocity.magnitude > deathForce)
        {
            lives --;
            livesText.text = "Lives: " + lives;

            spriteRenderer.enabled = false;
            collider.enabled = false;
            
            Invoke("Respawn", 2f);
            if(lives <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        CancelInvoke();
        
        if(gm.CheckForHighScore(score))
        {
            newHighScorePanel.SetActive(true);
        } else{
            gameOverPanel.SetActive(true);
            highScoreListText.text = "HIGH SCORE" + "\n" +"\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
        }
    }

    public void HihgScoreInput()
    {
        string newInput = highScoreInput.text;
        Debug.Log(newInput);
        newHighScorePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        PlayerPrefs.SetString("highscoreName",newInput);
        PlayerPrefs.SetInt("highscore",score);
        highScoreListText.text = "HIGH SCORE" + "\n" +"\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }
}
