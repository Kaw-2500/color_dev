using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCooltimeManager : MonoBehaviour
{
    private Dictionary<PlayerColorManager.PlayerColorState, bool> isColorOnCooldown = new();
    private Dictionary<PlayerColorManager.PlayerColorState, Coroutine> cooldownCoroutines = new();

    [SerializeField] private PlayerColorManager playerColorManager;

    private void Awake()
    {
        // 全色クールタイム初期化
        foreach (PlayerColorManager.PlayerColorState color in System.Enum.GetValues(typeof(PlayerColorManager.PlayerColorState)))
        {
            isColorOnCooldown[color] = false;
        }
    }

    public bool IsOnCooldown(PlayerColorManager.PlayerColorState color)
    {
        return isColorOnCooldown[color];
    }

    public void StartCooldown(PlayerColorManager.PlayerColorState color)
    {
        float time = playerColorManager.GetDataByColor(color).NormalAttackCoolTime;

        if (IsOnCooldown(color))
        {
            //クールタイム中ならこのメソッドは呼ばれないはずだが、既にクールタイム中なら何もしない
            return;
        }

        cooldownCoroutines[color] = StartCoroutine(CooldownCoroutine(color, time));
    }

    public void ResetCooldown(PlayerColorManager.PlayerColorState color)
    {
        if (cooldownCoroutines.ContainsKey(color) && cooldownCoroutines[color] != null)
        {
            StopCoroutine(cooldownCoroutines[color]);
        }
        isColorOnCooldown[color] = false;//色の切り替え時はクールタイムをリセットする
    }

    private IEnumerator CooldownCoroutine(PlayerColorManager.PlayerColorState color, float cooldownTime)
    {
        isColorOnCooldown[color] = true;
        yield return new WaitForSeconds(cooldownTime);
        isColorOnCooldown[color] = false;
    }
}
