using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrajectory3D : MonoBehaviour
{
    private LineRenderer lineRender;

    [SerializeField]
    private GameObject cannonBall;
    private Rigidbody cannonBallRG;

    private float force = 500;
    private float mass;
    private float fixdeDeltaTime;
    private float velocity;
    private float gravity;
    private float collisionCheckRadius = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        cannonBallRG = cannonBall.GetComponent<Rigidbody>();
        mass = cannonBallRG.mass;
        lineRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        DrawTrajectory();
        Movements();

        if (Input.GetKey(KeyCode.Space))
        {
            lineRender.enabled = true;
        }
        else
        {
            lineRender.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject ball = Instantiate(cannonBall, transform.position + transform.up, Quaternion.identity);
            Rigidbody ballRG = ball.GetComponent<Rigidbody>();
            ballRG.AddForce(transform.up * force);
        }
    }
    void DrawTrajectory()
    {
        lineRender.positionCount = SimulateArc().Count;

        for (int a = 0; a < lineRender.positionCount; a++)
        {
            lineRender.SetPosition(a, SimulateArc()[a]);
        }
    }
    private List<Vector3> SimulateArc()
    {
        List<Vector3> lineRendererPoints = new List<Vector3>();

        float maxDuration = 5f;
        float timeStepInterval = 0.1f;
        int maxSteps = (int)(maxDuration / timeStepInterval);

        Vector3 directionVector = transform.up;
        Vector3 lunchPosition = transform.position + transform.up;

        velocity = force / mass * Time.fixedDeltaTime;
        for(int i = 0; i< maxSteps; i++)
        {
            Vector3 calculatedPosition = lunchPosition + directionVector * velocity * i * timeStepInterval;
            calculatedPosition.y += Physics.gravity.y / 2 * Mathf.Pow(i * timeStepInterval, 2);
            lineRendererPoints.Add(calculatedPosition);
            if(CheckForCollision(calculatedPosition))
            {
                break;
            }
        }
        return lineRendererPoints;
    }
    private bool CheckForCollision(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, collisionCheckRadius);
        if (hits.Length > 0)
        {
            return true;
        }
        return false;
    }
    private void Movements()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);

        //transform.Translate(direction * 5 * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, -0.5f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, 0.5f);
        }
    }
}