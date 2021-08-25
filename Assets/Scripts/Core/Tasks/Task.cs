﻿using System;

namespace KlimLib.TaskQueueLib {
    public abstract class Task {
        public Action<Task> OnComplete = t => { };
        public Action<Task, Exception> OnFailed = (t, e) => { };

        public abstract void Run();
    }
}
