namespace Platform2DUtils.GameplaySystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class GameplaySystem : MonoBehaviour
    {
        ///<summary>
        /// Returns a Vector2 with Horizontal and Vertical axes.
        ///</summary>
        public static Vector2 Axis
        {
            get => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        ///<summary>
        /// Moves player in Horizontal axis with keyboard inputs.
        ///</summary>
        ///<param name="t">Transform component of the player</param>
        ///<param name="moveSpeed">The coeficient of speed</param>
        public static void TMovement(Transform t, float moveSpeed)
        {
            t.Translate(Vector2.right * Axis.x * moveSpeed);
        }

        ///<summary>
        /// Moves player in Horizontal axis with keyboard inputs uisng force.
        ///</summary>
        ///<param name="rb2D">Rigidbody2D component of the player</param>
        ///<param name="moveSpeed">The coeficient of speed</param>
        ///<param name="maxVel">Maximum velocity of rigidbody on x component</param>
        public static void MovementAddForce(Rigidbody2D rb2D, float moveSpeed, float maxVel)
        {
            rb2D.AddForce(Vector2.right * moveSpeed * Axis.x, ForceMode2D.Impulse);
            float velXClamp = Mathf.Clamp(rb2D.velocity.x, -maxVel, maxVel);
            rb2D.velocity = new Vector2(velXClamp, rb2D.velocity.y);
            if(Axis.x == 0)
            {
                rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
            }
        }

        ///<summary>
        /// Moves player in Horizontal axis with keyboard inputs uisng force.
        ///</summary>
        ///<param name="rb2D">Rigidbody2D component of the player</param>
        ///<param name="moveSpeed">The coeficient of speed</param>
        ///<param name="maxVel">Maximum velocity of rigidbody on x component</param>
        ///<param name="grounding">Detects if im touching groundf layer</param>
        public static void MovementAddForce(Rigidbody2D rb2D, float moveSpeed, float maxVel, bool grounding)
        {
            rb2D.AddForce(Vector2.right * moveSpeed * Axis.x, ForceMode2D.Impulse);
            float velXClamp = Mathf.Clamp(rb2D.velocity.x, -maxVel, maxVel);
            rb2D.velocity = new Vector2(velXClamp, rb2D.velocity.y);

            if (Axis.x == 0 && grounding)
            {
                rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
            }
        }

        ///<summary>
        /// Moves player in Horizontal axis with keyboard inputs using velocity.
        ///</summary>
        ///<param name="rb2D">Rigidbody2D component of the player</param>
        ///<param name="moveSpeed">The coeficient of speed</param>
        ///<param name="maxVel">Maximum velocity of rigidbody on x component</param>
        public static void MovementVelocity(Rigidbody2D rb2D, float moveSpeed, float maxVel)
        {
            rb2D.velocity = new Vector2(Axis.x * moveSpeed, rb2D.velocity.y);
            Vector2 clampedVelocity = Vector2.ClampMagnitude(rb2D.velocity, maxVel);
            rb2D.velocity = new Vector2(clampedVelocity.x, rb2D.velocity.y);
        }

        ///<summary>
        /// Moves player in Horizontal axis with keyboard inputs and multiplied by delta time.
        ///</summary>
        ///<param name="t">Transform component of the player</param>
        ///<param name="moveSpeed">The coeficient of speed</param>
        public static void TMovementDelta(Transform t, float moveSpeed)
        {
            t.Translate(Vector2.right * Axis.x * moveSpeed * Time.deltaTime);
        }

        ///<summary>
        /// Returns if jump button was buttondown.
        ///</summary>
        public static bool JumpBtn
        {
            get => Input.GetButtonDown("Jump");
        }

        ///<summary>
        /// Returns an array of players (and NPCs)
        ///</summary>
        public static Player[] FindPlayer 
        {
            get => FindObjectsOfType(typeof(Player)) as Player[];
        } 


        ///<summary>
        /// Moves player in the Vertical Axis with keyboard inputs using impulse
        /// <param name="jumpForce"></param>
        /// <param name="rb2D">Rigidbody Component of the gameObject</param>
        ///</summary>
        public static void Jump(Rigidbody2D rb2D, float jumpForce)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        ///<summary>
        /// Returns a normalized Vector2 with Horizontal and Vertical axes.
        ///</summary>
        public static Vector2 AxisTopdown
        {
            get => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized; 
        }

        ///<summary>
        /// Moves player in Horizontal and Vertial axis with keyboard inputs.
        ///</summary>
        ///<param name="t">Transform component of the player</param>
        ///<param name="moveSpeed">The coeficient of speed</param>
       public static void MovementTopdown(Transform t,float moveSpeed)
        {
             t.Translate(AxisTopdown * moveSpeed * Time.deltaTime, Space.World);
        }

        ///<summary>
        /// Makes the player jump in a topdown game by moving him/her in the Z axis and altering its
        //// scale to create the illusion of such jump.
        ///</summary>
        ///<param name="t">Transform component of the player</param>
        ///<param name="scale">Size coeficient of the player</param>
        public static void JumpTopdown(Transform t,float scale)
        {
            t.localScale =new Vector3(scale,scale,1.0f);
        }
        
        ///<summary>
        /// Checks the distance to the player in order to make the enemy chase him/her
        ///</summary>
        ///<param name="enemyTransform">Transform component of the enemy</param>
        ///<param name="target">Transform component of the player</param>
        ///<param name="chaseRadius">The distance from which the enemy will begin to chase the player</param>
        ///<param name="moveSpeed">The coeficient of the enemy's speed</param>
        public static void CheckDistance(Transform enemyTransform, Transform target, float chaseRadius, float moveSpeed)
        {
            if(target)
            {    
                if (Vector3.Distance(target.position, enemyTransform.position) <= chaseRadius)
                {
                    enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, target.position, moveSpeed * Time.deltaTime);
                }
            }
        }

        public static void Movement3D(Transform transform, float moveSpeed)
        {
              transform.Translate(Axis.normalized.magnitude * Vector3.forward * moveSpeed * Time.deltaTime);
              
        }

        public static  Vector3 Axis3D
        {
            get => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
/*
        public static  Vector3 Axis3DRotationPC
        {
            get => new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
        }

        public static  Vector3 Axis3DRotationConsole
        {
            get => new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
        }
*/
    }
}