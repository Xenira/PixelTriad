﻿using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Threading
{
    abstract class ThreadedJobWithQueue : ThreadedJob
    {
        private Queue<byte[]> m_Queue = new Queue<byte[]>();
        private bool m_IsCanceled = false;
        protected bool HasMessage
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                {
                    tmp = m_Queue.Any();
                }
                return tmp;
            }
        }
        protected byte[] Message
        {
            get
            {
                byte[] tmp;
                lock (m_Handle)
                {
                    if (!m_Queue.Any())
                    {
                        return null; // Should not happen
                    }
                    tmp = m_Queue.Dequeue();
                }
                return tmp;
            }
            set
            {
                if (value == null) return;
                lock (m_Handle)
                {
                    m_Queue.Enqueue(value);
                }
            }
        }

        private bool IsCancled
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                {
                    tmp = m_IsCanceled;
                }
                return tmp;
            }
            set
            {
                lock (m_Handle)
                {
                    m_IsCanceled = value;
                }
            }
        }

        public override bool Update()
        {
            if (IsCancled) return base.Update();
            if (!IsDone || !HasMessage) return false;
            IsDone = false;
            Start();
            return false;
        }
    }
}
