
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public AudioSource[] tab_audio;
    public float runSpeed = 40f;
    private bool m_alive = true;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    bool jump = false;
    public Animator animator;
    public PlayerController otherPlayer;
    public SpriteRenderer helpBulb;
    public GameObject m_deadScreen;
    public bool isLuna = false;
    private bool isOnLadder = false;

    public void Awake()
    {
        helpBulb.enabled = false;
        m_deadScreen.SetActive(false);
    }

    void Update()
    {
        #region Movement
        horizontalMove = 0f;
        verticalMove = 0f;
        if(!m_alive)
        {
            horizontalMove = 0;
            verticalMove = 0f;
            return;
        }
        if (controller && m_alive)
        {
            if (isLuna)
            {

                if (Input.GetKey(KeyCode.Z))
                {
                    if (isOnLadder)
                    {
                        verticalMove = -this.GetComponent<Rigidbody2D>().velocity.y+10;
                    }
                    else
                    {
                        jump = true;
                    }     
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    horizontalMove = -1 * runSpeed / (controller.IsGrounded() ? 1 : 2);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    horizontalMove = runSpeed / (controller.IsGrounded() ? 1 : 2);
                }
            }
            else
            {
                //Rival code
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (isOnLadder)
                    {
                        verticalMove = -this.GetComponent<Rigidbody2D>().velocity.y + 10;
                    }
                    else
                    {
                        jump = true;
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontalMove = -1 * runSpeed / (controller.IsGrounded() ? 1 : 2);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    horizontalMove = runSpeed / (controller.IsGrounded() ? 1 : 2);
                }
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime,verticalMove, false, jump);
        jump = false;
    }

    public IEnumerator Die()
    {
        if (IsAlive())
        {
            m_alive = false;
            int random = Random.Range(0, tab_audio.Length);
            tab_audio[random].Play();

            animator.SetTrigger("Death");
            yield return new WaitForSeconds(1.0f);
            m_deadScreen.SetActive(true);
            StartCoroutine(RetryLevel());
        }
    }

    public bool IsAlive()
    {
        return m_alive;
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator RetryLevel()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowHelpBulb(bool visible)
    {
        helpBulb.enabled = visible;
    }

    public void SetOnLadder(bool b)
    {
        isOnLadder = b;
    }
}
