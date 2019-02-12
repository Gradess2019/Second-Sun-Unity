using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Character
{
	private const float ROTATION_SPEED = 40;

	[SerializeField]
	private float energy = 100f;

	[SerializeField]
	private float energyDecrement = 1f;

	[SerializeField]
	private float stamina = 100f;

	[SerializeField]
	private float jumpPower;

	[SerializeField]
	private float hunger = 100f;

	[SerializeField]
	private float thirst = 100f;

	[SerializeField]
	private Joystick joystickMovement;

	[SerializeField]
	private Joystick joystickRotation;

	[SerializeField]
	private LootViewer lootViewer;

	[SerializeField]
	private NoiseTrigger noiseTrigger;

	[SerializeField]
	private SmellTrigger smellTrigger;

	private Dictionary<String, Container> containers;
	private Dictionary<String, Loot> items;
	private Dictionary<String, Skill> skills;

	private Vector3 velocity = Vector3.zero;
	private Vector3 startPlayerPosition;

	private Loot lootInMainHand;
	private Loot lootInAdditionalHand;

	public float Stamina { get => stamina; }
	public float Energy { get => energy; }

	private void Start()
	{
		speed /= 10;

		containers = new Dictionary<String, Container>();
		startPlayerPosition = transform.position;

		//Implement later
		smellTrigger.SetTriggerRadius(maxSmellRadius);

		//Test
		lootInMainHand = GetComponent<AttackTool>();
	}

	protected override void Update()
	{
		base.Update();
		Debug.Log(grabNum);
		DecreaseEnergy();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!isDead)
		{
			Move();

			velocity.x = Input.GetAxis("Horizontal");
			velocity.z = Input.GetAxis("Vertical");
			
			if (velocity.x != 0 || velocity.z != 0)
			{
				
				velocity.Normalize();
				Vector3 moveDirection = velocity * speed * Time.deltaTime * 20;

				if (IsCaught())
				{
					moveDirection /= 2f;
				}
				controller.Move(moveDirection);
				transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0f);
			}

			noiseTrigger.SetTriggerRadius(noiseRadius);
			noiseTrigger.CreateEvent();
			smellTrigger.CreateEvent();
		}
	}

	public void AddContainer(Container container)
	{
		if (!containers.ContainsKey(container.name))
		{
			containers.Add(container.name, container);
			lootViewer.AddContainerToView(container);
		}
	}

	public void RemoveContainer(Container container)
	{
		if (containers.ContainsKey(container.name))
		{
			containers.Remove(container.name);
			lootViewer.RemoveContainerFromView(container);
		}
	}

	protected override void Move()
	{
		Movement();
		Rotation();
	}

	protected override void Die()
	{
		base.Die();
		this.enabled = false;
		smellTrigger.ClearList();
		noiseTrigger.ClearList();
		Invoke("Respawn", 2f);
	}

	protected override void OnDestroy()
	{
		// Debug.Log("Player was destroyed");
	}

	public override IEnumerator Attack()
	{
		isAttacking = true;
		if (lootInMainHand is AttackTool)
		{
			AttackTool primaryWeapon = (AttackTool)lootInMainHand;
			yield return new WaitForSeconds(primaryWeapon.AttackSpeed);

			List<Character> characters = new List<Character>();

			for (int i = -primaryWeapon.NumOfRays / 2; i <= primaryWeapon.NumOfRays / 2; i++)
			{
				Vector3 rayDirection = (Quaternion.AngleAxis((primaryWeapon.Angle / primaryWeapon.NumOfRays) * i, Vector3.up)) * transform.forward;
				Debug.DrawRay(transform.position, rayDirection * primaryWeapon.AttackRange, Color.red, 3f);

				Ray ray = new Ray(transform.position, rayDirection);
				RaycastHit hit;
				LayerMask layerMask = LayerMask.GetMask("Raycast Block");
				if (Physics.Raycast(ray, out hit, primaryWeapon.AttackRange, layerMask))
				{
					Character overlappedCharacter = hit.collider.GetComponent<Character>();
					if (overlappedCharacter && !characters.Contains(overlappedCharacter))
					{
						characters.Add(overlappedCharacter);
						overlappedCharacter.DecreaseHealth(primaryWeapon.Damage);
					}
				}
			}
		}
		
		isAttacking = false;
	}

	private void Respawn()
	{
		transform.position = startPlayerPosition;
		// grabNum = 0;
		health = 100f;
		energy = 100f;
		stamina = 100f;
		this.enabled = true;
		isDead = false;
	}

	private void Movement()
	{
		if (joystickMovement)
		{
			if (joystickMovement.IsPressed)
			{
				Vector3 joystickMovementData = joystickMovement.GetInput();

				if (joystickMovementData == Vector3.zero)
				{
					return;
				}

				Vector3 moveDirection = joystickMovementData * Time.deltaTime * speed;
				moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

				if (IsCaught())
				{
					moveDirection /= (2f * grabNum);
				}

				float staminaDecrement = joystickMovementData.magnitude / joystickMovement.MaxPower;
				noiseRadius = maxNoiseRadius * joystickMovementData.magnitude / joystickMovement.MaxPower;

				if (joystickRotation && !joystickRotation.IsPressed)
				{
					controller.Move(moveDirection);
					DecreaseStamina(staminaDecrement);
					DecreaseEnergy();
					Helper.RotateToDirection(this, RotateToTarget(joystickMovementData));
				}
				else
				{
					controller.Move(moveDirection / 2f);
					noiseRadius /= 2;
				}
			}
			else
			{
				IncreaseStamina(1);
				noiseRadius = 0f;
			}
		}
		else
		{
			throw new System.NullReferenceException("joystickMovement is null!");
		}
	}

	private void Rotation()
	{
		if (joystickRotation)
		{
			if (joystickRotation.IsPressed)
			{
				Helper.RotateToDirection(this, RotateToTarget(joystickRotation.GetInput()));
			}
		}
		else
		{
			throw new System.NullReferenceException("joystickRotation is null!");
		}
	}

	private void IncreaseStamina(float staminaIncrement)
	{
		if (stamina < 100f)
		{
			stamina += staminaIncrement * Time.deltaTime;
		}
		else if (stamina > 100f)
		{
			stamina = 100f;
		}
	}

	private void DecreaseStamina(float staminaDecrement)
	{
		if (stamina > 0f)
		{
			stamina -= staminaDecrement * Time.deltaTime;
		}
		else if (stamina < 0f)
		{
			stamina = 0f;
		}
	}

	private void DecreaseEnergy()
	{
		if (energy > 0f)
		{
			energy -= energyDecrement * Time.deltaTime;
		}
		else if (energy < 0f)
		{
			energy = 0f;
		}
	}

	private Vector3 RotateToTarget(Vector3 targetVector)
	{
		return Vector3.RotateTowards
		(
			transform.forward, Vector3.Normalize(targetVector), ROTATION_SPEED * Time.deltaTime, 0.0f
		);
	}

	// void OnControllerColliderHit(ControllerColliderHit hit)
	// {
	// 	hitData = hit;
	// }

	// private float GetSlopeAngle()
	// {
	// 	if (hitData != null)
	// 	{
	// 		Vector3 normal = hitData.normal;
	// 		Vector3 horizontalNormal = new Vector3(normal.x, 0, normal.z);

	// 		float angle = Vector3.Angle(-normal, horizontalNormal) - 90;
	// 		return angle;
	// 	}
	// 	return 0f;
	// }

	// private void SlideDown()
	// {
	// 	if (hitData != null)
	// 	{
	// 		Vector3 horizontalNormal = new Vector3(hitData.normal.x, 0, hitData.normal.z);
	// 		Vector3 slideDirection = Quaternion.AngleAxis(GetSlopeAngle(), Vector3.forward) * horizontalNormal;
	// 		controller.Move(slideDirection * 10f * Time.deltaTime);
	// 		isSliding = true;
	// 	}
	// }
}