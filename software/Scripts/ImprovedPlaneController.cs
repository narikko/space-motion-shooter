using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO.Ports;


public class ImprovedPlaneController : MonoBehaviour
{
    public float thorttleIncrement = 0.1f;
    public float maxThrottle = 200f;
    public float turnEffectiveness = 100f;
    public float maxAngularVelocity = 10f;
    public float rollCorrectionSpeed = 3f;



    public float rollVelocity = 1f;

    private float throttle; // percentage of max engine
    private float roll; // left to right
    private float pitch; // front to back
    private float yaw; // left to right


    //Serial Port
    private SerialPort serialPort;
    public string portName = "COM7"; // Change to your COM port
    public int baudRate = 9600;

    public float time = 0;
    

    private Rigidbody rb;
    private PlayerControl pController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pController = new PlayerControl();
        pController.Airplane.Enable();

        /*
         * 
        serialPort = new SerialPort("COM7", baudRate)
        {
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            ReadTimeout = 1000,
            WriteTimeout = 1000
        };



        serialPort.Open();
        */

    }
    // Update is called once per frame

    bool fire = false;
    private void HandleInputs()
    {
        Vector3 inputs = pController.Airplane.ImprovedMovement.ReadValue<Vector3>();

        yaw = inputs.z *200;
        pitch = inputs.x *200;
        roll = inputs.y *200;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fire= true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            fire = false;
        }

        if (fire)
        {
            shooting();
        }

        time += Time.deltaTime;

        if(time > 1)
        {
   
            time = 0;

        }

        throttle = maxThrottle;

    }

    private void Update()
    {
        HandleInputs();

        try
        {
            // Check if there is data to read
            string data = serialPort.ReadLine();
            //ParseReceivedData(data);

            if(roll > -5 && roll < 5)
            {
                roll = 0;
            }

            if(pitch > -5 && pitch < 5)
            {
                pitch = 0;
            }

            if(pitch == 0 && roll == 0)
            {

                AccelerateToZeroRotation();
            }


        }
        catch (System.Exception e)
        {
            // Ignore timeout exceptions
        }


    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * throttle);
        rb.AddTorque(transform.up *  yaw * turnEffectiveness);
        rb.AddTorque(transform.right * roll * turnEffectiveness);

        //rb.angularVelocity = new Vector3(rb.angularVelocity.x , rb.angularVelocity.y , pitch * rollVelocity);
        rb.AddTorque(transform.forward * pitch * turnEffectiveness );

        if (rb.angularVelocity.magnitude > maxAngularVelocity)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxAngularVelocity;
        }

        rb.angularVelocity *= 0.9f;

    }

    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }

    private void ParseReceivedData(string data)
    {
        try
        {
            // Remove the "Received: " prefix
            string trimmedData = data.Replace("Received: ", "").Trim();

            // Split the string into individual components
            string[] components = trimmedData.Split(',');

            // Extract and parse pitch
            string pitchString = components[0].Split(':')[1].Trim(); // Get the value part of "APitch: 0.39"
            pitch = float.Parse(pitchString);

            // Extract and parse roll
            string rollString = components[1].Split(':')[1].Trim(); // Get the value part of "ARoll: -0.17"
            roll = float.Parse(rollString);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to parse data: {ex.Message}");
        }
    }

    private void AccelerateToZeroRotation()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        rb.AddTorque(transform.up * rotation.y * turnEffectiveness);
        rb.AddTorque(transform.right * rotation.x * turnEffectiveness);

    }

    [Header("Shooter Setting")]

    public float bulletSpeed = 10000f;
    public float attackCooldown = 0.2f;
    public Bullet bullet;
    public Transform[] firePositions;


    private int shooter = 0;
    private float shootime = 0f;
    private void shooting()
    {
        shootime += Time.deltaTime;
        if(shootime > attackCooldown)
        {
            shootime = 0;
            shooter = (shooter + 1) % 2;
            Bullet newBullet = Instantiate(bullet);
            newBullet.transform.position = firePositions[shooter].position;
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

}
