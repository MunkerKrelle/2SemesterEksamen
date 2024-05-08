using System;

namespace _2SemesterEksamen
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            GameWorld.Instance.Run();
        }
    }
}
