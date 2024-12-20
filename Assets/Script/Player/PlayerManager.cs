using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class PlayerManager : MonoBehaviour
// {
//    public static PlayerManager instance;
//
//    public Player player;
//    private void Awake()
//    {
//       if (instance != null)
//       {
//          Destroy(instance.gameObject);
//       }
//       else
//       {
//          instance = this;
//       }
//    }
// }
public class PlayerManager : MonoBehaviour
{
   public static PlayerManager instance;
   public Player player;

   private void Awake()
   {
      Debug.Log("who calls first awake from playermanager");
      // 确保 PlayerManager 在场景中的唯一性
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }

      // 确保 player 已经赋值
      if (player == null)
      {
         player = FindFirstObjectByType<Player>(); // 或者使用您特定的init逻辑
      }
   }
}