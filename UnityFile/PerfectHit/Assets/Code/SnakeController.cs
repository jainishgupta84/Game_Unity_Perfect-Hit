using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour {

    // Settings
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public int Gap = 10;
    public int Force;
    public int a;
    float TouchRightVal;


    // References
    public GameObject BodyPrefab;

    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start() {
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
    }

    // Update is called once per frame
    void FixedUpdate() {

        // Move forward
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right * Time.deltaTime * Force;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * Time.deltaTime * Force;
            }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            TouchRightVal = touch.deltaPosition.x;
            transform.position += transform.right * TouchRightVal * Time.deltaTime;
        }

        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts) {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }

        if(transform.position.y < -1)
        {
            Time.timeScale = 0;
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        GrowSnake();
    }

    private void GrowSnake() {
        // Instantiate body instance and
        // add it to the list
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }
}