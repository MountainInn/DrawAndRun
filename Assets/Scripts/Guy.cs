using UnityEngine;
using Dreamteck.Splines;
using System.Collections;

public class Guy : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] bool isRecruited;
    [SerializeField] Material recruitedMaterial;
    [SerializeField] Material notRecruitedMaterial;
    [Header("Components")]
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Animator animator;

    Vector2 previousOffset;

    public Squad squad;
    int
        _activation,
        _dieTrigger;

    void OnValidate()
    {
        ToggleMaterial(isRecruited);
    }

    void Awake()
    {
        _activation = Animator.StringToHash("isActive");
        _dieTrigger = Animator.StringToHash("dieTrigger");
    }

    public void Initialize(SplineComputer spline)
    {
        splineFollower.spline = spline;

        animator.SetFloat("runOffset", UnityEngine.Random.value);
        ToggleAnimation(isRecruited);
        ToggleFollowing(isRecruited);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Trap trap))
        {
            Die();
        }
        else if (other.TryGetComponent(out Gem gem))
        {
            gem.Collect();
        }
        else if (other.TryGetComponent(out Guy guy))
        {
            if (!guy.isRecruited)
            {
                Recruit();
            }
        }
    }

    void Recruit()
    {
        isRecruited = true;
        ToggleFollowing(true);
        ToggleMaterial(true);
        ToggleAnimation(true);
    }

    void Die()
    {
        ToggleFollowing(false);

        animator.SetTrigger(_dieTrigger);
        squad.RemoveGuy(this);
    }

    void ToggleFollowing(bool toggle)
    {
        splineFollower.follow = toggle;
    }

    void ToggleMaterial(bool toggle)
    {
        if (toggle)
        {
            skinnedMeshRenderer.material = recruitedMaterial;
        }
        else
        {
            skinnedMeshRenderer.material = notRecruitedMaterial;
        }
    }

    void ToggleAnimation(bool toggle)
    {
        animator.SetBool(_activation, toggle);
    }

    public void SetSplineWithOffset(SplineComputer spline, Vector2 offset)
    {
        splineFollower.spline = spline;

        Vector2 distance = offset - previousOffset;

        StartMoveCoroutine(distance);
       
        previousOffset = offset;
    }

    void StartMoveCoroutine(Vector2 offset)
    {
        float step = .05f;

        Spline.Direction direction =
            (offset.y < 0)
            ? Spline.Direction.Backward
            : Spline.Direction.Forward;

        offset.y = Mathf.Abs(offset.y);

        StartCoroutine(MoveCoroutine());

        IEnumerator MoveCoroutine()
        {
            for (float f = 0f; f < 1f; f += step)
            {
                Vector2 motionOffset = splineFollower.motion.offset;
                motionOffset.x += offset.x * step;
                splineFollower.motion.offset = motionOffset;

                splineFollower.Move_(offset.y * step, direction);

                yield return null;
            }
        }
    }
}
