using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Utilities
{
    public static class PlayerLoopUtilities
    {
        public static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove)
        {
            if (loop.subSystemList == null)
            {
                return;
            }

            var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);
            for (var i = 0; i < playerLoopSystemList.Count; i++)
            {
                if (playerLoopSystemList[i].type == systemToRemove.type &&
                    playerLoopSystemList[i].updateDelegate == systemToRemove.updateDelegate)
                {
                    {
                        playerLoopSystemList.RemoveAt(i);
                        loop.subSystemList = playerLoopSystemList.ToArray();
                    }
                }
            
                HandleSubSystemLoopForRemoval<T>(ref loop, systemToRemove);
            }
        }

        private static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem loop, PlayerLoopSystem systemToRemove) 
        {
            if (loop.subSystemList == null)
            {
                return;
            }

            for (var i = 0; i < loop.subSystemList.Length; i++)
            {
                RemoveSystem<T>(ref loop.subSystemList[i], systemToRemove);
            }
        }
        
        public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index) 
        {
            if (loop.type != typeof(T))
            {
                return HandleSubSystemLoopForInsertion<T>(ref loop, systemToInsert, index);
            }
            
            var playerLoopSystemList = new List<PlayerLoopSystem>();
            if (loop.subSystemList != null)
            {
                playerLoopSystemList.AddRange(loop.subSystemList);
            }
            
            playerLoopSystemList.Insert(index, systemToInsert);
            loop.subSystemList = playerLoopSystemList.ToArray();
            return true;
        }

        private static bool HandleSubSystemLoopForInsertion<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index) 
        {
            if (loop.subSystemList == null)
            {
                return false;
            }

            for (var i = 0; i < loop.subSystemList.Length; i++) 
            {
                if (!InsertSystem<T>(ref loop.subSystemList[i], in systemToInsert, index))
                {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
        
        public static void PrintPlayerLoop(PlayerLoopSystem loop) 
        {
            var sb = new StringBuilder();
            sb.AppendLine("Unity Player Loop");
            foreach (var subSystem in loop.subSystemList) 
            {
                PrintSubsystem(subSystem, sb, 0);
            }
            
            Debug.Log(sb.ToString());
        }

        static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level) 
        {
            sb.Append(' ', level * 2).AppendLine(system.type.ToString());
            if (system.subSystemList == null || system.subSystemList.Length == 0)
            {
                return;
            }

            foreach (var subSystem in system.subSystemList) 
            {
                PrintSubsystem(subSystem, sb, level + 1);
            }
        }
    }
}