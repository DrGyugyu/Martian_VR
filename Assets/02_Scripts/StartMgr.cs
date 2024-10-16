using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMgr : MonoBehaviour
{
    [SerializeField] private Rigidbody cameraRb;
    [SerializeField] private Button startBtn;
    [SerializeField] private Image UIImg;
    [SerializeField] private float flashSpeed = 1.0f;
    [SerializeField] private GameObject particalSystem;
    private Color normalColor;
    private bool isRed = false;

    void Start()
    {
        cameraRb.useGravity = false;
        normalColor = UIImg.color;

        StartCoroutine(FlashImage());
        startBtn.onClick.AddListener(() =>
        {
            cameraRb.useGravity = true;
            particalSystem.gameObject.SetActive(true);
            startBtn.gameObject.transform.localScale = Vector3.zero;
        });
    }

    private IEnumerator FlashImage()
    {
        while (true)
        {
            isRed = !isRed;
            UIImg.color = isRed ? Color.red : normalColor;
            yield return new WaitForSeconds(flashSpeed / 2.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("MarsScene");
    }
}