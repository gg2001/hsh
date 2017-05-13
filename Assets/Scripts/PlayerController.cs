using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public Text countText;
	public Text timerText;
	public float myCoolTimer = 91;

    private Rigidbody rb;
    private int count;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();
			Explode ();
		} else if (other.gameObject.CompareTag ("Banana")) {
			count = count - 1;
			SetCountText ();
		} else if (other.gameObject.CompareTag ("Pick Up First")) {
			other.gameObject.SetActive (false);
			Explode ();
			countText.text = "You Win!";
			timerText.text = ":) かち！";
			StartCoroutine(EndGame());
		}
    }

	void Explode() {
		var exp = GetComponent<ParticleSystem>();
		exp.Play();
	}

    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }

	void Update()
	{
		if (count < -2) {
			countText.text = "You lose!";
			timerText.text = ":( まけ！";
			StartCoroutine(EndGame());
		}

		myCoolTimer -= Time.deltaTime;
		timerText.text = myCoolTimer.ToString ("f0");

		if (myCoolTimer < 0) {
			countText.text = "You lose!";
			timerText.text = ":( まけ！";
			StartCoroutine(EndGame());
		}
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}