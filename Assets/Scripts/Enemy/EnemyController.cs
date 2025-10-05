using UnityEngine;

[RequireComponent(typeof(BaseStats), typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private CardType _dropCard;

    private BaseStats _stats;

    private void Awake()
    {
        _stats = GetComponent<BaseStats>();
    }

   public void DropLoot()
    {
        MessageQueue.Instance.SendMessage(new CardDroppedMessage
        {
            Type = _dropCard,
            Position = transform.position
        });

        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            PlayerController.Instance.transform.position,
            _stats.MovementSpeed * Time.deltaTime
        );
    }
}
