using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour { 
        //Check every frame for player input
        //Apply input every frame as movement
        private Rigidbody rb;
        public float speed = 20;  //should show up in the inspector
        public float jumpForce = 50; //should show up in the inspector
        public float poundForce = 80; //should show up in the inspector
        public float dashForce = 50;
        public string jumpButton;
        public string horizontalButton;
        public string verticalButton;
        public string dashButton;
        public int DefaultDashDuration = 10;
        public int DefaultDashCooldown = 25;
        private int DashDuration;
        private int DashCooldown;
        private bool DashInCooldown = false;
        private AudioSource footSteps;
        private AudioSource impacts;
        private Vector3 startPosition; //save the starting position of the player
        public int PlayerNum = -1;
        private GameObject Sumo;

        //Called on the first frame the script is active, often first frame of game
        void Start()
        {
            footSteps = gameObject.AddComponent<AudioSource>();
            footSteps.clip = Resources.Load<AudioClip>("Sounds/Footsteps");
            impacts = gameObject.AddComponent<AudioSource>();
            impacts.clip = Resources.Load<AudioClip>("Sounds/BallLanding");
            rb = GetComponent<Rigidbody>();
            startPosition = GetComponent<Rigidbody>().position;

            DashDuration = 0;
            DashCooldown = DefaultDashCooldown;
            
            gameObject.GetComponent<Renderer>().material.color = PlayerColor();
        }

        public void LinkSumo(GameObject sumo)
        {
            Sumo = sumo;
        }

        private void Update()
        {
            HandleDashCooldownAndDuration();
            HandlePlayerFall();
            
            if (jumpButton == null)
            {
                if (Sumo != null)
                {
                    Sumo.transform.position = rb.position;
                    Sumo.transform.position += new Vector3(0, -0.75f, 0);
                }
                return;
            }
            if (Input.GetButtonDown(jumpButton))
            {
                Jump();
            }

            if (Sumo != null)
            {
                Sumo.transform.position = rb.position;
                Sumo.transform.position += new Vector3(0, -0.75f, 0);
            }
        }

        public void Jump()
        {
            RaycastHit hit;
            var player = GetComponent<Rigidbody>();
            var origin = player.position;

            var ray = new Ray(origin, Vector3.down);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Surface" && hit.distance <= 1)
                {
                    if (impacts != null) { impacts.Play(); }
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                else
                {
                    rb.velocity = new Vector3();
                    rb.AddForce(Vector3.down * poundForce, ForceMode.Impulse);
                }
            }
        }

        public void Dash(Vector3 movement, float speed)
        {
            Debug.Log("Dash: " + DashDuration);
            if(DashDuration > 0)
            {
                DashDuration -= 1;

                rb.AddForce(movement * (speed + dashForce), ForceMode.Acceleration);
            } else
            {
                rb.AddForce(movement * speed);
            }
        }

        private void HandlePlayerFall()
        {
            if (rb.position.y < -15)
            {
                // lose a point for falling off.
                //var bodies = GetComponents<Rigidbody>();
                //foreach (var goal in bodies)
                //{
                //    Debug.Log(goal.tag);
                //    if (goal.tag == "Objective")
                //    {
                //        var list = goal.GetComponents<MonoBehaviour>();
                //        foreach (var item in list)
                //        {
                //            if (item is GoalBall)
                //            {
                //                Debug.Log((item as GoalBall).ScoreList[PlayerNum - 1]);
                //                (item as GoalBall).ScoreList[PlayerNum - 1] -= 1;
                //                (item as GoalBall).UpdateLeaderboard();
                //            }
                //        }

                //    }
                //}

                Vector3 resetPos = new Vector3(startPosition.x, startPosition.y, startPosition.z);
                resetPos.y += 5f;
                rb.velocity = new Vector3();
                rb.position = resetPos;
                if (Sumo != null)
                {
                    Sumo.transform.position = rb.position;
                    Sumo.transform.position += new Vector3(0, -0.75f, 0);
                }
                return;
            }
        }

        private void HandleDashCooldownAndDuration()
        {
            if(DashInCooldown && DashCooldown > 0)
            {
                DashCooldown -= 1;
            }
            else
            {
                if (DashDuration == 0 && !DashInCooldown)
                {
                    DashInCooldown = true;
                    DashCooldown = DefaultDashCooldown;
                } else if (DashDuration < DefaultDashDuration) {
                    DashDuration += 1;
                }
            }

            if (DashCooldown == 0 && DashInCooldown)
            {
                DashDuration = DefaultDashDuration;
                DashInCooldown = false;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            MonoBehaviour[] list = other.gameObject.GetComponents<MonoBehaviour>();
            //print("Checking behaviors:" + list.ToString());
            foreach (MonoBehaviour mb in list)
            {
                if (mb is GoalBall)
                {
                    if ((mb as GoalBall).CurrentOwner != PlayerNum)
                    {
                        (mb as GoalBall).CurrentOwner = PlayerNum;
                        other.gameObject.GetComponent<Renderer>().material.color = PlayerColor();
                    }
                }
            }
        }

        private Color PlayerColor()
        {

            Color myColor = Color.black;
            myColor.a = 0.15f;

            if (PlayerNum == 1) { 
            myColor.b = 1f;
            }
            else if (PlayerNum == 2)
            {
                myColor.r = 1f;
            }
            else if (PlayerNum == 3)
            {
                myColor.r = 1f;
                myColor.g = 1f;
                myColor.b = 1f;
            }
            else if (PlayerNum == 4)
            {
            }
            else { 
                    myColor = Color.yellow;
            }
            return myColor;
        }

        private void FixedUpdate()  //called before applying physics, movement code
        {
            float moveHorizontal = Input.GetAxis(horizontalButton);
            float moveVertical = Input.GetAxis(verticalButton);
            if (Mathf.Abs(moveHorizontal) > .75 || Mathf.Abs(moveVertical) > .75)
            {
                if(Sumo != null)
                {
                    Sumo.transform.eulerAngles = new Vector3(Sumo.transform.eulerAngles.x, (Mathf.Atan2(moveVertical*-1, moveHorizontal) * Mathf.Rad2Deg)+90, Sumo.transform.eulerAngles.z);
                }
                //print("horiz = " + moveHorizontal + " and vert = " + moveVertical);
                if (footSteps != null && !footSteps.isPlaying)
                {
                    footSteps.Play();
                }
            }
            else
            {
                if (footSteps != null && footSteps.isPlaying)
                {
                    footSteps.Stop();
                }
            }
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);  //determine force added to ball

            if (Input.GetButton(dashButton))
            {
                Dash(movement, speed);
            } else
            {
                rb.AddForce(movement * speed);
            }

        }
    } 
}
