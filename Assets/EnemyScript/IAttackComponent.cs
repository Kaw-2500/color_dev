using UnityEngine;

public interface IAttackComponent
{
    void Init(float direction,float force, float damage,Enemy enemy);
}