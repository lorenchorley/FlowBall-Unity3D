using NoFloEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	public float speed;
    public Quaternion rotation = Quaternion.identity;
    public WinnersDialog WinnersDialog;
    public LosersDialog LosersDialog;

    private Rigidbody rb;

    void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveVertical, 0, -moveHorizontal);
		rb.AddTorque (rotation * movement * speed);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick Up")) {
            Cursor.visible = true;
            other.gameObject.SetActive(false);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            WinnersDialog.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(WinnersDialog.transform as RectTransform);
        } else if (other.gameObject.CompareTag("Deadly Ground")) {
            Cursor.visible = true;
            LosersDialog.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(LosersDialog.transform as RectTransform);
        }
    }

}