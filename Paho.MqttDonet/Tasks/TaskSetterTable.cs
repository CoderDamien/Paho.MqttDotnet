﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示任务管理表
    /// 线程安全类型
    /// </summary>
    /// <typeparam name="T">任务ID类型</typeparam>
    [DebuggerDisplay("Count = {table.Count}")]
    class TaskSetterTable<T>
    {
        /// <summary>
        /// 任务行为字典
        /// </summary>
        private readonly ConcurrentDictionary<T, ITaskSetter> table;

        /// <summary>
        /// 任务行为表
        /// </summary>
        public TaskSetterTable()
        {
            this.table = new ConcurrentDictionary<T, ITaskSetter>();
        }

        /// <summary>
        /// 创建带id的任务并添加到列表中
        /// </summary>
        /// <typeparam name="TResult">任务结果类型</typeparam>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public ITaskSetter<TResult> Create<TResult>(T id)
        {
            var taskSetter = new TaskSetter<TResult>();
            this.table.TryAdd(id, taskSetter);
            return taskSetter;
        } 

        /// <summary>      
        /// 获取并移除与id匹配的任务
        /// 如果没有匹配则返回null
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public ITaskSetter Take(T id)
        {
            ITaskSetter taskSetter;
            this.table.TryRemove(id, out taskSetter);
            return taskSetter;
        }

        /// <summary>
        /// 清除所有任务
        /// </summary>
        public void Clear()
        {
            this.table.Clear();
        }
    }
}
