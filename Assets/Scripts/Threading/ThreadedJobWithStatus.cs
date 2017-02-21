using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Threading
{
    abstract class ThreadedJobWithStatus : ThreadedJob
    {
        private Queue<Message> m_Status = new Queue<Message>();
        private bool HasUpdate
        {
            get
            {
                bool tmp;
                lock(m_Handle)
                {
                    tmp = m_Status.Any();
                }
                return tmp;
            }
        }
        protected Message Status
        {
            get
            {
                Message tmp;
                lock (m_Handle)
                {
                    if (!m_Status.Any())
                    {
                        return null; // Should not happen
                    }
                    tmp = m_Status.Dequeue();
                }
                return tmp;
            }
            set
            {
                if (value == null) return;
                lock (m_Handle)
                {
                    m_Status.Enqueue(value);
                }
            }
        }

        public override bool Update()
        {
            if (HasUpdate)
            {
                OnStatusUpdate(Status);
            }
            return base.Update();
        }

        protected abstract void OnStatusUpdate(Message status);
    }
}
