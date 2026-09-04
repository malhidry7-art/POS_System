using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace POS_System
{
    public static class LicenseManager
    {
        // كلمة سر خاصة بك لا يعلمها أحد غيرك (Secret Salt)
        private const string SecretSalt = "PharmaTech@Nadeem2026_SecuredKey";
        private const string RegSubKey = @"Software\PharmaTech";

        // 1. جلب المعرف الفريد للجهاز (Hardware GUID)
        public static string GetHardwareId()
        {
            try
            {
                using (RegistryKey localMachineX64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (RegistryKey key = localMachineX64.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    if (key != null)
                    {
                        object val = key.GetValue("MachineGuid");
                        if (val != null)
                        {
                            return ComputeShortHash(val.ToString());
                        }
                    }
                }
            }
            catch { }
            return ComputeShortHash(Environment.MachineName + Environment.UserName);
        }

        // 2. التحقق من صحة مفتاح الترخيص
        public static bool ValidateKey(string hwid, string keyString, out DateTime expiryDate)
        {
            expiryDate = DateTime.MinValue;
            try
            {
                // المفتاح يتكون من جزئين مفصولين بشرطة: ExpDateHex-Signature
                string[] parts = keyString.Trim().Split('-');
                if (parts.Length != 2) return false;

                string expHex = parts[0];
                string signature = parts[1];

                // فك تاريخ الانتهاء
                long ticks = Convert.ToInt64(expHex, 16);
                DateTime exp = new DateTime(ticks);

                // التحقق من التوقيع الرقمي
                string expectedSig = GenerateSignature(hwid, expHex);
                if (signature.Equals(expectedSig, StringComparison.OrdinalIgnoreCase))
                {
                    expiryDate = exp;
                    return true;
                }
            }
            catch { }
            return false;
        }

        // 3. فحص حالة الترخيص الحالية للجهاز
        public static bool CheckCurrentLicense(out string message)
        {
            message = "";
            string hwid = GetHardwareId();

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegSubKey))
            {
                if (key == null)
                {
                    message = "النسخة غير مفعلة، يرجى إدخال مفتاح التنشيط.";
                    return false;
                }

                string savedKey = key.GetValue("LicenseKey") as string;
                string lastRunStr = key.GetValue("LastRun") as string;

                if (string.IsNullOrEmpty(savedKey))
                {
                    message = "لم يتم العثور على ترخيص صالح.";
                    return false;
                }

                DateTime expiryDate;
                if (!ValidateKey(hwid, savedKey, out expiryDate))
                {
                    message = "مفتاح الترخيص غير صالح لهذا الجهاز!";
                    return false;
                }

                // فحص تلاعب الوقت (Clock Tampering)
                DateTime lastRun = DateTime.MinValue;
                if (!string.IsNullOrEmpty(lastRunStr))
                {
                    long lastTicks = Convert.ToInt64(lastRunStr, 16);
                    lastRun = new DateTime(lastTicks);
                }

                if (DateTime.Now < lastRun)
                {
                    message = "تم التلاعب بساعة النظام! يرجى ضبط الوقت الصحيح.";
                    return false;
                }

                if (DateTime.Now.Date > expiryDate.Date)
                {
                    message = $"انتهت فترة الترخيص في: {expiryDate:yyyy-MM-dd}. يرجى التجديد.";
                    return false;
                }

                // تحديث آخر وقت تشغيل
                using (RegistryKey writeKey = Registry.CurrentUser.CreateSubKey(RegSubKey))
                {
                    writeKey.SetValue("LastRun", DateTime.Now.Ticks.ToString("X"));
                }

                return true;
            }
        }

        // 4. حفظ الترخيص في السجل
        public static void SaveLicense(string keyString)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegSubKey))
            {
                key.SetValue("LicenseKey", keyString.Trim());
                key.SetValue("LastRun", DateTime.Now.Ticks.ToString("X"));
            }
        }

        // دوال التشفير والحسابات
        public static string GenerateSignature(string hwid, string expHex)
        {
            using (SHA256 sha = SHA256.Create())
            {
                string raw = hwid + "-" + expHex + "-" + SecretSalt;
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return BitConverter.ToString(bytes).Replace("-", "").Substring(0, 16);
            }
        }

        public static string GenerateLicenseKey(string hwid, DateTime expiryDate)
        {
            string expHex = expiryDate.Date.Ticks.ToString("X");
            string sig = GenerateSignature(hwid, expHex);
            return $"{expHex}-{sig}";
        }

        private static string ComputeShortHash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 12);
            }
        }
    }
}