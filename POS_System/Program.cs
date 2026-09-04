namespace POS_System
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            string message;
            // فحص حالة الترخيص للجهاز
            if (!LicenseManager.CheckCurrentLicense(out message))
            {
                using (ActivationForm actForm = new ActivationForm(message))
                {
                    // إذا لم يتم إدخال مفتاح صالح، يتوقف التطبيق فوراً
                    if (actForm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            // تشغيل شاشة تسجيل الدخول بعد التحقق من التفعيل
           Application.Run(new FrmLogin());
           // Application.Run(new KeygenForm());
        }
    }
}