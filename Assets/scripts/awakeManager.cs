using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class awakeManager : MonoBehaviour
{
    [Header("Camera")]
    public float lerpTime;
    public GameObject CameraObject;
    public GameObject finalCameraPosition, startCameraPosition;

    [Header("Deafault Canvas")]
    public GameObject DeafaultCanvas;
    public Text DeafaultCanvasCurrency;

    [Header("Map Canvas")]
    public GameObject mapSelectorCanvas;

    [Header("Upgrades Canvas")]
    public Text upgradesCurrency;
    public GameObject upgradesCanvas;

    [Header("Vehicle Select Canvas")]
    public GameObject vehicleSelectCanvas;
    public GameObject buyButton;
    public GameObject startButton;
    public vehicleList listOfVehicles;
    public Text currency;
    public Text carInfo;

    [Header("Currency Settings")]
    public int startingCurrency = 1000;   // editable from Inspector

    public GameObject toRotate;
    [HideInInspector] public float rotateSpeed = 10f;
    [HideInInspector] public int vehiclePointer = 0;
    private bool finalToStart, startToFinal;

    /* ---------- helper ---------- */
    private void UpdateCurrencyUI()
    {
        int cur = PlayerPrefs.GetInt("currency");
        DeafaultCanvasCurrency.text = "$" + cur;
        upgradesCurrency.text = "$" + cur;
        currency.text = "$" + cur;
    }

    /* ---------- Unity ---------- */
    private void Awake()
    {
        // initialise (or repair) saved currency
        if (!PlayerPrefs.HasKey("currency") || PlayerPrefs.GetInt("currency") <= 0)
            PlayerPrefs.SetInt("currency", startingCurrency);

        mapSelectorCanvas.SetActive(false);
        DeafaultCanvas.SetActive(true);
        vehicleSelectCanvas.SetActive(false);

        UpdateCurrencyUI();

        vehiclePointer = PlayerPrefs.GetInt("pointer");
        GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer], Vector3.zero, toRotate.transform.rotation);
        childObject.transform.parent = toRotate.transform;
        getCarInfo();
    }

    private void FixedUpdate()
    {
        toRotate.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        cameraTranzition();
    }

    /* ---------- vehicle arrows ---------- */
    public void rightButton()
    {
        if (vehiclePointer < listOfVehicles.vehicles.Length - 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            vehiclePointer++;
            PlayerPrefs.SetInt("pointer", vehiclePointer);
            GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer], Vector3.zero, toRotate.transform.rotation);
            childObject.transform.parent = toRotate.transform;
            getCarInfo();
        }
    }

    public void leftButton()
    {
        if (vehiclePointer > 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            vehiclePointer--;
            PlayerPrefs.SetInt("pointer", vehiclePointer);
            GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer], Vector3.zero, toRotate.transform.rotation);
            childObject.transform.parent = toRotate.transform;
            getCarInfo();
        }
    }

    /* ---------- UI buttons ---------- */
    public void startGameButton()
    {
        DeafaultCanvas.SetActive(false);
        vehicleSelectCanvas.SetActive(false);
        mapSelectorCanvas.SetActive(true);
    }

    public void BuyButton()
    {
        int price = listOfVehicles.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carPrice;
        if (PlayerPrefs.GetInt("currency") >= price)
        {
            PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") - price);

            string carKey = listOfVehicles.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName.ToString();
            PlayerPrefs.SetString(carKey, carKey);

            UpdateCurrencyUI();
            getCarInfo();
        }
    }

    public void DeafaultCanvasStartButton()
    {
        mapSelectorCanvas.SetActive(false);
        DeafaultCanvas.SetActive(false);
        vehicleSelectCanvas.SetActive(true);
        upgradesCanvas.SetActive(false);
        startToFinal = true;
        finalToStart = false;
    }

    public void vehicleSelectCanvasStartButton()
    {
        mapSelectorCanvas.SetActive(false);
        DeafaultCanvas.SetActive(true);
        vehicleSelectCanvas.SetActive(false);
        finalToStart = true;
        startToFinal = false;
    }

    public void upgradesCanvasButton()
    {
        upgradesCanvas.SetActive(true);
        vehicleSelectCanvas.SetActive(false);
    }

    /* ---------- misc ---------- */
    public void getCarInfo()
    {
        string carKey = listOfVehicles.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName.ToString();
        int price = listOfVehicles.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carPrice;

        if (carKey == PlayerPrefs.GetString(carKey))
        {
            carInfo.text = "Owned";
            startButton.SetActive(true);
            buyButton.SetActive(false);
        }
        else
        {
            carInfo.text = carKey + " $ " + price;
            startButton.SetActive(false);
            buyButton.SetActive(true);
        }

        UpdateCurrencyUI();
    }

    public void cameraTranzition()
    {
        if (startToFinal)
            CameraObject.transform.position =
                Vector3.Lerp(CameraObject.transform.position, finalCameraPosition.transform.position, lerpTime * Time.deltaTime);

        if (finalToStart)
            CameraObject.transform.position =
                Vector3.Lerp(CameraObject.transform.position, startCameraPosition.transform.position, lerpTime * Time.deltaTime);
    }

    /* ---------- scene loads ---------- */
    public void loadMarioMap() => SceneManager.LoadScene("superMarioMap");
    public void loadComunityMap() => SceneManager.LoadScene("ComunityMap");
}
