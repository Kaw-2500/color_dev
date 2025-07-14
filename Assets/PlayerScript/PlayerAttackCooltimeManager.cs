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
        // �S�F�N�[���^�C��������
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
            //�N�[���^�C�����Ȃ炱�̃��\�b�h�͌Ă΂�Ȃ��͂������A���ɃN�[���^�C�����Ȃ牽�����Ȃ�
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
        isColorOnCooldown[color] = false;//�F�̐؂�ւ����̓N�[���^�C�������Z�b�g����
    }

    private IEnumerator CooldownCoroutine(PlayerColorManager.PlayerColorState color, float cooldownTime)
    {
        isColorOnCooldown[color] = true;
        yield return new WaitForSeconds(cooldownTime);
        isColorOnCooldown[color] = false;
    }
}
