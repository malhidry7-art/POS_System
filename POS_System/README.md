# Pharmacy Management System (نظام إدارة الصيدلية المتكامل)

A desktop Point of Sale (POS) and Inventory Management System designed for pharmacies, built using C# (.NET 6.0 Windows Forms) and SQL Server.

## 🚀 المميزات الرئيسية (Key Features)
- **نظام تسجيل الدخول والصلاحيات (Authentication & Roles):** شاشة تسجيل دخول مخصصة للمدير (Admin) والكاشير (Cashier).
- **إدارة الأصناف والمخزون (Inventory Management):** إضافة، تعديل، وحذف الأدوية مع متابعة الباركود والكميات المتوفرة وتنبيهات نواقص المخزون وتواريخ الانتهاء.
- **نقطة البيع (POS):** دعم كامل للباركود، الحساب الفوري للخصم والضريبة، دعم البيع الآجل/النقدي وتتبع العملاء.
- **الطباعة الحرارية:** طباعة فواتير مبيعات متوافقة مع الطابعات الحرارية (Thermal Receipt Printers).
- **تقارير المبيعات (Sales Reports):** تصفية واستعراض الفواتير بحسب النطاق الزمني مع تفاصيل المبيعات وحساب الأرباح والإجماليات.

## 🛠️ التقنيات المستخدمة (Tech Stack)
- **Language:** C# (.NET 6.0 WinForms)
- **Architecture:** Multi-Tier Architecture (BLL & DAL)
- **Database:** Microsoft SQL Server (Transact-SQL)
- **Data Access:** ADO.NET (SqlClient)

## ⚙️ طريقة التشغيل (How to Run)
1. تشغيل سكريبت إنشاء قاعدة البيانات `POS_InventoryDB.sql` على SQL Server.
2. فتح الحل (Solution) في Visual Studio 2022 وبناء المشروع (`Release Mode`).
3. تشغيل ملف `POS_System.exe` مباشرة.