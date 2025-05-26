using UnityEngine;

public class PlayerBasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;
	public bool canPush;
	[Range(0.5f, 5f)] public float strength = 1.1f;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (canPush) PushRigidBodies(hit);
	}

	private void PushRigidBodies(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;
		//밀 수 있는 레이어에 있는지 확인하기
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;
		//아래에 있는 물체는 무시
		if (hit.moveDirection.y < -0.3f) return;

		//수평방향으로만 계산하여 적용
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}