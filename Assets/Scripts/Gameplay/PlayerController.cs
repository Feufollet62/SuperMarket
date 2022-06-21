using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Script
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [Header("Info")]
        public int id;
        // Remplacer par scriptableojbect maybe ?
        [SerializeField] private Material[] playerMats;
        [SerializeField] private Transform model;
        [SerializeField] public Transform grabPoint;

        [Header("Mouvement")]
        [SerializeField] private float maxAccel = 30f;
        [SerializeField] private float maxAirAccel = 10f;
        [SerializeField] private float maxSpeed = 8f;

        [SerializeField] [Range(0, 1)] private float minGroundDotProduct = .9f;

        [SerializeField] private float dashStrength = 1f;
        [SerializeField] private float dashLength = .5f;

        [SerializeField] private float throwStrengthItem = 3f;
        [SerializeField] private float throwStrengthPlayer = 5f;

        [Header("Animation")]
        [SerializeField] private float animIntensity = 2f;
        [SerializeField] private float animMaxAngle = 20f;

        public bool controllable = true;
        public bool grabbing = false;

        // Private
        private Rigidbody _rb;
        private ParticleSystem _particleSystem;

        public List<Interactible> interactibles;
        public Interactible _grabbedObject;

        private Vector3 _velocity, _desiredVelocity, _inputMovement;
        private Vector3 contactNormal;

        private int groundContactCount, stepsSinceGrounded;
        private bool Grounded => groundContactCount > 0;

        //public bool _inputDash;
        private bool _inputInteract, _inputDash;
        public bool _dashing;
        private float _dashTimer;

        private float _animTimer;

        #endregion

        #region Built-in Functions

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();

            // Assigne les bons matériaux aux bons joueurs
            GetComponentInChildren<MeshRenderer>().material = playerMats[id];
        }

        private void Update()
        {
            GetInput();
            Animate();
        }

        private void FixedUpdate()
        {
            stepsSinceGrounded++;

            // Si on est pas grounded alors on tente SnapToGround()
            if (Grounded || SnapToGround())
            {
                stepsSinceGrounded = 0;
                if (groundContactCount > 1)
                {
                    // Si jamais on touche plusieurs colliders de sol, on fait la moyenne
                    contactNormal.Normalize();
                }
                Dash();
            }

            if (!_dashing) Move();
            else
            {
                // Pas de contact au sol
                contactNormal = Vector3.up;
            }

            Interact();

            // Doit rester à la fin
            ClearState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            EvaluateCollision(collision);
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            EvaluateCollision(collisionInfo);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check si c'est un objet ramassable, si oui on l'ajoute à la liste d'objets ramassables 
            if (other.gameObject.layer == LayerMask.NameToLayer("Interactible") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Interactible thisInteractible = other.GetComponent<Interactible>();

                if (!interactibles.Contains(thisInteractible))
                {
                    interactibles.Add(thisInteractible);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Interactible") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Interactible thisInteractible = other.GetComponent<Interactible>();

                if (interactibles.Contains(thisInteractible))
                {
                    interactibles.Remove(thisInteractible);
                }
            }
        }

        #endregion

        #region Custom Functions

        // Focntionnement input system: https://youtu.be/5tOOstXaIKE
        public void OnMovement(InputAction.CallbackContext context)
        {
            if (controllable)
            {
                Vector2 rawInput = context.ReadValue<Vector2>();
                _inputMovement = new Vector3(rawInput.x, 0, rawInput.y);
            }
            else
            {
                _inputMovement = Vector3.zero;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            // True si controllable = true et context.action.triggered = true
            _inputDash = controllable && context.action.triggered;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            _inputInteract = controllable && context.action.triggered;
        }

        public void SetControllable(bool isControllable)
        {
            if (isControllable)
            {
                controllable = true;
            }
            else
            {
                controllable = false;
                _velocity = Vector3.zero;
            }
        }

        private void GetInput()
        {
            _desiredVelocity = _inputMovement * maxSpeed;
        }

        private void Move()
        {
            // Ci dessous code pour prendre les pentes de manière smooth
            // L'idée est de projeter le vecteur de velocité sur le plan du sol actuel
            // Comme ça on se déplace le long de la pente

            _velocity = _rb.velocity;

            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentX = Vector3.Dot(_velocity, xAxis);
            float currentZ = Vector3.Dot(_velocity, zAxis);

            float accel = Grounded ? maxAccel : maxAirAccel;
            float maxSpeedChange = accel * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

            _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

            _rb.velocity = _velocity;
        }

        private void Dash()
        {
            // On dash déjà
            if (_dashing)
            {
                _dashTimer += Time.fixedDeltaTime;
                if (_dashTimer >= dashLength)
                {
                    _dashing = false;
                }
                return;
            }

            // Pas d'input
            if (!_inputDash || _inputMovement == Vector3.zero) return;

            // 1ere frame de dash
            _inputDash = false;
            _dashing = true;
            _dashTimer = 0f;

            _rb.AddForce(_velocity.normalized * dashStrength, ForceMode.Impulse);
        }

        private void Interact()
        {
            // Si pas d'input osef
            if (!_inputInteract) return;

            _inputInteract = false;

            if (InteractWithClosest())
            {
                return;
            }

            // Rien est interactible, alors on lance
            if (grabbing)
            {
                Throw();
            }
        }

        private bool InteractWithClosest()
        {
            // Si rien est à portée, rien ne se passe 
            if (interactibles.Count == 0) return false;

            // On trie la liste du plus proche au plus éloigné
            interactibles.Sort((a, b) =>
                      Vector2.Distance(transform.position, a.transform.position).
            CompareTo(Vector2.Distance(transform.position, b.transform.position)));

            foreach (Interactible interactible in interactibles)
            {
                if (interactible.Interact(this)) return true;
            }

            return false;
        }
        public void DropItem()
        {
            if (!grabbing) return;
            _grabbedObject = null;
            grabbing = false;

        }

        private void Throw()
        {
            if (!grabbing) return;

            Vector3 throwVector = new Vector3(model.forward.x, 0, model.forward.z) + _velocity;

            // Force dépend de si joueur bouge ou non
            if (_grabbedObject.type == InteractType.Player)
            {
                throwVector *= throwStrengthPlayer;
            }
            else
            {
                throwVector *= throwStrengthItem;
            }

            _grabbedObject.Throw(throwVector);
            _grabbedObject = null;
            grabbing = false;
        }

        private void Animate()
        {
            // Tout cela est temp et va disparaître quand on aura de vraies anims

            // Pas d'input
            if (_inputMovement == Vector3.zero)
            {
                model.Rotate(0, 0, 0);
                _particleSystem.Stop();
                _animTimer = 0;
                return;
            }

            // Effet de particule sur timer, à remplacer avec event d'animation
            _animTimer += Time.deltaTime;
            if (_animTimer > .5f)
            {
                _particleSystem.Play();
                _animTimer = 0f;
            }

            // On retrouve l'accélération
            Vector3 accel = _inputMovement * maxSpeed - _rb.velocity;

            model.forward = _rb.velocity;
            float angle = accel.magnitude * animIntensity;

            Mathf.Clamp(0, animMaxAngle, angle);

            model.Rotate(-angle, 0, 0);
        }

        private void EvaluateCollision(Collision col)
        {
            for (int i = 0; i < col.contactCount; i++)
            {
                Vector3 normal = col.GetContact(i).normal;

                // On check si c'est du sol; si oui, on l'ajoute pour faire la "moyenne" de toutes les collisions
                if (normal.y >= minGroundDotProduct)
                {
                    groundContactCount += 1;
                    contactNormal += normal;
                }
            }
        }

        private void ClearState()
        {
            groundContactCount = 0;
            contactNormal = Vector3.zero;
        }

        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - contactNormal * Vector3.Dot(vector, contactNormal);
        }

        private bool SnapToGround()
        {
            // Return sert à savoir si suite à la focntion on est au sol ou non

            // On a décollé depuis longtemps ou pas ?
            if (stepsSinceGrounded > 1)
            {
                return false;
            }

            // Quelque chose sous les pieds ?
            if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                return false;
            }

            // Sol ou mur ?
            if (hit.normal.y < minGroundDotProduct)
            {
                return false;
            }

            // On ajuste la vélocité pour qu'elle s'aligne avec le sol en dessous

            groundContactCount = 1;
            contactNormal = hit.normal;
            float speed = _velocity.magnitude;
            float dot = Vector3.Dot(_velocity, hit.normal);

            if (dot > 0f) // On ajuste la vélocité seulement si on "décolle" (Exemple: on passe le sommet d'une rampe)
            {
                _velocity = (_velocity - hit.normal * dot).normalized * speed;
                _rb.velocity = _velocity;
                Debug.DrawRay(transform.position, _velocity);
            }

            return true;
        }

        #endregion
    }
}