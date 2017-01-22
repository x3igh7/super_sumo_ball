using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour { 
        //Check every frame for player input
        //Apply input every frame as movement
        private Rigidbody rb;
        public float speed = 20;  //should show up in the inspector
        public float jumpForce = 50; //should show up in the inspector
        public float poundForce = 100; //should show up in the inspector
        public string jumpButton;
        public string horizontalButton;
        public string verticalButton;
        private AudioSource footSteps;
        private AudioSource impacts;
        private Vector3 startPosition; //save the starting position of the player
        public int PlayerNum = -1;

        //Called on the first frame the script is active, often first frame of game
        void Start()
        {
            footSteps = gameObject.AddComponent<AudioSource>();
            footSteps.clip = Resources.Load<AudioClip>("Sounds/Footsteps");
            impacts = gameObject.AddComponent<AudioSource>();
            impacts.clip = Resources.Load<AudioClip>("Sounds/BallLanding");
            rb = GetComponent<Rigidbody>();
            startPosition = GetComponent<Rigidbody>().position;
        }

        private void Update()
        {
            if (rb.position.y < -15)
            {
                Vector3 resetPos = new Vector3(startPosition.x, startPosition.y, startPosition.z);
                resetPos.y += 10f;
                rb.position = resetPos;
                rb.velocity = new Vector3();
                return;
            }
            if (jumpButton == null)
            {
                return;
            }
            if (Input.GetButtonDown(jumpButton))
            {
                Jump();
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
                    }
                }
            }
        }

        private void FixedUpdate()  //called before applying physics, movement code
        {
            float moveHorizontal = Input.GetAxis(horizontalButton);
            float moveVertical = Input.GetAxis(verticalButton);
            if (Mathf.Abs(moveHorizontal) > .75 || Mathf.Abs(moveVertical) > .75)
            {
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

            rb.AddForce(movement * speed);
        }
    } 
}
