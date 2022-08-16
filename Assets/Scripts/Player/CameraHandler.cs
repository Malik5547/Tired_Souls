using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;

        public Transform targetTransform;
        public Transform targetTransformWhileAiming;    //The transform camera folows
        public Transform cameraTransform;
        public Camera cameraObject;
        public Transform cameraPivotTransform;

        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask enviormentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float leftAndRightLookSpeed = 250f;
        public float leftAndRightAimingLookSpeed = 25f;
        public float followSpeed = 1f;
        public float upAndDownLookSpeed = 250f;
        public float upAndDownAimingLookSpeed = 25f;

        private float targetPosition;
        private float defaultPosition;

        private float leftAndRightAngle;
        private float upAndDownAngle;

        public float minimumLookUpAngle = -35;
        public float maximumLookUpAngle = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;

        List<CharacterManager> avilableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget = null;
        public CharacterManager rightLockTarget = null;
        public float maximumLockOnDistance = 30;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();

            singleton = this;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);

            inputHandler = FindObjectOfType<InputHandler>();
            cameraObject = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            //enviormentLayer = LayerMask.NameToLayer("Enviorment");
        }

        public void FollowTarget()
        {
            if (playerManager.isAiming)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;

            }
            else
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, Time.deltaTime / followSpeed);
                transform.position = targetPosition;
            }
            
            HandleCameraCollisions(Time.deltaTime);
        } 

        public void HandleCameraRotation()
        {
            if(inputHandler.lockOnFlag  && currentLockOnTarget != null)
            {
                HandleLockedCameraRotation();
            } 
            else if (playerManager.isAiming)
            {
                HandleAimedCameraRotation();
            }
            else
            {
                HandleStandardCameraRotation();
            }
        }

        public void HandleStandardCameraRotation()
        {
            leftAndRightAngle += inputHandler.mouseX * leftAndRightLookSpeed * Time.deltaTime;
            upAndDownAngle -= inputHandler.mouseY * upAndDownLookSpeed * Time.deltaTime;
            upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumLookUpAngle, maximumLookUpAngle);

            Vector3 rotation = Vector3.zero;
            rotation.y = leftAndRightAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = upAndDownAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        public void HandleLockedCameraRotation() 
        {
            
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
            
        }

        private void HandleAimedCameraRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

            Quaternion targetRotationX;
            Quaternion targetRotationY;

            Vector3 cameraRotationX = Vector3.zero;
            Vector3 cameraRotationY = Vector3.zero;

            leftAndRightAngle += (inputHandler.mouseX * leftAndRightAimingLookSpeed) * Time.deltaTime;
            upAndDownAngle -= (inputHandler.mouseY * upAndDownAimingLookSpeed) * Time.deltaTime;

            cameraRotationY.y = leftAndRightAngle;
            targetRotationY = Quaternion.Euler(cameraRotationY);
            targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
            transform.localRotation = targetRotationY;

            cameraRotationX.x = upAndDownAngle;
            targetRotationX = Quaternion.Euler(cameraRotationX);
            targetRotationX = Quaternion.Slerp(cameraTransform.localRotation, targetRotationX, 1);
            cameraTransform.localRotation = targetRotationX;
        }

        public void ResetAimCameraRotations()
        {
            cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //Handle collisions
        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition)))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 26);

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();
                CharacterStatsManager characterStats = colliders[i].GetComponent<CharacterStatsManager>();

                if(character != null && characterStats != null && !characterStats.isDead)
                {
                    Vector3 lockTargetDirection = character.transform.position - transform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAangle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if(character.transform.root != targetTransform.transform.root && 
                        viewableAangle > -50 && viewableAangle < 50 && 
                        distanceFromTarget <= maximumLockOnDistance)
                    {

                        if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            if((enviormentLayer & (1 << hit.collider.gameObject.layer)) != 0)
                            {
                                Debug.Log("Object in the way");
                            }
                            else
                            {
                                
                                avilableTargets.Add(character);

                            }
                        }
                    }
                }
            }

            for(int i = 0; i < avilableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(transform.position, avilableTargets[i].transform.position); 

                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = avilableTargets[i];
                }

                if (inputHandler.lockOnFlag)
                {
                    
                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(avilableTargets[i].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0f && distanceFromLeftTarget > shortestDistanceOfLeftTarget 
                        && avilableTargets[i] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = avilableTargets[i];
                    }
                    else if(relativeEnemyPosition.x >= 0f && distanceFromRightTarget < shortestDistanceOfRightTarget
                        && avilableTargets[i] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = avilableTargets[i];
                    }
                }

            }
        }

        public void ClearLockOnTargets()
        {
            avilableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }

}
