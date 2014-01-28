namespace VHDUpload
{
    using System;
    using System.Threading;

    internal class ProgressIndicator
    {
        private readonly char[] progressIndicator = { '-', '\\', '|', '/', '-' };
        private int progressState;
        private Timer timer;

        public ProgressIndicator()
        {
            Console.CursorVisible = false;

            this.timer = new Timer((state) =>
            {
                Console.Write("\r ");
                Console.Write(progressIndicator[progressState]);
                progressState = (progressState + 1) % progressIndicator.Length;
            }, null, Timeout.Infinite, 2000);
        }

        public void Enable()
        {
            this.timer.Change(0, 1000);
        }

        public void Disable()
        {
            this.timer.Change(Timeout.Infinite, 0);
        }
    }
}
