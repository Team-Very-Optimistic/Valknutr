using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 12f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;
		[SerializeField] private AnimationCurve m_Curve;

		Rigidbody m_Rigidbody;
		Animator m_Animator;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;
		private bool m_Dashing;
		private bool m_CastingProjectile;
		private bool m_CastingShield;
		private bool m_CastingBomb;
		public Transform transformChild;
		private static readonly int CastingBomb = Animator.StringToHash("CastingBomb");
		private static readonly int CastingProjectile = Animator.StringToHash("CastingProjectile");
		private static readonly int CastingShield = Animator.StringToHash("CastingShield");
		private static readonly int CastingDash = Animator.StringToHash("CastingDash");
		private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
		private static readonly int Forward = Animator.StringToHash("Forward");
		private static readonly int Turn = Animator.StringToHash("Turn");
		private static readonly int OnGround = Animator.StringToHash("OnGround");
		private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");

		public void Dash(float dashTime, float dashSpeed, Vector3 direction)
		{
			if (!m_Dashing)
			{
				m_Animator.applyRootMotion = false;
				StartCoroutine(routine: Dashing(dashTime, dashSpeed, direction));
			}
		}

		private IEnumerator Dashing(float dashTime, float dashSpeed, Vector3 direction)
		{
			float t = 0;
			float timeInterval = 0.02f;
			m_Dashing = true;
			Vector3 startRotation = transformChild.localEulerAngles;
			Vector3 endRotation = startRotation + new Vector3(360, 0, 0);
			var atan2 = (float) ((180f / Math.PI * Math.Atan2(direction.x, direction.z)) % 360f);
			transform.eulerAngles = new Vector3();
			transform.RotateAround(transform.position, Vector3.up, atan2);
			m_Crouching = true;
			// m_ForwardAmount = 1;
			while (t < dashTime)
			{
				t += timeInterval;
				UpdateAnimator(direction);
				m_Rigidbody.velocity = direction * (dashSpeed * m_Curve.Evaluate(t / dashTime));
				
				//make the 360 to turn before landing
				if (t  <= dashTime / 1.2f)
				{
					transformChild.localEulerAngles = Vector3.Lerp(startRotation, endRotation, t / (dashTime/ 1.3f));
				}
				else
				{
					m_Dashing = false;
					m_Crouching = false;

				}
				
				yield return new WaitForSeconds(timeInterval);
				//var position = transform.position;
				// position +=  direction * (dashSpeed * m_Curve.Evaluate(t / dashTime));
				// transform.position = position;
			}
			m_Dashing = false;
			m_Crouching = false;

			yield return null;
		}

		
		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
		}


		public void Move(Vector3 move, bool crouch, bool jump, bool dashing =false)
		{
			
			if (IsDisabled())
			{
				return;
			}
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();
			HandleGroundedMovement(crouch, jump);
			//control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				
			}
			else
			{
				HandleAirborneMovement();
			}

			//ScaleCapsuleForCrouching(crouch);
			//PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat(Forward, m_ForwardAmount, 0.0f, Time.deltaTime);
			m_Animator.SetFloat(Turn, m_TurnAmount, 0.0f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool(OnGround, m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat(JumpLeg, jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				//m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
			
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && !m_Dashing && Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
			
		}


		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				if (!m_Dashing)
					m_Animator.applyRootMotion = true;
			}
			else
			{
				//m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}

		public void CastPoint()
		{
			ClearCastingAnimation();
		}

		public bool IsDisabled()
		{
			return m_CastingBomb || m_CastingProjectile || m_CastingShield || m_Dashing;
		}

		public void SetCastingAnimation(CastAnimation animationType, float speed = 1f)
		{
			m_Animator.applyRootMotion = false;
			switch (animationType)
			{
				case CastAnimation.Bomb:
					m_CastingBomb = true;
					m_Animator.SetBool(CastingBomb, true);
					break;
				case CastAnimation.Projectile:
					m_CastingProjectile = true;
					//m_Animator.SetBool(CastingProjectile, true);
					m_Animator.SetFloat(AnimationSpeed, speed);
					m_Animator.Play("Projectile");
					break;
				case CastAnimation.Shield:
					m_CastingShield = true;
					m_Animator.SetBool(CastingShield, true);
					break;
				case CastAnimation.Movement:
					//m_Dashing = true;
					//m_Animator.SetBool(CastingDash, true);
					m_Animator.Play("Post");
					break;
				default:
					m_Animator.Play("Post");
					break;
			}
		}

		public void ClearCastingAnimation()
		{
			m_Animator.applyRootMotion = true;
			m_CastingBomb = false;
			m_CastingProjectile = false;
			m_CastingShield = false;
			m_Dashing = false;
			m_Animator.SetFloat(AnimationSpeed, 1f);
			m_Animator.SetBool(CastingBomb, false);
			m_Animator.SetBool(CastingProjectile, false);
			m_Animator.SetBool(CastingShield, false);
			m_Animator.SetBool(CastingDash, false);
		}

		public void StopMovement()
		{
			m_ForwardAmount = 0;
			m_Animator.SetFloat(Forward, m_ForwardAmount, 0.0f, Time.deltaTime);
		}
	}
}
