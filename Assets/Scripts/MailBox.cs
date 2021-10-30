using UnityEngine;

public class MailBox : MonoBehaviour
{
    public LayerMask hero;
    public Transform canvas;
    public GameObject mailDialog;
    float radius = 3f;
    Collider[] hearHero;
    void Start()
    {
        canvas.forward = -Camera.main.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        canvas.forward = -Camera.main.transform.forward;
        hearHero = Physics.OverlapSphere(transform.position, radius, hero, QueryTriggerInteraction.Ignore);
        if (hearHero.Length > 0)
        {
            canvas.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenMailDialog();
            }
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }

    void OpenMailDialog() {
        mailDialog.SetActive(true);
    }

    public void CloseMailDialog() {
        mailDialog.SetActive(false);
    }
}
