# 🔐 CryptoCourse - تشفير وأمنية معلومات  

مشروع أكاديمي لمادة **التشفير وأمنية المعلومات** ضمن قسم **علوم الحاسوب - جامعة إب**.  
المشروع يهدف إلى بناء منصة تعليمية عملية لفهم خوارزميات التشفير (كلاسيكية وحديثة) وتطبيقها مع واجهة مستخدم سهلة، بالإضافة إلى توفير اختبارات وحدات لضمان موثوقية التنفيذ.  

---

## 👨‍💻 إعداد
- م. **طارق العمري**

## 🧑‍🏫 إشراف
- م. **نادر الحشئي**

---

## 🎯 أهداف المشروع
- بناء بنية برمجية منظمة وقابلة للتوسع (Clean Architecture).  
- فصل منطق التشفير عن واجهة المستخدم (Separation of Concerns).  
- توفير واجهة استخدام رسومية (Windows Forms) لتطبيق الخوارزميات.  
- إضافة اختبارات وحدات (Unit Tests) لضمان صحة الخوارزميات.  
- تقديم أدوات مساعدة (Utilities) لتبسيط التعامل مع الملفات والنصوص.  

---

## 🏗️ هيكل الحل (Solution Layout)

```text
CryptoCourse.sln
│
├── 📁 CryptoCourse.Core (Class Library)
│   ├── Interfaces/
│   │   ├── IEncryptor.cs
│   │   ├── IDecryptor.cs
│   │   └── IAsymmetricKeyPairGenerator.cs
│   ├── Algorithms/
│   │   ├── Classical/
│   │   │   ├── CaesarCipher.cs
│   │   │   ├── VigenereCipher.cs
│   │   │   └── ...
│   │   └── Modern/
│   │       ├── RsaEducational.cs
│   │       └── SecureWrappers/
│   │           ├── AesWrapper.cs
│   │           └── RsaWrapper.cs
│   └── CryptoCourse.Core.csproj
│
├── 📁 CryptoCourse.WinFormsUI (Windows Forms App)
│   ├── Program.cs
│   ├── MainForm.cs
│   ├── Controls/
│   │   ├── CaesarPanel.cs
│   │   ├── RsaPanel.cs
│   │   └── ...
│   └── CryptoCourse.WinFormsUI.csproj
│
├── 📁 CryptoCourse.Tests (Unit Test Project - MSTest)
│   ├── Core/
│   │   ├── CaesarCipherTests.cs
│   │   ├── VigenereCipherTests.cs
│   │   └── ...
│   └── CryptoCourse.Tests.csproj
│
└── 📁 CryptoCourse.Utils (Class Library)
    ├── FileHelper.cs
    ├── StringConverter.cs
    └── CryptoCourse.Utils.csproj
